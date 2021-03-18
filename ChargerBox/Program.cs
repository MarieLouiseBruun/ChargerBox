﻿    using System;
    using ChargerBox;
    using ChargingStation;

    class Program
    {
        static void Main(string[] args)
        {
				// Assemble your system here from all the classes
                //DoorSimulator doorSimulator = new DoorSimulator();
                IDoor doorSimulator = new DoorSimulator();
                //RfidReaderSimulator rfidReaderSimulator = new RfidReaderSimulator();
                IRfidReader rfidReaderSimulator = new RfidReaderSimulator();
                
                StationControl stationControl = new StationControl(doorSimulator, rfidReaderSimulator);

                //Det man indtaster simulerer det som brugeren fysisk gør.
                //Fra program bliver der sat gang i Events, som StationControl får besked om

            bool finish = false;
            do
            {
                string input;
                System.Console.WriteLine("Indtast E, O, C, R: ");
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

                    default:
                        break;
                }

            } while (!finish);
        }
    }

