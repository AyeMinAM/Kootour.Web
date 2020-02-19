using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace MVCSite.Common
{
//    public static class ClientHelper
//    {
//        //private const string jsVersion = "20070411";
//        //private const string jsSystemVersion = "20070411";
//        //private const string cssVersion = "20070411";
//        //private static readonly string TIME_TAG = DateTime.Now.ToString( "yyyyMMddHH" );
////        System.Reflection.Assembly MyAssembly =
////;
////        System.Version AppVersion = MyAssembly.GetName ().Version;
////        string MyVersion = AppVersion.Major.ToString ()
////        + "." + AppVersion.Minor.ToString ()
////        + "." + AppVersion.Build.ToString ()
////        + "." + AppVersion.Revision.ToString ();
//        private static readonly string TIME_TAG = System.Reflection.Assembly.GetExecutingAssembly ().GetName ().Version.ToString();
//        private static readonly string jsVersion = TIME_TAG + ".js";
//        private static readonly string jsSystemVersion = TIME_TAG + ".js";
//        private static readonly string cssVersion = TIME_TAG + ".css";

//        private const string systemJsTemplate = "<script type=\"text/javascript\">document.write('<scr' + 'ipt src=\"{0}\" type=\"text/javascript\"></scr' + 'ipt>');</script>";
//        private const string systemJsOnlyIeTemplate = "<!--[if IE]><script type=\"text/javascript\">document.write('<scr' + 'ipt src=\"{0}\" type=\"text/javascript\"></scr' + 'ipt>');</script><![endif]-->";
//        private const string jsTemplate = "<script type=\"text/javascript\">e-jjj.System.include( \"{0}\" );</script>";

//#if DEBUG
//        private static readonly string jsVersonTemplate = "<script type=\"text/javascript\">var debug = true;var version=\"" + jsVersion + "\";var WWW51MathCookieDomain=\"" + WebConfigurationManager.AppSettings ["WWW51MathCookieDomain"] + "\";</script>";
//#else
//        private static readonly string jsVersonTemplate = "<script type=\"text/javascript\">var debug = false;var version=\"" + jsVersion + "\";var WWW51MathCookieDomain=\"" + WebConfigurationManager.AppSettings ["WWW51MathCookieDomain"] + "\";</script>";
//#endif

//        //private const string cssTemplate = "<script type=\"text/javascript\">document.write('<link {1} href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />');</script>";
//        //private const string cssOnlyIeTemplate = "<!--[if IE]><script type=\"text/javascript\">document.write('<link {1} href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />');</script><![endif]-->";
//        private const string cssTemplate = "<link {1} href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />";
//        private const string cssOnlyIeTemplate = "<!--[if IE]><link {1} href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" /><![endif]-->";


//        /// <summary>
//        /// 装载Js文件
//        /// </summary>
//        /// <param name="scriptFileUrl">Js文件路径</param>
//        /// <returns></returns>
//        public static string LoadJsFile( string scriptFileUrl )
//        {
//            return string.Format ( jsTemplate, AppendJsVersion ( scriptFileUrl ) );
//        }

//        /// <summary>
//        /// 装载Js文件
//        /// </summary>
//        /// <param name="scriptFileUrl">Js文件路径</param>
//        /// <returns></returns>
//        public static string LoadSystemJsFile( string scriptFileUrl )
//        {
//            return LoadSystemJsFile ( scriptFileUrl, false );
//        }

//        public static string RequireJsFile( string scriptFileUrl )
//        {
//            return string.Format ( jsTemplate, AppendJsVersion ( scriptFileUrl ) );
//        }

//        public static string ControlRequireJsFile( string scriptFileUrl )
//        {
//            return string.Format ( jsTemplate, AppendJsVersion ( scriptFileUrl ) );
//        }

//        /// <summary>
//        /// 装载Js文件
//        /// </summary>
//        /// <param name="scriptFileUrl">Js文件路径</param>
//        /// <param name="onlyIE">仅仅在IE下使用</param>
//        /// <returns></returns>
//        public static string LoadSystemJsFile( string scriptFileUrl, bool onlyIE )
//        {
//            scriptFileUrl = AppendJsSystemVersion ( scriptFileUrl );
//            if ( onlyIE )
//            {
//                return string.Format ( systemJsOnlyIeTemplate, scriptFileUrl );
//            }
//            return string.Format ( systemJsTemplate, scriptFileUrl );
//        }

//        private static string AppendJsVersion( string url )
//        {
//            if ( string.IsNullOrEmpty ( url ) )
//                return string.Empty;
//            if ( url.IndexOf ( "?" ) > 0 )
//                return url + "&version=" + jsVersion;
//            return url + "?version=" + jsVersion;
//        }

//        private static string AppendJsSystemVersion( string url )
//        {
//            if ( string.IsNullOrEmpty ( url ) )
//                return string.Empty;
//            if ( url.IndexOf ( "?" ) > 0 )
//                return url + "&version=" + jsSystemVersion;
//            return url + "?version=" + jsSystemVersion;
//        }

//        public static string GetVerson()
//        {
//            return jsVersonTemplate;
//        }


//        /// <summary>
//        /// 装载Css文件
//        /// </summary>
//        /// <param name="styleFileUrl">css文件路径</param>
//        /// <returns></returns>
//        public static string LoadCssFile( string styleFileUrl )
//        {
//            return LoadCssFile ( styleFileUrl, false );
//        }

//        /// <summary>
//        /// 装载Css文件
//        /// </summary>
//        /// <param name="styleFileUrl">Css文件路径</param>
//        /// <param name="onlyIE">仅仅在IE下使用</param>
//        /// <returns></returns>
//        public static string LoadCssFile( string styleFileUrl, bool onlyIE )
//        {
//            return LoadCssFile ( styleFileUrl, string.Empty, onlyIE );
//        }

//        public static string LoadCssFile( string styleFileUrl, string linkId )
//        {
//            return LoadCssFile ( styleFileUrl, linkId, false );
//        }

//        public static string LoadCssFile( string styleFileUrl, string linkId, bool onlyIE )
//        {
//            string linkIdString = string.Empty;
//            if ( !string.IsNullOrEmpty ( linkId ) )
//                linkIdString = "id=\"" + linkId + "\"";
//            if ( onlyIE )
//                return string.Format ( cssOnlyIeTemplate, AppendCssVersion ( styleFileUrl ), linkIdString );
//            return string.Format ( cssTemplate, AppendCssVersion ( styleFileUrl ), linkIdString );
//        }

//        private static string AppendCssVersion( string url )
//        {
//            if ( string.IsNullOrEmpty ( url ) )
//                return string.Empty;
//            if ( url.IndexOf ( "?" ) > 0 )
//                return url + "&version=" + cssVersion;
//            return url + "?version=" + cssVersion;
//        }

//        public static void SetClientCaching( DateTime lastModified )
//        {
//            HttpContext.Current.Response.Cache.SetETag ( lastModified.Ticks.ToString () );
//            HttpContext.Current.Response.Cache.SetLastModified ( lastModified );
//            HttpContext.Current.Response.Cache.SetCacheability ( HttpCacheability.Public );
//            HttpContext.Current.Response.Cache.SetMaxAge ( new TimeSpan ( 1, 0, 0, 0 ) );
//            HttpContext.Current.Response.Cache.SetSlidingExpiration ( true );
//        }

//        public static void SetFileCaching( string fileName )
//        {
//            HttpContext.Current.Response.AddFileDependency ( fileName );
//            HttpContext.Current.Response.Cache.SetETagFromFileDependencies ();
//            HttpContext.Current.Response.Cache.SetLastModifiedFromFileDependencies ();
//            HttpContext.Current.Response.Cache.SetCacheability ( HttpCacheability.Public );
//            HttpContext.Current.Response.Cache.SetMaxAge ( new TimeSpan ( 1, 0, 0, 0 ) );
//            HttpContext.Current.Response.Cache.SetSlidingExpiration ( true );
//        }

//        public static bool IsClientCached( HttpContext context, DateTime contentModified )
//        {
//            string clientCached = context.Request.Headers ["If-Modified-Since"];
//            if ( clientCached != null )
//            {
//                DateTime isModifiedSince;
//                if ( DateTime.TryParse ( clientCached, out isModifiedSince ) )
//                {
//                    return isModifiedSince > contentModified;
//                }
//            }
//            return false;
//        }




//    }
    public static class ClientHelper
    {
        public static readonly string VERSION_TOKEN = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        private static readonly string jsVersion = VERSION_TOKEN + ".js";
        private static readonly string jsSystemVersion = VERSION_TOKEN + ".js";
        private static readonly string cssVersion = VERSION_TOKEN + ".css";

        private const string systemJsTemplate = "<script type=\"text/javascript\">document.write('<scr' + 'ipt src=\"{0}\" type=\"text/javascript\"></scr' + 'ipt>');</script>";
        private const string systemDeferJsTemplate = "<script src=\"{0}\" type=\"text/javascript\" defer=\"defer\"></script>";

        private const string systemJsOnlyIeTemplate = "<!--[if IE]><script type=\"text/javascript\">document.write('<scr' + 'ipt src=\"{0}\" type=\"text/javascript\"></scr' + 'ipt>');</script><![endif]-->";
        private const string systemDeferJsOnlyIeTemplate = "<!--[if IE]><script src=\"{0}\" type=\"text/javascript\" defer=\"defer\"></script><![endif]-->";
        
        private const string jsTemplate = "<script type=\"text/javascript\">WMath.System.include( \"{0}\" );</script>";

#if DEBUG
        private static readonly string jsVersonTemplate = "<script type=\"text/javascript\">var debug = true;var version=\"" + jsVersion + "\";var wmathCookieDomain=\"" + WebConfigurationManager.AppSettings["WMathCookieDomain"] + "\";</script>";
#else
		private static readonly string jsVersonTemplate = "<script type=\"text/javascript\">var debug = false;var version=\"" + jsVersion + "\";var wmathCookieDomain=\"" + WebConfigurationManager.AppSettings ["WMathCookieDomain"] + "\";</script>";
#endif
        private const string cssTemplate = "<link {1} href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />";
        private const string cssOnlyIeTemplate = "<!--[if IE]><link {1} href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" /><![endif]-->";


        /// <summary>
        /// 装载Js文件
        /// </summary>
        /// <param name="scriptFileUrl">Js文件路径</param>
        /// <returns></returns>
        public static string LoadJsFile(string scriptFileUrl)
        {
            return string.Format(jsTemplate, AppendJsVersion(scriptFileUrl));
        }

        /// <summary>
        /// 装载Js文件
        /// </summary>
        /// <param name="scriptFileUrl">Js文件路径</param>
        /// <returns></returns>
        public static string LoadSystemJsFile(string scriptFileUrl)
        {
            return LoadSystemJsFile(scriptFileUrl, false);
        }

        public static string RequireJsFile(string scriptFileUrl)
        {
            return string.Format(jsTemplate, AppendJsVersion(scriptFileUrl));
        }

        public static string ControlRequireJsFile(string scriptFileUrl)
        {
            return string.Format(jsTemplate, AppendJsVersion(scriptFileUrl));
        }

        /// <summary>
        /// 装载Js文件
        /// </summary>
        /// <param name="scriptFileUrl">Js文件路径</param>
        /// <param name="onlyIE">仅仅在IE下使用</param>
        /// <returns></returns>
        public static string LoadSystemJsFile(string scriptFileUrl, bool onlyIE)
        {
            scriptFileUrl = AppendJsSystemVersion(scriptFileUrl);
            if (onlyIE)
            {
                return string.Format(systemJsOnlyIeTemplate, scriptFileUrl);
            }
            return string.Format(systemJsTemplate, scriptFileUrl);
        }


        /// <summary>
        /// 使用defer属性装载Js文件
        /// </summary>
        /// <param name="scriptFileUrl">Js文件路径</param>
        /// <returns></returns>
        public static string LoadSystemDeferJsFile(string scriptFileUrl)
        {
            return LoadSystemDeferJsFile(scriptFileUrl, false);
        }
        /// <summary>
        /// 使用defer属性装载Js文件
        /// </summary>
        /// <param name="scriptFileUrl">Js文件路径</param>
        /// <param name="onlyIE">仅仅在IE下使用</param>
        /// <returns></returns>
        public static string LoadSystemDeferJsFile(string scriptFileUrl, bool onlyIE)
        {
            scriptFileUrl = AppendJsSystemVersion(scriptFileUrl);
            if (onlyIE)
            {
                return string.Format(systemDeferJsOnlyIeTemplate, scriptFileUrl);
            }
            return string.Format(systemDeferJsTemplate, scriptFileUrl);
        }

        public static string AppendJsVersion(string url)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;
            if (url.IndexOf("?") > 0)
                return url + "&version=" + jsVersion;
            return url + "?version=" + jsVersion;
        }

        private static string AppendJsSystemVersion(string url)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;
            if (url.IndexOf("?") > 0)
                return url + "&version=" + jsSystemVersion;
            return url + "?version=" + jsSystemVersion;
        }

        public static string GetVerson()
        {
            return jsVersonTemplate;
        }


        /// <summary>
        /// 装载Css文件
        /// </summary>
        /// <param name="styleFileUrl">css文件路径</param>
        /// <returns></returns>
        public static string LoadCssFile(string styleFileUrl)
        {
            return LoadCssFile(styleFileUrl, false);
        }

        /// <summary>
        /// 装载Css文件
        /// </summary>
        /// <param name="styleFileUrl">Css文件路径</param>
        /// <param name="onlyIE">仅仅在IE下使用</param>
        /// <returns></returns>
        public static string LoadCssFile(string styleFileUrl, bool onlyIE)
        {
            return LoadCssFile(styleFileUrl, string.Empty, onlyIE);
        }

        public static string LoadCssFile(string styleFileUrl, string linkId)
        {
            return LoadCssFile(styleFileUrl, linkId, false);
        }

        public static string LoadCssFile(string styleFileUrl, string linkId, bool onlyIE)
        {
            string linkIdString = string.Empty;
            if (!string.IsNullOrEmpty(linkId))
                linkIdString = "id=\"" + linkId + "\"";
            if (onlyIE)
                return string.Format(cssOnlyIeTemplate, AppendCssVersion(styleFileUrl), linkIdString);
            return string.Format(cssTemplate, AppendCssVersion(styleFileUrl), linkIdString);
        }

        private static string AppendCssVersion(string url)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;
            if (url.IndexOf("?") > 0)
                return url + "&version=" + cssVersion;
            return url + "?version=" + cssVersion;
        }

        public static void SetClientCaching(DateTime lastModified)
        {
            HttpContext.Current.Response.Cache.SetETag(lastModified.Ticks.ToString());
            HttpContext.Current.Response.Cache.SetLastModified(lastModified);
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Public);
            HttpContext.Current.Response.Cache.SetMaxAge(new TimeSpan(1, 0, 0, 0));
            HttpContext.Current.Response.Cache.SetSlidingExpiration(true);
        }

        public static void SetFileCaching(string fileName)
        {
            HttpContext.Current.Response.AddFileDependency(fileName);
            HttpContext.Current.Response.Cache.SetETagFromFileDependencies();
            HttpContext.Current.Response.Cache.SetLastModifiedFromFileDependencies();
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Public);
            HttpContext.Current.Response.Cache.SetMaxAge(new TimeSpan(1, 0, 0, 0));
            HttpContext.Current.Response.Cache.SetSlidingExpiration(true);
        }

        public static bool IsClientCached(HttpContext context, DateTime contentModified)
        {
            string clientCached = context.Request.Headers["If-Modified-Since"];
            if (clientCached != null)
            {
                DateTime isModifiedSince;
                if (DateTime.TryParse(clientCached, out isModifiedSince))
                {
                    return isModifiedSince > contentModified;
                }
            }
            return false;
        }




    }


}
