using System;
using System.Collections.Generic;
using System.Text;
using ChargingStation.Interfaces;

namespace ChargingStation
{
    public class Display : IDisplay
    {

        public void Print(string text)
        {
            Console.WriteLine(text);
        }
    }
}
