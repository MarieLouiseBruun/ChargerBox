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
using NSubstitute;

namespace ChargerUnitTest
{
    [TestFixture]
    public class TestChargeControl
    {
        private IChargeControl _uut;
        private IUsbCharger _usbCharger;
        private IDisplay _display;
        private bool _connected;

        [SetUp]
        public void Setup()
        {
            _usbCharger = Substitute.For<IUsbCharger>();
            _uut = new ChargeControl(_usbCharger, _display);
        }

     
        [Test]
        public void GetConnectedFromCharger()
        {
            _usbCharger.Connected = true;
           _connected= _uut.GetConnected();
           Assert.That(_connected, Is.EqualTo(true));
        }

        [Test]
        public void GetUnConnectedFromCharger()
        {
            _usbCharger.Connected = false;
            _connected = _uut.GetConnected();
            Assert.That(_connected, Is.EqualTo(false));
        }


        [Test]
        public void StartChargeInCharger()
        {
            _uut.StartCharge();
            
            _usbCharger.Received(1).StartCharge();
        }
        [Test]
        public void StopChargeInCharger()
        {
            _uut.StopCharge();

            _usbCharger.Received(1).StopCharge();
        }

    }
}
