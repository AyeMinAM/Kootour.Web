using System;
using System.Configuration;
using System.Web;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Drawing.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
  
namespace MVCSite.Common
{
    public class ImageHelper
    {
        public static int MinWidthWithoutWatermark = 40;
        public static int MinHeightWithoutWatermark = 20;


        //[DllImport("gdi32.dll", EntryPoint = "BitBlt")]

        //public static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int wDest, int hDest, IntPtr hdcSource, int xSrc, int ySrc, int RasterOp);

        public static void CreateTextImage(string filePath, int width, int height, int startX, int startY, string[] texts,
            string ext, string fontName, int fontSize,string pinyin="")
        {
            if(texts == null||texts.Length<=0)
                return;
            Bitmap myBitmap = new Bitmap(width, height);
            int finalSize = fontSize;
            using (Graphics myGraphics = Graphics.FromImage(myBitmap))
            {
                myGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                myGraphics.SmoothingMode = SmoothingMode.HighQuality;
                myGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                myGraphics.CompositingQuality = CompositingQuality.HighQuality;
                myGraphics.FillRectangle(new SolidBrush(Color.Black), 0, 0, width, height);
                if (texts.Length == 1)
                {
                    var startPoint = new Point(startX, startY);
                    if (texts[0].Length > 1)
                    {
                        finalSize = fontSize - texts[0].Length * 20;
                        if (finalSize <= 24)
                            finalSize = 24;
                    }
                    myGraphics.DrawString(texts[0], new Font(fontName, finalSize), new SolidBrush(Color.White), startPoint);
                    if (!string.IsNullOrEmpty(pinyin))
                    {
                        myGraphics.DrawString(pinyin, new Font(fontName, 24), new SolidBrush(Color.White), new Point(4, 4));                    
                    }
                }
                else
                {
                    for (int i = 0, length = texts.Length; i < length; i++)
                    {
                        if (texts[i] == null || texts[i].Length <= 0)
                            continue;
                        if (texts[i].Length > 1)
                        {
                            finalSize = fontSize/ length - texts[i].Length * 20;
                            if (finalSize <= 24)
                                finalSize = 24;
                        }
                        myGraphics.DrawString(texts[i], new Font(fontName, finalSize),
                            new SolidBrush(Color.White), new Point(startX, startY + i * height / length));
                    }
                }
                switch (ext)
                {
                    case ".jpg":
                        myBitmap.Save(filePath, ImageFormat.Jpeg);
                        break;
                    case ".bmp":
                        myBitmap.Save(filePath, ImageFormat.Bmp);
                        break;
                    case ".png":
                        myBitmap.Save(filePath, ImageFormat.Png);
                        break;
                }
                myGraphics.Dispose();
                myBitmap.Dispose();
            }
        }


        #region public static Bitmap Assert( Bitmap image )
        /// <summary>
        /// avoid Exception:image has an indexed pixel format or its format is undefined.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Bitmap Assert(Bitmap image)
        {
            Graphics graphics = null;
            try
            {
                graphics = Graphics.FromImage(image);
            }
            catch
            {
                Bitmap indexedImage = image.Clone() as Bitmap;
                image.Dispose();
                if (indexedImage != null)
                {
                    image = GetThumbnailImage(indexedImage, indexedImage.Width, indexedImage.Height);
                    indexedImage.Dispose();
                }
                else
                {
                    //理论上不会发生
                    image = new Bitmap(image.Width, image.Height);
                }
            }
            finally
            {
                if (graphics != null)
                    graphics.Dispose();
            }
            return image;
        }

        #endregion

        #region ThumbnailImage

        /// <summary>
        /// 获取省略图（替代Bitmap.GetThumbnailImage）
        /// </summary>
        /// <param name="sourceImage"></param>
        /// <param name="targetSize"></param>
        /// <returns></returns>
        public static Bitmap GetThumbnailImage(Bitmap sourceImage, Size targetSize)
        {
            return GetThumbnailImage(sourceImage, targetSize.Width, targetSize.Height);
        }

        /// <summary>
        ///  获取省略图（替代Bitmap.GetThumbnailImage）
        /// </summary>
        /// <param name="sourceImage"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Bitmap GetThumbnailImage(Bitmap sourceImage, int width, int height)
        {
            return GetThumbnailImage(sourceImage, width, height, ImageClipType.ScaleToFit);
        }

