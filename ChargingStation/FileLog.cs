using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ChargingStation
{
    public class FileLog : IFileLog
    {
        private string logFile = "RfidLog.txt"; // Navnet på systemets log-fil

        public void LogDoorLocked(int id)
        {
            using (var writer = File.AppendText(logFile))
            {
                writer.WriteLine(DateTime.Now + "; Skab låst med RFID; {0}", id);
            }
        }

        public void LogDoorUnlocked(int id)
        {
            using (var writer = File.AppendText(logFile))
            {
                writer.WriteLine(DateTime.Now + "; Skab låst op med RFID; {0}", id);
            }
        }
    }
}
