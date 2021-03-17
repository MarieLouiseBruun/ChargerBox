using System;
using System.Collections.Generic;
using System.Text;

namespace ChargingStation
{
    public class DoorEventArgs : EventArgs
    {
        public bool IsOpen { set; get; }
    }
}
