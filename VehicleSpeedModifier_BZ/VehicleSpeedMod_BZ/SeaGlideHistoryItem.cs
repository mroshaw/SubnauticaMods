using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSpeedMod_BZ
{
    class SeaGlideHistoryItem
    {
        public Seaglide VehicleInstance;
        public float VehicleValue;

        public SeaGlideHistoryItem(Seaglide vehicleInstance, float vehicleValue)
        {
            VehicleInstance = vehicleInstance;
            VehicleValue = vehicleValue;
        }
    }
}
