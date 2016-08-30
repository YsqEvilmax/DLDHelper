using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace DLDHelper
{
    public class GoogleDriveUploader
    {
        private AppConfig m_Config;
        public AppConfig Config { get {return m_Config; } }
        private DriveService m_Service;

        public GoogleDriveUploader(AppConfig config)
        {
            m_Config = config;
        }

        [LogHelper(LogHelperAttribute.Target.Log)]
        public void Init()
        {
            UserCredential credential = null;

            if (string.IsNullOrEmpty(m_Config.GoogleAPIParams.Credentials))
            {
                throw new Exception("The Google API setting: Credentials is invalid!");
            }

            try
            {
                using (var stream = new FileStream(m_Config.GoogleAPIParams.Credentials, FileMode.Open, FileAccess.Read))
                {
                    string credPath = System.Environment.GetFolderPath(
                        System.Environment.SpecialFolder.Personal);
                    credPath = Path.Combine(credPath, ".credentials/drive-dotnet.json");

                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        m_Config.GoogleAPIParams.Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);
                }

                // Create Drive API service.
                if (m_Service == null)
                {
                    m_Service = new DriveService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = AppDomain.CurrentDomain.FriendlyName
                    });
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public void Upload(string path)
        {
            System.Console.WriteLine("Uploading {0}", path);
            if (Directory.Exists(path)) {
                UploadFolder(Directory.CreateDirectory(path), Get(m_Config.DestinationFolder));
            }
            else if (System.IO.File.Exists(path))
            {
                UploadFile(new FileInfo(path), Get(m_Config.DestinationFolder));
            }
            else
            {
                System.Console.WriteLine("The path {0} is invalid", path);
            }
            System.Console.WriteLine("Uploaded!", path);
        }

        public FileList List()
        {
            FilesResource.ListRequest listRequest = m_Service.Files.List();
            var files = listRequest.Execute();
            return files;
        }

        public Google.Apis.Drive.v3.Data.File Get(string name)
        {
            var files = List().Files;
            return files.SingleOrDefault(x => x.Name == name);
        }

        protected void UploadFolder(DirectoryInfo dir, Google.Apis.Drive.v3.Data.File root = null)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File();
            fileMetadata.Name = dir.Name;
            fileMetadata.MimeType = "application/vnd.google-apps.folder";
            //fileMetadata.Trashed = false;
            if(root != null)
            {
                fileMetadata.Parents = new List<string> { root.Id};
            }

            var res = Get(fileMetadata.Name);
            if (res == null)
            {
                var request = m_Service.Files.Create(fileMetadata);
                request.Fields = "id";
                res = request.Execute();
                Console.WriteLine("Folder ID: " + res.Id + "has been created!");
            }

            foreach (FileInfo fInfo in dir.GetFiles()
                .Where(x => m_Config.MIMETypes
                .Any(y => ("." + y.SubType)
                .Equals(x.Extension, StringComparison.OrdinalIgnoreCase))))
            {
                UploadFile(fInfo, res);
            }

            foreach(DirectoryInfo dInfo in dir.GetDirectories())
            {
                UploadFolder(dInfo, res);
            }
        }

        protected void UploadFile(FileInfo file, Google.Apis.Drive.v3.Data.File root = null)
        {
            //string folderId = file.Parents();
            var fileMetadata = new Google.Apis.Drive.v3.Data.File();
            fileMetadata.Name = file.Name;
            //fileMetadata.Trashed = false;
            if(root != null)
            {
                fileMetadata.Parents = new List<string> { root.Id };
            }

            var res = Get(fileMetadata.Name);
            if (res == null)
            {
                FilesResource.CreateMediaUpload request;
                using (var stream = new System.IO.FileStream(file.FullName,
                    System.IO.FileMode.Open))
                {
                    request = m_Service.Files.Create(
                        fileMetadata, stream,
                        m_Config.MIMETypes
                        .Single(x => ("." + x.SubType)
                        .Equals(file.Extension, StringComparison.OrdinalIgnoreCase))
                        .ToString());
                    request.Fields = "id";
                    System.Console.WriteLine("File {0} is uploading...", file.Name);
                    request.Upload();
                }
                var response = request.ResponseBody;
                Console.WriteLine("File ID : " + response.Id + "has been uploaded!");
            }
            else
            {
                Console.WriteLine("File ID : " + res.Id + "has been already there!");
            }
        }
    }
}



