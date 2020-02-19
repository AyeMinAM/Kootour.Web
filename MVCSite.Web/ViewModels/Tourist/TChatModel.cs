using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCSite.DAC.Entities;
using System.ComponentModel.DataAnnotations;
using MVCSite.ViewResource;
using MVCSite.Common;
namespace MVCSite.Web.ViewModels
{
    public class TChatMsg
    {
        public string Date { get; set; }
        public string Msg { get; set; }
        public bool IsFromMe { get; set; }
        public string PeerUserAvatarUrl { get; set; }
    }
    public class TChatModel : Layout
    {
        public int PeerID { get; set; }
        public string PeerAvatarUrl { get; set; }        
        public string PeerUserName { get; set; }
        public int PeerScore { get; set; }
        public int PeerReviewCount { get; set; }
        public string PeerLocation { get; set; }
        public string PeerRegDate { get; set; }
        public UserRole PeerRole { get; set; }
        public List<TChatMsg> Msgs { get; set; }
        public string UserAvatarUrl { get; set; }

    }
    public class TChatListModel
    {
        public List<IGrouping<string, TChatMsg>> Msgs { get; set; }
    }
}