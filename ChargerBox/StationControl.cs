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
        private int _id;

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
                    Console.WriteLine("Skabet er desværre låst");
                    break;
                case ChargeBoxState.DoorOpen:
                    _state = ChargeBoxState.Available;
                        Console.WriteLine("Indlæs RFID");
                    break;
                case ChargeBoxState.Available:
                    _state = ChargeBoxState.DoorOpen;
                    Console.WriteLine("Tilslut telefon");
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

                        _fileLog.LogToFile(id);

                        Console.WriteLine("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");

                        _state = ChargeBoxState.Locked;
                    }
                    else
                    {
                        Console.WriteLine("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
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

                        _fileLog.LogToFile(id);

                        Console.WriteLine("Tag din telefon ud af skabet og luk døren");
                        _state = ChargeBoxState.Available;
                    }
                    else
                    {
                        Console.WriteLine("Forkert RFID tag");
                    }

                    break;
            }
        }

        // Her mangler de andre trigger handlere

    }
}
