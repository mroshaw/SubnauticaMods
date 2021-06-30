using HarmonyLib;

namespace UnlimitedExtinguisher
{
    class ExtinguisherMod
    {
        /// <summary>
        /// Dock and Charge
        /// </summary>
        [HarmonyPatch(typeof(FireExtinguisher))]
        [HarmonyPatch("UseExtinguisher")]
        internal class FireExtinguisherFuel
        {
            [HarmonyPrefix]
            public static void Prefix(float douseAmount, ref float expendAmount)
            {
                // Logger.Log(Logger.Level.Debug, $"FireExtinguisher_UseExtinguisher");
                if (QMod.Config.UnlimitedFuel)
                {
                    // Logger.Log(Logger.Level.Debug, $"Setting expendAmount from {expendAmount} to 0.");
                    expendAmount = 0f;
                }

            }
        }
     }
}
