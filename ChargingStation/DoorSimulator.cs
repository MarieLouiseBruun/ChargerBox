using System;
using System.Collections.Generic;
using System.Text;

namespace ChargingStation
{
    public class DoorSimulator: IDoor
    {
        //Skal måske have DoorEventArgs ind her - ligesom for CurrentEventArgs og UsbChargerSimulator
        public void UnlockDoor()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<DoorEventArgs> IsOpenValueEvent;

        public void LockDoor()
        {
            throw new NotImplementedException();
        }

        public void OnDoorOpen()
        {
            throw new NotImplementedException();
        }

        public void OnDoorClose()
        {
            throw new NotImplementedException();
        }
    }
}
