using System;
using System.Data;       using System.Data.Common; 
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace MVCSite.Common
{

    /// <summary>
    /// 后台调用图片服务的函数
    /// </summary>
    public static class ImageServiceHelper
    {
        //public static string ImageServerRandomV3 = ImageServerSettings.Current.GetImageServer();
        //public static ImageServerSettings ImageServerSettingsV3 = ImageServerSettings.Current;
        //public readonly static ImageService.UploadImageWS UploadImageWSInstance = new ImageService.UploadImageWS();

        /// <summary>
        /// 判断是否上传了正确的文件类型
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static bool IsValidZipFile(string contentType)
        {
            string contentTypeLower = contentType.ToLower();
            if (contentTypeLower == "application/x-zip-compressed")
            {
                return true;
            }
            return false;
        }
        public static bool IsValidImageFile(string contentType)
        {
            string contentTypeLower = contentType.ToLower();

            if (
                contentTypeLower == "image/pjpeg" ||
                contentTypeLower == "image/jpeg" ||
                contentTypeLower == "image/gif" ||
                contentTypeLower == "image/png" ||
                contentTypeLower == "image/jpg"
               )
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check whether this image file has a valid file extension
        /// </summary>
        /// <param name="fileExt">the image file extension</param>
        /// <returns>true:valid image file extension</returns>
        public static bool IsValidImageFileExtension(string fileExt)
        {
            string fileExtLower = fileExt.ToLower();

            if (
                fileExtLower == ".pjpeg" ||
                fileExtLower == ".jpeg" ||
                fileExtLower == ".gif" ||
                fileExtLower == ".png" ||
                fileExtLower == ".jpg"
               )
            {
                return true;
            }
            return false;
        }

        //#region 判断文件大小是否超过2MB
        //public static bool IsValidFileSize(int contentLength)
        //{
        //    return contentLength <= ConfigSettings.ImageSizeLimit;
        //}
        //public static bool IsValidMoviePersonImageFileSize(int contentLength)
        //{
        //    return contentLength <= MwSettings.Instance.MoviePersonImageSizeLimit;// ConfigSettings.MoviePersonImageSizeLimit;
        //}
        //#endregion







        public static string GetBase64StrFromBitmap(Bitmap theImage)
        {
            MemoryStream ms = new MemoryStream();
            EncoderParameters myp = new EncoderParameters(1);
            myp.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo encoder in encoders)
            {
                if (encoder.MimeType == "image/jpeg")
                {
                    theImage.Save(ms, encoder, myp);
                    break;
                }
            }
            //获取图片字节组
            ms.Position = 0;
            BinaryReader br = new BinaryReader(ms);
            byte[] b = br.ReadBytes(Convert.ToInt32(ms.Length));
            ms.Close();
            br.Close();
            //获取图片字节组的Base64编码
            string imageString = Convert.ToBase64String(b);
            return imageString;
        }
        public static string GetBase64StrFromFileUpload(FileUpload fileUploader)
        {
            //获取图片字节组的Base64编码
            if (fileUploader != null && fileUploader.FileContent.Length > 0)
            {
                byte[] fileContent = new byte[SafeConvert.ToInt32(fileUploader.FileContent.Length)];
                using (Stream imageStream = fileUploader.FileContent)
                {
                    imageStream.Read(fileContent, 0, fileContent.Length);
                }
                return Convert.ToBase64String(fileContent);
            }
            else
            {
                return string.Empty;
            }
        }
        ///// <summary>
        ///// 后台上传图片 到/mg/ 目录下
        ///// </summary>
        ///// <param name="theImage"></param>
        ///// <returns></returns>
        //public static string ManageWebUploadImageV3(Bitmap theImage)
        //{
        //    string imageString = GetBase64StrFromBitmap(theImage);
        //    ////开始上传图片
        //    return UploadImageWSInstance.UploadByBase64String(imageString, 0, 0, ImageService.ImageClipType.Notset);
        //}

        //public static string ManageWebUploadImageV3(FileUpload fileUploader)
        //{
        //    string imageString = GetBase64StrFromFileUpload(fileUploader);        

        //    ////开始上传图片
        //    return UploadImageWSInstance.UploadByBase64String(imageString, 0, 0, ImageService.ImageClipType.Notset);
        //}

        //public static string ManageWebUploadImageV3(string imageString)
        //{
        //    return UploadImageWSInstance.UploadByBase64String(imageString, 0, 0, ImageService.ImageClipType.Notset);
        //}

        /// <summary>
        /// 杂志处理图片
        /// </summary>
        /// <param name="pictureFile"></param>
        /// <returns></returns>
        //public static string MagazineProcessImage(FileUpload fileUploader)
        //{
        //    string strImageFileName = ManageWebUploadImageV3(fileUploader);
        //    string strImageFileFullPath = ImageServerSettingsV3.GetUploadImageShortUrlWithoutRandom(UploadImageType.Origin, strImageFileName);
        //    return strImageFileFullPath;
        //}
        //public static string GetMovieImageUrl(object path, object fileName)
        //{
        //    return ImageServerSettingsV3.GetMovieImageUrl(PosterImageType.Clip_100_140_M3, SafeConvert.ToString(path), SafeConvert.ToString(fileName));
        //}

        //public static string GetMovieImageUrl(PosterImageType imageType, object path, object fileName)
        //{
        //    return ImageServerSettingsV3.GetMovieImageUrl(imageType, SafeConvert.ToString(path), SafeConvert.ToString(fileName));
        //}



        public static string GetNewMovieImageFileName()
        {
            Random ran = new Random();
            string strRan = string.Empty;
            while (strRan.Length < 7)
            {
                strRan = strRan + ran.Next().ToString();
            }
            string file_name = DateTime.Today.Year.ToString() +
                DateTime.Today.Month.ToString() +
                DateTime.Today.Day.ToString() +
                DateTime.Now.Hour.ToString() +
                DateTime.Now.Minute.ToString() +
                DateTime.Now.Second.ToString() +
                "." + strRan.Substring(0, 7);
            
            return file_name;
        }

        //We do NOT use the ID creation arithmetic anymore
        //public static string GetNewProblemAttachedImageFileName()
        //{
        //    Random ran = new Random();
        //    string strRan = string.Empty;
        //    while (strRan.Length < 7)
        //    {
        //        strRan = strRan + ran.Next().ToString();
        //    }
        //    string file_name = DateTime.Today.Year.ToString() +
        //        DateTime.Today.Month.ToString() +
        //        DateTime.Today.Day.ToString() +
        //        DateTime.Now.Hour.ToString() +
        //        DateTime.Now.Minute.ToString() +
        //        DateTime.Now.Second.ToString() +
        //        "." + strRan.Substring(0, 7);
        //    return file_name+".jpg";
        //}


        public static bool GetLocalImageHeightAndWidth(FileUpload fileUpload, ref int imageWidth, ref int imageHeight)
        {
            if (fileUpload.FileName == string.Empty)
            {
                imageWidth=0;
                imageHeight=0;
                return false;
            }

            //获取图片字节组的Base64编码
            string strBase64 = GetBase64StrFromFileUpload(fileUpload);

            //System.Drawing.Image image = System.Drawing.Image.FromStream(fileUpload.PostedFile.InputStream);
            byte[] bytes = Convert.FromBase64String(strBase64);
            if (bytes == null)
            {
                imageWidth = 0;
                imageHeight = 0;
                return false;

            }
            MemoryStream ms = StreamHelper.GetMemoryStreamFromByteArray(bytes);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
            Bitmap bitmap = new Bitmap(image);
            image.Dispose();
            imageWidth = bitmap.Width;
            imageHeight = bitmap.Height;
            bitmap.Dispose();
            ms.Close();
            return true;
        }

        /// <summary>
        /// Get the image's width and height from the local full file name.
        /// </summary>
        /// <param name="localFile">the full local image file name to calculate</param>
        /// <param name="imageWidth">this image's width</param>
        /// <param name="imageHeight">this image's height</param>
        /// <returns></returns>
        public static bool GetLocalImageHeightAndWidth(string localFile, ref int imageWidth, ref int imageHeight)
        {
            if (!FileHelper.IsLocalFile(localFile))
            {
                imageWidth = 0;
                imageHeight = 0;
                return false;
            }
            //System.Drawing.Image image = System.Drawing.Image.FromStream(fileUpload.PostedFile.InputStream);
            byte[] bytes = FileHelper.GetFileBinary(localFile);
            if (bytes == null)
            {
                imageWidth = 0;
                imageHeight = 0;
                return false;
            
            }
            MemoryStream ms = StreamHelper.GetMemoryStreamFromByteArray(bytes);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
            Bitmap bitmap = new Bitmap(image);
            image.Dispose();
            imageWidth = bitmap.Width;
            imageHeight = bitmap.Height;
            bitmap.Dispose();
            ms.Close();
            return true;
        }

        /// <summary>
        /// Get the image's width and height from image bytes
        /// </summary>
        /// <param name="bytes">the image string bytes</param>
        /// <param name="imageWidth">this image's width</param>
        /// <param name="imageHeight">this image's height</param>
        /// <returns></returns>
        //public static bool GetBytesImageHeightAndWidth(byte[] bytes, ref int imageWidth, ref int imageHeight)
        //{
        //    MemoryStream ms = StreamHelper.GetMemoryStreamFromByteArray(bytes);
        //    System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
        //    Bitmap bitmap = new Bitmap(image);
        //    image.Dispose();
        //    imageWidth = bitmap.Width;
        //    imageHeight = bitmap.Height;
        //    bitmap.Dispose();
        //    ms.Close();
        //    return true;
        //}

        /// <summary>
        /// Create the file name for the newly created image
        /// </summary>
        /// <param name="paperID"></param>
        /// <param name="sectionID"></param>
        /// <param name="problemID"></param>
        /// <returns></returns>
        public static string GetNewProblemAttachedImageFileName(int paperID,int sectionID,int problemID,int imageSequ,string fileExtension)
        {
            string strRan = FileHelper.Get5DigitsRandomString();
            StringBuilder sbFileName = new StringBuilder(100);
            sbFileName.Append(DateTime.Today.Year.ToString());
            sbFileName.Append(DateTime.Today.Month.ToString());
            sbFileName.Append(DateTime.Today.Day.ToString());
            sbFileName.Append(DateTime.Now.Hour.ToString());
            sbFileName.Append(DateTime.Now.Minute.ToString());
            sbFileName.Append(DateTime.Now.Second.ToString());
            sbFileName.Append(".");
            sbFileName.Append(paperID.ToString());
            sbFileName.Append(DateTime.Today.Month.ToString().PadLeft(2, '0'));
            sbFileName.Append(sectionID.ToString());
            sbFileName.Append(DateTime.Today.Day.ToString().PadLeft(2, '0'));
            sbFileName.Append(problemID.ToString());
            sbFileName.Append(DateTime.Now.Hour.ToString().PadLeft(2, '0'));
            sbFileName.Append(imageSequ.ToString());

            sbFileName.Append(strRan);
            sbFileName.Append(fileExtension);

            //string file_name = DateTime.Today.Year.ToString() +
            //    DateTime.Today.Month.ToString() +
            //    DateTime.Today.Day.ToString() +
            //    DateTime.Now.Hour.ToString() +
            //    DateTime.Now.Minute.ToString() +
            //    DateTime.Now.Second.ToString() +
            //    "." + paperID.ToString() + DateTime.Today.Month.ToString().PadLeft(2,'0')+
            //    sectionID.ToString() + DateTime.Today.Day.ToString().PadLeft(2, '0') + problemID.ToString() + strRan;

            return sbFileName.ToString();
        }

        /// <summary>
        /// Get the fie directory for the newly created image 
        /// </summary>
        /// <returns></returns>
        public static string GetDirOfNewSpiderImageFile()
        {

            StringBuilder imageDir = new StringBuilder(100); ;
            imageDir.Append(@"spider\");
            imageDir.Append(DateTime.Today.Year.ToString());
            imageDir.Append(@"\");
            imageDir.Append(DateTime.Today.DayOfYear.ToString());
            imageDir.Append(@"\");
            imageDir.Append(DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0'));
            imageDir.Append(@"\");
            return imageDir.ToString();

        }
        /// <summary>
        /// Get the fie directory for the newly editor uploaded image 
        /// </summary>
        /// <returns></returns>
        public static string GetDirOfNewEditorUploadedImageFile()
        {
            StringBuilder imageDir = new StringBuilder(100); ;
            imageDir.Append(@"mw\");
            imageDir.Append(DateTime.Today.Year.ToString());
            imageDir.Append(@"\");
            imageDir.Append(DateTime.Today.DayOfYear.ToString());
            imageDir.Append(@"\");
            imageDir.Append(DateTime.Now.Minute.ToString());
            imageDir.Append(@"\");
            return imageDir.ToString();
        }


        #region 判断文件大小是否超过2MB
        public static bool IsValidFileSize(int contentLength)
        {
            return contentLength <= 1024 * 1024 * 2;
        }
        public static bool IsValidProblemAttachedImageFileSize(int contentLength)
        {
            return contentLength <= 1024 * 1024 * 5;
        }
        #endregion




    }
}
