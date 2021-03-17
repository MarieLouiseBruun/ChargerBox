using System;
using System.Collections.Generic;
using System.Text;

namespace ChargingStation
{
    public interface IDoor
    {
        void LockDoor();
        void UnlockDoor();
        void OnDoorOpen();
        void OnDoorClose();
    }
}
