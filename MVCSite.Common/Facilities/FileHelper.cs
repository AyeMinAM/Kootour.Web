using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.AccessControl;
using System.Text;
using System.IO;
using System.Security.Permissions;
using System.Threading;

namespace MVCSite.Common
{
	/// <summary>
	/// 文件操作类
	/// </summary>
	public static class FileHelper
	{
        public static string GlobalTimeFormat = @"yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// Get the full local directory of a local file
        /// </summary>
        /// <param name="localURI">the file with full path</param>
        /// <returns>the full local directory with "\"</returns>
        public static string GetFullLocalDirectoryOfFile(string localURI)
        {
            int lastIndex = localURI.LastIndexOf(@"\");
            if (lastIndex < 0)
            {
                //Sometimes we copy the file URI from the navigator address bar,this include "/"
                lastIndex = localURI.LastIndexOf(@"/");
            }
            if (lastIndex < 0)
            { //Still can NOT find,this might be wrong,return directly.
                return string.Empty;
            }
            return localURI.Substring(0, lastIndex+1);
        }

        /// <summary>
        /// Get the physical path for a designated relative path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Check whether the extension is a valid file extension string.
        /// </summary>
        /// <returns>true :valid file extension.</returns>
        public static bool  IsValidFileExtension(string extension)
        {
            string lowerExt=extension.ToLower();
            if (lowerExt == ".doc" || lowerExt == ".ppt" || lowerExt == ".rar" || lowerExt == ".zip"
                || lowerExt == ".jpg" || lowerExt == ".jpeg" || lowerExt == ".gif" || lowerExt == ".htm" || lowerExt == ".html")
                return true;
            else
                return false;


        }
        /// <summary>
        /// Get the relative fie directory separated by hours.
        /// </summary>
        /// <returns>the relative file directory</returns>
        public static string GetRelativeDirSeparatedByHour()
        {
            string relativeDir = string.Empty;
            relativeDir = DateTime.Today.Year.ToString() + @"\" +
                DateTime.Today.DayOfYear.ToString() + @"\" + DateTime.Now.Hour.ToString() + @"\";
            return relativeDir;
        }

        public static bool DownloadWordFileFromUri(string sourceUri, string destPath, ref string relativeDocFile, ref int fileSize, ref string originalFileURI)
        { 
            return DownloadWordFileFromUri(sourceUri,string.Empty, destPath, ref  relativeDocFile, ref  fileSize, ref  originalFileURI);
        }
        /// <summary>
        /// Download a word file from internet and save it on local disk
        /// </summary>
        /// <param name="sourceUri">the word file uri on the internet</param>
        /// <param name="destPath">the path save on local disk</param>
        /// <param name="fileSize">the size of the word file</param>
        /// <returns>true:succeed</returns>
        public static bool DownloadWordFileFromUri(string sourceUri,string referer,string destPath, ref string relativeDocFile, ref int fileSize, ref string originalFileURI)
        {
            byte[] fileBytes = null;
            string fullFileName = string.Empty;
            string wordFileName = string.Empty;
            string relativePath = string.Empty;
            try
            {
                using (MemoryStream msRequest = RequestHelper.GetRequest(sourceUri,referer, ref originalFileURI))
                {
                    using (MemoryStream ms = StreamHelper.GetMemoryStreamFromStream(msRequest))
                    {
                        fileBytes = ms.ToArray();
                        ms.Close();
                    }
                    msRequest.Close();
                }
                if (fileBytes != null && fileBytes.Length > 0)
                {
                    fileSize = fileBytes.Length;
                    relativePath = FileHelper.GetRelativeDirSeparatedByHour();
                    //If the source file have extension,we should retrieve it.
                    //e.g. sometimes it might be a PPT file.
                    string fileExtension = StringHelper.GetFileExtension(originalFileURI);

                    wordFileName = FileHelper.GetNewDocumentFileName(fileExtension);
                    relativeDocFile = relativePath + wordFileName;
                    if (destPath.EndsWith("\\"))
                        fullFileName = destPath + relativeDocFile;
                    else
                        fullFileName = destPath + "\\" + relativeDocFile;

                    FileHelper.SaveFileBinary(fileBytes, fullFileName);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            fileBytes = null;
            return true;
        }

        public static byte[] DownloadWordFileFromUriStep1(string sourceUri, ref string originalFileURI)
        {
            return DownloadWordFileFromUriStep1(sourceUri, string.Empty, ref originalFileURI);
        }

        /// <summary>
        /// Download a word file from internet in 2 steps and save it on local disk.
        /// This is the first step:download it from the internet.
        /// </summary>
        /// <param name="sourceUri">the word file uri on the internet</param>
        /// <param name="originalFileURI">the real document URI to return on the internet(sometimes the downloaded document is moved to another place.)</param>
        /// <returns>byte[]: the byte arrary containing the document contents.</returns>
        public static byte[] DownloadWordFileFromUriStep1(string sourceUri,string referer,ref string originalFileURI)
        {
            byte[] fileBytes = null;
            string fullFileName = string.Empty;
            string wordFileName = string.Empty;
            string relativePath = string.Empty;
            try
            {
                using (MemoryStream msRequest = RequestHelper.GetRequest(sourceUri,referer,ref originalFileURI))
                {
                    using (MemoryStream ms = StreamHelper.GetMemoryStreamFromStream(msRequest))
                    {
                        fileBytes = ms.ToArray();
                        ms.Close();
                    }
                    msRequest.Close();
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            return fileBytes;
        }

        /// <summary>
        /// Download a word file from internet in 2 steps and save it on local disk.
        /// This is the second step:save it to the local disk.
        /// </summary>
        /// <param name="fileBytes">the byte array containing the document contents.</param>
        /// <param name="destPath">the destination directory to save the document.</param>
        /// <param name="relativeDocFile">the relative document path to return.</param>
        /// <param name="fileSize">the file size to return</param>
        /// <param name="originalFileURI">the real document URI to return on the internet(sometimes the downloaded document is moved to another place.)</param>
        /// <returns>true: succeed to save to disk s</returns>
        public static bool DownloadWordFileFromUriStep2(byte[] fileBytes, string destPath, ref string relativeDocFile, ref int fileSize, string originalFileURI)
        {
            string fullFileName = string.Empty;
            string wordFileName = string.Empty;
            string relativePath = string.Empty;
            try
            {
                if (fileBytes != null && fileBytes.Length > 0)
                {

                    fileSize = fileBytes.Length;
                    relativePath = FileHelper.GetRelativeDirSeparatedByHour();
                    //If the source file have extension,we should retrieve it.
                    //e.g. sometimes it might be a PPT file.
                    string fileExtension = StringHelper.GetFileExtension(originalFileURI);

                    wordFileName = FileHelper.GetNewDocumentFileName(fileExtension);
                    relativeDocFile = relativePath + wordFileName;
                    if (destPath.EndsWith("\\"))
                        fullFileName = destPath + relativeDocFile;
                    else
                        fullFileName = destPath + "\\" + relativeDocFile;

                    FileHelper.SaveFileBinary(fileBytes, fullFileName);
                }
            }
            catch (Exception e)
            {
                relativeDocFile = string.Empty;
                fileSize = 0;
                throw e;
            }
            fileBytes = null;
            return true;
        }


        /// <summary>
        /// Create a new document file name,the default is word ".doc" file name.
        /// </summary>
        /// <returns>The new document file name</returns>
        public static string GetNewDocumentFileName(string extension)
        {
            Random ran = new Random();
            string strRan = string.Empty;
            StringBuilder sbRan = new StringBuilder(10);
            while (sbRan.Length < 5)
            {
                sbRan.Append(ran.Next().ToString());
            }
            if (sbRan.Length > 5)
                strRan = sbRan.ToString().Substring(0, 5);

            StringBuilder sbFileName = new StringBuilder(60);
            sbFileName.Append(DateTime.Today.Year.ToString());
            sbFileName.Append(DateTime.Today.Month.ToString().PadLeft(2, '0'));
            sbFileName.Append(DateTime.Today.Day.ToString().PadLeft(2, '0'));
            sbFileName.Append(DateTime.Now.Hour.ToString().PadLeft(2, '0'));
            sbFileName.Append(DateTime.Now.Minute.ToString().PadLeft(2, '0'));
            sbFileName.Append(DateTime.Now.Second.ToString().PadLeft(2, '0'));
            sbFileName.Append(DateTime.Now.Millisecond.ToString().PadLeft(3, '0'));
            sbFileName.Append(".");
            sbFileName.Append(strRan);
            if (extension.Length > 0 && FileHelper.IsValidFileExtension(extension))
                sbFileName.Append(extension);
            else
            {//By default, we create a word document file.
                sbFileName.Append(".doc");
            }
            return sbFileName.ToString();
        }


        /// <summary>
        /// Get the absolute full file name when inputing a file path and relative file name
        /// </summary>
        /// <param name="path">the file path</param>
        /// <param name="relativeName">the relative file name</param>
        /// <returns>the absolute full file name</returns>
        public static string GetAbsoluteFullFileName(string path, string relativeName)
        {
            if (path == string.Empty)
                return string.Empty;
            string fullFileName=string.Empty;
            string filePath = path;
            string fileRelative = relativeName;

            if (FileHelper.IsLocalFile(path))
            {//E:\51mathimage2\pai\
                if (path.IndexOf(@"/") >= 0)
                    filePath = path.Replace(@"/", @"\");
                if (relativeName.IndexOf(@"/") >= 0)
                    fileRelative = relativeName.Replace(@"/", @"\");
                if (!filePath.EndsWith(@"\") && !fileRelative.StartsWith(@"\"))
                    fullFileName = filePath + @"\" + fileRelative;
                else if (filePath.EndsWith(@"\") && fileRelative.StartsWith(@"\"))
                    fullFileName = filePath.Substring(0, filePath.Length - 1) + fileRelative;
                else
                    fullFileName = filePath + fileRelative;
            }
            else
            { //http://localhost/51mathimage2/pai
                if (path.IndexOf(@"\") >= 0)
                    filePath = path.Replace(@"\", @"/");
                if (relativeName.IndexOf(@"\") >= 0)
                    fileRelative = relativeName.Replace(@"\", @"/");
                if (!filePath.EndsWith(@"/") && !fileRelative.StartsWith(@"/"))
                    fullFileName = filePath + @"/" + fileRelative;
                else if (filePath.EndsWith(@"/") && fileRelative.StartsWith(@"/"))
                    fullFileName = filePath.Substring(0, filePath.Length - 1) + fileRelative;
                else
                    fullFileName = filePath + fileRelative;
            
            }
            return fullFileName.Trim();
        
        }

        /// <summary>
        /// Check whether a source file is local file
        /// </summary>
        /// <param name="fileName">the file with path to be checked with</param>
        /// <returns>true:local file,it means this file is located in this computer or in the internal network(LAN).</returns>
        public static bool IsLocalFile(string file)
        {
            if (file.StartsWith("http:") || file.StartsWith("HTTP:"))
                return false;
            else
                return true;

        }

        /// <summary>
        /// Log informatin at the designated physical path
        /// </summary>
        /// <param name="filePhysicalPath"></param>
        /// <param name="text"></param>
		public static void Log( string filePhysicalPath, string text )
		{
			StringBuilder stringBuilder = new StringBuilder ();
            //stringBuilder.Append ( "///---Begin(" );
            //stringBuilder.Append ( System.DateTime.Now.ToString ( "yyyy-MM-dd HH:mm:ss" ) );
            //stringBuilder.Append ( ")---///" );
            stringBuilder.Append ( System.DateTime.Now.ToString ( FileHelper.GlobalTimeFormat ) );
            //stringBuilder.Append ( Environment.NewLine );
			stringBuilder.Append ( text );
			stringBuilder.Append ( Environment.NewLine );
            //stringBuilder.Append ( "///----End----///" );
			Write ( filePhysicalPath, stringBuilder.ToString () );
		}

		public static void Write( string filePhysicalPath, string text )
		{
			CheckDirectory ( filePhysicalPath );
			try
			{
				System.IO.FileStream fileStream;
				if ( !System.IO.File.Exists ( filePhysicalPath ) )
				{
					fileStream = new System.IO.FileStream ( filePhysicalPath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite );
				}
				else
				{
					fileStream = new System.IO.FileStream ( filePhysicalPath, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite );
				}
				System.IO.StreamWriter streamWriter = new System.IO.StreamWriter ( fileStream, System.Text.Encoding.Default );
				streamWriter.Write ( text );
				streamWriter.WriteLine ();
				streamWriter.Close ();
				fileStream.Close ();
			}
			catch ( Exception exception )
			{
				throw( exception );
			}
		}


		public static void Unzip( Stream srcStream, Stream destStream )
		{
			using ( GZipStream input = new GZipStream ( srcStream, CompressionMode.Decompress ) )
			{
				byte [] bytes = new byte [4096];
				int n;
				while ( ( n = input.Read ( bytes, 0, bytes.Length ) ) != 0 )
				{
					destStream.Write ( bytes, 0, n );
				}
			}
		}
        public static string GetFileExtension(string fileName)
        {
            return GetFileExtension(fileName, string.Empty);
        }
		public static string GetFileExtension( string fileName,string defaultExt)
		{
			if ( string.IsNullOrEmpty ( fileName ) )
                return defaultExt;
			string extension= fileName.Substring ( fileName.LastIndexOf ( "." ) + 1 );
            if (string.IsNullOrEmpty(extension))
                return defaultExt;
            if(extension.Length>4)
                return defaultExt;
            return extension;        
		}

		public static string GetFileNameNoExtension( string fileName )
		{
			if ( string.IsNullOrEmpty ( fileName ) )
				return string.Empty;
			return fileName.Substring ( 0, fileName.LastIndexOf ( "." ) );
		}

        /// <summary>
        /// 保存文本文件内容
        /// </summary>
        /// <param name="content">要保存的内容</param>
        /// <param name="filePath">文件路径</param>
        public static void SaveZipFileText( string content, string filePath )
        {
        	byte[] bytes = StreamHelper.Compress( content );
        	SaveFileBinary( bytes, filePath );
        }

		public static void SaveDeflateCompressFileText( string content, string filePath )
		{
			byte [] bytes = StreamHelper.DeflateCompress( content );
			SaveFileBinary ( bytes, filePath );
		}


		#region 保存文本文件内容
		/// <summary>
		/// 保存文本文件内容
		/// </summary>
		/// <param name="content">要保存的内容</param>
		/// <param name="filePath">文件路径</param>
		public static void SaveFileText( string content, string filePath )
		{
			CheckDirectory ( filePath );
			using ( StreamWriter sw = File.CreateText ( filePath ) )
			{
				using( TextWriter tw = TextWriter.Synchronized ( sw ) )
				{
					tw.Write ( content );
					tw.Close ();
				}
			}
		}

		public static void CheckDirectory( string filePath )
		{
			string path = Path.GetDirectoryName ( filePath );
			if ( !Directory.Exists ( path ) )
			{
				Directory.CreateDirectory ( path );
			}
		}

		#endregion

		#region 取得文本文件内容
		/// <summary>
		/// 取得文本文件内容
		/// </summary>
		/// <param name="filePath">文件路径</param>
		/// <returns>文本文件内容</returns>
		public static string GetFileText( string filePath )
		{
			using ( StreamReader streamReader = File.OpenText ( filePath ) )
			{
				return streamReader.ReadToEnd ();
			}
		}
		#endregion


		#region 保存二进制文件内容
		/// <summary>
		/// 保存二进制文件内容
		/// </summary>
		/// <param name="content">要保存的内容</param>
		/// <param name="filePath">文件路径</param>
		public static void SaveFileBinary( byte [] content, string filePath )
		{
			CheckDirectory( filePath );
			using ( FileStream fs = new FileStream ( filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite ) )
			{
				using( BinaryWriter bw = new BinaryWriter ( fs ) )
				{
					bw.Write ( content );
					//for ( int i = 0, count = content.Length; i < count; i++ )
					//{
					//    bw.Write ( content [i] );
					//}
                    bw.Close();
				}
                fs.Close();
			}
		}

		public static void SaveFileBinary( byte [] content1, byte [] content2, string filePath )
		{
			CheckDirectory ( filePath );
			using ( FileStream fs = new FileStream ( filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite ) )
			{
				using ( BinaryWriter bw = new BinaryWriter ( fs ) )
				{
					bw.Write ( content1 );
					bw.Write ( content2 );
					//for ( int i = 0, count = content.Length; i < count; i++ )
					//{
					//    bw.Write ( content [i] );
					//}
				}
			}
		}

		#endregion

		#region 读取二进制文件内容
		/// <summary>
		/// 读取二进制文件内容
		/// </summary>
		/// <param name="filePath">文件路径</param>
		public static byte [] GetFileBinary( string filePath )
		{
			byte[] content = null;
			if ( File.Exists( filePath ) )
			{
				using ( FileStream fs = new FileStream ( filePath, FileMode.Open, FileAccess.Read, FileShare.Read ) )
				{
					using ( BinaryReader br = new BinaryReader ( fs ) )
					{
						content = br.ReadBytes ( Convert.ToInt32 ( fs.Length ) );
					}

				}
			}
			return content;
		}
		#endregion

        #region Delete a existing file
        public static void DeleteFileAndParentDirIfEmpty(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;
            if (File.Exists(fileName))
                FileHelper.Delete(fileName);
            string directoryName = Path.GetDirectoryName(fileName);
            if (string.IsNullOrEmpty(directoryName))
                return;
            if (!Directory.Exists(directoryName))
                return;
            var files = Directory.GetFiles(directoryName);
            if (files == null || files.Length <= 0)
            {
                Directory.Delete(directoryName);
            }
        }
        public static void Delete( string fileName )
		{
			int retries = 10;
			int msecsBetweenRetries = 100;
			while ( retries > 0 )
			{
				try
				{
					File.Delete ( fileName );
				}
				catch(Exception exp)
				{
					retries--;
					Thread.Sleep ( msecsBetweenRetries );
                    throw(new Exception(string.Format("Error deleting {0}, Message:{1},try for another {2} times.",
                        fileName, exp.Message, retries)));
				}
				break;
			}
			if ( retries == 0 )
			{
				throw ( new Exception ( string.Format ( "Error deleting {0}, perhaps it is in use by another process?", fileName ) ) );
			}
        }
        #endregion Delete a existing file

        #region Copy a file and a directory
        /// <summary>
        /// Copy a file to a designated path
        /// </summary>
        /// <param name="sourceFileName">the source file name</param>
        /// <param name="destFilePath">the destination path</param>
        public static void CopyFileTo(string sourceFileFullName, string destFilePath)
        {
            string outFileFullName = string.Empty;
            CopyFileTo(sourceFileFullName, destFilePath, ref outFileFullName);
        }
        /// <summary>
        /// Copy a file to a designated path
        /// </summary>
        /// <param name="sourceFileName">the source file name</param>
        /// <param name="destFilePath">the destination path</param>
        /// <param name="outFileFullName">the output file full name,empty if the copy operation fails.</param>
        public static void CopyFileTo(string sourceFileFullName, string destFilePath, ref string outFileFullName)
        {

            FileInfo sourceFileInfo = new FileInfo(sourceFileFullName);
            string sourceFileName = sourceFileInfo.Name;
           
            string destFullFileName = string.Empty;
            if (destFilePath.EndsWith("\\"))
                destFullFileName = destFilePath + sourceFileName;
            else
                destFullFileName = destFilePath +"\\"+ sourceFileName;

            outFileFullName = string.Empty;

            //check the destinatoin path
            if (!Directory.Exists(destFilePath))
            {
                Directory.CreateDirectory(destFilePath);
            }

            if (!File.Exists(sourceFileFullName))
            {
                throw new Exception(string.Format("Error moving {0}, this file does NOT exist.", sourceFileFullName));
            }

            if (File.Exists(destFullFileName))
            {
                throw new Exception(string.Format("Moving destination file {0} exists, this file is deleted.", destFullFileName));
                File.Delete(destFullFileName);
            }
            try
            {
                sourceFileInfo.CopyTo(destFullFileName,true);
                outFileFullName = destFullFileName;
            }
            catch (Exception ex)
            {
                throw(new Exception(string.Format("Moving file failed(Source file {0} move to {1}).",sourceFileName,destFullFileName)));
            }

        }

        /// <summary>
        /// Copy a source directory and all its contents to a destination directory.
        /// </summary>
        /// <param name="sourceDirectory">the source directory</param>
        /// <param name="destDirectory">the destination directory</param>
        public static void CopyDirectoryTo(string sourceDirectory, string destDirectory)
        {
            //check the sourceDirectory 
            if (!Directory.Exists(sourceDirectory))
            {
                throw  new Exception(string.Format("Error copy directory, this source directory {0} does NOT exist.", sourceDirectory));

            }
            //check the destDirectory 
            if (!Directory.Exists(destDirectory))
            {
                Directory.CreateDirectory(destDirectory);

            }
            DirectoryInfo directory = new DirectoryInfo(sourceDirectory);
            DirectoryInfo[] subDirectorys = directory.GetDirectories("*.*");
            FileInfo[] subFiles = directory.GetFiles();
            for (int i = 0, count = subFiles.Length; i < count; i++)
            {
                FileHelper.CopyFileTo(subFiles[i].FullName, destDirectory);

            }
            string destSubDirectory = string.Empty;
            foreach (DirectoryInfo directoryInfo in subDirectorys)
            {
                if (destDirectory.EndsWith("\\"))
                    destSubDirectory = destDirectory + directoryInfo.Name;
                else
                    destSubDirectory = destDirectory + "\\" + directoryInfo.Name;
                FileHelper.CopyDirectoryTo(directoryInfo.FullName, destSubDirectory);
            
            }
        
        }
        /// <summary>
        /// Get  "copy to " directory
        /// </summary>
        /// <returns></returns>
        public static string GetDirOfCopyToDirectory(string rootDir)
        {
            string copyToDir = string.Empty;
            if (rootDir.EndsWith("\\"))
                copyToDir = rootDir + DateTime.Today.Year.ToString() + @"\" + (DateTime.Today.DayOfYear / 7).ToString() + @"\";
            else
                copyToDir = rootDir + @"\" + DateTime.Today.Year.ToString() + @"\" + (DateTime.Today.DayOfYear / 7).ToString() + @"\";
            return copyToDir;
        }
        #endregion Move a file and a directory


        /// <summary>
        /// Create a string with 5 random digits 
        /// </summary>
        /// <returns>a string with 5 random digits</returns>
        public static string Get5DigitsRandomString()
        {
            Random ran = new Random();
            string strRan = string.Empty;
            StringBuilder sbRan = new StringBuilder(10);
            while (sbRan.Length < 5)
            {
                sbRan.Append(ran.Next().ToString());
            }
            if (sbRan.Length > 5)
                strRan = sbRan.ToString().Substring(0, 5);

            return strRan;
        }


    }
}
