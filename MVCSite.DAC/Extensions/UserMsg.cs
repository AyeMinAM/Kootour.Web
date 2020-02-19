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
    public enum UserMsgStatus
    {
        Initial = 0,
        Read = 1,
        All=99,
    }
    public partial class UserMsg
    {

        #region Constructors
        public UserMsg()
        {
        }
        public UserMsg(IDataRecord record)
		{
			FieldLoader loader = new FieldLoader(record);

            this.ID = loader.LoadInt32("ID");
            this.FromUserID = loader.LoadInt32("FromUserID");
            this.ToUserID = loader.LoadInt32("ToUserID");
            this.Message = loader.LoadString("Message");
            this.EnterTime = loader.LoadDateTime("EnterTime");
            this.ModifyTime = loader.LoadDateTime("ModifyTime");
            this.Status = loader.LoadByte("Status");
		}
        #endregion

    }

}
