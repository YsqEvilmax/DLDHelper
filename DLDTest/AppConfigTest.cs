using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DLDHelper;
using System.IO;

namespace DLDTest
{
    [TestClass]
    public class AppConfigTest
    {
        private static readonly string m_Path = "AppConfig.xml";
        [TestMethod]
        public void WhenSerialize_FileStoredLocally()
        {
            try
            {
                if (File.Exists(m_Path))
                {
                    File.Delete(m_Path);
                }

                AppConfig cfg = new AppConfig();

                cfg.Seed();

                Serializer<AppConfig>.Serialize(m_Path, cfg);

                Assert.IsTrue(File.Exists(m_Path));
            }
            finally
            {
                File.Delete(m_Path);
            }
        }

        [TestMethod]
        public void WhenDeserialize_InstanceLoadedRuntimely()
        {
            try
            {
                if (!File.Exists(m_Path))
                {
                    AppConfig cfgg = new AppConfig();

                    cfgg.Seed();

                    Serializer<AppConfig>.Serialize(m_Path, cfgg);
                }

                AppConfig cfg = Serializer<AppConfig>.Deserialize(m_Path);

                Assert.IsNotNull(cfg);
                Assert.IsNotNull(cfg.DestinationFolder);
                Assert.IsNotNull(cfg.SourceFolder);
                Assert.IsNotNull(cfg.GoogleAPIParams);
                Assert.IsNotNull(cfg.MIMETypes);
                Assert.IsNotNull(cfg.XunLeiDownLoading);
            }
            finally
            {
                File.Delete(m_Path);
            }
        }
    }
}
