using HarmonyLib;
using Logger = QModManager.Utility.Logger;

namespace VehicleSpeedMod_BZ
{
    class TestMod
    {
        /// <summary>
        /// Very simple hook into the PDA to check that Harmony PatchAll is working
        /// </summary>
        [HarmonyPatch(typeof(PDAScanner))]
        [HarmonyPatch("Initialize")]
        internal class TestPDAScanner
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                Logger.Log(Logger.Level.Debug, $"PDA Scanner Init");
            }
        }
    }
}
