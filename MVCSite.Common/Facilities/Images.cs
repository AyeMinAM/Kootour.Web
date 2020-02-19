using System;
using System.Collections;
using System.Data;       using System.Data.Common; 
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Encoder = System.Drawing.Imaging.Encoder;
using System.Collections.Generic;

using System.Web.Caching;
using System.Collections.Specialized;
using System.Xml;

namespace MVCSite.Common
{
    public enum FileChangeType
    {
        Created = 0,
        Deleted = 1
    }

    /// <summary>
    /// 通用上传图片类型
    /// </summary>
    public enum UploadImageType
    {
        Origin = 1,
        /// <summary>
        /// 省略图，具体大小由上传时指定
        /// </summary>
        Clip = 2,

        Clip_60_80 = 3
    }
    public enum ImageFileType
    {
        Notset = -1,
        /// <summary>
        /// 会员头像
        /// </summary>
        UserAvatar = 13,
        /// <summary>
        /// 会员默认头像
        /// </summary>
        UserHead = 0,
        /// <summary>
        /// 用户上传照片
        /// </summary>
        UserPhoto = 1,
        UserPhotoDeleted=2,

        /// <summary>
        /// 通用上传
        /// </summary>
        UploadImage = 14,








        //For vjiaoshi Word database.
        WordTermImage = 1001,
        ExamPaperImage,
        ThumbnailImage,

    }

    public sealed class ImageSettings
    {
        #region Member variables & constructor
        ImageSettingCollection settings = null;
        //private static object lockObject = new object ();
        private static ImageSettings instance = null;
        //public static readonly string CacheKey = "WMath.ImageSettings";
        static readonly string configFile = "Config/ImageSettings.config";

        public ImageSettings(XmlDocument doc)
        {
            settings = new ImageSettingCollection();
            XmlNode root = doc.SelectSingleNode("ImageSettings");
            for (int i = 0, count = root.ChildNodes.Count; i < count; i++)
            {
                XmlNode n = root.ChildNodes[i];
                if (n.NodeType != XmlNodeType.Comment)
                {
                    ImageSettingInfo settingInfo = new ImageSettingInfo(n.Attributes["Name"].Value, n.Attributes["Path"].Value, n.Attributes["RuntimePath"].Value, n.Attributes["PathTag"].Value);
                    for (int j = 0, jcount = n.ChildNodes.Count; j < jcount; j++)
                    {
                        XmlNode imageNode = n.ChildNodes[j];
                        if (imageNode.NodeType != XmlNodeType.Comment)
                        {
                            ImageObject imageInfo = new ImageObject();
                            imageInfo.Type = imageNode.Attributes["Type"].Value;
                            imageInfo.Width = Convert.ToInt32(imageNode.Attributes["Width"].Value);
                            imageInfo.Height = Convert.ToInt32(imageNode.Attributes["Height"].Value);
                            imageInfo.ClipType = (ImageClipType)Enum.Parse(typeof(ImageClipType), imageNode.Attributes["ClipType"].Value, true);
                            imageInfo.Postfix = imageNode.Attributes["Postfix"].Value;
                            imageInfo.WatermarkPath = imageNode.Attributes["WatermarkPath"].Value;
                            imageInfo.SetWhiteBackground = Convert.ToBoolean(imageNode.Attributes["SetWhiteBackground"].Value);
                            string watermarkPosition = imageNode.Attributes["WatermarkPosition"].Value;
                            if (!string.IsNullOrEmpty(watermarkPosition))
                            {
                                imageInfo.PositionType = (WatermarkPositionType)Enum.Parse(typeof(WatermarkPositionType), watermarkPosition, true);
                            }
                            settingInfo.Images.Add(imageInfo);
                        }
                    }
                    settings.Add(settingInfo);
                }

            }
        }

        #endregion

        #region GetConfig
        public static ImageSettings GetConfig()
        {
            /*
            ImageSettings instance = CachingService.Current.Get ( CacheKey ) as ImageSettings;
            if ( instance == null )
            {
                lock ( lockObject )
                {
                    instance = CachingService.Current.Get( CacheKey ) as ImageSettings;
                    if ( instance == null )
                    {
                        string file = Globals.GetPhysicalPath( configFile );
                        XmlDocument doc = new XmlDocument();
                        doc.Load( file );
                        instance = new ImageSettings( doc );
                        CachingService.Current.Add( CacheKey, instance, file );
                    }
                }
            }
             * */
            if (instance == null)
            {
                string file = GetPhysicalPath(configFile);
                XmlDocument doc = new XmlDocument();
                doc.Load(file);
                instance = new ImageSettings(doc);
            }
            return instance;
        }