        public static Bitmap GetThumbnailImage(Bitmap sourceImage, int width, int height, ImageClipType clipType)
        {
            Rectangle srcRectangle;
            Rectangle destRectangle;
            switch (clipType)
            {
                case ImageClipType.Notset:
                    srcRectangle = destRectangle = new Rectangle(0, 0, sourceImage.Width, sourceImage.Height);
                    break;
                case ImageClipType.ScaleToFit:
                    ScaleToFit(sourceImage.Size, width, height, out srcRectangle, out destRectangle);
                    break;
                case ImageClipType.FixWidthTrimHeight:
                    FixWidthTrimHeight(sourceImage.Size, width, height, out srcRectangle, out destRectangle);
                    //srcRectangle =  new Rectangle(0, 0, sourceImage.Width, sourceImage.Height);
                    //destRectangle = new Rectangle(0, 0, width, height);
                    break;
                case ImageClipType.FixWidth:
                    FixWidth(sourceImage.Size, width, height, out srcRectangle, out destRectangle);
                    break;
                case ImageClipType.FixWidthOrFixHeight:
                    FixWidthOrFixHeight(sourceImage.Size, width, height, out srcRectangle, out destRectangle);
                    break;
                case ImageClipType.FixWidthAndFixHeight:
                    FixWidthAndFixHeight(sourceImage.Size, width, height, out srcRectangle, out destRectangle);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("clipType");
            }
            //TODO:整理
            if (sourceImage.Size != srcRectangle.Size)
            {
                Bitmap srcBitmap = new Bitmap(srcRectangle.Width, srcRectangle.Height);
                using (Graphics g = Graphics.FromImage(srcBitmap))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    //g.InterpolationMode = InterpolationMode.High;
                    //g.SmoothingMode = SmoothingMode.HighQuality;
                    //g.Clear ( Color.White );
                    g.DrawImage(sourceImage, 0, 0, srcRectangle.Width, srcRectangle.Height);
                }
                Bitmap destBitmap = new Bitmap(destRectangle.Width, destRectangle.Height);
                using (Graphics g = Graphics.FromImage(destBitmap))
                {
                    //g.InterpolationMode = InterpolationMode.High;
                    //g.SmoothingMode = SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;

                    //g.Clear ( Color.White );
                    g.DrawImage(srcBitmap, destRectangle, new Rectangle(srcRectangle.X, srcRectangle.Y, destRectangle.Width, destRectangle.Height), GraphicsUnit.Pixel);
                }
                srcBitmap.Dispose();
                return destBitmap;
            }
            else
            {
                Bitmap destBitmap = new Bitmap(destRectangle.Width, destRectangle.Height);
                using (Graphics g = Graphics.FromImage(destBitmap))
                {
                    //g.CompositingQuality = CompositingQuality.AssumeLinear;
                    //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    //g.SmoothingMode = SmoothingMode.HighQuality;

                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;

                    //g.Clear ( Color.White );
                    g.DrawImage(sourceImage, new Rectangle(0, 0, destRectangle.Width, destRectangle.Height), srcRectangle, GraphicsUnit.Pixel);
                }
                return destBitmap;
            }
        }

        #region private static void ScaleToFit( Size srcImageSize, int destWidth, int destHeight, out Rectangle srcRectangle, out Rectangle destRectangle )
        /// <summary>
        /// 等比缩放
        /// </summary>
        /// <param name="srcImageSize"></param>
        /// <param name="destWidth"></param>
        /// <param name="destHeight"></param>
        /// <param name="srcRectangle"></param>
        /// <param name="destRectangle"></param>
        private static void ScaleToFit(Size srcImageSize, int destWidth, int destHeight, out Rectangle srcRectangle, out Rectangle destRectangle)
        {
            if (srcImageSize.Width > destWidth ||
                 srcImageSize.Height > destHeight)
            {
                int sourceWidth = srcImageSize.Width;
                int sourceHeight = srcImageSize.Height;
                int destX = 0;
                int destY = 0;
                float nPercent = 0;
                float nPercentW = ((float)destWidth / (float)sourceWidth);
                float nPercentH = ((float)destHeight / (float)sourceHeight);
                if (nPercentH < nPercentW)
                {
                    nPercent = nPercentH;
                    destX = System.Convert.ToInt16((destWidth - (sourceWidth * nPercent)) / 2);
                }
                else
                {
                    nPercent = nPercentW;
                    destY = System.Convert.ToInt16((destHeight - (sourceHeight * nPercent)) / 2);
                }
                destWidth = (int)(sourceWidth * nPercent);
                destHeight = (int)(sourceHeight * nPercent);
            }
            srcRectangle = new Rectangle(0, 0, srcImageSize.Width, srcImageSize.Height);
            destRectangle = new Rectangle(0, 0, destWidth <= 0 ? 1 : destWidth, destHeight <= 0 ? 1 : destHeight);
        }

        #endregion

