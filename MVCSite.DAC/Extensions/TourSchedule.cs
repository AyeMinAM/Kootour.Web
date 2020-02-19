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

    public partial class TourSchedule
    {
        #region Constructors
        public TourSchedule()
        {
            this.ID = 0;
            this.TourID = 0;
            this.Name = string.Empty;
            this.BeginDate = DateTime.UtcNow;
            this.EndDate = DateTime.UtcNow;
            this.DateRange = string.Empty;
            this.IsMonday = false;
            this.IsTuesday = false;
            this.IsWednesday = false;
            this.IsThursday = false;
            this.IsFriday = false;
            this.IsSaturday = false;
            this.IsSunday = false;

            this.DiscountTourists = 0;
            this.DiscountValue = 0;
            this.DiscountPercent = 0;

            this.StartTime1 = string.Empty;
            this.StartTime2 = string.Empty;
            this.StartTime3 = string.Empty;
            this.StartTime4 = string.Empty;
            this.StartTime5 = string.Empty;
            this.StartTime6 = string.Empty;

            this.NetPrice = 0;
            this.SugRetailPrice = 0;
            this.CommisionPay = 0;

            this.EnterTime = DateTime.UtcNow;
            this.ModifyTime = DateTime.UtcNow;
        }
        public TourSchedule(TourSchedule src)
        {
            this.ID = src.ID;
            this.TourID = src.TourID;
            this.Name = src.Name;
            this.BeginDate = src.BeginDate;
            this.EndDate = src.EndDate;
            this.DateRange = src.DateRange;
            this.IsMonday = src.IsMonday;
            this.IsTuesday = src.IsTuesday;
            this.IsWednesday = src.IsWednesday;
            this.IsThursday = src.IsThursday;
            this.IsFriday = src.IsFriday;
            this.IsSaturday = src.IsSaturday;
            this.IsSunday = src.IsSunday;

            this.DiscountTourists = src.DiscountTourists;
            this.DiscountValue = src.DiscountValue;
            this.DiscountPercent = src.DiscountPercent;

            this.StartTime1 = src.StartTime1;
            this.StartTime2 = src.StartTime2;
            this.StartTime3 = src.StartTime3;
            this.StartTime4 = src.StartTime4;
            this.StartTime5 = src.StartTime5;
            this.StartTime6 = src.StartTime6;

            this.NetPrice = src.NetPrice;
            this.SugRetailPrice = src.SugRetailPrice;
            this.CommisionPay = src.CommisionPay;

            this.EnterTime = src.EnterTime;
            this.ModifyTime = src.ModifyTime;
        }
        /// <summary>
        /// Constructor using IDataRecord.
        /// </summary>
        public TourSchedule(IDataRecord record)
        {
            FieldLoader loader = new FieldLoader(record);

            this.ID = loader.LoadInt32("ID");
            this.TourID = loader.LoadInt32("TourID");
            this.Name = loader.LoadString("Name");
            this.DateRange = loader.LoadString("DateRange");
            this.BeginDate = loader.LoadDateTime("BeginDate");
            this.EndDate = loader.LoadDateTime("EndDate");
            this.IsMonday = loader.LoadBoolean("IsMonday");
            this.IsTuesday = loader.LoadBoolean("IsTuesday");
            this.IsWednesday = loader.LoadBoolean("IsWednesday");
            this.IsThursday = loader.LoadBoolean("IsThursday");
            this.IsFriday = loader.LoadBoolean("IsFriday");
            this.IsSaturday = loader.LoadBoolean("IsSaturday");
            this.IsSunday = loader.LoadBoolean("IsSunday");

            this.DiscountTourists = loader.LoadInt32("DiscountTourists");
            this.DiscountValue = loader.LoadInt32("DiscountValue");
            this.DiscountPercent = loader.LoadFloat("DiscountPercent");

            this.StartTime1 = loader.LoadString("StartTime1");
            this.StartTime2 = loader.LoadString("StartTime2");
            this.StartTime3 = loader.LoadString("StartTime3");
            this.StartTime4 = loader.LoadString("StartTime4");
            this.StartTime5 = loader.LoadString("StartTime5");
            this.StartTime6 = loader.LoadString("StartTime6");

            this.NetPrice = loader.LoadFloat("NetPrice");
            this.SugRetailPrice = loader.LoadFloat("SugRetailPrice");
            this.CommisionPay = loader.LoadFloat("CommisionPay");

            this.EnterTime = loader.LoadDateTime("EnterTime");
            this.ModifyTime = loader.LoadDateTime("ModifyTime");
        }

        #endregion
    }
}
