using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using MVCSite.DAC.Entities;
using WMath.Facilities;
using MVCSite.DAC.Interfaces;
using MVCSite.Common;

namespace MVCSite.DAC.Instrumentation
{
    public class Tracker
    {
        private readonly IRepositoryStats repositoryStats;
        public Tracker(IRepositoryStats _repositoryStats)
        {
            repositoryStats = _repositoryStats;
        }
        public void     OnSessionStart()
        {
            EnsurePermanentCookiePlaced();

            var Session = HttpContext.Current.Session;
            var Request = HttpContext.Current.Request;
            var Response = HttpContext.Current.Response;


            try
            {
                string Referer = "";
                try
                {
                    Session["startpage"] = Request.ServerVariables["SCRIPT_NAME"].ToString();
                    if (Request.ServerVariables["HTTP_USER_AGENT"] != null)
                    {
                        if (Request.ServerVariables.Get("HTTP_USER_AGENT").ToLower().IndexOf("url control") > 0)
                        {
                            Response.End();
                        }
                        if (Request.ServerVariables.Get("HTTP_USER_AGENT").ToLower().IndexOf("quid") > 0)
                        {
                            Response.End();
                        }
                    }
                    else
                    {
                        Referer = "ignor";
                    }
                    if (Request.ServerVariables.Get("PATH_INFO").ToString().ToLower() == "/inbox.aspx")
                    {
                        Referer = "ignor";
                    }
                    if (Request.ServerVariables.Get("HTTP_referer") == null)
                    {
                        Referer = "ignor";
                    }
                    else
                    {
                        if ((Request.ServerVariables.Get("HTTP_referer").StartsWith("http://www.51math.com")) ||
                            (Request.ServerVariables.Get("HTTP_referer").StartsWith("http://sxj.51math.com")))
                        {
                            Referer = "ignor";
                        }
                        else
                        {
                            Referer = Request.ServerVariables.Get("HTTP_referer");
                            if (Referer.Length > 550)
                            {
                                Referer = Referer.Substring(0, 549);
                            }
                            if (Referer.IndexOf("mail.") > 0 && Request["dating"] == null)
                            {
                                Referer = "ignor";
                            }
                        }
                    }
                    if (Request["dating"] != null)
                    {
                        try
                        {
                            Referer = Request.ServerVariables.Get("HTTP_referer");
                        }
                        catch
                        {
                            Referer = "unknown";
                        }
                    }
                    if (Request["_from"] != null)
                    {
                        try
                        {
                            Referer = Request.ServerVariables.Get("HTTP_referer");
                        }
                        catch
                        {
                            Referer = "unknown";
                        }
                    }
                }

                catch
                {
                }

                if (Request.ServerVariables.Get("PATH_INFO").Contains("lander_group") &&
                    ((Referer == null) || (Referer == "ignor")))
                {
                    Referer = Request.ServerVariables.Get("HTTP_referer");
                    if (Referer == null)
                    {
                        Referer = "affiliate/direct";
                    }
                    else
                    {
                        if (Referer.Length > 550)
                        {
                            Referer = Referer.Substring(0, 549);
                        }
                    }
                }
                ///

                //if (Referer != "ignor")
                {
                    string pagevar = "";
                    try
                    {
                        if (Request.QueryString.Get("dating") != null)
                        {
                            pagevar = Request.QueryString.Get("dating");
                        }
                        else if (Request.QueryString.Get("_from") != null)
                        {
                            pagevar = Request.QueryString.Get("_from");
                        }
                        else
                        {
                            if (Request.QueryString.Get("toolbar") != null)
                            {
                                if (Request.QueryString.Get("toolbar") == "1")
                                {
                                    pagevar = "toolbar";
                                }
                                if (Request.QueryString.Get("toolbar") == "2")
                                {
                                    pagevar = "pop";
                                }
                                if (Request.QueryString.Get("toolbar") == "3")
                                {
                                    pagevar = "toobar3";
                                }
                            }
                        }
                        if (Request.QueryString.Get("profile_id") != null)
                        {
                            pagevar = Request.QueryString.Get("profile_id");
                        }
                        if (Request.QueryString.Get("city_id") != null && Request.QueryString.Get("profile_id") == null)
                        {
                            pagevar = Request.QueryString.Get("city_id");
                        }

                        string TempAgent = "";
                        if (Request.ServerVariables.Get("HTTP_USER_AGENT").Length > 100) 
                        {
                            TempAgent = Request.ServerVariables.Get("HTTP_USER_AGENT").ToString().Replace("'", "''").Substring(0, 80);
                        }
                        else
                        {
                            TempAgent = Request.ServerVariables.Get("HTTP_USER_AGENT").ToString().Replace("'", "''");
                        }
                        Visits visits = new Visits();
                        visits.visitdate = DateTime.UtcNow;
                        visits.firstvisit = DateTime.UtcNow;
                        visits.Session_id=Session.SessionID;
                        visits.IP=Request.ServerVariables.Get("REMOTE_ADDR");                        
                        visits.browser=TempAgent;
                        visits.path=Request.ServerVariables.Get("PATH_INFO") + "?" +
                                        Request.ServerVariables.Get("QUERY_STRING");
                        visits.referer=Referer;
                        visits.Pagevar=pagevar;
                        if (Request["kw"] != null)
                        {
                            visits.keyword= Request["kw"].ToString();
                        }
                        try
                        {
                            HttpCookie cookieC;
                            cookieC = Request.Cookies["tmp_track"];
                            if (cookieC?.Value != null && cookieC.Value.ToString() != "")
                            {
                                //dp.AddParameter("@tmp_track", cookieC.Value.ToString(), SqlDbType.Int);
                            }
                        }
                        catch
                        {
                        }

                        try
                        {
                            HttpCookie cookiedd = HttpContext.Current.Request.Cookies["ft"];
                            if (cookiedd != null)
                            {
                                System.DateTime firstvisit;
                                firstvisit = System.DateTime.Parse(cookiedd?.Value.ToString());
                                visits.firstvisit = firstvisit;
                            }
                        }
                        catch
                        {
                        }
                        try
                        {
                            HttpCookie cookieD;
                            cookieD = Request.Cookies["my_ipcountry"];
                            if (cookieD?.Value != null && cookieD?.Value.ToString() != "")
                            {
                                visits.ipcountry_id = SafeConvert.ToInt32(cookieD.Value.ToString());
                            }
                        }
                        catch
                        {
                        }
                        repositoryStats.CreateVisits(visits);
                    }
                    catch(Exception exp)
                    {
                        
                    }
                }
                //if (Request.ServerVariables["HTTP_USER_AGENT"] != null &&
                //    Request.ServerVariables.Get("HTTP_USER_AGENT").IndexOf("bot.htm") > 0 ||
                //    Request.ServerVariables.Get("HTTP_USER_AGENT").IndexOf("yahoo.com") > 0 ||
                //    Request.ServerVariables.Get("HTTP_USER_AGENT").IndexOf("ask.com") > 0)
                //{
                //    Session.Abandon();
                //}
            }
            catch
            {

            }
        }
        private void    EnsurePermanentCookiePlaced()
        {
            try
            {
                HttpCookie cookiedd = HttpContext.Current.Request.Cookies["ft"];
                if (cookiedd == null)
                {
                    HttpCookie cookie;
                    cookie = new HttpCookie("ft");
                    string currentDate1 = DateTime.UtcNow.ToString("F");
                    cookie.Value = currentDate1;
                    DateTime dtNow = DateTime.UtcNow;
                    TimeSpan tsMinute = new TimeSpan(10000, 0, 0, 0);
                    cookie.Expires = dtNow + tsMinute;
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
            }
            catch
            {

            }
        }
    }
}
