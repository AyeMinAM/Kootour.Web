using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Web.Mvc;
using System.IO;
using MVCSite.DAC.Common;
using System.Data;
using MVCSite.Common;

namespace MVCSite.DAC.Entities
{

    public partial class BankInfo
    {
        #region Constructors
        public BankInfo()
        {
            this.ID = 0;
            this.Name = string.Empty;
            this.Branch = string.Empty;
            this.AccountNo = string.Empty;
            this.AccountType = string.Empty;
            this.AccountOwner = string.Empty;
            this.EnterTime = DateTime.UtcNow;
            this.ModifyTime = DateTime.UtcNow;
        }
        public BankInfo(BankInfo src)
        {
            this.ID = src.ID;
            this.Name = src.Name;
            this.Branch = src.Branch;
            this.AccountNo = src.AccountNo;
            this.AccountType = src.AccountType;
            this.AccountOwner = src.AccountOwner;
            this.EnterTime = src.EnterTime;
            this.ModifyTime = src.ModifyTime;
        }
        /// <summary>
        /// Constructor using IDataRecord.
        /// </summary>
        public BankInfo(IDataRecord record)
        {
            FieldLoader loader = new FieldLoader(record);

            this.ID = loader.LoadInt32("ID");
            this.Name = loader.LoadString("Name");
            this.Branch = loader.LoadString("Branch");
            this.AccountNo = loader.LoadString("AccountNo");
            this.AccountType = loader.LoadString("AccountType");
            this.AccountOwner = loader.LoadString("AccountOwner");
            this.EnterTime = loader.LoadDateTime("EnterTime");
            this.ModifyTime = loader.LoadDateTime("ModifyTime");
        }

        #endregion
    }
}
