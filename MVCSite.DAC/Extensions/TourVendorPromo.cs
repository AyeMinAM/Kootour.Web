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

    public partial class TourVendorPromo
    {
        #region Constructors
        public TourVendorPromo()
        {
            this.ID = 0;
            this.PromoName = string.Empty;
            this.PromoValue = 0;
            this.PromoPercent = 0;
            this.SortNo = 0;
            this.DateRange = string.Empty;
            this.BeginDate = DateTime.UtcNow;
            this.EndDate = DateTime.UtcNow;
            this.OpenToUse = true;
            this.MinTouristsToUse = 0;
            this.MinValueToUse = 0;
            this.EnterTime = DateTime.UtcNow;
            this.ModifyTime = DateTime.UtcNow;
            this.TourID = 0;
            this.GuideID = 0;
        }
        public TourVendorPromo(TourVendorPromo src)
        {
            this.ID = src.ID;
            this.PromoName = src.PromoName;
            this.PromoValue = src.PromoValue;
            this.PromoPercent = src.PromoPercent;
            this.SortNo = src.SortNo;
            this.DateRange = src.DateRange;
            this.BeginDate = src.BeginDate;
            this.EndDate = src.EndDate;
            this.OpenToUse = src.OpenToUse;
            this.MinTouristsToUse = src.MinTouristsToUse;
            this.MinValueToUse = src.MinValueToUse;
            this.EnterTime = src.EnterTime;
            this.ModifyTime = src.ModifyTime;
            this.TourID = src.TourID;
            this.GuideID = src.GuideID;
        }
        /// <summary>
        /// Constructor using IDataRecord.
        /// </summary>
        public TourVendorPromo(IDataRecord record)
        {
            FieldLoader loader = new FieldLoader(record);

            this.ID = loader.LoadInt32("ID");
            this.PromoName = loader.LoadString("PromoName");
            this.PromoValue = loader.LoadInt32("PromoValue");
            this.PromoPercent = loader.LoadDouble("PromoPercent");
            this.SortNo = loader.LoadByte("SortNo");
            this.DateRange = loader.LoadString("DateRange");
            this.BeginDate = loader.LoadDateTime("BeginDate");
            this.EndDate = loader.LoadDateTime("EndDate");
            this.OpenToUse = loader.LoadBoolean("OpenToUse");
            this.MinTouristsToUse = loader.LoadInt32("MinTouristsToUse");
            this.MinValueToUse = loader.LoadInt32("MinValueToUse");
            this.EnterTime = loader.LoadDateTime("EnterTime");
            this.ModifyTime = loader.LoadDateTime("ModifyTime");
            this.TourID = loader.LoadInt32("TourID");
            this.GuideID = loader.LoadInt32("GuideID");
        }

        #endregion
    }
}
