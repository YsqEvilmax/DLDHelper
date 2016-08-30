using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLDHelper
{
    static class AppLog
    {
        private static readonly string m_LogPath = "log.txt";
        public static void Log(string message)
        {
            using (StreamWriter sw = File.AppendText(m_LogPath))
            {
                sw.Write("\r\nLog Entry : ");
                sw.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                sw.WriteLine("  :");
                sw.WriteLine("  :{0}", message);
                sw.WriteLine("-------------------------------");
            }
        }

        public static void Dump()
        {
            using(StreamReader sr = new StreamReader(m_LogPath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }
    }
}
