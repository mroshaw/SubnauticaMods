using System;
using HarmonyLib;
using SMLHelper.V2;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger = QModManager.Utility.Logger;

namespace SeaTruckSpeedMod_BZ
{
    class SeaTruckMod
    {
        [HarmonyPatch(typeof(SeaTruckMotor))]
        [HarmonyPatch("GetWeight")]
        internal class SeaTruckWeight
        {
            [HarmonyPostfix]
            public static void Postfix(ref float __result)
            {
                Logger.Log(Logger.Level.Info, $"In post fix with weight {__result}");
                float modifier = SeaTruckSpeedQMod.Config.SeaTruckSpeedModifier;
                Logger.Log(Logger.Level.Info, $"Modifying with {modifier}");
                float newWeight = __result / modifier;
                Logger.Log(Logger.Level.Info, $"New weight {newWeight}");
                __result = newWeight;
            }
        }
    }
}
