using System;
using System.Collections.Generic;
using System.Text;

namespace ChargingStation
{
    public class CurrentEventArgs : EventArgs
    {
        // Value in mA (milliAmpere)
        public double Current { set; get; }
    }
}
