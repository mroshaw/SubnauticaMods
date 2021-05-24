using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSpeedMod_BZ
{
    /// <summary>
    /// This class allows us to keep track of Prawn Suits that we've modded so we can dynamically
    /// change the modifier in real time.
    /// </summary>
    /// 
    internal class PrawnSuitHistoryItem
    {
        public Exosuit VehicleInstance;
        public float VehicleValue;

        public PrawnSuitHistoryItem(Exosuit vehicleInstance, float vehicleValue)
        {
            VehicleInstance = vehicleInstance;
            VehicleValue = vehicleValue;
        }
    }
}
