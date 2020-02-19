using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using MVCSite.EmailService;
using MVCSite.Biz;
using MVCSite.DAC.Interfaces;
using Microsoft.Practices.Unity;
using MVCSite.DAC.Entities;
using MVCSite.DAC.Common;
using System.Threading.Tasks;
using MVCSite.DAC.Instrumentation;
using MVCSite.Common;


namespace MVCSite.EmailService
{
    public class EmailService : ServiceBase
    {
        private Timer _mailcheckTimer;
        private readonly ILogger _logger;
        //private readonly Emailer    _emailer;
        private readonly BizUnityContainer _bizContainer;
        private readonly EmailerSettings _settings;
        private readonly EmailGenerator _emailGenerator;
        public EmailService()
        {

            _settings = ReadEmailerSettingsFromConfig();
            //_emailer = new Emailer(_settings);
            _bizContainer = new BizUnityContainer(new ConnectionStrings
            {
                kootourConnectionString = "name=kootourConnectionString",
                statConnectionString = "name=statConnectionString",

            });
            _emailGenerator = _bizContainer.Resolve<EmailGenerator>();
            _logger = _bizContainer.Resolve<ILogger>(); 

        }

        static void Main(string[] args) 
        {
            log4net.Config.XmlConfigurator.Configure();

            if (args.Length > 0 && (args[0] == "/i" || args[0] == "/install")) { Install(args); return; }
            if (args.Length > 0 && (args[0] == "/u" || args[0] == "/uninstall")) { Uninstall(args); return; }

            var servicesToRun = new ServiceBase[]  {  new EmailService() };

            if (Environment.UserInteractive) //if runs as a console application
            {
                var type = typeof(ServiceBase);
                const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
                var method = type.GetMethod("OnStart", flags);

                foreach (var service in servicesToRun)
                    method.Invoke(service, new object[] { args });

                Console.WriteLine("Press any key to exit ...");
                Console.Read();

                foreach (var service in servicesToRun)
                    service.Stop();
            }
            else
                ServiceBase.Run(servicesToRun);
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            _logger.LogInfo("Starting service ...");
            int waitSecondsAfterScan = _settings.WaitSecondsAfterScan;
            if (waitSecondsAfterScan <= 0)
                waitSecondsAfterScan = 10;

            _mailcheckTimer = new Timer(stub => 
            {
                try
                {
                    var repository = _bizContainer.Resolve<IRepositoryStats>();
                    var emailsToSend = repository.QueuedEmailsGetAll().ToArray();
                    ParallelOptions parallelOptions = new ParallelOptions();
                    parallelOptions.MaxDegreeOfParallelism = _settings.MaxDegreeOfParallelism;
                    Parallel.ForEach(emailsToSend, parallelOptions, email => ProcessSafe(() =>
                    {
                        var threadRepository = _bizContainer.Resolve<IRepositoryStats>();
                        _logger.LogInfo(string.Format("Sending email to '{0}'", email.Receiver));
                        var log = new SendEmailLog();
                        log.ID = Guid.NewGuid();
                        log.Sender = email.Sender;
                        log.Receiver = email.Receiver;
                        log.Title = email.Title;
                        log.Body = email.Body;
                        log.EnterTime = email.EnterTime;
                        log.SendTime = DateTime.UtcNow;
                        log.SendResult = true;
                        log.SiteType = email.SiteType;
                        log.EMailType = email.EmailType;
                        try
                        {
                            var emailer = new Emailer(_settings);
                            emailer.SendHtml(email.Sender, email.Receiver, email.Title, email.Body);
                            emailer = null;
                        }
                        catch (Exception e)
                        {
                            log.SendResult = false;
                            _logger.LogError(e);
                        }
                        threadRepository.SendEmailLogCreate(log);
                        threadRepository.QueuedEmailsRemoveByGuid(email.ID);
                        _logger.LogInfo("Email has been sent");
                        if (_settings.SendEmailInterval>0)
                            Thread.Sleep(_settings.SendEmailInterval);
                        threadRepository.DetachAllObjectsInContext();
                        threadRepository = null;
                        log = null;

                    }));
                    PromotionModel emailVariables = null;
                    repository.EmailAccountToSendsGetTop100().ToList().ForEach(account =>
                    {
                        emailVariables = new PromotionModel();
                        emailVariables.PromotionLink = account.Link;
                        emailVariables.PromotionName = account.TemplateName;
                        var emailBody = _emailGenerator.GetPromotionEmailString(emailVariables);
                        _logger.LogInfo(" Email will be queued for " + account.Email + " with subject " + account.Subject);
                        repository.EnqueueEmail(_settings.SmtpServerUserName, account.Email, account.Subject, emailBody,
                            SendEmailSite.Vjiaoshi, SendEmailType.Promotion);
                        _logger.LogInfo(" Email has been queued for " + account.Email + " with subject " + account.Subject);
                        repository.EmailAccountToSendsRemove(account);
                        emailVariables = null;
                        emailBody = null;
                    });
                    repository.DetachAllObjectsInContext();
                    repository = null;
                }
                catch (Exception e)
                {
                    _logger.LogError(e);
                }
                finally
                {
                    _mailcheckTimer.Change(TimeSpan.FromSeconds(waitSecondsAfterScan), TimeSpan.Zero);
                }
            }, null, TimeSpan.FromSeconds(waitSecondsAfterScan), TimeSpan.Zero);

            _logger.LogInfo("Service started");
        }
        void ProcessSafe(Action action)
        {
            try
            {
                action();
            }
            catch (ThreadAbortException threadExp)
            {
                throw;
            }
            catch (Exception exc12) //don't use "e" here - VS bug
            {
                var msg = string.Format("something unexpected happened: {0}", exc12.Message);
                _logger.LogError(msg, exc12);
            }
        }

