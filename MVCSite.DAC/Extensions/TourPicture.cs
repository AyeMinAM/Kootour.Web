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

    public partial class TourPicture
    {
        #region Constructors
        public TourPicture()
        {
            this.ID = 0;
            this.TourID= 0;
            this.RelativePath = string.Empty;
            this.SortNo = 0;
            this.EnterTime = DateTime.UtcNow;
            this.ModifyTime = DateTime.UtcNow;
        }
        public TourPicture(TourPicture src)
        {
            this.ID = src.ID;
            this.TourID = src.TourID;
            this.RelativePath = src.RelativePath;
            this.SortNo = src.SortNo;
            this.EnterTime = src.EnterTime;
            this.ModifyTime = src.ModifyTime;
        }
        /// <summary>
        /// Constructor using IDataRecord.
        /// </summary>
        public TourPicture(IDataRecord record)
        {
            FieldLoader loader = new FieldLoader(record);

            this.ID = loader.LoadInt32("ID");
            this.TourID = loader.LoadInt32("TourID");
            this.RelativePath = loader.LoadString("RelativePath");
            this.SortNo = loader.LoadByte("SortNo");
            this.EnterTime = loader.LoadDateTime("EnterTime");
            this.ModifyTime = loader.LoadDateTime("ModifyTime");
        }

        #endregion
    }
}
