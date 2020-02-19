using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVCSite.DAC.Interfaces;
using MVCSite.DAC.Instrumentation;
using MVCSite.DAC.Entities;
using System.Data.Objects;
using WMath.Facilities;
using System.Data.Entity.Validation;
using System.Data;
using MVCSite.DAC.Common;
using System.Data.Entity.Infrastructure;

namespace MVCSite.DAC.Repositories
{
    public class RepositoryStats : IRepositoryStats
    {
        protected readonly StatDataContext _dataContext;
        protected readonly ObjectContext _objectContext;

        public RepositoryStats(StatDataContext dataContext)
        {
            _dataContext = dataContext;
            _objectContext = ((IObjectContextAdapter)_dataContext).ObjectContext;
        }
        public IEnumerable<EmailAccountToSend> EmailAccountToSendsGetTop100()
        {
            return _dataContext.EmailAccountToSends.OrderBy(x => x.EnterTime).Take(100);
        }
        public void EmailAccountToSendsRemove(EmailAccountToSend account)
        {
            try
            {
                _dataContext.EmailAccountToSends.Remove(account);
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw;
            }
            return;
        }
        public IEnumerable<QueuedEmails> QueuedEmailsGetAll()
        {
            return _dataContext.QueuedEmails;
        }
        public void QueuedEmailsRemove(QueuedEmails email)
        {
            try
            {
                _dataContext.QueuedEmails.Remove(email);
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw;
            }
            return;
        }
        public void QueuedEmailsRemoveByGuid(Guid id)
        {
            try
            {
                var email = _dataContext.QueuedEmails.Where(x => x.ID == id).SingleOrDefault();
                if (email != null)
                {
                    _dataContext.QueuedEmails.Remove(email);
                    _dataContext.SaveChanges();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                throw;
            }
            return;
        }
        public void SendEmailLogCreate(SendEmailLog email)
        {
            try
            {
                _dataContext.SendEmailLog.Add(email);
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw;
            }
            return;
        }
        public Visits CreateVisits(Visits _visits)
        {
            Logger _Logger = new Logger();
            try
            {
                //_Logger.LogInfo(" New Visit will be added " + _visits.path);
                _dataContext.Visits.Add(_visits);
                //_Logger.LogInfo(" New Visit will be saved " + _visits.path);
                _dataContext.SaveChanges();
                //_Logger.LogInfo(" New Visit has been saved " + _visits.path);
            }
            catch (DbEntityValidationException dbEx)
            {
                //_Logger.LogInfo(" New Visit failed to be saved " + _visits.path);
                throw;
            }
            return _visits;
        }
        public CianQuestionLog CreateCianQuestionLog(CianQuestionLog _cianQuestionLog)
        {

            try
            {
                _dataContext.CianQuestionLog.Add(_cianQuestionLog);
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw;
            }
            return _cianQuestionLog;
        }
        public void EnqueueEmail(string from, string to, string subject, string body,
            SendEmailSite site = SendEmailSite.Vjiaoshi, SendEmailType type = SendEmailType.EmailConfirmation)
        {
            Logger _Logger = new Logger();
            try
            {
                var email = new QueuedEmails
                {
                    ID = Guid.NewGuid(),
                    Sender = from,
                    Receiver = to,
                    Title = subject,
                    Body = body,
                    EnterTime = DateTime.UtcNow,
                    SiteType = (byte)site,
                    EmailType = (int)type
                };

                //_Logger.LogInfo(" Email will be added to queued for " + to + " with subject " + subject);
                _dataContext.QueuedEmails.Add(email);
                //_Logger.LogInfo(" Email will be queued for " + to + " with subject " + subject);
                _dataContext.SaveChanges();
                //_Logger.LogInfo(" Email QUEUED for " + to + " with subject " + subject);


            }
            catch (DbEntityValidationException dbEx)
            {
                //_Logger.LogError(" Email FAILED to be queued for " + to + " with subject " + subject);
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        var error = string.Format("ValidationError--Property: {0} Error: {1}",
                            validationError.PropertyName, validationError.ErrorMessage);
                        //_Logger.LogError(" Email detailed error for " + to + " with subject " + subject + " is " + error);
                    }
                }
            }
        }
        public void DetachAllObjectsInContext()
        {
            var objectStateEntries = _objectContext
                            .ObjectStateManager
                            .GetObjectStateEntries(EntityState.Added | EntityState.Deleted |
                            EntityState.Modified | EntityState.Unchanged);
            foreach (var objectStateEntry in objectStateEntries)
            {
                _objectContext.Detach(objectStateEntry.Entity);
            }
            //_objectContext.SaveChanges();
            _objectContext.Dispose();

        }
    }
}