        protected override void OnStop()
        {
            base.OnStop();
            _mailcheckTimer.Dispose();
            _logger.LogInfo("Service stopped");
        }

        static EmailerSettings ReadEmailerSettingsFromConfig()
        {
            var emailerSettings = new EmailerSettings { SmtpServerName = ConfigurationManager.AppSettings["Mailer.SMTPServer"] };
            if (ConfigurationManager.AppSettings["Mailer.SMTPServerPort"] != null)
                emailerSettings.SmtpServerPort = int.Parse(ConfigurationManager.AppSettings["Mailer.SMTPServerPort"]);
            if (ConfigurationManager.AppSettings["Mailer.SMTPServerUserName"] != null)
            {
                emailerSettings.RequiresAuthentication = true;
                emailerSettings.SmtpServerUserName = ConfigurationManager.AppSettings["Mailer.SMTPServerUserName"];
            }
            if (ConfigurationManager.AppSettings["Mailer.SMTPServerPassword"] != null)
                emailerSettings.SmtpServerUserPassword = ConfigurationManager.AppSettings["Mailer.SMTPServerPassword"];
            if (ConfigurationManager.AppSettings["Mailer.SMTPDisplayUserName"] != null)
            {
                emailerSettings.SMTPDisplayUserName = ConfigurationManager.AppSettings["Mailer.SMTPDisplayUserName"];
            }
            if (ConfigurationManager.AppSettings["Mailer.EnableSSL"] != null)
                emailerSettings.EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["Mailer.EnableSSL"]);

            if (ConfigurationManager.AppSettings["Mailer.SendToFolder"] != null)
                emailerSettings.SendToFolder = bool.Parse(ConfigurationManager.AppSettings["Mailer.SendToFolder"]);
            if (ConfigurationManager.AppSettings["Mailer.OutgoingFolder"] != null)
                emailerSettings.OutgoingFolder = ConfigurationManager.AppSettings["Mailer.OutgoingFolder"];
            if (ConfigurationManager.AppSettings["Mailer.SendEmailInterval"] != null)
                emailerSettings.SendEmailInterval = int.Parse(ConfigurationManager.AppSettings["Mailer.SendEmailInterval"]);
            if (ConfigurationManager.AppSettings["Mailer.MaxDegreeOfParallelism"] != null)
                emailerSettings.MaxDegreeOfParallelism = Convert.ToInt32(ConfigurationManager.AppSettings["Mailer.MaxDegreeOfParallelism"]);
            if (ConfigurationManager.AppSettings["Mailer.WaitSecondsAfterScan"] != null)
                emailerSettings.WaitSecondsAfterScan = Convert.ToInt32(ConfigurationManager.AppSettings["Mailer.WaitSecondsAfterScan"]);
            else
                emailerSettings.WaitSecondsAfterScan = 10;
            return emailerSettings;
        }

        static void Install(string[] args) 
        {
            Console.WriteLine("Installing ..."); 
            using (var installer = new AssemblyInstaller(typeof(EmailService).Assembly, args)) 
            { 
                installer.UseNewContext = true; 
                var state = new Hashtable(); 
                try 
                {
                    installer.Install(state); 
                    installer.Commit(state);
                    Console.WriteLine("Installed successfully !"); 
                } 
                catch 
                {
                    Console.WriteLine("Unexpected error happened. Rolling back ..."); 
                    installer.Rollback(state);
                    Console.WriteLine("Rolled back to initial state."); 
                    throw;
                } 
            } 
        }
        static void Uninstall(string[] args)
        {
            Console.WriteLine("Uninstalling ...");
            using (var installer = new AssemblyInstaller(typeof(EmailService).Assembly, args))
            {
                installer.UseNewContext = true;
                var state = new Hashtable();
                try
                {
                    installer.Uninstall(state);
                    Console.WriteLine("Uninstalled successfully !"); 
                }
                catch
                {
                    Console.WriteLine("Unexpected error happened. Rolling back ..."); 
                    installer.Rollback(state);
                    Console.WriteLine("Rolled back to initial state."); 
                    throw;
                }
            } 
        }
    } 
}