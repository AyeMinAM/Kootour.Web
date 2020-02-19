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
    public partial class UserWish
    {

        #region Constructors
        public UserWish()
        {
        }
        public UserWish(IDataRecord record)
		{
			FieldLoader loader = new FieldLoader(record);
			
			this.ID=loader.LoadInt32("ID");
			this.UserID=loader.LoadInt32("UserID");
			this.TourID=loader.LoadInt32("TourID");
			this.EnterTime=loader.LoadDateTime("EnterTime");
			this.ModifyTime=loader.LoadDateTime("ModifyTime");
			this.TourName=loader.LoadString("TourName");
			this.SugRetailPrice=loader.LoadFloat("SugRetailPrice");
			this.NowPrice=loader.LoadFloat("NowPrice");
			this.TourImgPath=loader.LoadString("TourImgPath");
			this.BookingType=loader.LoadByte("BookingType");
		}
        #endregion

    }

}
