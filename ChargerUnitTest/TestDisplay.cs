using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChargingStation;
using ChargingStation.Interfaces;

namespace ChargerUnitTest
{
    [TestFixture]
    public class TestDisplay
    {
        private IDisplay _uut;
        private StringWriter _stringWriter;

        [SetUp]
        public void Setup()
        {
            _uut = new Display();
            _stringWriter = new StringWriter();
            System.Console.SetOut(_stringWriter);
        }

       

     
        [Test]
        public void PrintMessageOnDisplay()
        {
            _uut.Print("Hej Frank - god påske");
            var text = _stringWriter.ToString();
            Assert.AreEqual("Hej Frank - god påske\r\n", text);
        }

    }
}
