using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCSite.DAC.Entities;
using System.ComponentModel.DataAnnotations;
using MVCSite.ViewResource;
namespace MVCSite.Web.ViewModels
{
    public class DashBoardMsg
    {
        public int FromUserId { get; set; } 
        public string UserAvatarUrl { get; set; }
        public string UserName { get; set; }        
        public string Msg { get; set; }
        public string Date { get; set; }
        
    }
    public class DashBoardModel : Layout
    {
        public int MessageCount { get; set; }
        public List<DashBoardMsg> Msgs { get; set; }
        public int NextPageNo { get; set; }
    }

}