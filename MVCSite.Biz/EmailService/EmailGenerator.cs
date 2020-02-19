using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using RazorEngine;
using RazorEngine.Templating;
using MVCSite.DAC.Interfaces;
using MVCSite.DAC.Common;
using System.Collections.Specialized;
using MVCSite.DAC.Instrumentation;
using MVCSite.Common;

namespace MVCSite.Biz
{
    //public static class FileSystemHelper
    //{
    //    public static DirectoryInfo GetFolderFromParentPaths(string name,bool isPrephecy)
    //    {
    //        var executingAssemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"file:\", string.Empty);
    //        var relativeDir=@"\EmailService\Templates\";
    //        if(isPrephecy)
    //            relativeDir = @"\EmailService\PTemplates\";
    //        var dir = new DirectoryInfo(executingAssemblyPath + relativeDir);
    //        return dir;
    //        //DirectoryInfo extractionScriptsDir = null;
    //        //do
    //        //{
    //        //    extractionScriptsDir = dir.GetDirectories().Where(x => x.Name == name).SingleOrDefault();
    //        //} while ((dir = dir.Parent) != null && extractionScriptsDir == null);
    //        //if (extractionScriptsDir == null)
    //        //    throw new Exception(string.Format("Unable to find folder specified. Executing assembly path:'{0}'", executingAssemblyPath));

    //        //return extractionScriptsDir;
    //    }
    //}

    public class EmailGenerator
    {
        private readonly DirectoryInfo _emailTemplateDir;
        private static StringDictionary _emailTemplates=new StringDictionary();
        private readonly ILogger _logger;

        public EmailGenerator(ILogger logger)
        {
            _logger = logger;
            _emailTemplateDir = FileSystemHelper.GetPhysicalPath(@"\EmailService\Templates\");
            var layout = ReadTemplate("_Layout");
            Engine.Razor.AddTemplate(layout, "Layout");
            //var layoutTemplate = Razor.GetTemplate(layout, "Layout"); //load layout template and force it compilation and caching
        }

        //public EmailGenerator(ILogger logger)
        //{
        //    _logger = logger;
        //    _emailTemplateDir = FileSystemHelper.GetPhysicalPath(@"\EmailService\Templates\");
        //    var layout = ReadTemplate("_BookingConfirmationEmailLayout");
        //    var layoutTemplate = Razor.GetTemplate(layout, "Layout"); //load layout template and force it compilation and caching
        //}

        public string GetForgotPasswordString(EmailForgotPassword model)
        {
            return GetEmailString("ForgotPassword", model);
        }
        public string GetConfirmationString(EmailConfirmation model)
        {
            return GetEmailString("email_confirmation", model);
        }
        public string GetEmailUserContactCSRString(EmailUserContactCSR model)
        {
            return GetEmailString("emailContactCSR", model);
        }
        public string GetUserAddedNotificationString(UserAddedNotification model)
        {
            return GetEmailString("UserAddedNotification", model);
        }
        public string GetActivityNotificationString(ActivityNotification model)
        {
            return GetEmailString("ActivityNotification", model);
        }

        public string GetInviteFriendString(EmailInviteFriend model)
        {
            return GetEmailString("EmailInviteFriend", model);
        }

        public string GetPromotionEmailString(PromotionModel model)
        {
            return GetEmailString(model.PromotionName, model);
        }

        public string GetTravelerBookingConfirmationEmailString(BookingConfirmationModel model)
        {
            return GetEmailString("TravelerBookingConfirmation", model);
        }
        public string GetGuideBookingConfirmationEmailString(BookingConfirmationModel model)
        {
            return GetEmailString("GuideBookingConfirmation", model);
        }
        public string GetAccountManagerBookingConfirmationEmailString(BookingConfirmationModel model)
        {
            return GetEmailString("AccountManagerBookingConfirmation", model);
        }
        string GetEmailString<T>(string templateName, T model) where T : EmailModel
        {
            if (string.IsNullOrEmpty(templateName))
                throw new ArgumentNullException("templateName");

            //using (var service = new IsolatedTemplateService())
            //{
            //    var templateString = ReadTemplate(templateName);
            //    string result = service.Parse(templateString, model);
            //    return result;
            //}

            //var oldTemplate = Razor.Resolve(templateName);
            //if (oldTemplate == null)
            //{
            //    var templateString = ReadTemplate(templateName);
            //    Razor.Compile(templateString, typeof(T), templateName);
            //    //oldTemplate=Razor.GetTemplate(templateString, templateName);
            //}
            //var pageText = Razor.Run(templateName, model);

            if (!Engine.Razor.IsTemplateCached(templateName, model.GetType()))
            {
                var templateString = ReadTemplate(templateName);
                Engine.Razor.AddTemplate(templateName, templateString);
            }

            var pageText = Engine.Razor.RunCompile(templateName, model.GetType(), model);
            return pageText;
        }
        string ReadTemplate(string name)
        {
            if (_emailTemplates.ContainsKey(name))
            {
                //_logger.LogInfo(string.Format("_emailTemplates.ContainsKey({0})== true.", name));
                return _emailTemplates[name];
            }
            //_logger.LogInfo(string.Format("_emailTemplates.ContainsKey({0})== false.", name));
            var templatePath = Path.Combine(_emailTemplateDir.FullName, string.Format("{0}.cshtml", name));
            var content = string.Empty;
            using (var reader = File.OpenText(templatePath))
            {
                content = reader.ReadToEnd();
            }
            
            _emailTemplates.Add(name, content);
            return content;
        }
    }
}
