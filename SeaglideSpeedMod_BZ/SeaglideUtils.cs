namespace DaftAppleGames.SeaglideSpeedMod_BZ
{
    /// <summary>
    /// Static Utils class for re-use across the mod
    /// </summary>
    internal static class SeaglideUtils
    {
        /// <summary>
        /// Static method to apply given force to the Seaglide instance
        /// </summary>
        /// <param name="seaglideInstance"></param>
        /// <param name="seaglideForceModifier"></param>
        public static void UpdateSeaglide(Seaglide seaglideInstance, float seaglideForceModifier)
        {
            // Get current force
            float currentForce = seaglideInstance.powerGlideForce;

            // Apply changes
            UpdateSeaglide(seaglideInstance, seaglideForceModifier, currentForce);
        }

        /// <summary>
        /// Static overloaded method to use a given default force with the modifier
        /// </summary>
        /// <param name="seaglideInstance"></param>
        /// <param name="defaultForce"></param>
        /// <param name="seaglideForceModifier"></param>
        public static void UpdateSeaglide(Seaglide seaglideInstance, float defaultForce, float seaglideForceModifier)
        {
            SeaglideSpeedModPluginBz.Log.LogInfo($"Updating Seaglide: ({seaglideInstance.name})");

            // Apply modifier
            float newForce = defaultForce * seaglideForceModifier;
            seaglideInstance.powerGlideForce = newForce;

            SeaglideSpeedModPluginBz.Log.LogInfo($"Original force: {defaultForce} to new force: {newForce}");
        }
    }
}
