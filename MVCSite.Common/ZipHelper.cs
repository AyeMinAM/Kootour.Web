using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Collections;
using HtmlAgilityPack;
using System.IO;
using System.Text;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using System.Xml.Linq;

namespace MVCSite.Common
{
    public static class ZipHelper
    {
        public static byte[] ZipMultiFiles(string dir, string[] filenames)
        {
            try
            {
                byte[] outputBuffer = null;
                string parentDir = dir, fullPath = string.Empty;
                if (!parentDir.EndsWith("\\"))
                    parentDir += "\\";
                //DateTime current = DateTime.Now;
                //string zipfName = "Request" + current.Date.Day.ToString() + current.Date.Month.ToString() + current.Date.Year.ToString() + current.TimeOfDay.Duration().Hours.ToString() + current.TimeOfDay.Duration().Minutes.ToString() + current.TimeOfDay.Duration().Seconds.ToString();
                // Zip up the files - From SharpZipLib Demo Code
                MemoryStream output = new MemoryStream();
                using (ZipOutputStream s = new ZipOutputStream(output))//(File.Create(dirwhereZip + "\\" + zipfName + ".zip")))
                {
                    s.SetLevel(9); // 0-9, 9 being the highest level of compression

                    byte[] buffer = new byte[4096];

                    foreach (string file in filenames)
                    {
                        fullPath = parentDir + file;
                        if (!File.Exists(fullPath) || IsDirectory(fullPath))
                            continue;
                        ZipEntry entry = new ZipEntry(file);//Path.GetFileName(file));
                        entry.DateTime = DateTime.Now;
                        s.PutNextEntry(entry);

                        using (FileStream fs = File.OpenRead(fullPath))
                        {
                            int sourceBytes;
                            do
                            {
                                sourceBytes = fs.Read(buffer, 0, buffer.Length);
                                s.Write(buffer, 0, sourceBytes);
                            }
                            while (sourceBytes > 0);
                        }

                    }
                    //StreamUtils.Copy(memStreamIn, s, new byte[4096]);

                    //outputBuffer=new byte[s.Length];
                    //s.Read(outputBuffer, 0, (int)s.Length);
                    s.IsStreamOwner = false;
                    s.Finish();
                    s.Close();
                }
                output.Position = 0;
                outputBuffer = output.ToArray();
                output.Close();
                return outputBuffer;
                // clean up files by deleting the temp folder and its content
                //System.IO.Directory.Delete(dirwhereZip + "\\TempZipFile\\", true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void UnZipFile(string inputPathOfZipFile,string password, string unzipToPath, bool extractAll, 
            string specificSingleFileFolder="", string specificSingleFileName="")
        {
            char[] delimiter = { '/' };
            string strentry = string.Empty;
            string[] strvalues;
            bool extractCurrent = false;
            try
            {
                if (File.Exists(inputPathOfZipFile))
                {
                    string baseDirectory = unzipToPath;
                    if (!Directory.Exists(baseDirectory))
                    {
                        Directory.CreateDirectory(baseDirectory);
                    }
                    using (ZipInputStream ZipStream = new ZipInputStream(File.OpenRead(inputPathOfZipFile)))
                    {
                        ZipEntry theEntry;
                        if (!string.IsNullOrEmpty(password))
                            ZipStream.Password = password;
                        while ((theEntry = ZipStream.GetNextEntry()) != null)
                        {
                            extractCurrent = false;
                            strentry = theEntry.ToString();
                            if (!extractAll)
                            {
                                strvalues = strentry.Split(delimiter);
                                if ((strvalues[strvalues.Length - 1].ToString().ToUpper() == specificSingleFileName.ToUpper()) &&
                                    (strvalues[strvalues.Length - 2].ToString().ToUpper() == specificSingleFileFolder.ToUpper()))
                                {
                                    extractCurrent = true;
                                }
                            }
                            else
                                extractCurrent = true;
                            if (extractCurrent)
                            {
                                string directoryName = Path.GetDirectoryName(theEntry.Name);
                                if (directoryName.Length > 0)
                                {
                                    Directory.CreateDirectory(@"" + baseDirectory + @"\" + directoryName);
                                }
                                if (theEntry.IsFile)
                                {
                                    if (theEntry.Name != "")
                                    {

                                        string strNewFile = @"" + baseDirectory + @"\" + theEntry.Name;
                                        if (File.Exists(strNewFile))
                                        {
                                            continue;
                                        }
                                        using (FileStream streamWriter = File.Create(strNewFile))
                                        {
                                            int size = 2048;
                                            byte[] data = new byte[2048];
                                            while (true)
                                            {
                                                size = ZipStream.Read(data, 0, data.Length);
                                                if (size > 0)
                                                    streamWriter.Write(data, 0, size);
                                                else
                                                    break;
                                            }
                                            streamWriter.Close();
                                        }
                                    }
                                }
                                else if (theEntry.IsDirectory)
                                {
                                    string strNewDirectory = @"" + baseDirectory + @"\" + theEntry.Name;

                                    if (!Directory.Exists(strNewDirectory))
                                    {
                                        Directory.CreateDirectory(strNewDirectory);
                                    }
                                }
                            }
                        }
                        ZipStream.Close();
                    }
                }
            }
            catch (Exception excp)
            {
                throw;
            }
            return;
        }


        public static string gzipCompress(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            MemoryStream ms = new MemoryStream();
            using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
            {
                zip.Write(buffer, 0, buffer.Length);
            }

            ms.Position = 0;
            MemoryStream outStream = new MemoryStream();

            byte[] compressed = new byte[ms.Length];
            ms.Read(compressed, 0, compressed.Length);

            byte[] gzBuffer = new byte[compressed.Length + 4];
            System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
            return Convert.ToBase64String(gzBuffer);
        }

        public static string gzipDecompress4Net(string compressedText)
        {
            byte[] gzBuffer = Convert.FromBase64String(compressedText);
            using (MemoryStream ms = new MemoryStream())
            {
                int msgLength = BitConverter.ToInt32(gzBuffer, 0);
                ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

                byte[] buffer = new byte[msgLength];

                ms.Position = 0;
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    zip.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }
        //http://stackoverflow.com/questions/6717165/how-can-i-zip-and-unzip-a-string-using-gzipoutputstream-that-is-compatible-with
        //	//Without 4 bytes to compatible with android
        public static string gzipDecompress4Mobile(string compressedText)
        {
            byte[] gzip = Convert.FromBase64String(compressedText);
            // Create a GZIP stream with decompression mode.
            // ... Then create a buffer and write into while reading from the GZIP stream.
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return Encoding.UTF8.GetString(memory.ToArray());
                }
            }
        }

        private static bool IsDirectory(string path)
        {
            System.IO.FileAttributes fa = System.IO.File.GetAttributes(path);
            bool isDirectory = false;
            if ((fa & FileAttributes.Directory) == FileAttributes.Directory)
            {
                isDirectory = true;
            }
            return isDirectory;
        }
        public static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        public static string GZZip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    msi.CopyTo(gs);
                    //CopyTo(msi, gs);
                }

                var zipBytes = mso.ToArray();
                string result = Encoding.UTF8.GetString(zipBytes, 0, zipBytes.Length);
                return result;
            }
        }

        public static byte[] GZUnzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    gs.CopyTo(mso);
                    //CopyTo(gs, mso);
                }

                return mso.ToArray();
            }
        }
        public static string GZUnzipToString(byte[] bytes)
        {
            return Encoding.UTF8.GetString(GZUnzip(bytes));
        }
    }
}