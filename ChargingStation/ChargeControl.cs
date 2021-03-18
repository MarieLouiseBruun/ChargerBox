using System;
using System.Collections.Generic;
using System.Text;

namespace ChargingStation
{
    public class ChargeControl : IChargeControl
    {
        public bool Connected { get; set; }
        private IUsbCharger usbCharger = new UsbChargerSimulator();

        public void StartCharge()
        {
            usbCharger.StartCharge();
        }

        public void StopCharge()
        {
            usbCharger.StopCharge();
        }
    }
}
