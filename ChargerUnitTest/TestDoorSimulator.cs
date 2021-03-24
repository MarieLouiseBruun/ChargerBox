using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
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
        private StringWriter _stringWriter;

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
            _stringWriter = new StringWriter();
            System.Console.SetOut(_stringWriter);
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

        [Test]
        public void DoorLockedSimulated()
        {
            _uut.LockDoor();
            var text = _stringWriter.ToString();
            Assert.AreEqual("**Dør låst**\r\n", text);
        }

        [Test]
        public void DoorUnLockedSimulated()
        {
            _uut.UnlockDoor();
            var text = _stringWriter.ToString();
            Assert.AreEqual("**Dør ulåst**\r\n", text);
        }
    }
}
