using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChargerBox;
using ChargingStation;
using ChargingStation.Interfaces;
using NSubstitute;

namespace ChargerUnitTest
{
    [TestFixture]
    public class TestStationControl
    {
        private StationControl _uut;
        private IChargeControl _chargeControl;
        private IDisplay _display;
        private IDoor _door;
        private IFileLog _fileLog;
        private IRfidReader _rfidReader;
        private IUsbCharger _usbCharger;

        [SetUp]
        public void Setup()
        {
            _display = Substitute.For<IDisplay>();
            _door = Substitute.For<IDoor>();
            _fileLog = Substitute.For<IFileLog>();
            _rfidReader = Substitute.For<IRfidReader>();
            _usbCharger = Substitute.For<IUsbCharger>();
            _chargeControl = Substitute.For<IChargeControl>(_usbCharger);

            _uut = new StationControl(_door, _rfidReader, _chargeControl, _fileLog, _display);
        }

        #region IDDetected

        [TestCase(9999)]
        [TestCase(1212)]
        [TestCase(7788)]
        public void RFIDDetected_DifferentArguments_RFIDIsCorrect(int id)
        {
            _rfidReader.RfidEvent += Raise.EventWith(this, new RfidEventArgs() { RfID = id });
            Assert.That(_uut._id, Is.EqualTo(id));
        }

        #endregion
    }
}
