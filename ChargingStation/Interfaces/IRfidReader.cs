using System;
using System.Collections.Generic;
using System.Text;

namespace ChargingStation
{
    public interface IRfidReader
    {
        void OnRfidRead(int id);
    }
}
