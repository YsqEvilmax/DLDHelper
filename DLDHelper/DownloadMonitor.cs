using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DLDHelper
{
    public class DownloadMonitor
    {
        private AppConfig m_Config = Serializer<AppConfig>.Deserialize("AppConfig.xml");

        public DownloadMonitor()
        {
            m_detecter = new DownloadDetecter(m_Config);
            m_uploader = new GoogleDriveUploader(m_Config);
            m_uploader.Init();
        }
        public void Idle()
        {
            // add DownloadEvent
            m_eventChecker.Add(new DownloadEvent(m_detecter, m_uploader));

            while (true)
            {
                foreach(Event e in m_eventChecker)
                {
                    e.Run();
                }
            }
        }

        private ISet<Event>  m_eventChecker = new HashSet<Event>();
        private DownloadDetecter m_detecter;
        private GoogleDriveUploader m_uploader;

    }


}
