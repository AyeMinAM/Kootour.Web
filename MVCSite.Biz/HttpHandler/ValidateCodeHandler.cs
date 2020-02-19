using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Drawing;
using WMath.Facilities;
using System.Drawing.Imaging;



namespace MVCSite.Biz.HttpHandler
{
	public sealed class ValidateCodeHandler : IHttpHandler
	{
		#region IsReusable

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		#endregion

		public void ProcessRequest( HttpContext context )
		{
			context.Response.Clear ();
			context.Response.Buffer = false;
			context.Response.Expires = 0;
			context.Response.ExpiresAbsolute = DateTime.Now.AddDays ( -1 );
            ResponseValidateCode(context);
			context.Response.End ();
        }

        private static void ResponseValidateCode( HttpContext context )
		{
            //string validateCodeString = DataProtection.Decrypt(context.Request.QueryString["code"], DataProtection.Store.User);
			string validateCodeString = VCode.Decode ( context.Request.QueryString ["code"] );
            if (validateCodeString.Length == 0) return;

			List<Image> images = new List<Image>();
			int width = 0;
			foreach ( char validateCodeChar in validateCodeString )
			{
				string imagePath = "~\\Images\\ValidateCode\\" + validateCodeChar + ".gif";
				Image image = Image.FromFile ( context.Server.MapPath ( imagePath ) );
				width += image.Width;
				images.Add( image );
			}
			Bitmap validateCodeBitmap = new Bitmap( width, 37 );
			Graphics graphics = Graphics.FromImage( validateCodeBitmap );
			graphics.Clear ( Color.Transparent );
			//System.Drawing.Image backgroundImage = System.Drawing.Image.FromFile ( context.Server.MapPath ( "~/Images/mixed.gif" ) );
			//graphics.DrawImage ( backgroundImage, 0, 0, width, 37 );
			//backgroundImage.Dispose ();
			int x = 0;
			foreach ( Image image in images )
			{
				graphics.DrawImage( image, x, 0 );
				x += image.Width;
				image.Dispose();
			}
			graphics.Flush();
			graphics.Dispose();
			DistortImage( validateCodeBitmap, 5.0 );
			MemoryStream stream = new MemoryStream();
			validateCodeBitmap.Save( stream, ImageFormat.Gif );
			validateCodeBitmap.Dispose();
			byte[] buffer = new byte[stream.Length];
			stream.Position = 0;
			stream.Read( buffer, 0, buffer.Length );
			context.Response.ContentType = "image/gif";
			context.Response.BinaryWrite ( buffer );
		}

		/// <summary>
		/// 取得验证码
		/// </summary>
		private static void OldResponseValidateCode( HttpContext context )
		{
			//generate a random number.
			Random ro = new Random ();
			string code = VCode.Decode ( context.Request.QueryString ["code"] );

			//string code = DataProtection.Decrypt ( context.Request.QueryString ["code"], DataProtection.Store.User );
			if ( code.Length == 0 )
			{
				code = "ERRO";
			}
			//get mixed image file and create a new bitmap.
			System.Drawing.Image theMixedImage = System.Drawing.Image.FromFile ( context.Server.MapPath ( "~/Images/mixed.gif" ) );
			Bitmap bmpMixed = new Bitmap ( theMixedImage );
			Bitmap bmpCode = new Bitmap ( 150, 40, PixelFormat.Format24bppRgb );
			//draw the mixed background.
			Graphics g = Graphics.FromImage ( bmpCode );
			g.DrawImage ( bmpMixed, new Rectangle ( ro.Next ( -150, 0 ), ro.Next ( -30, 0 ), 300, 80 ), 0, 0, bmpMixed.Width, bmpMixed.Height, GraphicsUnit.Pixel );
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
			//draw the digits.
			Font drawFont = null;
			Color BrushColor = Color.Red;
			Single x, y;

			for ( int i = 0; i < 4; ++i )
			{
				x = ro.Next ( i * 34, i * 34 + 8 );
				y = ro.Next ( i, 6 + i );

				switch ( ro.Next ( 1, 6 ) )
				{
					case 1:
						BrushColor = Color.DarkGreen;
						break;
					case 2:
						BrushColor = Color.DarkViolet;
						break;
					case 3:
						BrushColor = Color.Blue;
						break;
					case 4:
						BrushColor = Color.DarkOrange;
						break;
					case 5:
						BrushColor = Color.Red;
						break;
					case 6:
						BrushColor = Color.Purple;
						break;
				}

				SolidBrush drawBrush = new SolidBrush ( BrushColor );


				switch ( ro.Next ( 1, 3 ) )
				{
					case 1:
						drawFont = new Font ( "Verdana", 20, FontStyle.Bold );
						break;
					default:
						drawFont = new Font ( "Verdana", 20, FontStyle.Italic | FontStyle.Bold );
						break;
				}
				g.DrawString ( code.Substring ( i, 1 ), drawFont, drawBrush, x, y );
			}

			DistortImage ( bmpCode, 5.0 );

			bmpCode.Save ( context.Response.OutputStream, ImageFormat.Jpeg );
			g.Dispose ();
			theMixedImage.Dispose ();
			bmpMixed.Dispose ();
			bmpCode.Dispose ();
		}

		private static void DistortImage( Bitmap b, double distortion )
		{
			int width = b.Width, height = b.Height;
			FastBitmap fastBitmap = new FastBitmap ( b );
			using ( Bitmap copy = ( Bitmap ) b.Clone () )
			{
				FastBitmap copyfastBitmap = new FastBitmap ( copy );
				for ( int y = 0; y < height; y++ )
				{
					for ( int x = 0; x < width; x++ )
					{
						int newX = ( int ) ( x + ( distortion * Math.Sin ( Math.PI * y / 64.0 ) ) );
						int newY = ( int ) ( y + ( distortion * Math.Cos ( Math.PI * x / 64.0 ) ) );
						if ( newX < 0 || newX >= width )
							newX = 0;
						if ( newY < 0 || newY >= height )
							newY = 0;
						fastBitmap.SetPixel ( x, y, copyfastBitmap.GetPixel ( newX, newY ) );
					}
				}
				copyfastBitmap.Release ();
			}
			fastBitmap.Release ();
			//int width = b.Width, height = b.Height;

			//using ( Bitmap copy = ( Bitmap ) b.Clone () )
			//{

			//    for ( int y = 0; y < height; y++ )
			//    {

			//        for ( int x = 0; x < width; x++ )
			//        {

			//            int newX = ( int ) ( x + ( distortion * Math.Sin ( Math.PI * y / 64.0 ) ) );

			//            int newY = ( int ) ( y + ( distortion * Math.Cos ( Math.PI * x / 64.0 ) ) );

			//            if ( newX < 0 || newX >= width )
			//                newX = 0;

			//            if ( newY < 0 || newY >= height )
			//                newY = 0;

			//            b.SetPixel ( x, y, copy.GetPixel ( newX, newY ) );

			//        }

			//    }

			//}

        }

    }
}
