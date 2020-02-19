using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MVCSite.DAC.Repositories
{
    public partial class EFDataContext
    {
        private static readonly object locker = new object();
        public EFDataContext(string connectionString, string stub)
            : base(CreateEFConnectionString(connectionString, "Repositories.EFDataContext")) {
                lock (locker)
                {
                    //this.ChangeTracker.DetectChanges();
                    this.Database.Initialize(force: true);
                }         
        }

        protected static string CreateEFConnectionString(string connectionString, string edmxPathAndName)
        {
            if (connectionString.Contains("metadata"))
                return connectionString;

            if (connectionString.StartsWith("name="))
            {
                var s = connectionString.Replace("name=", string.Empty).Trim();
                string connectString = ConfigurationManager.ConnectionStrings[s].ConnectionString;
                return CreateEFConnectionString(connectString, edmxPathAndName);
            }

            var efConnection = new System.Data.EntityClient.EntityConnectionStringBuilder
            {
                Metadata = string.Format("res://*/{0}.csdl|res://*/{0}.ssdl|res://*/{0}.msl", edmxPathAndName),
                Provider = "System.Data.SqlClient",
                ProviderConnectionString = connectionString
            };


            return efConnection.ConnectionString;
        }
    }

    public partial class StatDataContext
    {
        public StatDataContext(string connectionString, string stub)
            : base(CreateEFConnectionString(connectionString, "Repositories.StatDataContext")) { }

        protected static string CreateEFConnectionString(string connectionString, string edmxPathAndName)
        {
            if (connectionString.Contains("metadata"))
                return connectionString;

            if (connectionString.StartsWith("name="))
            {
                var s = connectionString.Replace("name=", string.Empty).Trim();
                string connectString = ConfigurationManager.ConnectionStrings[s].ConnectionString;
                return CreateEFConnectionString(connectString, edmxPathAndName);
            }

            var efConnection = new System.Data.EntityClient.EntityConnectionStringBuilder
            {
                Metadata = string.Format("res://*/{0}.csdl|res://*/{0}.ssdl|res://*/{0}.msl", edmxPathAndName),
                Provider = "System.Data.SqlClient",
                ProviderConnectionString = connectionString
            };


            return efConnection.ConnectionString;
        }
    }
    public partial class GuideDataContext
    {
        public GuideDataContext(string connectionString, string stub)
            : base(CreateEFConnectionString(connectionString, "Repositories.GuideDataContext")) { }

        protected static string CreateEFConnectionString(string connectionString, string edmxPathAndName)
        {
            if (connectionString.Contains("metadata"))
                return connectionString;

            if (connectionString.StartsWith("name="))
            {
                var s = connectionString.Replace("name=", string.Empty).Trim();
                string connectString = ConfigurationManager.ConnectionStrings[s].ConnectionString;
                return CreateEFConnectionString(connectString, edmxPathAndName);
            }

            var efConnection = new System.Data.EntityClient.EntityConnectionStringBuilder
            {
                Metadata = string.Format("res://*/{0}.csdl|res://*/{0}.ssdl|res://*/{0}.msl", edmxPathAndName),
                Provider = "System.Data.SqlClient",
                ProviderConnectionString = connectionString
            };


            return efConnection.ConnectionString;
        }
    }

}
