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
        private enum ChargeBoxState
        {
            Available,
            Locked,
            DoorOpen
        };

        // Her mangler flere member variable
        private ChargeBoxState _state;
        private IChargeControl _charger;
        //private IUsbCharger _usbCharger;
        private int _oldId;
        private IDoor _doorSimulator;
        private bool _open;

        //
        private string logFile = "logfile.txt"; // Navnet på systemets log-fil

        // Her mangler constructor
        public StationControl(IDoor doorSimulator, IRfidReader rfidReader, IChargeControl charger)
        {
            _charger = charger;
            //_usbCharger = usbCharger
            _doorSimulator = doorSimulator;
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
            _oldId = e.RfID;
            RfidDetected(_oldId);
        }

        private void DoorAffected()
        {
            if (!_open)
            {
                //der skal også være mulighed for at den er lukket og i brug? men måske bliver det løst i AFIDdetected
               _state= ChargeBoxState.Available;
               Console.WriteLine("Indlæs RFID");
            }
            else
            {
                //hvad sker der hvis døren er låst? ikke nødvendigvis her der skal tages højde for det.
                _state = ChargeBoxState.DoorOpen;
                Console.WriteLine("Tilslut telefon");
            }
        }

            // Eksempel på event handler for eventet "RFID Detected" fra tilstandsdiagrammet for klassen
        //Metode til når man scanner id-kortet på Rfid-readeren
        private void RfidDetected(int id/*, object? sender, DoorEventArgs e*/)
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
                        //Skal evt. flyttes til FileLog eller skrives helt om
                        using (var writer = File.AppendText(logFile))
                        {
                            writer.WriteLine(DateTime.Now + ": Skab låst med RFID: {0}", id);
                        }

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

                        //Skal evt. flyttes til FileLog eller skrives helt om
                        using (var writer = File.AppendText(logFile))
                        {
                            writer.WriteLine(DateTime.Now + ": Skab låst op med RFID: {0}", id);
                        }

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