        #region 固定宽截高
        /// <summary>
        /// 固定宽截高
        /// 仅以“概览页封面图：宽限制150，高限制200”为例
        /// 宽>150，高>200：将宽处理成150，高等比清晰的缩放后，如果高超过200，则高截取0-200的位置
        /// 宽>150，高<200：将宽处理成150，高等比清晰的缩放
        /// 宽<150，高>200：应该没有此类图片
        /// 宽<150，高<200：应该没有此类图片
        /// </summary>
        /// <param name="srcImageSize"></param>
        /// <param name="destWidth"></param>
        /// <param name="destHeight"></param>
        /// <param name="srcRectangle"></param>
        /// <param name="destRectangle"></param>
        /// <returns></returns>
        private static void FixWidthTrimHeight(Size srcImageSize, int destWidth, int destHeight, out Rectangle srcRectangle, out Rectangle destRectangle)
        {
            int srcX = 0, srcY = 0;
            int srcWidth = srcImageSize.Width;
            int srcHeight = srcImageSize.Height;
            int width = srcWidth;
            int height = srcHeight;
            if (srcWidth <= destWidth && srcHeight <= destHeight)
            {
                //影人概览图不做处理
                srcRectangle = new Rectangle(srcX, srcY, srcWidth, srcHeight);
                destRectangle = new Rectangle(0, 0, width, height);
            }
            else
            {
                //if ( srcWidth > destWidth )
                //{
                width = destWidth;
                height = Convert.ToInt32((float)srcHeight / (float)srcWidth * destWidth);
                if (height > destHeight)
                {
                    //srcY = ( height - destHeight ) / 2;
                    srcWidth = destWidth;
                    srcHeight = height;
                    height = destHeight;
                }
                //}
                //else
                //{
                //    width = destWidth;
                //    height = Convert.ToInt32 ( ( float ) srcHeight / ( float ) srcWidth * destWidth );
                //    //srcWidth = width;
                //    //srcHeight = Convert.ToInt32 ( ( float ) srcHeight / ( float ) srcWidth * destWidth );
                //}
                srcRectangle = new Rectangle(srcX, srcY, srcWidth, srcHeight);
                destRectangle = new Rectangle(0, 0, width, height);
            }
        }

        #endregion

        #region 定宽等比缩放，不限制高
        /// <summary>
        /// 定宽等比缩放，不限制高
        /// </summary>
        /// <param name="srcImageSize"></param>
        /// <param name="destWidth"></param>
        /// <param name="destHeight"></param>
        /// <param name="srcRectangle"></param>
        /// <param name="destRectangle"></param>
        /// <returns></returns>
        private static void FixWidth(Size srcImageSize, int destWidth, int destHeight, out Rectangle srcRectangle, out Rectangle destRectangle)
        {
            int srcX = 0, srcY = 0;
            int srcWidth = srcImageSize.Width;
            int srcHeight = srcImageSize.Height;
            int width = srcWidth;
            int height = srcHeight;
            if (srcWidth > destWidth)
            {
                width = destWidth;
                height = Convert.ToInt32((float)srcHeight / (float)srcWidth * destWidth);
            }
            srcRectangle = new Rectangle(srcX, srcY, srcWidth, srcHeight);
            destRectangle = new Rectangle(0, 0, width, height);
        }

        #endregion

        #region 固定宽或者固定高
        /// <summary>
        /// 固定宽或者固定高
        /// 宽>=100，高>=140，宽>=高：将宽处理成100，高等比清晰的缩放
        /// 宽>=100，高>=140，宽<=高：将高处理成140，宽等比清晰的缩放
        /// 宽>=100，高<=140：将宽处理成100，高等比清晰的缩放
        /// 宽<=100，高>=140：将高处理成140，宽等比清晰的缩放
        /// 宽<=100，高<=140：应该没有此类图片
        /// </summary>
        /// <param name="srcImageSize"></param>
        /// <param name="destWidth"></param>
        /// <param name="destHeight"></param>
        /// <param name="srcRectangle"></param>
        /// <param name="destRectangle"></param>
        /// <returns></returns>
        private static void FixWidthOrFixHeight(Size srcImageSize, int destWidth, int destHeight, out Rectangle srcRectangle, out Rectangle destRectangle)
        {
            int srcX = 0, srcY = 0;
            int srcWidth = srcImageSize.Width;
            int srcHeight = srcImageSize.Height;
            int width = srcWidth;
            int height = srcHeight;
            if (srcWidth >= destWidth && srcHeight >= destHeight)
            {
                //宽>100，高>140，宽/高>100/140：将宽处理成100，高等比清晰的缩放
                //宽>100，高>140，宽/高<100/140：将高处理成140，宽等比清晰的缩放
                float srcPercent = ((float)srcWidth / (float)srcHeight);
                float destPercent = ((float)destWidth / (float)destHeight);
                if (srcPercent >= destPercent)
                {
                    width = destWidth;
                    height = Convert.ToInt32((float)srcHeight / (float)srcWidth * destWidth);
                }
                else
                {
                    height = destHeight;
                    width = Convert.ToInt32((float)srcWidth / (float)srcHeight * destHeight);
                }
            }
            else if (srcWidth >= destWidth && srcHeight <= destHeight)
            {
                width = destWidth;
                height = Convert.ToInt32((float)srcHeight / (float)srcWidth * destWidth);
            }
            else if (srcWidth <= destWidth && srcHeight >= destHeight)
            {
                height = destHeight;
                width = Convert.ToInt32((float)srcWidth / (float)srcHeight * destHeight);
            }
            srcRectangle = new Rectangle(srcX, srcY, srcWidth, srcHeight);
            destRectangle = new Rectangle(0, 0, width, height);
        }

        #endregion

