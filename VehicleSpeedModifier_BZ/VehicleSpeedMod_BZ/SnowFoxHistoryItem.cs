using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSpeedMod_BZ
{
    /// <summary>
    /// This class allows us to keep track of Snow Fox that we've modded so we can dynamically
    /// change the modifier in real time.
    /// </summary>
    /// 
    internal class SnowFoxHistoryItem
    {
        public Hoverbike VehicleInstance;
        public float VehicleValue;

        public SnowFoxHistoryItem(Hoverbike vehicleInstance, float vehicleValue)
        {
            VehicleInstance = vehicleInstance;
            VehicleValue = vehicleValue;
        }
    }
}
