using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCSite.Web.ViewModels
{
    public class AddMembersModel
    {
        public string BoardTitle { get; set; }
        public IEnumerable<MemberModel> MemberList { get; set; }
        public InviteMemberModel InviteMember { get; set; }

        //public IEnumerable<MemberModel> MemberList { get; set; }
    }


}