        #region 固定宽和高，如果小于则放大，宽截取中间的需求宽
        /// <summary>
        /// 固定宽和高，如果小于则放大，宽截取中间的需求宽
        /// </summary>
        /// <param name="srcImageSize"></param>
        /// <param name="destWidth"></param>
        /// <param name="destHeight"></param>
        /// <param name="srcRectangle"></param>
        /// <param name="destRectangle"></param>
        /// <returns></returns>
        private static void FixWidthAndFixHeight(Size srcImageSize, int destWidth, int destHeight, out Rectangle srcRectangle, out Rectangle destRectangle)
        {
            int srcX = 0, srcY = 0;
            int destX = 0, destY = 0;
            int srcWidth = srcImageSize.Width;
            int srcHeight = srcImageSize.Height;
            int width = srcWidth;
            int height = srcHeight;
            float srcPercent = ((float)srcWidth / (float)srcHeight);
            float destPercent = ((float)destWidth / (float)destHeight);
            //如果源宽大于目的宽，源高大于目的高
            if (srcWidth > destWidth && srcHeight > destHeight)
            {
                //if ( srcWidth > srcHeight )
                if (srcPercent > destPercent)
                {
                    height = destHeight;
                    width = Convert.ToInt32((float)srcWidth / (float)srcHeight * destHeight);
                    if (width > destWidth)
                    {
                        srcX = (width - destWidth) / 2;
                        srcWidth = width;
                        srcHeight = destHeight;
                        width = destWidth;
                    }
                }
                else
                {
                    width = destWidth;
                    height = Convert.ToInt32((float)srcHeight / (float)srcWidth * destWidth);
                    if (height > destHeight)
                    {
                        //srcY = ( height - destHeight ) / 2;
                        srcWidth = destWidth;
                        srcHeight = height;
                        height = destHeight;
                    }
                }
            }
            //如果源宽大于目的宽，源高小于目的高
            else if (srcWidth > destWidth && srcHeight < destHeight)
            {
                width = destWidth;
                height = destHeight;
                //srcX = ( Convert.ToInt32 ( ( float ) srcWidth / ( float ) srcHeight * destHeight ) - srcWidth ) / 2;
                int _srcWidth = Convert.ToInt32((float)srcWidth / (float)srcHeight * destHeight);
                srcX = (_srcWidth - width) / 2;
                srcWidth = _srcWidth;
                srcHeight = destHeight;
            }
            //如果源宽小于目的宽，源高大于目的高
            else if (srcWidth < destWidth && srcHeight > destHeight)
            {
                width = destWidth;
                height = destHeight;
                //srcY = ( Convert.ToInt32 ( ( float ) srcHeight / ( float ) srcWidth * destWidth ) - srcHeight ) / 2;
                int _srcHeight = Convert.ToInt32((float)srcHeight / (float)srcWidth * destWidth);
                //srcY = ( _srcHeight - height ) / 2;
                srcWidth = destWidth;
                srcHeight = _srcHeight;
            }
            //如果源宽小于目的宽，源高小于目的高
            else
            {
                //if ( srcWidth > srcHeight )
                if (srcPercent > destPercent)
                {
                    height = destHeight;
                    width = Convert.ToInt32((float)srcWidth / (float)srcHeight * destHeight);
                    if (width > destWidth)
                    {
                        srcX = (width - destWidth) / 2;
                        srcWidth = width;
                        srcHeight = destHeight;
                        width = destWidth;
                    }
                }
                else
                {
                    width = destWidth;
                    height = Convert.ToInt32((float)srcHeight / (float)srcWidth * destWidth);
                    if (height > destHeight)
                    {
                        //srcY = ( height - destHeight ) / 2;
                        srcWidth = destWidth;
                        srcHeight = height;
                        height = destHeight;
                    }
                }
            }
            srcRectangle = new Rectangle(srcX, srcY, srcWidth, srcHeight);
            destRectangle = new Rectangle(destX, destY, width, height);

        }

        #endregion

        #endregion




 

    public static byte[] getData(string filePath)

    {

        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        BinaryReader br = new BinaryReader(fs);

        byte[] data = br.ReadBytes((int)fs.Length);

        br.Close();

        fs.Close();

        return data;

    }




