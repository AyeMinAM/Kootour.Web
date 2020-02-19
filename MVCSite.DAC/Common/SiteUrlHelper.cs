using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MVCSite.DAC.Entities;


namespace MVCSite.DAC.Common
{
    public partial class SiteUrlHelper
    {
        public static string GenerateUserPublicUrl(User user)
        {
            return string.Empty;
        }


        public static string GenerateSingleFavoriteURL(int id, string title, bool isPublic = false)
        {
            var pureTitle = GetPureTitleInUrl(title);
            if (!isPublic)
                return string.Format("/Favorite/{0}/{1}/Private/", pureTitle, id).ToLower();
            else
                return string.Format("/Favorite/{0}/{1}/ListPublic/", pureTitle, id).ToLower();

        }

        public static string GenerateSingleBookURL(int id)
        {
            return string.Format("/Word/OneBook/{0}/", id).ToLower();
        }

        public static string GenerateSingleBoardURL(int id, string title, bool isPublic = false)
        {
            var pureTitle = GetPureTitleInUrl(title);
            if (!isPublic)
                return string.Format("/Board/{0}/{1}/PrivateIndex/", pureTitle, id).ToLower();
            else
                return string.Format("/Board/{0}/{1}/Index/", pureTitle, id).ToLower();

        }

        public static string GetPureTitleInUrl(string title)
        {
            const int maxWordsCountInTitle = 5;
            var valueReg = new Regex(@"\b\w{5,}\b", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var sb = new StringBuilder(100);
            var matchResult = valueReg.Match(title);
            var count = 0;
            while (matchResult.Success)
            {
                count++;
                if (count > maxWordsCountInTitle)
                    break;
                if (count > 1)
                {
                    sb.Append("-");
                    sb.Append(matchResult.Value);
                }
                else
                    sb.Append(matchResult.Value);
                matchResult = matchResult.NextMatch();
            }
            string resultTitle = sb.ToString();
            if (resultTitle.Length < 10)
            {//The title is too short,get the whole title without non-character
                valueReg = new Regex(@"[^\w]+", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                resultTitle = valueReg.Replace(title, "-");
            }
            return resultTitle;
        }

        public static bool IsValidDealURL(string url)
        {
            var valueReg = new Regex(@"^http(s)?://[a-z0-9-]+(\.[a-z0-9-]+)+(:[0-9]+)?(/.*)?$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var matchResult = valueReg.Match(url);
            return matchResult.Success;
        }
        //public static string GetSourceSiteName(string originalUrl)
        //{
        //    var caReg = new Regex(@"\.ab\.ca|\.bc\.ca|\.mb\.ca|\.nb\.ca|\.nf\.ca|\.nl\.ca|\.ns\.ca|\.nt\.ca|\.nu\.ca|\.on\.ca|\.pe\.ca|\.qc\.ca|\.sk\.ca|\.yk\.ca", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        //    var usReg = new Regex(@"\.ak\.us|\.al\.us|\.ar\.us|\.az\.us|\.ca\.us|\.co\.us|\.ct\.us|\.dc\.us|\.de\.us|\.dni\.us|\.fed\.us|\.fl\.us|\.ga\.us|\.hi\.us|\.ia\.us|\.id\.us|\.il\.us|\.in\.us|\.isa\.us|\.kids\.us|\.ks\.us|\.ky\.us|\.la\.us|\.ma\.us|\.md\.us|\.me\.us|\.mi\.us|\.mn\.us|\.mo\.us|\.ms\.us|\.mt\.us|\.nc\.us|\.nd\.us|\.ne\.us|\.nh\.us|\.nj\.us|\.nm\.us|\.nsn\.us|\.nv\.us|\.ny\.us|\.oh\.us|\.ok\.us|\.or\.us|\.pa\.us|\.ri\.us|\.sc\.us|\.sd\.us|\.tn\.us|\.tx\.us|\.ut\.us|\.vt\.us|\.va\.us|\.wa\.us|\.wi\.us|\.wv\.us|\.wy\.us", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        //    var ukReg = new Regex(@"\.ac\.uk|\.co\.uk|\.gov\.uk|\.ltd\.uk|\.me\.uk|\.mil\.uk|\.mod\.uk|\.net\.uk|\.nic\.uk|\.nhs\.uk|\.org\.uk|\.plc\.uk|\.police\.uk|\.sch\.uk|\.bl\.uk|\.british-library\.uk|\.icnet\.uk|\.jet\.uk|\.nel\.uk|\.nls\.uk|\.national-library-scotland\.uk|\.parliament\.uk|\.sch\.uk", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        //    var name = string.IsNullOrEmpty(originalUrl) ? string.Empty : new Uri(originalUrl).Host;
        //    if (string.IsNullOrEmpty(name))
        //        return string.Empty;
        //    string[] nameArray = name.Split('.');
        //    if (nameArray.Length == 2)
        //    {
        //        name = nameArray[0];
        //    }
        //    else if (nameArray.Length >= 3)
        //    {
        //        if (caReg.Match(name).Success || usReg.Match(name).Success || ukReg.Match(name).Success)
        //        {
        //            name = nameArray[nameArray.Length - 3];
        //        }
        //        else
        //        {
        //            name = nameArray[nameArray.Length - 2];
        //        }
        //    }
        //    //name = StringHelper.UppercaseFirst(name);
        //    name = name.Trim();
        //    return name;
        //}

        //public static string GetSourceSiteHost(string originalUrl)
        //{
        //    return string.IsNullOrEmpty(originalUrl) ? string.Empty : new Uri(originalUrl).Host;
        //}

    }
}
