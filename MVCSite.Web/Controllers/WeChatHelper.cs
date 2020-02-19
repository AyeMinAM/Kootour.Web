using MVCSite.DAC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using System.Text;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;

namespace MVCSite.Web.Controllers
{
    public class WeChatHelper
    {
        public WeChatHelper()
        {
            //appid = "wxb8cabbd26a0031cf";
            //appsecret = "9a09ea25429d9ccf34a96a57f00f45e1";
        }
        public string GetLoginUrl(int role)
        {
            var loginUrl = string.Format("https://open.weixin.qq.com/connect/qrconnect?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_login&state={2}{3}{4}#wechat_redirect",
                appid, redirectUrl, WeChatState,stateSeparator,role);
            return loginUrl;
        }

        #region 属性
        public const string stateSeparator = "-"; 
        public static string WeChatState = string.Format("KTWCT{0}{1}{2}{3}", DateTime.UtcNow.Millisecond,
            DateTime.UtcNow.Second, DateTime.UtcNow.Minute, DateTime.UtcNow.Hour); 
        public const string appid = "wxb8cabbd26a0031cf";  //公众微信平台下可以找到
        public const string appsecret = "9a09ea25429d9ccf34a96a57f00f45e1";  //公众微信平台下可以找到
        //public const string redirectUrl = "http://guides.kootour.com/Account/LogOnWeChat";  //公众微信平台下可以找到
        public const string redirectUrl = "http://www.kootour.com/Account/LogOnWeChat";  //公众微信平台下可以找到
        #endregion

        //根据appid，secret，code获取微信openid、access token信息
        public OAuth_Token Get_token(string Code)
        {
            //获取微信回传的openid、access token
            string Str = GetJson("https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + appid + "&secret=" + appsecret + "&code=" + Code + "&grant_type=authorization_code");
            //微信回传的数据为Json格式，将Json格式转化成对象
            OAuth_Token Oauth_Token_Model = JsonHelper.ParseFromJson<OAuth_Token>(Str);
            return Oauth_Token_Model;
        }

        //刷新Token(好像这个刷新Token没有实际作用)
        public OAuth_Token refresh_token(string REFRESH_TOKEN)
        {
            string Str = GetJson("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid=" + appid + "&grant_type=refresh_token&refresh_token=" + REFRESH_TOKEN);
            OAuth_Token Oauth_Token_Model = JsonHelper.ParseFromJson<OAuth_Token>(Str);
            return Oauth_Token_Model;
        }

        //根据openid，access token获得用户信息
        public OAuthUser Get_UserInfo(string accessToken, string OPENID)
        {
            string Str = GetJson("https://api.weixin.qq.com/sns/userinfo?access_token=" + accessToken + "&openid=" + OPENID);
            OAuthUser OAuthUser_Model = JsonHelper.ParseFromJson<OAuthUser>(Str);
            return OAuthUser_Model;
        }

        //访问微信url并返回微信信息
        protected string GetJson(string url)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Encoding = Encoding.UTF8;
            string returnText = wc.DownloadString(url);

            if (returnText.Contains("errcode"))
            {
                //可能发生错误
            }
            return returnText;
        }


        /// <summary>
        /// token类
        /// </summary>
        public class OAuth_Token
        {
            public OAuth_Token()
            {

                //
                //TODO: 在此处添加构造函数逻辑
                //
            }
            //access_token	网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同
            //expires_in	access_token接口调用凭证超时时间，单位（秒）
            //refresh_token	用户刷新access_token
            //openid	用户唯一标识，请注意，在未关注公众号时，用户访问公众号的网页，也会产生一个用户和公众号唯一的OpenID
            //scope	用户授权的作用域，使用逗号（,）分隔
            public string _access_token;
            public string _expires_in;
            public string _refresh_token;
            public string _openid;
            public string _scope;
            public string access_token
            {
                set { _access_token = value; }
                get { return _access_token; }
            }
            public string expires_in
            {
                set { _expires_in = value; }
                get { return _expires_in; }
            }

            public string refresh_token
            {
                set { _refresh_token = value; }
                get { return _refresh_token; }
            }
            public string openid
            {
                set { _openid = value; }
                get { return _openid; }
            }
            public string scope
            {
                set { _scope = value; }
                get { return _scope; }
            }

        }

        /// <summary>
        /// 用户信息类
        /// </summary>
        public class OAuthUser
        {
            public OAuthUser()
            { }
            #region 数据库字段
            private string _openID;
            private string _searchText;
            private string _nickname;
            private string _sex;
            private string _province;
            private string _city;
            private string _country;
            private string _headimgUrl;
            private string _privilege;
            #endregion

            #region 字段属性
            /// <summary>
            /// 用户的唯一标识
            /// </summary>
            public string openid
            {
                set { _openID = value; }
                get { return _openID; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string SearchText
            {
                set { _searchText = value; }
                get { return _searchText; }
            }
            /// <summary>
            /// 用户昵称 
            /// </summary>
            public string nickname
            {
                set { _nickname = value; }
                get { return _nickname; }
            }
            /// <summary>
            /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知 
            /// </summary>
            public string sex
            {
                set { _sex = value; }
                get { return _sex; }
            }
            /// <summary>
            /// 用户个人资料填写的省份
            /// </summary>
            public string province
            {
                set { _province = value; }
                get { return _province; }
            }
            /// <summary>
            /// 普通用户个人资料填写的城市 
            /// </summary>
            public string city
            {
                set { _city = value; }
                get { return _city; }
            }
            /// <summary>
            /// 国家，如中国为CN 
            /// </summary>
            public string country
            {
                set { _country = value; }
                get { return _country; }
            }
            /// <summary>
            /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空
            /// </summary>
            public string headimgurl
            {
                set { _headimgUrl = value; }
                get { return _headimgUrl; }
            }
            /// <summary>
            /// 用户特权信息，json 数组，如微信沃卡用户为（chinaunicom）其实这个格式称不上JSON，只是个单纯数组
            /// </summary>
            public string privilege
            {
                set { _privilege = value; }
                get { return _privilege; }
            }
            #endregion
        }

        /// <summary>
        /// 将Json格式数据转化成对象
        /// </summary>
        public class JsonHelper
        {
            /// <summary>  
            /// 生成Json格式  
            /// </summary>  
            /// <typeparam name="T"></typeparam>  
            /// <param name="obj"></param>  
            /// <returns></returns>  
            public static string GetJson<T>(T obj)
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    json.WriteObject(stream, obj);
                    string szJson = Encoding.UTF8.GetString(stream.ToArray()); return szJson;
                }
            }
            /// <summary>  
            /// 获取Json的Model  
            /// </summary>  
            /// <typeparam name="T"></typeparam>  
            /// <param name="szJson"></param>  
            /// <returns></returns>  
            public static T ParseFromJson<T>(string szJson)
            {
                T obj = Activator.CreateInstance<T>();
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(szJson)))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                    return (T)serializer.ReadObject(ms);
                }
            }
        }
    
    
    }
}