        public static bool IsAllowedExtension(HttpPostedFile postFile)
        {
            string fileclass = string.Empty;
            //byte buffer;
            //buffer = postFile.InputStream.ReadByte();
            fileclass = SafeConvert.ToString(postFile.InputStream.ReadByte());
            //buffer = postFile.InputStream.ReadByte();
            fileclass += SafeConvert.ToString(postFile.InputStream.ReadByte());
            postFile.InputStream.Position = 0;
            if (fileclass == "255216" || fileclass == "7173" || fileclass == "6677" || fileclass == "13780") //说明255216是jpg;7173是gif;6677是BMP,13780是PNG;7790是exe,8297是rar
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        # region GetThumbnailImage for Game with , Need Furthor Optimized

        public static Bitmap GetThumbnailImageForGame(Bitmap sourceImage, Size targetSize)
        {
            return GetThumbnailImageForGame(sourceImage, targetSize.Width, targetSize.Height);
        }

        public static Bitmap GetThumbnailImageForGame(Bitmap sourceImage, int width, int height)
        {
            return GetThumbnailImageForGame(sourceImage, width, height, ImageClipType.ScaleToFit);
        }

        public static Bitmap GetThumbnailImageForGame(Bitmap sourceImage, int width, int height, ImageClipType clipType)
        {
            Rectangle srcRectangle;
            Rectangle destRectangle;
            switch (clipType)
            {
                case ImageClipType.Notset:
                    srcRectangle = destRectangle = new Rectangle(0, 0, sourceImage.Width, sourceImage.Height);
                    break;
                case ImageClipType.ScaleToFit:
                    ScaleToFit(sourceImage.Size, width, height, out srcRectangle, out destRectangle);
                    break;
                case ImageClipType.FixWidthTrimHeight:
                    FixWidthTrimHeight(sourceImage.Size, width, height, out srcRectangle, out destRectangle);
                    break;
                case ImageClipType.FixWidth:
                    FixWidth(sourceImage.Size, width, height, out srcRectangle, out destRectangle);
                    break;
                case ImageClipType.FixWidthOrFixHeight:
                    FixWidthOrFixHeight(sourceImage.Size, width, height, out srcRectangle, out destRectangle);
                    break;
                case ImageClipType.FixWidthAndFixHeight:
                    FixWidthAndFixHeight(sourceImage.Size, width, height, out srcRectangle, out destRectangle);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("clipType");
            }
            //TODO:整理
            if (sourceImage.Size != srcRectangle.Size)
            {
                Bitmap srcBitmap = new Bitmap(srcRectangle.Width, srcRectangle.Height);
                using (Graphics g = Graphics.FromImage(srcBitmap))
                {
                    g.InterpolationMode = InterpolationMode.Default;
                    g.SmoothingMode = SmoothingMode.Default;
                    g.PixelOffsetMode = PixelOffsetMode.Default;
                    g.CompositingQuality = CompositingQuality.Default;
                    //g.InterpolationMode = InterpolationMode.High;
                    //g.SmoothingMode = SmoothingMode.HighQuality;
                    //g.Clear ( Color.White );
                    g.DrawImage(sourceImage, 0, 0, srcRectangle.Width, srcRectangle.Height);
                }
                Bitmap destBitmap = new Bitmap(destRectangle.Width, destRectangle.Height);
                using (Graphics g = Graphics.FromImage(destBitmap))
                {
                    //g.InterpolationMode = InterpolationMode.High;
                    //g.SmoothingMode = SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.Default;
                    g.SmoothingMode = SmoothingMode.Default;
                    g.PixelOffsetMode = PixelOffsetMode.Default;
                    g.CompositingQuality = CompositingQuality.Default;

                    //g.Clear ( Color.White );
                    g.DrawImage(srcBitmap, destRectangle, new Rectangle(srcRectangle.X, srcRectangle.Y, destRectangle.Width, destRectangle.Height), GraphicsUnit.Pixel);
                }
                srcBitmap.Dispose();
                return destBitmap;
            }
            else
            {
                Bitmap destBitmap = new Bitmap(destRectangle.Width, destRectangle.Height);
                using (Graphics g = Graphics.FromImage(destBitmap))
                {
                    //g.CompositingQuality = CompositingQuality.AssumeLinear;
                    //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    //g.SmoothingMode = SmoothingMode.HighQuality;

                    g.InterpolationMode = InterpolationMode.Default;
                    g.SmoothingMode = SmoothingMode.Default;
                    g.PixelOffsetMode = PixelOffsetMode.Default;
                    g.CompositingQuality = CompositingQuality.Default;

                    //g.Clear ( Color.White );
                    g.DrawImage(sourceImage, new Rectangle(0, 0, destRectangle.Width, destRectangle.Height), srcRectangle, GraphicsUnit.Pixel);
                }
                return destBitmap;
            }
        }

        #endregion

    }

    public unsafe class FastBitmap
    {

        public struct PixelData
        {
            public byte blue;
            public byte green;
            public byte red;
        }

        Bitmap Subject;
        int SubjectWidth;
        BitmapData bitmapData = null;
        Byte* pBase = null;

