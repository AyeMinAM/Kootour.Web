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
    public partial class UserTourReview
    {

        #region Constructors
        public UserTourReview()
        {
        }
        /// <summary>
        /// Constructor using IDataRecord.
        /// </summary>
        public UserTourReview(IDataRecord record)
        {
            FieldLoader loader = new FieldLoader(record);

            this.ID = loader.LoadInt32("ID");
            this.UserID = loader.LoadInt32("UserID");
            this.TourID = loader.LoadInt32("TourID");
            this.Accuracy = loader.LoadByte("Accuracy");
            this.Communication = loader.LoadByte("Communication");
            this.Services = loader.LoadByte("Services");
            this.Knowledge = loader.LoadByte("Knowledge");
            this.Value = loader.LoadByte("Value");
            this.Comment = loader.LoadString("Comment");
            this.EnterTime = loader.LoadDateTime("EnterTime");
            this.ModifyTime = loader.LoadDateTime("ModifyTime");
            this.AverageScore = loader.LoadDouble("AverageScore");
        }
        #endregion
        public string UserName
        {
            get;
            set;
        }
        public string UserAvatarUrl
        {
            get;
            set;
        }

    }

}
