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

    public partial class Promo
    {
        #region Constructors
        public Promo()
        {
            this.ID = 0;
            this.PromoName = string.Empty;
            this.Code = string.Empty;
            this.PromoValue = 0;
            this.PromoPercent = 0;
            this.OpenToUse = true;
            this.MinTouristsToUse = 0;
            this.MinValueToUse = 0;
            this.BeginDate = DateTime.UtcNow;
            this.EndDate = DateTime.UtcNow;
            //this.DateRange = string.Empty;
            this.GuideID = 0;
            this.TourID = 0;
            this.EnterTime = DateTime.UtcNow;
            this.ModifyTime = DateTime.UtcNow;
        }
        public Promo(Promo src)
        {
            this.ID = src.ID;
            this.PromoName = src.PromoName;
            this.Code = src.Code;
            this.PromoValue = src.PromoValue;
            this.PromoPercent = src.PromoPercent;
            this.OpenToUse = src.OpenToUse;
            this.MinTouristsToUse = src.MinTouristsToUse;
            this.MinValueToUse = src.MinValueToUse;
            this.BeginDate = src.BeginDate;
            this.EndDate = src.EndDate;
            //this.DateRange = src.DateRange;
            this.GuideID = src.GuideID;
            this.TourID = src.TourID;
            this.EnterTime = src.EnterTime;
            this.ModifyTime = src.ModifyTime;
        }
        /// <summary>
        /// Constructor using IDataRecord.
        /// </summary>
        public Promo(IDataRecord record)
        {
            FieldLoader loader = new FieldLoader(record);
            this.ID = loader.LoadInt32("ID");
            this.PromoName = loader.LoadString("PromoName");
            this.Code = loader.LoadString("Code");
            this.PromoValue = loader.LoadInt32("PromoValue");
            this.PromoPercent = loader.LoadFloat("PromoPercent");
            this.OpenToUse = loader.LoadBoolean("OpenToUse");
            this.MinTouristsToUse = loader.LoadInt32("MinTouristsToUse");
            this.MinValueToUse = loader.LoadInt32("MinValueToUse");
            this.BeginDate = loader.LoadDateTime("BeginDate");
            this.EndDate = loader.LoadDateTime("EndDate");
            //this.DateRange = loader.LoadString("DateRange");
            this.GuideID = loader.LoadInt32("GuideID");
            this.TourID = loader.LoadInt32("TourID");
            this.EnterTime = loader.LoadDateTime("EnterTime");
            this.ModifyTime = loader.LoadDateTime("ModifyTime");
        }

        #endregion
    }
}
