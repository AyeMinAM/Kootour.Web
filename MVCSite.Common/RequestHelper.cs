using System;
using System.Collections;
using System.Runtime.InteropServices; 
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Configuration;
using System.Net.Cache;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Linq;
using MVCSite.DAC.Common;


namespace MVCSite.Common
{
    public class DownloadHtmlInfo
    {
        public string Html { get; set; }
        public Uri ResponseUri { get; set; }

    }
	public class RequestHelper
	{
        public static string GetClientIpAddress(HttpRequestBase request)
        {
            try
            {
                var userHostAddress = request.UserHostAddress;

                // Attempt to parse.  If it fails, we catch below and return "0.0.0.0"
                // Could use TryParse instead, but I wanted to catch all exceptions
                IPAddress.Parse(userHostAddress);

                var xForwardedFor = request.ServerVariables["X_FORWARDED_FOR"];

                if (string.IsNullOrEmpty(xForwardedFor))
                    return userHostAddress;

                // Get a list of public ip addresses in the X_FORWARDED_FOR variable
                var publicForwardingIps = xForwardedFor.Split(',').Where(ip => !IsPrivateIpAddress(ip)).ToList();

                // If we found any, return the last one, otherwise return the user host address
                return publicForwardingIps.Any() ? publicForwardingIps.Last() : userHostAddress;
            }
            catch (Exception)
            {
                // Always return all zeroes for any failure (my calling code expects it)
                return "0.0.0.0";
            }
        }

        private static bool IsPrivateIpAddress(string ipAddress)
        {
            // http://en.wikipedia.org/wiki/Private_network
            // Private IP Addresses are: 
            //  24-bit block: 10.0.0.0 through 10.255.255.255
            //  20-bit block: 172.16.0.0 through 172.31.255.255
            //  16-bit block: 192.168.0.0 through 192.168.255.255
            //  Link-local addresses: 169.254.0.0 through 169.254.255.255 (http://en.wikipedia.org/wiki/Link-local_address)

            var ip = IPAddress.Parse(ipAddress);
            var octets = ip.GetAddressBytes();

            var is24BitBlock = octets[0] == 10;
            if (is24BitBlock) return true; // Return to prevent further processing

            var is20BitBlock = octets[0] == 172 && octets[1] >= 16 && octets[1] <= 31;
            if (is20BitBlock) return true; // Return to prevent further processing

            var is16BitBlock = octets[0] == 192 && octets[1] == 168;
            if (is16BitBlock) return true; // Return to prevent further processing

            var isLinkLocalAddress = octets[0] == 169 && octets[1] == 254;
            return isLinkLocalAddress;
        }
		public static WebHeaderCollection GetRequestHead( string requestUrl )
		{
            var webRequest = (HttpWebRequest) WebRequest.Create(requestUrl);
            webRequest.Method = "GET";
            webRequest.Credentials = CredentialCache.DefaultCredentials;
            using ( WebResponse webResponse = webRequest.GetResponse() )
            {
                return webResponse.Headers;
            }
		}
        public static HttpStatusCode GetHttpStatusCode(string requestUrl)
        {
             try
             {
                var webRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
                webRequest.Method = "GET";
                webRequest.Credentials = CredentialCache.DefaultCredentials;
                using (var httpWResp = (HttpWebResponse)webRequest.GetResponse())
                {
                    return httpWResp.StatusCode;
                }
             }
             catch (WebException webExp)
             {
                 return HttpStatusCode.NotFound;
             }
             catch (UriFormatException webExp)
             {
                 return HttpStatusCode.NotFound;
             }
        }

        public static MemoryStream GetRequest(string requestUrl)
        {
            string absURL = string.Empty;
            return GetRequest(requestUrl,string.Empty, ref absURL);
        }

        public static MemoryStream GetRequest(string requestUrl, string referer, ref string fileAbsoluteURI)
        {
            return GetRequest(requestUrl, "",referer, ref fileAbsoluteURI);
        }

        public static MemoryStream CommonGetRequest(string requestUrl, string data, string referer, ref string fileAbsoluteURI)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
            webRequest.Method = "GET";
            webRequest.Credentials = CredentialCache.DefaultCredentials;

            webRequest.Accept = "*/*";

            webRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-cn");

            //webRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            webRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "");

            webRequest.Referer = referer;



            webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
            webRequest.Headers.Add(HttpRequestHeader.KeepAlive, "TRUE");
            //webRequest.Headers.Add(HttpRequestHeader.Connection, "Keep-Alive");
            webRequest.Headers.Add(HttpRequestHeader.Pragma, "no-cache");

