using System;
using System.Collections.Generic;
using System.Text;

namespace ChargingStation
{
    public interface IDoor
    {
        bool IsOpenValue { get; set; }
        event EventHandler<DoorEventArgs> IsOpenValueEvent;
        void LockDoor();
        void UnlockDoor();
        void OnDoorOpen();
        void OnDoorClose();
    }
}