        #endregion

        #region Public properties

        #region public ImageSettingInfo GetSetting( string name )
        public ImageSettingInfo GetSetting(string name)
        {
            ImageSettingInfo setting = settings.Get(name);
            return setting;
        }

        public ImageSettingInfo GetSetting(ImageFileType imageFileType)
        {
            //string name = Enum.GetName ( typeof ( ImageFileType ), imageFileType );
            //return GetSetting ( name );
            return GetSetting(imageFileType.ToString());
        }

        #endregion

        #endregion

        public static string GetPhysicalPath(string path)
        {
            string file = null;
            HttpContext context = HttpContext.Current;
            if (context != null)
                file = context.Server.MapPath("~/" + path);
            else
                file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            return file;
        }


    }

    #region ImageSettingInfo
    [Serializable]
    public sealed class ImageSettingInfo
    {
        private string name = string.Empty;
        private string path = string.Empty;
        private string runtimePath = string.Empty;
        private string pathTag = string.Empty;
        private ImageList images = new ImageList();
        //private string url = string.Empty;
        //private int width = 0;
        //private int height = 0;
        //private long length = 0;
        //private string postfix = string.Empty;
        //private ImageClipType clipType = ImageClipType.ScaleToFit;
        //private CreateImageStatus createImageType = CreateImageStatus.NotSet;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public ImageList Images
        {
            get
            {
                return images;
            }
            set
            {
                images = value;
            }
        }

        //public string Url
        //{
        //    get
        //    {
        //        return url;
        //    }
        //    set
        //    {
        //        url = value;
        //    }
        //}

        //public int Width
        //{
        //    get
        //    {
        //        return width;
        //    }
        //    set
        //    {
        //        width = value;
        //    }
        //}

        //public int Height
        //{
        //    get
        //    {
        //        return height;
        //    }
        //    set
        //    {
        //        height = value;
        //    }
        //}

        //public long Length
        //{
        //    get
        //    {
        //        return length;
        //    }
        //    set
        //    {
        //        length = value;
        //    }
        //}

        //public CreateImageStatus CreateImageStatus
        //{
        //    get
        //    {
        //        return createImageType;
        //    }
        //    set
        //    {
        //        createImageType = value;
        //    }
        //}


        //public string Postfix
        //{
        //    get
        //    {
        //        return postfix;
        //    }
        //    set
        //    {
        //        postfix = value;
        //    }
        //}

