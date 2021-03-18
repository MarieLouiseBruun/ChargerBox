using System;
using System.Collections.Generic;
using System.Text;

namespace ChargingStation
{
    public class ChargeControl : IChargeControl
    {
        public bool Connected { get; set; }

        public void StartCharge()
        {
            throw new NotImplementedException();
        }

        public void StopCharge()
        {
            throw new NotImplementedException();
        }
    }
}