        public FastBitmap(Bitmap SubjectBitmap)
        {
            this.Subject = SubjectBitmap;
            try
            {
                LockBitmap();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void Release()
        {
            try
            {
                UnlockBitmap();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Bitmap Bitmap
        {
            get
            {
                return Subject;
            }
        }

        public void SetPixel(int X, int Y, Color Colour)
        {
            try
            {
                PixelData* p = PixelAt(X, Y);
                p->red = Colour.R;
                p->green = Colour.G;
                p->blue = Colour.B;
            }
            catch (AccessViolationException ave)
            {
                throw (ave);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Color GetPixel(int X, int Y)
        {
            try
            {
                PixelData* p = PixelAt(X, Y);
                return Color.FromArgb((int)p->red, (int)p->green, (int)p->blue);
            }
            catch (AccessViolationException ave)
            {
                throw (ave);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LockBitmap()
        {
            GraphicsUnit unit = GraphicsUnit.Pixel;
            RectangleF boundsF = Subject.GetBounds(ref unit);
            Rectangle bounds = new Rectangle((int)boundsF.X,
                (int)boundsF.Y,
                (int)boundsF.Width,
                (int)boundsF.Height);

            SubjectWidth = (int)boundsF.Width * sizeof(PixelData);
            if (SubjectWidth % 4 != 0)
            {
                SubjectWidth = 4 * (SubjectWidth / 4 + 1);
            }

            bitmapData = Subject.LockBits(bounds, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            pBase = (Byte*)bitmapData.Scan0.ToPointer();
        }

        private PixelData* PixelAt(int x, int y)
        {
            return (PixelData*)(pBase + y * SubjectWidth + x * sizeof(PixelData));
        }

        private void UnlockBitmap()
        {
            Subject.UnlockBits(bitmapData);
            bitmapData = null;
            pBase = null;
        }
    }


    #region SAMPLE CODE
    //using System;
    //using System.Drawing;
    //using System.Drawing.Imaging;
    //using System.Drawing.Drawing2D;
    public class ReDrawImg
    {
        private string WorkingDirectory = string.Empty; //路径
        private string ImageName = string.Empty; //被处理的图片
        private string ImageWater = string.Empty; //水印图片
        private string FontString = string.Empty; //水印文字

        enum DealType { NONE, WaterImage, WaterFont, DoubleDo }; //枚举命令

        private DealType dealtype;

        public ReDrawImg()
        { }

        public string PublicWorkingDirectory
        {
            get
            {
                return WorkingDirectory;
            }
            set
            {
                WorkingDirectory = value;
            }
        }

        public string PublicImageName
        {
            get
            {
                return ImageName;
            }
            set
            {
                ImageName = value;
            }
        }

        public string PublicImageWater
        {
            get
            {
                return ImageWater;
            }
            set //设置了水印图片的话说明是要水印图片效果的
            {
                dealtype = DealType.WaterImage;
                ImageWater = value;
            }
        }

        public string PublicFontString
        {
            get
            {
                return FontString;
            }
            set //设置了水印文字的话说明是要水印文字效果的
            {
                dealtype = DealType.WaterFont;
                FontString = value;
            }
        }

        public void DealImage()
        {
            IsDouble();

            switch (dealtype)
            {
                case DealType.WaterFont: WriteFont(); break;
                case DealType.WaterImage: WriteImg(); break;
                case DealType.DoubleDo: WriteFontAndImg(); break;
            }

        }

        private void IsDouble()
        {
            if (ImageWater + "" != "" && FontString + "" != "")
            {
                dealtype = DealType.DoubleDo;
            }
        }

        private void WriteFont()
        {
            //set a working directory
            //string WorkingDirectory = @"C:\Watermark_src\WaterPic"

            //define a string of text to use as the Copyright message
            //string Copyright = "Copyright ?2002 - AP Photo/David Zalubowski"

            //string Copyright = @"http://www.dj9158.com"

            //create a image object containing the photograph to watermark
            Image imgPhoto = Image.FromFile(WorkingDirectory + ImageName);
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            //create a Bitmap the Size of the original photograph
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            //load the Bitmap into a Graphics object
            Graphics grPhoto = Graphics.FromImage(bmPhoto);

            //------------------------------------------------------------
            //Step #1 - Insert Copyright message
            //------------------------------------------------------------

            //Set the rendering quality for this Graphics object
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;

            //Draws the photo Image object at original size to the graphics object.
            grPhoto.DrawImage(
            imgPhoto, // Photo Image object
            new Rectangle(0, 0, phWidth, phHeight), // Rectangle structure
            0, // x-coordinate of the portion of the source image to draw.
            0, // y-coordinate of the portion of the source image to draw.
            phWidth, // Width of the portion of the source image to draw.
            phHeight, // Height of the portion of the source image to draw.
            GraphicsUnit.Pixel); // Units of measure

            //-------------------------------------------------------
            //to maximize the size of the Copyright message we will
            //test multiple Font sizes to determine the largest posible
            //font we can use for the width of the Photograph
            //define an array of point sizes you would like to consider as possiblities
            //-------------------------------------------------------
            int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };

            Font crFont = null;
            SizeF crSize = new SizeF();

            //Loop through the defined sizes checking the length of the Copyright string
            //If its length in pixles is less then the image width choose this Font size.
            for (int i = 0; i < 7; i++)
            {
                //set a Font object to Arial (i)pt, Bold
                //crFont = new Font("arial", sizes[i], FontStyle.Bold);

                crFont = new Font("arial", sizes[i], FontStyle.Bold);

                //Measure the Copyright string in this Font
                crSize = grPhoto.MeasureString(FontString, crFont);

                if ((ushort)crSize.Width < (ushort)phWidth)
                    break;
            }

            //Since all photographs will have varying heights, determine a
            //position 5% from the bottom of the image
            int yPixlesFromBottom = (int)(phHeight * .05);

            //Now that we have a point size use the Copyrights string height
            //to determine a y-coordinate to draw the string of the photograph
            float yPosFromBottom = ((phHeight - yPixlesFromBottom) - (crSize.Height / 2));

            //Determine its x-coordinate by calculating the center of the width of the image
            float xCenterOfImg = (phWidth / 2);

            //Define the text layout by setting the text alignment to centered
            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;

            //define a Brush which is semi trasparent black (Alpha set to 153)
            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));

            //Draw the Copyright string
            grPhoto.DrawString(FontString, //string of text
            crFont, //font
            semiTransBrush2, //Brush
            new PointF(xCenterOfImg + 1, yPosFromBottom + 1), //Position
            StrFormat);

            //define a Brush which is semi trasparent white (Alpha set to 153)
            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));

            //Draw the Copyright string a second time to create a shadow effect
            //Make sure to move this text 1 pixel to the right and down 1 pixel
            grPhoto.DrawString(FontString, //string of text
            crFont, //font
            semiTransBrush, //Brush
            new PointF(xCenterOfImg, yPosFromBottom), //Position
            StrFormat);
            imgPhoto = bmPhoto;
            grPhoto.Dispose();

            //save new image to file system.
            imgPhoto.Save(WorkingDirectory + ImageName + "_finally.jpg", ImageFormat.Jpeg);
            imgPhoto.Dispose();
            //Text alignment
        }


        private void WriteImg()
        {
            //set a working directory
            //string WorkingDirectory = @"C:\Watermark_src\WaterPic"

            //create a image object containing the photograph to watermark
            Image imgPhoto = Image.FromFile(WorkingDirectory + ImageName);
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            //create a Bitmap the Size of the original photograph
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            //load the Bitmap into a Graphics object
            Graphics grPhoto = Graphics.FromImage(bmPhoto);

            //create a image object containing the watermark
            Image imgWatermark = new Bitmap(WorkingDirectory + ImageWater);
            int wmWidth = imgWatermark.Width;
            int wmHeight = imgWatermark.Height;

            //Set the rendering quality for this Graphics object
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;

            //Draws the photo Image object at original size to the graphics object.
            grPhoto.DrawImage(
            imgPhoto, // Photo Image object
            new Rectangle(0, 0, phWidth, phHeight), // Rectangle structure
            0, // x-coordinate of the portion of the source image to draw.
            0, // y-coordinate of the portion of the source image to draw.
            phWidth, // Width of the portion of the source image to draw.
            phHeight, // Height of the portion of the source image to draw.
            GraphicsUnit.Pixel); // Units of measure

            //------------------------------------------------------------
            //Step #2 - Insert Watermark image
            //------------------------------------------------------------

            //Create a Bitmap based on the previously modified photograph Bitmap
            Bitmap bmWatermark = new Bitmap(bmPhoto);
            bmWatermark.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
            //Load this Bitmap into a new Graphic Object
            Graphics grWatermark = Graphics.FromImage(bmWatermark);

            //To achieve a transulcent watermark we will apply (2) color
            //manipulations by defineing a ImageAttributes object and
            //seting (2) of its properties.
            ImageAttributes imageAttributes = new ImageAttributes();

            //The first step in manipulating the watermark image is to replace
            //the background color with one that is trasparent (Alpha=0, R=0, G=0, B=0)
            //to do this we will use a Colormap and use this to define a RemapTable
            ColorMap colorMap = new ColorMap();

            //My watermark was defined with a background of 100% Green this will
            //be the color we search for and replace with transparency
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);

            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            //The second color manipulation is used to change the opacity of the
            //watermark. This is done by applying a 5x5 matrix that contains the
            //coordinates for the RGBA space. By setting the 3rd row and 3rd column
            //to 0.3f we achive a level of opacity
            float[][] colorMatrixElements = {
new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
new float[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f},
new float[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f},
new float[] {0.0f, 0.0f, 0.0f, 0.3f, 0.0f},
new float[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}};
            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
            ColorAdjustType.Bitmap);

            //For this example we will place the watermark in the upper right
            //hand corner of the photograph. offset down 10 pixels and to the
            //left 10 pixles

            int xPosOfWm = ((phWidth - wmWidth) - 10);
            int yPosOfWm = 10;

            grWatermark.DrawImage(imgWatermark,
            new Rectangle(xPosOfWm, yPosOfWm, wmWidth, wmHeight), //Set the detination Position
            0, // x-coordinate of the portion of the source image to draw.
            0, // y-coordinate of the portion of the source image to draw.
            wmWidth, // Watermark Width
            wmHeight, // Watermark Height
            GraphicsUnit.Pixel, // Unit of measurment
            imageAttributes); //ImageAttributes Object

            //Replace the original photgraphs bitmap with the new Bitmap
            imgPhoto = bmWatermark;
            grPhoto.Dispose();
            grWatermark.Dispose();

            //save new image to file system.
            imgPhoto.Save(WorkingDirectory + ImageName + "_finally.jpg", ImageFormat.Jpeg);
            imgPhoto.Dispose();
            imgWatermark.Dispose();

        }

        private void WriteFontAndImg()
        {
            //create a image object containing the photograph to watermark
            Image imgPhoto = Image.FromFile(WorkingDirectory + ImageName);
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            //create a Bitmap the Size of the original photograph
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            //load the Bitmap into a Graphics object
            Graphics grPhoto = Graphics.FromImage(bmPhoto);

            //create a image object containing the watermark
            Image imgWatermark = new Bitmap(WorkingDirectory + ImageWater);
            int wmWidth = imgWatermark.Width;
            int wmHeight = imgWatermark.Height;

            //------------------------------------------------------------
            //Step #1 - Insert Copyright message
            //------------------------------------------------------------

            //Set the rendering quality for this Graphics object
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;

            //Draws the photo Image object at original size to the graphics object.
            grPhoto.DrawImage(
            imgPhoto, // Photo Image object
            new Rectangle(0, 0, phWidth, phHeight), // Rectangle structure
            0, // x-coordinate of the portion of the source image to draw.
            0, // y-coordinate of the portion of the source image to draw.
            phWidth, // Width of the portion of the source image to draw.
            phHeight, // Height of the portion of the source image to draw.
            GraphicsUnit.Pixel); // Units of measure

            //-------------------------------------------------------
            //to maximize the size of the Copyright message we will
            //test multiple Font sizes to determine the largest posible
            //font we can use for the width of the Photograph
            //define an array of point sizes you would like to consider as possiblities
            //-------------------------------------------------------
            int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };

            Font crFont = null;
            SizeF crSize = new SizeF();

            //Loop through the defined sizes checking the length of the Copyright string
            //If its length in pixles is less then the image width choose this Font size.
            for (int i = 0; i < 7; i++)
            {
                //set a Font object to Arial (i)pt, Bold
                crFont = new Font("arial", sizes[i], FontStyle.Bold);
                //Measure the Copyright string in this Font
                crSize = grPhoto.MeasureString(FontString, crFont);

                if ((ushort)crSize.Width < (ushort)phWidth)
                    break;
            }

            //Since all photographs will have varying heights, determine a
            //position 5% from the bottom of the image
            int yPixlesFromBottom = (int)(phHeight * .05);

            //Now that we have a point size use the Copyrights string height
            //to determine a y-coordinate to draw the string of the photograph
            float yPosFromBottom = ((phHeight - yPixlesFromBottom) - (crSize.Height / 2));

            //Determine its x-coordinate by calculating the center of the width of the image
            float xCenterOfImg = (phWidth / 2);

            //Define the text layout by setting the text alignment to centered
            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;

            //define a Brush which is semi trasparent black (Alpha set to 153)
            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));

