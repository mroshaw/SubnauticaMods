namespace DaftAppleGames.SeaglideSpeedMod_BZ
{
    /// <summary>
    /// Class to allow us to maintain a list of patches Seaglides
    /// </summary>
    internal class SeaglideHistoryItem
    {
        // Public properties
        public Seaglide SeaglideInstance;
        public float SeaglideForce;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="seaglideInstance"></param>
        /// <param name="seaglideForce"></param>
        public SeaglideHistoryItem(Seaglide seaglideInstance, float seaglideForce)
        {
            SeaglideInstance = seaglideInstance;
            SeaglideForce = seaglideForce;
        }

        /// <summary>
        /// Constructor with implicit force from instance
        /// </summary>
        /// <param name="seaglideInstance"></param>
        public SeaglideHistoryItem(Seaglide seaglideInstance)
        {
            SeaglideInstance = seaglideInstance;
            SeaglideForce = seaglideInstance.powerGlideForce;
        }
    }
}
