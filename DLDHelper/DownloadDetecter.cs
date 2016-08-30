using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLDHelper
{
    public class DownloadDetecter
    {
        private AppConfig m_Config;
        public AppConfig Config { get {return m_Config; } }

        public DownloadDetecter(AppConfig config)
        {
            m_Config = config;
        }

        public void CollectFinished(string path)
        {
            if (Directory.Exists(path))
            {
                CollectFinished(Directory.CreateDirectory(path));
            }
            else
            {
                System.Console.WriteLine("Please make sure path {0} is valid!", path);
            }
        }

        private void CollectFinished(DirectoryInfo dir)
        {
            bool allSubDirIn = true;
            foreach(DirectoryInfo subdir in dir.GetDirectories())
            {
                CollectFinished(subdir);
                if (!DownLoadedList.Contains(subdir))
                {
                    allSubDirIn = false;
                }
            }

            if((!dir.GetFiles().Any(x => m_Config.XunLeiDownLoading.Postfixs.Contains(x.Extension)))
                && allSubDirIn)
            {
                m_DownloadedList.Add(dir);
                m_DownloadedList =  new HashSet<DirectoryInfo>(m_DownloadedList
                    .Except(m_DownloadedList
                    .Where(x => dir
                    .GetDirectories()
                    .Any(y => x == y))));                               
            }
        }

        private ISet<DirectoryInfo> m_DownloadedList = new HashSet<DirectoryInfo>();
        public ISet<DirectoryInfo> DownLoadedList { get {return m_DownloadedList; } }
    }
}