            //Draw the Copyright string
            grPhoto.DrawString(FontString, //string of text
            crFont, //font
            semiTransBrush2, //Brush
            new PointF(xCenterOfImg + 1, yPosFromBottom + 1), //Position
            StrFormat);

            //define a Brush which is semi trasparent white (Alpha set to 153)
            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));

            //Draw the Copyright string a second time to create a shadow effect
            //Make sure to move this text 1 pixel to the right and down 1 pixel
            grPhoto.DrawString(FontString, //string of text
            crFont, //font
            semiTransBrush, //Brush
            new PointF(xCenterOfImg, yPosFromBottom), //Position
            StrFormat); //Text alignment

            //------------------------------------------------------------
            //Step #2 - Insert Watermark image
            //------------------------------------------------------------

            //Create a Bitmap based on the previously modified photograph Bitmap
            Bitmap bmWatermark = new Bitmap(bmPhoto);
            bmWatermark.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
            //Load this Bitmap into a new Graphic Object
            Graphics grWatermark = Graphics.FromImage(bmWatermark);

            //To achieve a transulcent watermark we will apply (2) color
            //manipulations by defineing a ImageAttributes object and
            //seting (2) of its properties.
            ImageAttributes imageAttributes = new ImageAttributes();

            //The first step in manipulating the watermark image is to replace
            //the background color with one that is trasparent (Alpha=0, R=0, G=0, B=0)
            //to do this we will use a Colormap and use this to define a RemapTable
            ColorMap colorMap = new ColorMap();

            //My watermark was defined with a background of 100% Green this will
            //be the color we search for and replace with transparency
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);

            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            //The second color manipulation is used to change the opacity of the
            //watermark. This is done by applying a 5x5 matrix that contains the
            //coordinates for the RGBA space. By setting the 3rd row and 3rd column
            //to 0.3f we achive a level of opacity
            float[][] colorMatrixElements = {
new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
new float[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f},
new float[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f},
new float[] {0.0f, 0.0f, 0.0f, 0.3f, 0.0f},
new float[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}};
            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
            ColorAdjustType.Bitmap);

            //For this example we will place the watermark in the upper right
            //hand corner of the photograph. offset down 10 pixels and to the
            //left 10 pixles

            int xPosOfWm = ((phWidth - wmWidth) - 10);
            int yPosOfWm = 10;

            grWatermark.DrawImage(imgWatermark,
            new Rectangle(xPosOfWm, yPosOfWm, wmWidth, wmHeight), //Set the detination Position
            0, // x-coordinate of the portion of the source image to draw.
            0, // y-coordinate of the portion of the source image to draw.
            wmWidth, // Watermark Width
            wmHeight, // Watermark Height
            GraphicsUnit.Pixel, // Unit of measurment
            imageAttributes); //ImageAttributes Object

            //Replace the original photgraphs bitmap with the new Bitmap
            imgPhoto = bmWatermark;
            grPhoto.Dispose();
            grWatermark.Dispose();

            //save new image to file system.
            imgPhoto.Save(WorkingDirectory + ImageName + "_finally.jpg", ImageFormat.Jpeg);
            imgPhoto.Dispose();
            imgWatermark.Dispose();

        }
    }


    #endregion





}
