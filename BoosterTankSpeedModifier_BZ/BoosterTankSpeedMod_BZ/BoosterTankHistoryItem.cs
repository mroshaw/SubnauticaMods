namespace SeaTruckSpeedMod_BZ
{
    /// <summary>
    /// This class allows us to keep track of Booster Tanks that we've modded so we can dynamically
    /// change the modifier in real time.
    /// </summary>
    /// 
    internal class BoosterTankHistoryItem
    {
        public SuitBoosterTank BoosterInstance;
        public float MotorForce;
        public float OxygenConsumption;

        public BoosterTankHistoryItem(SuitBoosterTank boosterInstance, float motorForce, float oxygenConsumption)
        {
            BoosterInstance = boosterInstance;
            MotorForce = motorForce;
            OxygenConsumption = oxygenConsumption;
        }
    }
}
