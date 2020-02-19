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

    public partial class GuiderExcludedDates
    {
        #region Constructors
        public GuiderExcludedDates()
        {
            this.ID = 0;
            this.UserID = 0;
            this.Date = string.Empty;
            this.Status = 0;
            this.EnterTime = DateTime.UtcNow;
            this.ModifyTime = DateTime.UtcNow; 
            
        }
        public GuiderExcludedDates(GuiderExcludedDates src)
        {
            this.ID = 0;
            this.UserID = 0;
            this.Date = string.Empty;
            this.Status = 0;
            this.EnterTime = DateTime.UtcNow;
            this.ModifyTime = DateTime.UtcNow;

        }
        /// <summary>
        /// Constructor using IDataRecord.
        /// </summary>
        public GuiderExcludedDates(IDataRecord record)
        {
            FieldLoader loader = new FieldLoader(record);

            this.ID = loader.LoadInt32("ID"); 
            this.UserID = loader.LoadInt32("UserID");
            this.Date = loader.LoadString("Date");
            this.Status = loader.LoadByte("Status");
            this.EnterTime = loader.LoadDateTime("EnterTime");
            this.ModifyTime = loader.LoadDateTime("ModifyTime");
        }

        #endregion
    }
}
