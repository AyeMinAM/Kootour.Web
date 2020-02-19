using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MVCSite.Common
{
    public class ConstantDefs
    {

        public const string SessionStateLogonToken = "LogonToken";
        public const string SessionStateQQ = "QQ_state";


    }
    public class PhoneConstants
    {
        public static Regex fullPhoneRegex = new Regex(@"^0(10[23456789]{1}\d{7})|(2\d{1}[23456789]{1}\d{7})|([3-9]{1}\d{2}[23456789]{1}\d{6,7})$",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static Regex mobileRegex = new Regex(@"^1[3578]{1}\d{9}$", //7 added for my private Canada phone.
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static Regex areaRegex = new Regex(@"^0[123456789]{1}\d{1,2}$",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static Regex phoneRegex = new Regex(@"^[23456789]{1}\d{5,7}$",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static Regex free400800Regex = new Regex(@"^[48]{1}00\d{7}$",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static Regex usaCanadaPhoneRegex = new Regex(@"^001[2345678]{1}\d{9}$",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

    }
}
