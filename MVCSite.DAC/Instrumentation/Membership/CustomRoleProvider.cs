using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using Microsoft.Practices.Unity;

namespace MVCSite.DAC.Instrumentation.Membership
{
   /*
    public class CustomRoleProvider : RoleProvider
    {
        public static IUnityContainer RootContainer { set; private get; }


        private UsersContext getRepository()
        {
            return RootContainer.Resolve<UsersContext>();
        }
        private UsersContext createRepository()
        {
            return RootContainer.Resolve<UsersContext>("perResolveRepository");
        }

        private string _applicationName;

        //queries
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            if (string.IsNullOrEmpty(name))
            {
                name = "CodeFirstRoleProvider";
            }
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Code-First Role Provider");
            }

            base.Initialize(name, config);

            _applicationName = config["applicationName"];
        }
        public override string ApplicationName
        {
            get { return _applicationName; }
            set { _applicationName = value; }
        }
        public override bool RoleExists(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw CreateArgumentNullOrEmptyException("roleName");
            }
            var context = getRepository();
            {
                dynamic result = context.Roles.FirstOrDefault(rl => rl.RoleName == roleName);
                if (result != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public override bool IsUserInRole(string userName, string roleName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw CreateArgumentNullOrEmptyException("userName");
            }
            if (string.IsNullOrEmpty(roleName))
            {
                throw CreateArgumentNullOrEmptyException("roleName");
            }
            var context = getRepository();
            {
                dynamic user = context.Users.FirstOrDefault(Usr => Usr.Username == userName);
                if (user == null)
                {
                    return false;
                }
                dynamic role = context.Roles.FirstOrDefault(Rl => Rl.RoleName == roleName);
                if (role == null)
                {
                    return false;
                }
                return user.Roles.Contains(role);
            }
        }
        public override string[] GetAllRoles()
        {
            var context = getRepository();
            {
                return context.Roles.Select(Rl => Rl.RoleName).ToArray();
            }
        }
        public override string[] GetUsersInRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw CreateArgumentNullOrEmptyException("roleName");
            }
            var context = getRepository();
            {
                var role = context.Roles.FirstOrDefault(Rl => Rl.RoleName == roleName);
                if (role == null)
                {
                    throw new InvalidOperationException("Role not found");
                }
                return context.Roles
                    .Join(context.Users, x => x.RoleId, x => x.IdGuid, (x, y) => new {x.RoleName, y.Username})
                    .Where(x => x.RoleName == roleName).Select(x => x.Username).ToArray();
                //return role.Users.Select(Usr => Usr.Username).ToArray();
            }
        }
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw CreateArgumentNullOrEmptyException("roleName");
            }
            if (string.IsNullOrEmpty(usernameToMatch))
            {
                throw CreateArgumentNullOrEmptyException("usernameToMatch");
            }
            var context = getRepository();
            {
                var query = context.Roles
                                .Join(context.Users, x => x.RoleId, x => x.IdGuid, (x, y) => new {x.RoleName, y.Username})
                                .Where(x => x.RoleName == roleName && x.Username.Contains(usernameToMatch)).Select(x => x.Username);
                //var query = from Rl in context.Roles from Usr in Rl.Users where Rl.RoleName == roleName && Usr.Username.Contains(usernameToMatch) select Usr.Username;
                return query.ToArray();
            }
        }
        public override string[] GetRolesForUser(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw CreateArgumentNullOrEmptyException("userName");
            }
            var context = getRepository();

            {
                var user = context.Users.FirstOrDefault(Usr => Usr.Username == userName);
                if (user == null)
                {
                    return new string[] { };
                    //throw new InvalidOperationException(string.Format("User not found: {0}", userName));
                }
                return context.Roles
                   .Join(context.Users, x => x.RoleId, x => x.IdGuid, (x, y) => new { x.RoleName, y.Username })
                   .Where(x => x.Username == userName).Select(x => x.RoleName).ToArray();
                //return user.Roles.Select(Rl => Rl.RoleName).ToArray();
            }
        }

        //commands
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw CreateArgumentNullOrEmptyException("roleName");
            }
            using (var context = createRepository())
            {
                dynamic role = context.Roles.FirstOrDefault(Rl => Rl.RoleName == roleName);
                if (role == null)
                {
                    throw new InvalidOperationException("Role not found");
                }
                if (throwOnPopulatedRole)
                {
                    dynamic usersInRole = role.Users.Any;
                    if (usersInRole)
                    {
                        throw new InvalidOperationException(string.Format("Role populated: {0}", roleName));
                    }
                }
                else
                {
                    foreach (User usr_loopVariable in role.Users)
                    {
                        var usr = usr_loopVariable;
                        context.Users.DeleteObject(usr);
                    }
                }
                context.Roles.DeleteObject(role);
                context.SaveChanges();
                return true;
            }
        }
        public override void CreateRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw CreateArgumentNullOrEmptyException("roleName");
            }
            using (var context = createRepository())
            {
                dynamic role = context.Roles.FirstOrDefault(Rl => Rl.RoleName == roleName);
                if (role != null)
                {
                    throw new InvalidOperationException(string.Format("Role exists: {0}", roleName));
                }
                Role NewRole = new Role
                {
                    RoleId = Guid.NewGuid(),
                    RoleName = roleName
                };
                context.DeleteObject(NewRole);
                context.SaveChanges();
            }
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            using (var context = createRepository())
            {
                var users = context.Users.Where(usr => usernames.Contains(usr.Username)).ToList();
                var roles = context.Roles.Where(rl => roleNames.Contains(rl.RoleName)).ToList();
                foreach (User user_loopVariable in users)
                {
                    var user = user_loopVariable;
                    foreach (Role role_loopVariable in roles)
                    {
                        var role = role_loopVariable;
                        var userRole = context.Roles.Join(context.Users, x => x.RoleId, x => x.IdGuid, (x, y) => new { x.RoleName, x.RoleId, y.Username, UserId = y.IdGuid })
                                              .Where(x => x.RoleId == role.RoleId).SingleOrDefault();
                        if (userRole != null)
                        {
                            context.UsersInRoles.AddObject(new UsersInRoles());
                            context.Roles.AddObject(context.Roles.Where(x => x.RoleId == userRole.RoleId).Single());
                        }
                        if (!user.Roles.Contains(role))
                        {
                            user.Roles.Add(role);
                        }
                    }
                }
                context.SaveChanges();
            }
        }
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
         
            using (var context = createRepository())
            {
                foreach (string usernameLoopVariable in usernames)
                {
                    var username = usernameLoopVariable;
                    string us = username;
                    User user = context.Users.FirstOrDefault(u => u.Username == us);
                    if (user != null)
                    {
                        foreach (string rolename_loopVariable in roleNames)
                        {
                            var rolename = rolename_loopVariable;
                            var rl = rolename;
                            string variable = usernameLoopVariable;
                            var roleName = context.Roles.Join(context.Users, x => x.RoleId, x => x.IdGuid, (x, y) => new { x.RoleName, y.Username })
                                         .Where(x => x.Username == variable);
                            Role role = user.Roles.FirstOrDefault(r => r.RoleName == rl);
                            if (role != null)
                            {
                                user.Roles.Remove(role);
                            }
                        }
                    }
                }
                context.SaveChanges();
            }
           
        }
        private ArgumentException CreateArgumentNullOrEmptyException(string paramName)
        {
            return new ArgumentException(string.Format("Argument cannot be null or empty: {0}", paramName));
        }
    }
    */
}
