using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Collections;
using HtmlAgilityPack;
using MVCSite.Common;

namespace MVCSite.Common
{
    public static class CommunityHelper
    {
        public static string GetMembersIdFromUserIds(int[] userIds)
        {
            if (userIds == null || userIds.Length <= 0)
                return string.Empty;
            return userIds.OrderBy(x => x).Select(x => x.ToString()).Aggregate((prev, next) => { return prev + ConstantData.SQLSelectInStringSeparator + next; });

        }
        public static int[] GetIntUserIdsFromString(string membersId)
        {
            if (string.IsNullOrEmpty(membersId))
                return null;
            return membersId.SplitToIntArray(ConstantData.SQLSelectInStringSeparator);
        }
    }
}