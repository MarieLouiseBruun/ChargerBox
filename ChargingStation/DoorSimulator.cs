using System;
using System.Collections.Generic;
using System.Text;

namespace ChargingStation
{
    public class DoorSimulator: IDoor
    {
        public bool IsOpenValue { get; private set; }

        public event EventHandler<DoorEventArgs> IsOpenValueEvent;

        //Skal måske have DoorEventArgs ind her - ligesom for CurrentEventArgs og UsbChargerSimulator
        public void UnlockDoor()
        {
            Console.WriteLine("Door is unlocked");
        }


        public void LockDoor()
        {
            Console.WriteLine("Door is locked");
        }

        public void OnDoorOpen()
        {
            IsOpenValue = true;
            IsOpenValueEvent?.Invoke(this, new DoorEventArgs() { IsOpen = this.IsOpenValue });
        }

        public void OnDoorClose()
        {
            IsOpenValue = false;
            IsOpenValueEvent?.Invoke(this, new DoorEventArgs() { IsOpen = this.IsOpenValue });
        }
    }
}
