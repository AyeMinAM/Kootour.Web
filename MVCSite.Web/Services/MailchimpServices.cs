using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using MailChimp.Net;
using MailChimp.Net.Interfaces;
using MailChimp.Net.Models;
using Microsoft.Practices.Unity;
using MVCSite.Common.NameHelper;
using MVCSite.DAC.Entities;
using MVCSite.DAC.Instrumentation;
using MVCSite.DAC.Interfaces;

namespace MVCSite.Web.Services
{
    public static class MailChimpServices
    {
        public static async void SubscribeToMailChimpList(string email, string fullName)
        {
            var MailChimpApiKey = WebConfigurationManager.AppSettings["MailChimpApiKey"];
            var MailChimpListId = WebConfigurationManager.AppSettings["MailChimpListId"];

            IMailChimpManager manager = new MailChimpManager(MailChimpApiKey);

            // Use the Status property if updating an existing member
            var member = new MailChimp.Net.Models.Member { EmailAddress = email, StatusIfNew = Status.Subscribed };
            member.MergeFields.Add("MERGE1", fullName);
            await manager.Members.AddOrUpdateAsync(MailChimpListId, member);
        }
    }
}