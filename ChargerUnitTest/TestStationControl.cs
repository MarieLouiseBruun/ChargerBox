using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChargerBox;
using ChargingStation;

namespace ChargerUnitTest
{
    [TestFixture]
    public class TestStationControl
    {
        private StationControl _uut;
        [SetUp]
        public void Setup()
        {
           _uut = new StationControl(new DoorSimulator(),new RfidReaderSimulator(), new ChargeControl(new UsbChargerSimulator()), new FileLog());
        }

        [Test] public void hej()
        {
            _uut.DoorAffected();
            Assert.That(_uut._state,Is.EqualTo(StationControl.ChargeBoxState.DoorOpen));
        }
        
    }
}
