using System;
using System.Collections.Generic;
using System.Text;

namespace ChargingStation
{
    public interface IChargeControl
    {
       // bool Connected { get; set; }
       //double Current { get; set; }
        void StartCharge();
        void StopCharge();
        bool GetConnected();
    }
}
