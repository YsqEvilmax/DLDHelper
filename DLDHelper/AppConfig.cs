using Google.Apis.Drive.v3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DLDHelper
{
    public struct MIMEType
    {
        public string MainType { get; set; }
        public string SubType { get; set; }

        public override string ToString()
        {
            return MainType + "/" + SubType;
        }
    }

    public class GoogleAPI
    {
        public string[] Scopes { get; set; }
        public string Credentials { get; set; }
    }

    public class Downloading
    {
        public string[] Postfixs { get; set; }
    }

    public class AppConfig
    {
        public string SourceFolder { get; set; }
        public string DestinationFolder { get; set; }
        public GoogleAPI GoogleAPIParams { get; set; }
        public List<MIMEType> MIMETypes { get; set; }
        public Downloading XunLeiDownLoading { get; set; }

        public void Seed()
        {
            this.SourceFolder = "D:\\New";
            this.DestinationFolder = "BT";
            this.GoogleAPIParams = new GoogleAPI();
            this.GoogleAPIParams.Credentials = "Credentials\\client.json";
            this.GoogleAPIParams.Scopes = new string[] { DriveService.Scope.DriveFile };
            this.MIMETypes = new List<MIMEType>()
            {
                new MIMEType {MainType = "video", SubType = "avi" },
                new MIMEType {MainType = "video", SubType = "mp4" },
                new MIMEType {MainType = "image", SubType = "jpeg" },
                new MIMEType {MainType = "image", SubType = "jpg" },
                new MIMEType {MainType = "application", SubType = "zip" },
                new MIMEType {MainType = "application", SubType = "rar" }
            };
            this.XunLeiDownLoading = new Downloading();
            this.XunLeiDownLoading.Postfixs = new string[]{ ".xltd", ".td"};
        }
    }



    public static class Serializer<T> where T : class
    {
        public static XmlSerializer xml = new XmlSerializer(typeof(T));
        public static void Serialize(string filename, T t)
        {
            try
            {
                using (Stream stream = new FileStream(filename,
             FileMode.Create,
             FileAccess.Write, FileShare.None))
                {
                    Serializer<T>.xml.Serialize(stream, t);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
        }

        public static T Deserialize(string filename)
        {
            T obj = null;
            try
            {
                using (Stream stream = new FileStream(filename,
              FileMode.Open,
              FileAccess.Read,
              FileShare.Read))
                {
                    obj = xml.Deserialize(stream) as T;
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return obj;
        }
    }
}
