using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Collections;
using HtmlAgilityPack;

namespace MVCSite.Common
{
    public enum RealLocationType
    {
        World = 1,
        Country = 2,
        Region = 3,
        State = 4,
        Province = 5,
        City = 6,
        Other = 7,
        BizCircle = 8,
        County = 9,

    }
    public class LocationRelation
    {
        public string ParentNameCn;
        public string NameCn;
        public RealLocationType Type;
        public string NameEn;
        public int LocationId;
    }

    public static class LocationHelper
    {
        public static List<LocationRelation> GetConflictedNameCnLocation()
        {
            List<LocationRelation> locations = new List<LocationRelation>() { 
                new LocationRelation{
                    NameCn="安阳",
                    LocationId= 490
                },
                new LocationRelation{
                    NameCn="鞍山",
                    LocationId=724
                },
                new LocationRelation{
                    NameCn="朝阳",
                    LocationId= 728
                },
                new LocationRelation{
                    NameCn="大连",
                    LocationId=  729
                },
                new LocationRelation{
                    NameCn="大同",
                    LocationId= 857
                },
                new LocationRelation{
                    NameCn="丹东",
                    LocationId= 730
                },
                new LocationRelation{
                    NameCn="抚顺",
                    LocationId= 735
                },
                new LocationRelation{
                    NameCn="汉中",
                    LocationId=796 
                },
                new LocationRelation{
                    NameCn="淮安",
                    LocationId=636 
                },
                new LocationRelation{
                    NameCn="黄石",
                    LocationId=573 
                },
                new LocationRelation{
                    NameCn="金华",
                    LocationId=984 
                },
                new LocationRelation{
                    NameCn="九江",
                    LocationId=680 
                },
                new LocationRelation{
                    NameCn="临沂",
                    LocationId= 21472
                },
                new LocationRelation{
                    NameCn="马鞍山",
                    LocationId= 313
                },
                new LocationRelation{
                    NameCn="南阳",
                    LocationId= 505
                },
                new LocationRelation{
                    NameCn="上海",
                    LocationId=292 
                },
                new LocationRelation{
                    NameCn="潍坊",
                    LocationId= 839
                },
                new LocationRelation{
                    NameCn="延吉",
                    LocationId=719 
                },
                new LocationRelation{
                    NameCn="阳江",
                    LocationId= 403
                },
                new LocationRelation{
                    NameCn="银川",
                    LocationId=777 
                },
                new LocationRelation{
                    NameCn="榆林",
                    LocationId=21473 
                },
                new LocationRelation{
                    NameCn="岳阳",
                    LocationId= 623
                },
                new LocationRelation{
                    NameCn="中山",
                    LocationId= 409
                },
                new LocationRelation{
                    NameCn="资阳",
                    LocationId= 916
                }
            };
            return locations;
        }

    }
}