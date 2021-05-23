using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger = QModManager.Utility.Logger;

namespace SeaTruckSpeedMod_BZ
{
    class SeaGlideMod
    {
        [HarmonyPatch(typeof(Seaglide))]
        [HarmonyPatch("GetActiveScalar")]
        internal class SeaTruckSpeed_Scalar
        {
            [HarmonyPostfix]
            public static void Postfix(ref float __result)
            {
                Logger.Log(Logger.Level.Info, $"In post fix with scalar {__result}");
                float modifier = SeaTruckSpeedQMod.Config.SeaTruckSpeedModifier;
                Logger.Log(Logger.Level.Info, $"Modifying with {modifier}");
                float newScalar = __result * SeaTruckSpeedQMod.Config.SeaTruckSpeedModifier;
                Logger.Log(Logger.Level.Info, $"New scalar {newScalar}");
                __result *= SeaTruckSpeedQMod.Config.SeaTruckSpeedModifier;
            }
        }
    }
}
