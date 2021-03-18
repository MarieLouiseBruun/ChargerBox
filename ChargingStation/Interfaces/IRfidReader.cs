using System;
using System.Collections.Generic;
using System.Text;

namespace ChargingStation
{
    public interface IRfidReader
    {
        event EventHandler<RfidEventArgs> RfidEvent;
        void OnRfidRead(int id);
    }
}
