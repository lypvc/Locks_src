using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MyApplic
{
    public class LogHepler
    {
        public static void Log(Exception e)
        {
            Log(null, e);
        }

        public static void Log(string info)
        {
            Log(info, null);
        }

        public static void Log(string info, Exception e)
        {
            Log(info, e, false);
        }

        public static void Log(string info, Exception e, bool closetime)
        {
            try
            {
                string logfile = AppDomain.CurrentDomain.BaseDirectory + "\\log\\" +
                    (e == null ? "passclient_info_" : "passclient_exception_") +
                    DateTime.Now.ToString("yyyy-MM-dd") + ".log";

                CreateLogDirectory();
                if (!closetime)
                    File.AppendAllText(logfile, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                if (info != null)
                    File.AppendAllText(logfile, info);
                if (e != null)
                    File.AppendAllText(logfile, e.ToString());

                File.AppendAllText(logfile, " " + Environment.NewLine);
                if (e == null)
                    Debug.WriteLine($"DateTime:{DateTime.Now}   Msg:{info}");
                if (e != null)
                    Debug.WriteLine($"DateTime:{DateTime.Now}   Msg:{e.Message}\r\tStackTrace:{e.StackTrace}");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CreateLogDirectory()
        {
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\log");
        }
    }
}
