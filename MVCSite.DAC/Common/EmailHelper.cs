using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MVCSite.DAC.Common
{
    public enum SendEmailType
    { 
        EmailConfirmation=1,
        ForgotPassword,
        InviteFriend,
        Promotion,
    }
    public enum SendEmailSite
    {
        Vjiaoshi = 1,
        _51Math,

    }
    public static class EmailHelper
    {
        public static bool IsEmailAddressValid(string email)
        {
            if (email == null)
                return false;
            const string pattern = @"^([\w\!\#$\%\&\'\*\+\-\/\=\?\^\`{\|\}\~]+\.)*[\w\!\#$\%\&\'\*\+\-\/\=\?\^\`{\|\}\~]+@((((([a-zA-Z0-9]{1}[a-zA-Z0-9\-]{0,62}[a-zA-Z0-9]{1})|[a-zA-Z])\.)+[a-zA-Z]{2,6})|(\d{1,3}\.){3}\d{1,3}(\:\d{1,5})?)$";
            var match = Regex.Match(email.Trim(), pattern, RegexOptions.IgnoreCase);
            return match.Success;
        }
    }
}
