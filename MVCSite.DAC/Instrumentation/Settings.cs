using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Web;
using MVCSite.Common;

namespace MVCSite.DAC.Instrumentation
{
    public sealed class Settings
    {
        #region Member variables & constructor
        private static Settings instance = null;
        Dictionary<string, string> settings = null;

        public Settings(XmlDocument doc)
        {
            settings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            //settings = new NameValueCollection( );
            XmlNode root = doc.SelectSingleNode("Settings");
            foreach (XmlNode n in root.ChildNodes)
            {
                if (n.NodeType != XmlNodeType.Comment)
                {
                    string name = n.Attributes["name"].Value.ToLower();
                    string setting = n.Attributes["value"].Value;
                    if (settings.ContainsKey(name))
                    {
                       throw(new Exception("Settings.config配置错误，" + name + "|" + n.InnerXml));
                    }
                    else
                    {
                        settings.Add(name, setting);
                    }
                }

            }
        }

        #endregion

        #region GetConfig
        public static Settings GetConfig()
        {
            if (instance == null)
            {
                string file = null;
                string configFile = @"Config/Settings.config";
                HttpContext context = HttpContext.Current;
                if (context != null)
                    file = context.Server.MapPath("~/" + configFile);
                else
                    file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile);
                XmlDocument doc = new XmlDocument();
                doc.Load(file);
                instance = new Settings(doc);
            }
            return instance;
        }

        #endregion

        #region Public properties

        public string GetSetting(string key)
        {
            if (settings.ContainsKey(key))
            {
                return settings[key];
            }
            return string.Empty;
        }

        public string ReturnUrl
        {
            get
            {
                return GetSetting("ReturnUrl");
            }
        }

        #endregion

        public string GetString(string key)
        {
            return GetSetting(key);
        }

        public int GetInt32(string key)
        {
            return SafeConvert.ToInt32(GetSetting(key));
        }

        public bool GetBoolean(string key)
        {
            return SafeConvert.ToBoolean(GetSetting(key));
        }
    }
}
