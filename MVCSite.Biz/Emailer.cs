using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVCSite.DAC.Interfaces;
using MVCSite.Biz.Interfaces;
using MVCSite.DAC.Entities;
using MVCSite.DAC.Common;

namespace MVCSite.Biz
{
    public class Emailer : IEmailer
    {
        private readonly IRepositoryStats _repositoryStats;
        private readonly ISiteConfiguration _configuration;

        public Emailer(IRepositoryStats repositoryStats, ISiteConfiguration configuration)
        {
            _repositoryStats = repositoryStats;
            _configuration = configuration;
        }

        public void SendHtml(string from, string to, string subject, string body)
        {
            _repositoryStats.EnqueueEmail(from, to, subject, body, SendEmailSite.Vjiaoshi, SendEmailType.EmailConfirmation);
        }

        public void SendUserEmail(User userProfile, string to, string subject, string bodyHtml)
        {
            SendHtml(_configuration.DontReplyEmailAddress, to, subject, bodyHtml);
        }
    }
}
