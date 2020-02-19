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

    public partial class TourPriceBreakdown
    {
        #region Constructors
        public TourPriceBreakdown()
        {
            this.ID = 0;
            this.TourID = 0;
            this.EndPoint1 = 0;
            this.EndPoint2 = 0;
            this.DiscountValue = 0;
            this.DiscountPercent = 0;
            this.SortNo = 0;
            this.BeginDate = DateTime.UtcNow;
            this.EndDate = DateTime.UtcNow;
            this.DateRange = string.Empty;
            this.EnterTime = DateTime.UtcNow;
            this.ModifyTime = DateTime.UtcNow; 
            
        }
        public TourPriceBreakdown(TourExtra src)
        {
            this.ID = 0;
            this.TourID = 0;
            this.EndPoint1 = 0;
            this.EndPoint2 = 0;
            this.DiscountValue = 0;
            this.DiscountPercent = 0;
            this.SortNo = 0;
            this.BeginDate = DateTime.UtcNow;
            this.EndDate = DateTime.UtcNow;
            this.DateRange = string.Empty;
            this.EnterTime = DateTime.UtcNow;
            this.ModifyTime = DateTime.UtcNow;

        }
        /// <summary>
        /// Constructor using IDataRecord.
        /// </summary>
        public TourPriceBreakdown(IDataRecord record)
        {
            FieldLoader loader = new FieldLoader(record);

            this.ID = loader.LoadInt32("ID"); 
            this.TourID = loader.LoadInt32("TourID");
            this.EndPoint1 = loader.LoadInt32("EndPoint1");
            this.EndPoint2 = loader.LoadInt32("EndPoint2");
            this.DiscountValue = loader.LoadInt32("DiscountValue");
            this.DiscountPercent = loader.LoadDouble("DiscountPercent");
            this.SortNo = loader.LoadByte("SortNo");
            this.BeginDate = loader.LoadDateTime("BeginDate");
            this.EndDate = loader.LoadDateTime("EndDate");
            this.DateRange = loader.LoadString("DateRange");
            this.EnterTime = loader.LoadDateTime("EnterTime");
            this.ModifyTime = loader.LoadDateTime("ModifyTime");
        }

        #endregion
    }
}
