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
    public class TestDoorSimulator
    {
        private DoorSimulator _uut;
        private DoorEventArgs _eventArgs; 

        [SetUp]
        public void Setup()
        {
            _eventArgs = null; 

            _uut = new DoorSimulator();

            _uut.IsOpenValueEvent +=
                (o, args) =>
                {
                    _eventArgs = args;
                };
        }

        [Test]
        public void DoorStateChanged_IsOpenSetToTrue_EventFired()
        {
            _uut.OnDoorOpen();
            Assert.That(_eventArgs, Is.Not.Null);
        }

        [Test]
        public void DoorStateChanged_IsOpenSetToFalse_EventFired()
        {
            _uut.OnDoorClose();
            Assert.That(_eventArgs, Is.Not.Null);
        }

        [Test]
        public void DoorStateChanged_IsOpenSetToTrue_CorrectNewStateReceived()
        {
            _uut.OnDoorOpen();
            Assert.That(_eventArgs.IsOpen, Is.EqualTo(true));
        }

        [Test]
        public void DoorStateChanged_IsOpenSetToFalse_CorrectNewStateReceived()
        {
            _uut.OnDoorClose();
            Assert.That(_eventArgs.IsOpen, Is.EqualTo(false));
        }

        //Test Lock/Unlock - Console.Writeline()
    }
}
