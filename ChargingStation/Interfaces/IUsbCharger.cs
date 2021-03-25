﻿using System;

namespace ChargingStation
{
    public interface IUsbCharger
    {
        // Event triggered on new current value
        event EventHandler<CurrentEventArgs> CurrentValueEvent;

        // Direct access to the current current value
        double CurrentValue { get; }

        // Require connection status of the phone
        bool Connected { get; set; }

        // Start charging
        void StartCharge();
        // Stop charging
        void StopCharge();
        void SimulateConnected(bool connected);
       
    }
}