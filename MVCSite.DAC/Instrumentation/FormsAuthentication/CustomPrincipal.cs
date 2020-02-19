using System.Security.Principal;
using System.Web.Security;

namespace MVCSite.DAC.Instrumentation
{
    public class CustomPrincipal : GenericPrincipal
    {
        public CustomPrincipal(GenericIdentity genericIdentity, FormsTicketDataV1 userData)
            : base(genericIdentity, new[] { "User" })
        {
            UserData = userData;
        }

        public FormsTicketDataV1 UserData { get; set; }
    }
}