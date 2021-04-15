using System;
using System.Collections.Generic;
using System.Text;
using ChargingStation.Interfaces;

namespace ChargingStation
{
    public class ChargeControl : IChargeControl
    {
        //public bool Connected { get; set; }
        private IUsbCharger _usbCharger;
        private IDisplay _display;
        private double _current;  


        public ChargeControl(IUsbCharger usbCharger, IDisplay display)
        {
            _usbCharger = usbCharger;
            _display = display;
            _usbCharger.CurrentValueEvent += HandleCurrentEvent;
        }

        //HandleCurrentEvent

        private void HandleCurrentEvent(object? sender, CurrentEventArgs e)
        {
            _current = e.Current;
            if (_current == 0){ }
            else if (_current > 0 && _current <= 5)
            {
                _display.Print("Telefonen er fuldt opladt");
                StopCharge();
            }
            else if (_current > 5 && _current <= 500)
            {
                _display.Print("Ladning er i gang");
            }
            else if (_current > 500)
            {
                StopCharge();
                _display.Print("FEJL!!!!!!");
            }
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
