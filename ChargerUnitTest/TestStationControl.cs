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
        private ChargeControl _chargeControl;
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
            _chargeControl = Substitute.For<ChargeControl>(_usbCharger);

            _uut = new StationControl(_door, _rfidReader, _chargeControl, _fileLog, _display);
        }

        #region TestDoorOpen

        [Test]
        public void DoorOpen_Available_DisplayCalled()
        {
            _door.IsOpenValueEvent += Raise.EventWith<DoorEventArgs>(this, new DoorEventArgs());
            _display.Received().Print("Tilslut telefon");
        }


        [Test]
        public void DoorOpen_DoorOpen_DisplayCalledOnce()
        {
            _door.IsOpenValueEvent += Raise.EventWith<DoorEventArgs>(this, new DoorEventArgs());

            _door.IsOpenValueEvent += Raise.EventWith<DoorEventArgs>(this, new DoorEventArgs());
            _display.Received(1).Print("Indlæs RFID");
        }

        [Test]
        public void DoorOpen_Locked_DisplayCalledOnce()
        {
            _door.IsOpenValueEvent += Raise.EventWith<DoorEventArgs>(this, new DoorEventArgs());
            _usbCharger.CurrentValueEvent += Raise.EventWith<CurrentEventArgs>(this, new CurrentEventArgs() { Current = 1 });

            _usbCharger.Connected = true;
            _door.IsOpenValue = false;
            _door.IsOpenValueEvent += Raise.EventWith<DoorEventArgs>(this, new DoorEventArgs());
            _rfidReader.RfidEvent += Raise.EventWith<RfidEventArgs>(this, new RfidEventArgs() { RfID = 123 });

            _door.IsOpenValueEvent += Raise.EventWith<DoorEventArgs>(this, new DoorEventArgs());
            _display.Received(1).Print("Skabet er desværre låst");
        }

        #endregion

        #region DetectedID

        [TestCase(9999)]
        [TestCase(1212)]
        [TestCase(7788)]
        public void RFIDDetected_DifferentArguments_RFIDIsCorrect(int id)
        {
            _rfidReader.RfidEvent += Raise.EventWith(new RfidEventArgs { RfID = id });
            Assert.That(_uut._id, Is.EqualTo(id));
        }

        #endregion

        #region StateAvailable_RFIdDetected


        [Test]
        public void RFIdDetected_MobileConnectedAndAvailable_DoorLockedChargerStart()
        {
            _usbCharger.Connected = true;

            _usbCharger.CurrentValueEvent += Raise.EventWith<CurrentEventArgs>(this, new CurrentEventArgs() { Current = 1 });
            _rfidReader.RfidEvent += Raise.EventWith(new RfidEventArgs { RfID = 123456 });

            _door.Received(1).LockDoor();
            _chargeControl.Received(1).StartCharge();
            _fileLog.Received(1).LogDoorLocked(123456);
            _display.Received(1).Print("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");

            _display.DidNotReceive().Print("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
        }


        [Test]
        public void RFIdDetected_MobileNotConnectedAndAvaliable_ConnectingError()
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith<CurrentEventArgs>(this, new CurrentEventArgs() { Current = 0 });
            _rfidReader.RfidEvent += Raise.EventWith(new RfidEventArgs { RfID = 123456 });

            _display.Received(1).Print("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");

            _door.DidNotReceive().LockDoor();
            _chargeControl.DidNotReceive().StartCharge();
            _fileLog.DidNotReceive().LogDoorLocked(123456);
            _display.DidNotReceive().Print("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");
        }

        #endregion

        #region DoorOpen_RFIdDetected

        [Test]
        public void RFIdDetected_Open_NoMethodCallReceived()
        {
            _door.IsOpenValueEvent += Raise.EventWith<DoorEventArgs>(this, new DoorEventArgs());
            _rfidReader.RfidEvent += Raise.EventWith(new RfidEventArgs { RfID = 123456 });

            _door.DidNotReceive().ReceivedCalls();
            _chargeControl.DidNotReceive().ReceivedCalls();
            _fileLog.DidNotReceive().ReceivedCalls();
            _display.DidNotReceive().ReceivedCalls();
        }

        #endregion

        #region Locked_RFIdDetected
        [Test]
        public void RFIdDetected_MobileConnectedAndStateLocked_DoorUnlockedChargerStop_sameID()
        {
            _usbCharger.Connected = true;

            _usbCharger.CurrentValueEvent += Raise.EventWith<CurrentEventArgs>(this, new CurrentEventArgs() { Current = 1 });
            _rfidReader.RfidEvent += Raise.EventWith(new RfidEventArgs { RfID = 123456 });

            _rfidReader.RfidEvent += Raise.EventWith(new RfidEventArgs { RfID = 123456 });
            _door.Received(1).UnlockDoor();
            _chargeControl.Received(1).StartCharge();
            _fileLog.Received(2).LogDoorUnlocked(123456);
            _display.Received(1).Print("Tag din telefon ud af skabet og luk døren");

            _display.DidNotReceive().Print("Forkert RFID tag");
        }

        [Test]
        public void RFIdDetected_MobileConnectedAndLocked_NotSameIdPrintRFIDError()
        {
            _usbCharger.Connected = true;

            _usbCharger.CurrentValueEvent += Raise.EventWith<CurrentEventArgs>(this, new CurrentEventArgs() { Current = 1 });
            _rfidReader.RfidEvent += Raise.EventWith(new RfidEventArgs { RfID = 123456 });

            _rfidReader.RfidEvent += Raise.EventWith(new RfidEventArgs { RfID = 123457 });
            _display.Received(1).Print("Forkert RFID tag");

            _door.DidNotReceive().UnlockDoor();
            _chargeControl.DidNotReceive().StopCharge();
            _fileLog.DidNotReceive().LogDoorUnlocked(123457);
            _display.DidNotReceive().Print("Tag din telefon ud af skabet og luk døren");
        }

        #endregion
    }
}
