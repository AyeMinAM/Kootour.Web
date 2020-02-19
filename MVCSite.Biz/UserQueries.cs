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
    public class UserQueries : QueriesBase, IUserQueries
    {

        private readonly IRepositoryUsers _repositoryUsers;
        private readonly ISiteConfiguration _configuration;
        public UserQueries(ISecurity securityService,
                              IWebApplicationContext webApplicationContext,
                              IRepositoryUsers repositoryUsers,
                              ISiteConfiguration configuration)
            : base(securityService,webApplicationContext)
        {
            _repositoryUsers = repositoryUsers;
            _configuration = configuration;

        }
        public IEnumerable<User> UserGetGlobalUsers()
        {
            return _repositoryUsers.GetGlobalUsers();
        }



        public User GetByMobileOrNull(string mobile)
        {
            return _repositoryUsers.GetByMobileOrNull(mobile);
        }
        public User GetByPhoneOrNull(string area, string local)
        {
            return _repositoryUsers.GetByPhoneOrNull(area, local);
        }
        public User UserGetById(int userId)
        {
            return _repositoryUsers.GetByIdOrNull(userId);
        }
        public User UserGetCurrentUser()
        {
            var userId = _security.GetCurrentUserId();
            if (userId <=0)
                return null;
            return _repositoryUsers.UserGetCurrentUser(userId);
        }

    }
}
