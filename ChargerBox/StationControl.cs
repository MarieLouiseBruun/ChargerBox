using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChargingStation;
using ChargingStation.Interfaces;

namespace ChargerBox
{
    public class StationControl
    {
        // Enum med tilstande ("states") svarende til tilstandsdiagrammet for klassen
        public enum ChargeBoxState
        {
            Available,
            Locked,
            DoorOpen
        };
        
        private ChargeBoxState _state;
        private IChargeControl _charger;
        private IFileLog _fileLog;

        private IDisplay _display;
        //private IUsbCharger _usbCharger;
        private int _oldId;
        private IDoor _doorSimulator;
        private bool _open; //slettes? 
        public int _id { get; set; }

        public StationControl(IDoor doorSimulator, IRfidReader rfidReader, IChargeControl charger, IFileLog fileLog, IDisplay display)
        {
            _charger = charger;
            _fileLog = fileLog; 
            //_usbCharger = usbCharger
            _doorSimulator = doorSimulator;
            _display = display;
            doorSimulator.IsOpenValueEvent += HandleDoorEvent;
            rfidReader.RfidEvent += HandleRfIdEvent;
        }

        //HandleCurrentEvent

        private void HandleDoorEvent(object? sender, DoorEventArgs e)
        {
            _open = e.IsOpen;
            DoorAffected();
        }

        private void HandleRfIdEvent(object? sender, RfidEventArgs e)
        {
            _id = e.RfID;
            RfidDetected(_id);
        }

        private void DoorAffected()
        {
            switch (_state)
            {
                case ChargeBoxState.Locked:
                    _display.Print("Skabet er desværre låst");
                    break;
                case ChargeBoxState.DoorOpen:
                    _state = ChargeBoxState.Available;
                    _display.Print("Indlæs RFID");
                    break;
                case ChargeBoxState.Available:
                    _state = ChargeBoxState.DoorOpen;
                    _display.Print("Tilslut telefon");
                    break;
            }
        }

            // Eksempel på event handler for eventet "RFID Detected" fra tilstandsdiagrammet for klassen
        //Metode til når man scanner id-kortet på Rfid-readeren
        private void RfidDetected(int id)
        {
            switch (_state)
            {
                case ChargeBoxState.Available:
                    // Check for ladeforbindelse
                    if (_charger.GetConnected())
                    {
                        _doorSimulator.LockDoor();
                        _charger.StartCharge();
                        _oldId = id;

                        _fileLog.LogDoorLocked(id);

                        _display.Print("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");

                        _state = ChargeBoxState.Locked;
                    }
                    else
                    {
                        _display.Print("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
                    }

                    break;

                case ChargeBoxState.DoorOpen:
                    // Ignore
                    break;

                case ChargeBoxState.Locked:
                    // Check for correct ID
                    if (id == _oldId)
                    {
                        _charger.StopCharge();

                        _doorSimulator.UnlockDoor();

                        _fileLog.LogDoorUnlocked(id);

                        _display.Print("Tag din telefon ud af skabet og luk døren");
                        _state = ChargeBoxState.Available;
                    }
                    else
                    {
                        _display.Print("Forkert RFID tag");
                    }

                    break;
            }
        }

        // Her mangler de andre trigger handlere

    }
}
