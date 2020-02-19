using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MVCSite.Common
{
    public enum WebSiteID
    { 
        VJiaoshi=0,
        Tanchang8=1,
    };
    public partial class SiteHelper
    {

        public static string GetSourceSiteName(string originalUrl)
        {
            var caReg = new Regex(@"\.ab\.ca|\.bc\.ca|\.mb\.ca|\.nb\.ca|\.nf\.ca|\.nl\.ca|\.ns\.ca|\.nt\.ca|\.nu\.ca|\.on\.ca|\.pe\.ca|\.qc\.ca|\.sk\.ca|\.yk\.ca", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var usReg = new Regex(@"\.ak\.us|\.al\.us|\.ar\.us|\.az\.us|\.ca\.us|\.co\.us|\.ct\.us|\.dc\.us|\.de\.us|\.dni\.us|\.fed\.us|\.fl\.us|\.ga\.us|\.hi\.us|\.ia\.us|\.id\.us|\.il\.us|\.in\.us|\.isa\.us|\.kids\.us|\.ks\.us|\.ky\.us|\.la\.us|\.ma\.us|\.md\.us|\.me\.us|\.mi\.us|\.mn\.us|\.mo\.us|\.ms\.us|\.mt\.us|\.nc\.us|\.nd\.us|\.ne\.us|\.nh\.us|\.nj\.us|\.nm\.us|\.nsn\.us|\.nv\.us|\.ny\.us|\.oh\.us|\.ok\.us|\.or\.us|\.pa\.us|\.ri\.us|\.sc\.us|\.sd\.us|\.tn\.us|\.tx\.us|\.ut\.us|\.vt\.us|\.va\.us|\.wa\.us|\.wi\.us|\.wv\.us|\.wy\.us", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var ukReg = new Regex(@"\.ac\.uk|\.co\.uk|\.gov\.uk|\.ltd\.uk|\.me\.uk|\.mil\.uk|\.mod\.uk|\.net\.uk|\.nic\.uk|\.nhs\.uk|\.org\.uk|\.plc\.uk|\.police\.uk|\.sch\.uk|\.bl\.uk|\.british-library\.uk|\.icnet\.uk|\.jet\.uk|\.nel\.uk|\.nls\.uk|\.national-library-scotland\.uk|\.parliament\.uk|\.sch\.uk", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var name = string.IsNullOrEmpty(originalUrl) ? string.Empty : new Uri(originalUrl).Host;
            if (string.IsNullOrEmpty(name))
                return string.Empty;
            string[] nameArray = name.Split('.');
            if (nameArray.Length == 2)
            {
                name = nameArray[0];
            }
            else if (nameArray.Length >= 3)
            {
                if (caReg.Match(name).Success || usReg.Match(name).Success || ukReg.Match(name).Success)
                {
                    name = nameArray[nameArray.Length - 3];
                }
                else
                {
                    name = nameArray[nameArray.Length - 2];
                }
            }
            //name = StringHelper.UppercaseFirst(name);
            name = name.Trim();
            return name;
        }

        public static string GetSourceSiteHost(string originalUrl)
        {
            return string.IsNullOrEmpty(originalUrl) ? string.Empty : new Uri(originalUrl).Host;
        }

    }
}
