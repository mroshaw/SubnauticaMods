using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSpeedMod_BZ
{
    /// <summary>
    /// This class allows us to keep track of SeaTrucks that we've modded so we can dynamically
    /// change the modifier in real time.
    /// </summary>
    /// 
    internal class SeaTruckHistoryItem
    {
        public SeaTruckMotor VehicleInstance;
        public float VehicleValue;

        public SeaTruckHistoryItem(SeaTruckMotor vehicleInstance, float vehicleValue)
        {
            VehicleInstance = vehicleInstance;
            VehicleValue = vehicleValue;
        }
    }
}
