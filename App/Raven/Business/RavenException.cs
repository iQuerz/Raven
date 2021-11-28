using System;
using System.IO;

namespace App.Business
{
    public class RavenException : Exception
    {
        public RavenException()
            : base()
        {

        }

        public RavenException(string message)
            : base(message)
        {

        }

        public static void Log(Exception e)
        {
            // TODO: A good dynamic way of setting path of log files. Possibly saving it in AppSettings table.
            // TODO: Delete logs from previous version(s) on update.
            // TODO: Automated logs upload somewhere?

            // create folder if needed
            if (!Directory.Exists("logs"))
                Directory.CreateDirectory("logs");

            // File names have "YYYY-MM-DD_HH-mm" format.
            DateTime date = DateTime.Now;
            string filename = $"{date.Year}-{date.Month}-{date.Day}_{date.Hour}-{date.Minute}.log";

            // log the exception
            StreamWriter sw = new StreamWriter($"logs\\{filename}");

            sw.WriteLine($"{DateTime.Now} in {e.Source}.\n");
            sw.WriteLine(e.Message + "\n");
            sw.WriteLine(e.StackTrace + "\n");

            sw.Dispose();
        }
    }
}