        //public ImageClipType ClipType
        //{
        //    get
        //    {
        //        return clipType;
        //    }
        //    set
        //    {
        //        clipType = value;
        //    }
        //}


        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
            }
        }

        public string RuntimePath
        {
            get
            {
                return runtimePath;
            }
            set
            {
                runtimePath = value;
            }
        }

        public string PathTag
        {
            get
            {
                return pathTag;
            }
            set
            {
                pathTag = value;
            }
        }

        public ImageSettingInfo(string name)
        {
            this.name = name;
        }


        public ImageSettingInfo(string name, string path, string runtimePath, string pathTag)
        {
            this.name = name;
            this.path = path;
            this.runtimePath = runtimePath;
            this.pathTag = pathTag;
        }

        //public ImageSettingInfo( string imageName, string imageUrl, int width, int height, long length )
        //{
        //    this.name = imageName;
        //    this.url = imageUrl;
        //    this.width = width;
        //    this.height = height;
        //    this.length = length;
        //}

        //public ImageSettingInfo( string imageName, int width, int height, string postfix, ImageClipType trimType )
        //{
        //    this.name = imageName;
        //    this.width = width;
        //    this.height = height;
        //    this.postfix = postfix;
        //    this.clipType = trimType;
        //}
    }

    #endregion

    #region ImageSettingCollection
    [Serializable]
    public class ImageSettingCollection : CollectionBase
    {
        #region Fields

        Dictionary<string, int> nameIndexs;
        private static object lockObject = new object();

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        public ImageSettingCollection()
        {
        }

        public ImageSettingCollection(ImageSettingInfo[] value)
        {
            this.AddRange(value);
        }
        public ImageSettingInfo this[int index]
        {
            get
            {
                return ((ImageSettingInfo)(this.List[index]));
            }
        }
        public int Add(ImageSettingInfo value)
        {
            return this.List.Add(value);
        }
        public void AddRange(ImageSettingInfo[] value)
        {
            for (int i = 0; (i < value.Length); i++)
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(ImageSettingCollection value)
        {
            for (int i = 0; (i < value.Count); i++)
            {
                this.Add((ImageSettingInfo)value.List[i]);
            }
        }
        public bool Contains(ImageSettingInfo value)
        {
            if (IndexOf(value) > -1)
                return true;
            return false;
        }

        public void CopyTo(ImageSettingInfo[] array, int index)
        {
            this.List.CopyTo(array, index);
        }

        public int IndexOf(ImageSettingInfo value)
        {
            for (int i = 0; i < this.List.Count; i++)
            {
                if (string.Compare(value.Name, (this.List[i] as ImageSettingInfo).Name, true) == 0)
                    return i;
            }
            return -1;
        }

        //public void Insert(int index, CommunityPageConfigInfo value)
        //{
        //    List.Insert(index, value);
        //}
        //public void Remove(CommunityPageConfigInfo value)
        //{
        //    List.Remove(value);
        //}

        public ImageSettingInfo Get(string name)
        {
            if (nameIndexs == null)
            {
                lock (lockObject)
                {
                    if (nameIndexs == null)
                    {
                        nameIndexs = new Dictionary<string, int>(List.Count, StringComparer.InvariantCultureIgnoreCase);
                        for (int i = 0, count = List.Count; i < count; i++)
                        {
                            ImageSettingInfo settingInfo = List[i] as ImageSettingInfo;
                            if (settingInfo != null && !nameIndexs.ContainsKey(settingInfo.Name))
                            {
                                nameIndexs.Add(settingInfo.Name, i);
                            }
                        }
                    }
                }
            }
            if (nameIndexs != null && nameIndexs.ContainsKey(name))
            {
                int index = nameIndexs[name];
                if (index > -1 && index < List.Count)
                {
                    return List[index] as ImageSettingInfo;
                }
            }
            return null;
        }

        #endregion

        #region PrivateMethods


        #endregion
    }

    #endregion

    #region CreateImageStatus
    [Serializable]
    public enum CreateImageStatus
    {
        NotSet = -1,
        Success = 0,
        MaxWidth = 11,
        MaxHeight = 12,
        MinWidth = 13,
        MinHeight = 14,
        MaxSize = 15,
        MinSize = 16,
        ErrorFormat = 17,
        UnknownError = 9
    }

    #endregion

    #region ImageObject
    [Serializable]
    public sealed class ImageObject
    {
        private string name = string.Empty;
        private string type = string.Empty;
        private string imageUrl = string.Empty;
        private int width = 0;
        private int height = 0;
        private long length = 0;
        private string postfix = string.Empty;
        private ImageClipType clipType = ImageClipType.ScaleToFit;
        private string watermarkPath = string.Empty;
        private WatermarkPositionType positionType = WatermarkPositionType.TopCenter;
        private CreateImageStatus createImageStatus = CreateImageStatus.NotSet;
        private bool setWhiteBackground;

        public bool SetWhiteBackground
        {
            get { return setWhiteBackground; }
            set { setWhiteBackground = value; }
        }


        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        public string ImageUrl
        {
            get
            {
                return imageUrl;
            }
            set
            {
                imageUrl = value;
            }
        }

        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        public long Length
        {
            get
            {
                return length;
            }
            set
            {
                length = value;
            }
        }

        public CreateImageStatus CreateImageStatus
        {
            get
            {
                return createImageStatus;
            }
            set
            {
                createImageStatus = value;
            }
        }


        public string Postfix
        {
            get
            {
                return postfix;
            }
            set
            {
                postfix = value;
            }
        }

        public ImageClipType ClipType
        {
            get
            {
                return clipType;
            }
            set
            {
                clipType = value;
            }
        }


        public string WatermarkPath
        {
            get
            {
                return watermarkPath;
            }
            set
            {
                watermarkPath = value;
            }
        }

        public WatermarkPositionType PositionType
        {
            get
            {
                return positionType;
            }
            set
            {
                positionType = value;
            }
        }

        public ImageObject()
        {

        }
        public ImageObject(string imageName, string imageUrl, int width, int height, long length)
        {
            this.name = imageName;
            this.imageUrl = imageUrl;
            this.width = width;
            this.height = height;
            this.length = length;
        }

        public ImageObject(string imageName, int width, int height, string postfix, ImageClipType trimType)
        {
            this.name = imageName;
            this.width = width;
            this.height = height;
            this.postfix = postfix;
            this.clipType = trimType;
        }


    }

    #endregion

    #region ImageList
    [Serializable]
    public class ImageList : CollectionBase
    {
        #region Fields

        Dictionary<string, int> typeIndexs = null;
        Dictionary<string, int> postfixIndexs = null;
        private static object lockObjectForType = new object();
        private static object lockObjectForPostfix = new object();
        #endregion

        #region Properties

        #endregion

        #region Public Methods

        public ImageList()
        {
        }

        //public ImageList( ImageObject [] value )
        //{
        //    this.AddRange ( value );
        //}
        public ImageObject this[int index]
        {
            get
            {
                return ((ImageObject)(this.List[index]));
            }
        }
        public int Add(ImageObject value)
        {
            //typeIndexs.Add ( value.Type.ToString (), this.List.Count.ToString () );
            //postfixIndexs.Add ( value.Postfix.ToString (), this.List.Count.ToString () );
            return this.List.Add(value);
        }
        //public void AddRange( ImageObject [] value )
        //{
        //    for ( int i = 0; ( i < value.Length ); i++ )
        //    {
        //        this.Add ( value [i] );
        //    }
        //}
        //public void AddRange( ImageList value )
        //{
        //    for ( int i = 0; ( i < value.Count ); i++ )
        //    {
        //        this.Add ( ( ImageObject ) value.List [i] );
        //    }
        //}
        public bool Contains(ImageObject value)
        {
            if (IndexOf(value) > -1)
                return true;
            return false;
        }

        public void CopyTo(ImageObject[] array, int index)
        {
            this.List.CopyTo(array, index);
        }

        public int IndexOf(ImageObject value)
        {
            for (int i = 0; i < this.List.Count; i++)
            {
                if (string.Compare(value.Name, (this.List[i] as ImageObject).Name, true) == 0)
                    return i;
            }
            return -1;
        }

        public void Insert(int index, ImageObject value)
        {
            List.Insert(index, value);
        }
        public void Remove(ImageObject value)
        {
            List.Remove(value);
        }

        public ImageObject Get(string type)
        {
            //if ( typeIndexs == null )
            //{
            //    lock( lockObjectForType )
            //    {
            if (typeIndexs == null)
            {
                typeIndexs = new Dictionary<string, int>(List.Count, StringComparer.InvariantCultureIgnoreCase);
                for (int i = 0, count = List.Count; i < count; i++)
                {
                    ImageObject settingInfo = List[i] as ImageObject;
                    if (settingInfo != null && !typeIndexs.ContainsKey(settingInfo.Type))
                    {
                        typeIndexs.Add(settingInfo.Type, i);
                    }
                }
            }
            //    }
            //}
            if (typeIndexs != null && typeIndexs.ContainsKey(type))
            {
                int index = typeIndexs[type];
                if (index > -1 && index < List.Count)
                {
                    return List[index] as ImageObject;
                }
            }
            return null;
        }

        public ImageObject GetByPostfix(string postfix)
        {
            //if ( postfixIndexs == null )
            //{
            //    lock( lockObjectForPostfix )
            //    {
            if (postfixIndexs == null)
            {
                postfixIndexs = new Dictionary<string, int>(List.Count, StringComparer.InvariantCultureIgnoreCase);
                for (int i = 0, count = List.Count; i < count; i++)
                {
                    ImageObject settingInfo = List[i] as ImageObject;
                    if (settingInfo != null && !postfixIndexs.ContainsKey(settingInfo.Postfix))
                    {
                        postfixIndexs.Add(settingInfo.Postfix.ToString(), i);
                    }
                }
            }
            //    }
            //}
            if (postfixIndexs != null && postfixIndexs.ContainsKey(postfix))
            {
                int index = postfixIndexs[postfix];
                if (index > -1 && index < List.Count)
                {
                    return List[index] as ImageObject;
                }
            }
            return null;
        }

        #endregion

        #region PrivateMethods


        #endregion
    }

    #endregion



    public static class Images
    {

        #region 产生文件名
        /// <summary>
        /// 产生文件名
        /// </summary>
        /// <param name="ext">后缀名，例如“jpg”，不用则传空字符串</param>
        /// <returns>返回一个随机生成的文件名</returns>
        public static string GenerateFileName(string ext)
        {
            string a = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            string b = new Randoms().Next(10000000, 99999999).ToString();
            string c = a + "." + b;
            if (!string.IsNullOrEmpty(ext))
            {
                if (ext.StartsWith("."))
                    c += ext;
                else
                    c += "." + ext;
            }
            return c;
        }
        #endregion


    }


}
