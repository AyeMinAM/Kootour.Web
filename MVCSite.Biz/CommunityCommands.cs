using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVCSite.DAC.Interfaces;
using MVCSite.DAC.Instrumentation;
using MVCSite.Biz.Interfaces;
using MVCSite.DAC.Entities;
using MVCSite.DAC.Common;
using MVCSite.DAC.Instrumentation.Membership;
using System.IO;
using System.Collections;
using System.Web;
using System.Web.Mvc;
using DevTrends.MvcDonutCaching;
//using SignalR;
using MVCSite.Common;

namespace MVCSite.Biz
{
    class CommunityCommands : CommandsBase, ICommunityCommands
    {
        private readonly ISecurity _securityService;
        private readonly IWebApplicationContext _webContext;
        private readonly IEmailer _emailer;
        private readonly EmailGenerator _emailGenerator;
        private readonly IRepositoryUsers _repositoryUsers;
        private readonly ISiteConfiguration _configuration;

        private readonly PageFlow _pageFlow;
        private readonly IRepository _repository;

        public CommunityCommands(ISecurity securityService,
                              IWebApplicationContext webApplicationContext,
                              IEmailer emailer,
                              EmailGenerator emailGenerator,
                              IRepositoryUsers repositoryUsers,
                              ISiteConfiguration configuration,
                              ILogger logger,
                              PageFlow pageFlow,
                              IRepository repository)
            : base(logger)
        {
            _securityService = securityService;
            _webContext = webApplicationContext;
            _emailer = emailer;
            _emailGenerator = emailGenerator;
            _repositoryUsers = repositoryUsers;
            _configuration = configuration;
            _pageFlow = pageFlow;
            _repository = repository;
        }
        public void UpdateChatContent(int chatId)
        {
            if (chatId <= 0)
                return;
            //var context = GlobalHost.ConnectionManager.GetHubContext<CommunicateHub>();
            //context.Clients[chatId.ToString()].refreshWhenAudioPosted(chatId.ToString());
        }
    }
}

