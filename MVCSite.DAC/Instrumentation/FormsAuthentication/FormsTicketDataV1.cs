using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCSite.DAC.Instrumentation
{
    public class FormsTicketDataV1
    {
        public int UserId;
        public int UserRole;
        public string Initials;
        public string FullName;
    }
    public class FormsTicketData
    {
        public Guid UserId;
        public int UserRole;
        public string Initials;
        public string FullName;
    }
}