            var ms = GetResponseStream(ref webRequest, ref fileAbsoluteURI);
            webRequest = null;
            return ms;


        }

        public static MemoryStream GetRequest(string requestUrl, string data, string referer, ref string fileAbsoluteURI)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
            webRequest.Method = "GET";
            webRequest.Credentials = CredentialCache.DefaultCredentials;

            webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";




            webRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-us,en;q=0.5");

            //webRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            webRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
            webRequest.Headers.Add(HttpRequestHeader.AcceptCharset, "ISO-8859-1,utf-8;q=0.7,*;q=0.7");

            webRequest.Referer = referer;

            webRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.2.12) Gecko/20101026 Firefox/3.6.12";
            webRequest.Headers.Add(HttpRequestHeader.KeepAlive, "115");
            //webRequest.Connection="keep-alive";

            //webRequest.Headers.Add(HttpRequestHeader.Connection, "Keep-Alive");
            //webRequest.Headers.Add(HttpRequestHeader.Pragma, "no-cache");

            var ms = GetResponseStream(ref webRequest, ref fileAbsoluteURI);
            webRequest = null;
            return ms;


        }


		public static MemoryStream PostRequest( string requestUrl, string data )
		{
			return PostRequest( requestUrl, data, Encoding.UTF8 );
		}
        public static MemoryStream PostRequest(string requestUrl, string data, Encoding encoding)
        {
            return PostRequest(requestUrl, data, Encoding.UTF8,10000);
        }

		public static MemoryStream PostRequest( string requestUrl, string data, Encoding encoding ,int timeoutMiliseconds,
            string referer = "", string cookie = "", string contentType = "")
		{
			var webRequest = ( HttpWebRequest ) WebRequest.Create( requestUrl );
            if (!string.IsNullOrEmpty(contentType))
                webRequest.ContentType = contentType;
            else
			    webRequest.ContentType = "application/x-www-form-urlencoded";
			webRequest.Method = "POST";
            webRequest.Timeout = timeoutMiliseconds;
            webRequest.Referer = referer;
            webRequest.Headers.Add(HttpRequestHeader.Cookie, cookie);

			WriteRequestStream( ref webRequest, data, encoding );
            var sourceFileURI = string.Empty;
            var resultMS= GetResponseStream(ref webRequest, ref sourceFileURI);
            webRequest.Abort();
            webRequest = null;
            sourceFileURI = null;
            return resultMS;
		}
        public static MemoryStream PostRequest(string requestUrl, byte[] data, Encoding encoding, int timeoutMiliseconds,
            string referer = "", string cookie = "", string contentType = "")
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
            if (!string.IsNullOrEmpty(contentType))
                webRequest.ContentType = contentType;
            else
                webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            webRequest.Timeout = timeoutMiliseconds;
            //webRequest.Referer = referer;
            //webRequest.Headers.Add(HttpRequestHeader.Cookie, cookie);
            webRequest.UserAgent="Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.22 (KHTML, like Gecko) Chrome/25.0.1364.172 Safari/537.22";
            //webRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate,sdch");
            webRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "utf-8");
            
            webRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8");
            webRequest.Headers.Add(HttpRequestHeader.AcceptCharset, "ISO-8859-1,utf-8;q=0.7,*;q=0.3");

            WriteRequestStream(ref webRequest, data, encoding);
            var sourceFileURI = string.Empty;
            var resultMS = GetResponseStream(ref webRequest, ref sourceFileURI);
            webRequest.Abort();
            webRequest = null;
            sourceFileURI = null;
            return resultMS;
        }

		public static MemoryStream SoapRequest( string requestUrl, string data, string soapAction )
		{
			return SoapRequest( requestUrl, data, soapAction, Encoding.UTF8 );
		}

		public static MemoryStream SoapRequest( string requestUrl, string data, string soapAction, Encoding encoding )
		{
			var webRequest = ( HttpWebRequest ) WebRequest.Create( requestUrl );
			webRequest.ContentType = "text/xml; charset=utf-8";
			webRequest.Method = "POST";
			webRequest.Headers.Add( "SOAPAction", soapAction );
			WriteRequestStream( ref webRequest, data, encoding );
            string sourceFileURI = string.Empty;

            return GetResponseStream(ref webRequest, ref sourceFileURI);
		}

		private static void WriteRequestStream( ref HttpWebRequest webRequest, string data )
		{
			WriteRequestStream( ref webRequest, data, Encoding.Default );
		}

		private static void WriteRequestStream( ref HttpWebRequest webRequest, string data, Encoding encoding )
		{
			using ( var requestStream = webRequest.GetRequestStream() )
			{
				requestStream.Write( encoding.GetBytes( data ), 0, data.Length );
				requestStream.Close();
			}
		}
        private static void WriteRequestStream(ref HttpWebRequest webRequest, byte[] data, Encoding encoding)
        {
            using (var requestStream = webRequest.GetRequestStream())
            {
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
            }
        }
        private static MemoryStream GetResponseStream(ref HttpWebRequest webRequest, ref string fileAbsoluteURI)
		{
			using ( WebResponse webResponse = webRequest.GetResponse() ) 
			{

                //When downloading the files,we need get the file name to determine its file extension.
                fileAbsoluteURI = webResponse.ResponseUri.AbsoluteUri;

				Stream stream =  webResponse.GetResponseStream();
				var memoryStream = new MemoryStream();
				byte[] bs = new byte[256];
				for ( int j = stream.Read( bs, 0, ( int ) bs.Length ); j > 0; j = stream.Read( bs, 0, ( int ) bs.Length ) )
				{
					memoryStream.Write( bs, 0, j );
				}
                stream.Flush();
				stream.Close();
                stream = null;
				memoryStream.Position = ( long )0;
                bs = null;
                webResponse.Close();
				return memoryStream;
			}
		}

		public static string GetResponseStream1( ref HttpWebRequest webRequest )
		{
			using ( var webResponse = webRequest.GetResponse() ) 
			{
				using ( var responseStream = new StreamReader( webResponse.GetResponseStream(), System.Text.Encoding.GetEncoding( "gb2312" ) ) )
				{
					return responseStream.ReadToEnd();
				}
			}
		}
        public static byte[] DownloadFile(string url, TimeSpan? timeout = null)
        {
            if (StaticSiteConfiguration.IsOfflineTest)
            {
                return null;
            }
            var uri = new Uri(url);
            var cookieContainer = new CookieContainer();
            if (timeout == null)
                timeout = TimeSpan.FromSeconds(15);
            var request = GetNewRequest(uri, cookieContainer, timeout);
            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            WebResponse webResponse = request.GetResponse();
            HttpWebResponse response=webResponse as HttpWebResponse;
            if (response !=null )
            {
                if ((response.StatusCode != HttpStatusCode.OK &&
                    response.StatusCode != HttpStatusCode.Moved &&
                    response.StatusCode != HttpStatusCode.Redirect) /*&&
                (response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase) ||
                response.ContentType.StartsWith("audio", StringComparison.OrdinalIgnoreCase) ||
                response.ContentType.StartsWith("zip", StringComparison.OrdinalIgnoreCase)
                )*/)
                {
                    return null;
                }            
            }
            byte[] imageBytes = null;

            // if the remote file was found, download it
            using (Stream inputStream = webResponse.GetResponseStream())
            {
                using (MemoryStream outputStream = new MemoryStream())
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    do
                    {
                        bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                        outputStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead != 0);
                    imageBytes = outputStream.ToArray();

                }
            }
            return imageBytes;
        }
        public static string DownloadImageInString(string url)
        {
            string base64String = string.Empty;
            // Convert byte[] to Base64 String
            byte[] imageBytes = DownloadFile(url, TimeSpan.FromSeconds(15));
            base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }
        public static string DownloadHtml(Uri url)
        {
            return DownloadHtml(url, TimeSpan.FromSeconds(30), null).Html;
        }

        public static DownloadHtmlInfo DownloadHtml(Uri url, TimeSpan? timeout = null, object headerParameters = null)
        {
            StringDictionary headerDict = null;
            if (headerParameters != null)
            {
                headerDict = new StringDictionary();
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(headerParameters);
                foreach (PropertyDescriptor prop in props)
                {
                    object val = prop.GetValue(headerParameters);
                    var propName = prop.Name;
                    headerDict.Add(propName,val.ToString());
                }
            }
            return DownloadHtml(url, timeout, headerDict);
        }
        public static DownloadHtmlInfo DownloadHtml(Uri url, TimeSpan? timeout = null, CookieContainer cookies = null, StringDictionary headerParameters = null)
        {
            if (StaticSiteConfiguration.IsOfflineTest)
            {
                return new DownloadHtmlInfo
                {
                    Html = string.Empty,
                    ResponseUri = url,
                };
            }
            StringDictionary _emailTemplates=new StringDictionary();
            CookieContainer cookieContainer = null;
            if (cookies == null)
                cookieContainer = new CookieContainer();
            else
                cookieContainer = cookies;

            var request = (HttpWebRequest)GetNewRequest(url, cookieContainer, timeout);
            if (headerParameters != null)
            {
                foreach (string key in headerParameters.Keys)
                {
                    if(key.ToLower()=="referer")
                        request.Referer = headerParameters[key];
                    else
                        request.Headers[key] = headerParameters[key];

                }
            }

            var response = (HttpWebResponse)request.GetResponse();

            while (response.StatusCode != HttpStatusCode.Found && response.StatusCode != HttpStatusCode.OK)
            {
                response.Close();
                var location = response.Headers["Location"];
                if (location == null)
                    break;
                string locationString = location.ToString();
                Uri moveToUri = null;
                try
                {
                    moveToUri = new Uri(locationString);
                }
                catch
                {
                    if (!locationString.StartsWith(@"/"))
                        locationString = @"/" + locationString;
                    moveToUri = new Uri(url.Scheme + "://" + url.Host + locationString);
                }
                request = (HttpWebRequest)GetNewRequest(moveToUri, cookieContainer);
                response = (HttpWebResponse)request.GetResponse();
            }

            // get correct charset and encoding from the server's header
            string Charset = response.CharacterSet;
            if (string.IsNullOrEmpty(Charset))
                Charset = "utf-8";
            Encoding encoding = Encoding.GetEncoding(Charset);
            // read response into memory stream
            MemoryStream memoryStream;
            using (Stream responseStream = response.GetResponseStream())
            {
                memoryStream = new MemoryStream();
                byte[] buffer = new byte[1024];
                int byteCount;
                do
                {
                    byteCount = responseStream.Read(buffer, 0, buffer.Length);
                    memoryStream.Write(buffer, 0, byteCount);
                } while (byteCount > 0);
                responseStream.Close();
            }
            // set stream position to beginning
            memoryStream.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(memoryStream, encoding);
            var strWebPage = sr.ReadToEnd();
            // Check real charset meta-tag in HTML
            int CharsetStart = strWebPage.IndexOf("charset=");
            string RealCharset = string.Empty;
            if (CharsetStart > 0)
            {
                CharsetStart += 8;
            }
            else
            {//for xml file
                CharsetStart = strWebPage.IndexOf("encoding=");
                if (CharsetStart > 0)
                    CharsetStart += 9;          
            }
            if (CharsetStart > 0)
            {
                int CharsetEnd = strWebPage.IndexOfAny(new[] { ' ', '\"', ';', '\'' }, CharsetStart);
                if (CharsetEnd == CharsetStart)
                {
                    CharsetStart++;
                    CharsetEnd = strWebPage.IndexOfAny(new[] { ' ', '\"', ';', '\'' }, CharsetStart);
                }
                RealCharset =
                       strWebPage.Substring(CharsetStart, CharsetEnd - CharsetStart);            
            }
            if(!string.IsNullOrEmpty(RealCharset) && RealCharset != Charset)
            {
                // real charset meta-tag in HTML differs from supplied server header???
                StreamReader sr2=null;
                try
                {
                    // get correct encoding
                    Encoding CorrectEncoding = Encoding.GetEncoding(RealCharset);
                    // reset stream position to beginning
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    // reread response stream with the correct encoding
                    sr2 = new StreamReader(memoryStream, CorrectEncoding);
                    strWebPage = sr2.ReadToEnd();
                    // Close and clean up the StreamReader
                    sr2.Close();
                }
                catch
                { 
                    if(sr2!=null)
                        sr2.Close();
                }
            }
            // dispose the first stream reader object
            sr.Close();
            return new DownloadHtmlInfo
            {
                Html = strWebPage,
                ResponseUri = response.ResponseUri,
            };
        }

        public static WebRequest GetNewRequest(Uri targetUrl, CookieContainer cookieContainer, TimeSpan? timeout = null)
        {
            if (timeout == null)
                timeout = TimeSpan.FromSeconds(10);
            var webRequest=HttpWebRequest.Create(targetUrl);
            var request = webRequest as HttpWebRequest;
            if (request == null)
                return webRequest;
            request.Timeout = (int)timeout.Value.TotalMilliseconds;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.125 Safari/537.36";
            //request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
            //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.152 Safari/535.19";
            //request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            //request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            //request.Headers.Add("Accept-Language", "en-US,en;q=0.8,zh-CN;q=0.6,zh;q=0.4");
            //request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");

            //request.Proxy = new WebProxy("localhost", 8888);
            request.Proxy = null;
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            return request;
        }

	}
}
