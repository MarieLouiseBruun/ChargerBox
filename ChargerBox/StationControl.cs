using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChargingStation;

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

        // Her mangler flere member variable
        public ChargeBoxState _state;
        private IChargeControl _charger;
        private IFileLog _fileLog; 
        //private IUsbCharger _usbCharger;
        private int _oldId;
        private IDoor _doorSimulator;
        private bool _open; //slettes? 
        private int _id;
        private bool _locked; //slettes?

        // Her mangler constructor
        public StationControl(IDoor doorSimulator, IRfidReader rfidReader, IChargeControl charger, IFileLog fileLog)
        {
            _charger = charger;
            _fileLog = fileLog; 
            //_usbCharger = usbCharger
            _doorSimulator = doorSimulator;
            doorSimulator.IsOpenValueEvent += HandleDoorEvent;
            rfidReader.RfidEvent += HandleRfIdEvent;
        }

        public void HandleDoorEvent(object? sender, DoorEventArgs e)
        {
            _open = e.IsOpen;
            DoorAffected();
        }

        private void HandleRfIdEvent(object? sender, RfidEventArgs e)
        {
            _id = e.RfID;
            RfidDetected(_id);
        }

        public void DoorAffected()
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
