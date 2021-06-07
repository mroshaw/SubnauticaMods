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
    internal class BoosterTankHistoryItem
    {
        public SuitBoosterTank BoosterInstance;
        public float MotorForce;

        public BoosterTankHistoryItem(SuitBoosterTank boosterInstance, float motorForce)
        {

            BoosterInstance = boosterInstance;
            MotorForce = motorForce;
        }
    }
}
