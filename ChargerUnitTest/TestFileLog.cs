using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChargingStation;

namespace ChargerUnitTest
{
    [TestFixture]
    public class TestFileLog
    {
        private FileLog _uut;
        private String logpath;
        private String[] lines;
        private String[] inputFields;

        [SetUp]
        public void Setup()
        {
            _uut = new FileLog();
            logpath = "RfidLog.txt";
        }

        [Test]
        public void LogDoorLocked_Logfile_Contains_MessageAndId()
        {
            _uut.LogDoorLocked(123);
            lines = System.IO.File.ReadAllLines(logpath);
            String lastline = lines[lines.Length - 1];
            inputFields = lastline.Split(';');
            String message = inputFields[1];
            String id = inputFields[2];

            Assert.Multiple(() =>
            {
                Assert.That(message, Is.EqualTo(" Skab låst med RFID"));
                Assert.That(id, Is.EqualTo(" 123"));
            });
        }

        [Test]
        public void LogDoorUnlocked_Logfile_Contains_MessageAndId()
        {
            _uut.LogDoorUnlocked(123);
            lines = System.IO.File.ReadAllLines(logpath);
            String lastline = lines[lines.Length - 1];
            inputFields = lastline.Split(';');
            String message = inputFields[1];
            String id = inputFields[2];

            Assert.Multiple(() =>
            {
                Assert.That(message, Is.EqualTo(" Skab låst op med RFID"));
                Assert.That(id, Is.EqualTo(" 123"));
            });
        }






    }
}
