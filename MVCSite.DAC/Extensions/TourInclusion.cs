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

    public partial class TourInclusion
    {
         #region Constructors
        public TourInclusion()
        {
            this.ID = 0;
            this.TourID = 0;
            this.Name = string.Empty;
            this.SortNo = 0;
            this.EnterTime = DateTime.UtcNow;
            this.ModifyTime = DateTime.UtcNow; 
        }
        public TourInclusion(TourInclusion src)
        {
            this.ID = src.ID;
            this.TourID = src.TourID;
            this.Name = src.Name;
            this.SortNo = src.SortNo;
            this.EnterTime = src.EnterTime;
            this.ModifyTime = src.ModifyTime;
        }
        /// <summary>
        /// Constructor using IDataRecord.
        /// </summary>
        public TourInclusion(IDataRecord record)
        {
            FieldLoader loader = new FieldLoader(record);

            this.ID = loader.LoadInt32("ID");
            this.TourID = loader.LoadInt32("TourID");
            this.Name = loader.LoadString("Name");
            this.SortNo = loader.LoadByte("SortNo");
            this.EnterTime = loader.LoadDateTime("EnterTime");
            this.ModifyTime = loader.LoadDateTime("ModifyTime");
        }

        #endregion
    }
}
