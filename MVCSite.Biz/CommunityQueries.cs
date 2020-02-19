using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVCSite.DAC.Services;
using MVCSite.Biz.Interfaces;
using MVCSite.DAC.Interfaces;
using MVCSite.DAC.Entities;
using System.Security;
using MVCSite.DAC.Repositories;
using DevTrends.MvcDonutCaching;

namespace MVCSite.Biz
{
    public class CommunityQueries : QueriesBase, ICommunityQueries
    {

        private readonly IRepositoryUsers _repositoryUsers;
        private readonly ISiteConfiguration _configuration;
        public CommunityQueries(ISecurity securityService,
                              IWebApplicationContext webApplicationContext,
                              IRepositoryUsers repositoryUsers,
                              ISiteConfiguration configuration)
            : base(securityService,webApplicationContext)
        {
            _repositoryUsers = repositoryUsers;
            _configuration = configuration;
        }

    }
}
