using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SubtitlesMatcher.SilentRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SubsSilentFinder matcher = new SubsSilentFinder();

                matcher.FindAndDownloadSub(args[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fatal Error: see event viewer for details");
                EventLog.WriteEntry("Application", ex.ToString(), EventLogEntryType.Error);
            }
        }
    }
}
