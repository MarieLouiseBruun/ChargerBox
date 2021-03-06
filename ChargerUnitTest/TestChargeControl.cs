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
            _display = Substitute.For<IDisplay>();
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

        //Genaflevering
        //ved hjælp BVA har vi udvalgt værdier, der giver forskellige udfald (1 udfald pr. test): 

        [Test]
        public void HandleCurrentEvent_CurrentIs0()
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = 0 });
            _display.DidNotReceive().Print("");
            _usbCharger.DidNotReceive().StopCharge();
        }

        [TestCase(3)]
        [TestCase(5)]
        public void HandleCurrentEvent_CurrentIs3or5(double current)
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = current});
            _display.Received(1).Print("Telefonen er fuldt opladt");
            _usbCharger.Received(1).StopCharge();
        }

        [TestCase(200)]
        [TestCase(500)]
        public void HandleCurrentEvent_CurrentIs200or500(double current)
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = current });
            _display.Received().Print("Ladning er i gang");
        }

        [Test]
        public void HandleCurrentEvent_CurrentIs600()
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = 600 });
            _display.Received(1).Print("FEJL!!!!!!");
            _usbCharger.Received(1).StopCharge();
        }
    }
}
