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
    public class TestRfidReaderSimulator
    {
        private RfidReaderSimulator _uut;
        private RfidEventArgs _eventArgs; 

        [SetUp]
        public void Setup()
        {
            _eventArgs = null; 

            _uut = new RfidReaderSimulator();

            _uut.RfidEvent +=
                (o, args) =>
                {
                    _eventArgs = args;
                };
        }

        [Test]
        public void RfidChanged_IdSetToNewValue_EventFired()
        {
            _uut.OnRfidRead(100);
            Assert.That(_eventArgs, Is.Not.Null);
        }

        [Test]
        public void RfidChanged_IdSetToNewValue_CorrectNewIdReceived()
        {
            _uut.OnRfidRead(100);
            Assert.That(_eventArgs.RfID, Is.EqualTo(100));
        }




    }
}
