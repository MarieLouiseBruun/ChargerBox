using System;
using System.Collections.Generic;
using System.Text;

namespace ChargingStation
{
    public class RfidEventArgs : EventArgs
    {
        public int RfID { set; get; }
    }
}
