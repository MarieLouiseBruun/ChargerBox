using System;
using System.Collections.Generic;
using System.Text;

namespace ChargingStation
{
    public class RfidReaderSimulator : IRfidReader
    {
        public int Rfid { get; private set; }

        public event EventHandler<RfidEventArgs> RfidEvent;

        public void OnRfidRead(int id)
        {
            Rfid = id;
            RfidEvent?.Invoke(this, new RfidEventArgs() { RfID  = this.Rfid });
        }
    }
}
