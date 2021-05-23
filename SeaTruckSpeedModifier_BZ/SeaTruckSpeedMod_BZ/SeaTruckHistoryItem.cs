using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaTruckSpeedMod_BZ
{
    /// <summary>
    /// This class allows us to keep track of SeaTrucks that we've modded so we can dynamically
    /// change the modifier in real time.
    /// </summary>
    /// 
    internal class SeaTruckHistoryItem
    {
        public SeaTruckMotor SeaTruckInstance;
        public float SeaTruckDrag;

        public SeaTruckHistoryItem(SeaTruckMotor truckInstance, float truckDrag)
        {
            SeaTruckInstance = truckInstance;
            SeaTruckDrag = truckDrag;
        }
    }
}
