using System;
using System.Collections.Generic;
using System.Text;

namespace ChargingStation
{
    public class RfidReaderSimulator : IRfidReader
    {
        public void OnRfidRead(in int id)
        {
            throw new NotImplementedException();
        }
        public int rfId { get; private set; }

        public event EventHandler<RfidEventArgs> RfidEvent;
        public void OnRfidRead(int id)
        {
            RfidEvent?.Invoke(this, new RfidEventArgs() { RfID  = this.rfId });
        }
    }
}
