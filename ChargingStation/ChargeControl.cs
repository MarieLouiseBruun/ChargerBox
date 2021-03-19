using System;
using System.Collections.Generic;
using System.Text;

namespace ChargingStation
{
    public class ChargeControl : IChargeControl
    {
        //public bool Connected { get; set; }
        private IUsbCharger _usbCharger;


        public ChargeControl(IUsbCharger usbCharger)
        {
            _usbCharger = usbCharger;
        }

        public bool GetConnected()
        {
            return _usbCharger.Connected;
        }
        public void StartCharge()
        {

            _usbCharger.StartCharge();
        }

        public void StopCharge()
        {
            _usbCharger.StopCharge();
        }
    }
}
