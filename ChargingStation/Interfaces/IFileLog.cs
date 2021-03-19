using System;
using System.Collections.Generic;
using System.Text;

namespace ChargingStation
{
    public interface IFileLog
    {
        void LogToFile(int id);
    }
}
