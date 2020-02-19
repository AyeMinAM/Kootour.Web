using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using System.Web;

namespace MVCSite.Common
{
    public static class StreamHelper
    {

        public static string GetStringFromStream(Stream stream)
        {
            return GetStringFromStream(stream, System.Text.Encoding.Default);
        }

        public static string GetStringFromStream(Stream stream, System.Text.Encoding encoding)
        {
            stream.Position = 0;
            using (StreamReader streamReader = new StreamReader(stream, encoding))
            {
                var readResult= streamReader.ReadToEnd();
                streamReader.DiscardBufferedData();
                streamReader.Close();
                return readResult;
            }
        }

        public static MemoryStream GetMemoryStreamFromByteArray(byte[] data)
        {
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Write(data, 0, data.Length);
            return memoryStream;
        }

        public static MemoryStream GetMemoryStreamFromStream(Stream stream)
        {
            MemoryStream memoryStream = new MemoryStream();
            byte[] bs = new byte[256];
            for (int j = stream.Read(bs, 0, (int)bs.Length); j > 0; j = stream.Read(bs, 0, (int)bs.Length))
            {
                memoryStream.Write(bs, 0, j);
            }
            //stream.Close();
            memoryStream.Position = (long)0;
            return memoryStream;
        }



        public static byte[] Compress(string content)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(content);
            MemoryStream ms = new MemoryStream();
            Stream zipStream = new GZipStream(ms, CompressionMode.Compress);
            zipStream.Write(bytes, 0, bytes.Length);
            zipStream.Close();
            byte[] compressedData = (byte[])ms.ToArray();
            ms.Close();
            return compressedData;
        }

        public static byte[] Compress(string content, bool gb2312)
        {
            byte[] bytes = gb2312 ? System.Text.Encoding.GetEncoding("gb2312").GetBytes(content) : System.Text.Encoding.UTF8.GetBytes(content);
            MemoryStream ms = new MemoryStream();
            Stream zipStream = new GZipStream(ms, CompressionMode.Compress);
            zipStream.Write(bytes, 0, bytes.Length);
            zipStream.Close();
            byte[] compressedData = (byte[])ms.ToArray();
            ms.Close();
            return compressedData;
        }

        public static byte[] DeflateCompress(string content)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(content);
            MemoryStream ms = new MemoryStream();
            Stream zipStream = new DeflateStream(ms, CompressionMode.Compress);
            zipStream.Write(bytes, 0, bytes.Length);
            zipStream.Close();
            byte[] compressedData = (byte[])ms.ToArray();
            ms.Close();
            return compressedData;
        }

        public static byte[] DeflateCompress(string content, bool gb2312)
        {
            byte[] bytes = gb2312 ? System.Text.Encoding.GetEncoding("gb2312").GetBytes(content) : System.Text.Encoding.UTF8.GetBytes(content);
            MemoryStream ms = new MemoryStream();
            Stream zipStream = new DeflateStream(ms, CompressionMode.Compress);
            zipStream.Write(bytes, 0, bytes.Length);
            zipStream.Close();
            byte[] compressedData = (byte[])ms.ToArray();
            ms.Close();
            return compressedData;
        }
        public static byte[] Compress(byte[] unzippedBytes)
        {
            MemoryStream ms = new MemoryStream();
            Stream zipStream = new GZipStream(ms, CompressionMode.Compress);
            zipStream.Write(unzippedBytes, 0, unzippedBytes.Length);
            zipStream.Close();
            byte[] compressedData = (byte[])ms.ToArray();
            ms.Close();
            return compressedData;
        }

        public static byte[] DeflateCompress(byte[] unzippedBytes)
        {
            MemoryStream ms = new MemoryStream();
            Stream zipStream = new DeflateStream(ms, CompressionMode.Compress);
            zipStream.Write(unzippedBytes, 0, unzippedBytes.Length);
            zipStream.Close();
            byte[] compressedData = (byte[])ms.ToArray();
            ms.Close();
            return compressedData;
        }

    }


}