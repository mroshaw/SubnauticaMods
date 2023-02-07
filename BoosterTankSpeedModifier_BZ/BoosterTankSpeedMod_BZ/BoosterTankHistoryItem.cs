namespace DaftAppleGames.BoosterTankSpeedMod_BZ
{
    /// <summary>
    /// This class allows us to keep track of Booster Tanks that we've modded so we can dynamically
    /// change the modifier in real time.
    /// </summary>
    /// 
    internal class BoosterTankHistoryItem
    {
        // Instance and corresponding settings
        public SuitBoosterTank BoosterInstance;
        public float MotorForce;
        public float OxygenConsumption;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="boosterInstance"></param>
        /// <param name="motorForce"></param>
        /// <param name="oxygenConsumption"></param>
        public BoosterTankHistoryItem(SuitBoosterTank boosterInstance, float motorForce, float oxygenConsumption)
        {
            BoosterInstance = boosterInstance;
            MotorForce = motorForce;
            OxygenConsumption = oxygenConsumption;
        }
    }
}
