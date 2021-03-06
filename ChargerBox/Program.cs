    using System;
    using System.ComponentModel.DataAnnotations;
    using ChargerBox;
    using ChargingStation;
    using ChargingStation.Interfaces;

    class Program
    {
        static void Main(string[] args)
        {
				// Assemble your system here from all the classes
                //DoorSimulator doorSimulator = new DoorSimulator();
                IUsbCharger charger = new UsbChargerSimulator();
                IDisplay display = new Display();
                IChargeControl chargeControl = new ChargeControl(charger, display); 
                IDoor doorSimulator = new DoorSimulator();
                //RfidReaderSimulator rfidReaderSimulator = new RfidReaderSimulator();
                IRfidReader rfidReaderSimulator = new RfidReaderSimulator();
                IFileLog fileLog = new FileLog();
                
                StationControl stationControl = new StationControl(doorSimulator, rfidReaderSimulator, chargeControl, fileLog, display);

                //Det man indtaster simulerer det som brugeren fysisk gør.
                //Fra program bliver der sat gang i Events, som StationControl får besked om

            bool finish = false;
            do
            {
                string input;
                System.Console.WriteLine("Indtast E, O, C, R, Y, N: ");
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) continue;

                switch (input[0])
                {
                    case 'E':
                        finish = true;
                        break;

                    case 'O':
                        doorSimulator.OnDoorOpen();
                        break;

                    case 'C':
                     doorSimulator.OnDoorClose();
                    break;

                    case 'R':
                        System.Console.WriteLine("Indtast RFID id: ");
                        string idString = System.Console.ReadLine();

                        int id = Convert.ToInt32(idString);
                        
                     rfidReaderSimulator.OnRfidRead(id);
                    break;

                case 'Y':
                        charger.SimulateConnected(true);
                        break;
                case 'N':
                    charger.SimulateConnected(false);
                    break;

                    default:
                        break;
                }

            } while (!finish);
        }
    }

