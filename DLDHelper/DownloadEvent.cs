using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLDHelper
{
    public class DownloadEvent : Event
    {
        public DownloadEvent(ISet<DirectoryInfo> e) : base(e)
        { }

        public DownloadEvent(DownloadDetecter d, GoogleDriveUploader g) : this(d.DownLoadedList)
        {
            OnListening = x =>
            {
                d.CollectFinished(d.Config.SourceFolder);
            };

            OnHandling = x =>
            {
                ISet<DirectoryInfo> finishedList = x as ISet<DirectoryInfo>;
                foreach (DirectoryInfo dir in finishedList)
                {
                    g.Upload(dir.FullName);
                    Directory.Delete(dir.FullName, true);
                }
                finishedList.Clear();
            };

        }
    }
}
