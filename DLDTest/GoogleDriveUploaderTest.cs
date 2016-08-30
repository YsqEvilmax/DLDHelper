using DLDHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLDTest
{
    [TestClass]
    public class GoogleDriveUploaderTest
    {
        private AppConfig cfg;
        public GoogleDriveUploaderTest()
        {
            cfg = new AppConfig();
            cfg.Seed();
        }

        [TestMethod]
        public void WhenSingleFile_UploadIfCompleted()
        {
            FakeFile("test.txt");
            GoogleDriveUploader uploader = new GoogleDriveUploader(cfg);
            uploader.Init();

            //Can not execute Google Drive API in Unit Test, need further investegation.
            //uploader.Upload(Path.Combine(cfg.SourceFolder, "test.txt"));

            //var files = uploader.List();
            //Assert.IsTrue(files.Files.Any(x => x.Name.Equals("test.txt")));
        }

        private void FakeFile(string filename)
        {
            string fullPath = Path.Combine(cfg.SourceFolder, filename);
            FakeFolder(cfg.SourceFolder);
            if (!File.Exists(fullPath))
            {
                FileStream stream = File.Create(fullPath);
                using(StreamWriter sw = new StreamWriter(stream))
                {
                    sw.WriteLine("This is a test file!");
                }
            }
        }

        private void FakeFolder(string dirname)
        {
            try
            {
                string fullPath = Path.Combine(cfg.SourceFolder, dirname);
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }
    }
}
