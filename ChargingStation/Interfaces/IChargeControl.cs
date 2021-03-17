using System;
using System.Collections.Generic;
using System.Text;

namespace ChargingStation
{
    public interface IChargeControl
    {
        bool Connected { get; set; }
        void StartCharge();
        void StopCharge();
    }
}
