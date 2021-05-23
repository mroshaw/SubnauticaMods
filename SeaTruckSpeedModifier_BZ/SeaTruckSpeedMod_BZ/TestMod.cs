using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger = QModManager.Utility.Logger;

namespace SeaTruckSpeedMod_BZ
{
    class TestMod
    {
        [HarmonyPatch(typeof(PDAScanner))]
        [HarmonyPatch("Initialize")]
        internal class TestPDAScanner
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                Logger.Log(Logger.Level.Info, $"PDA Scanner Init {SeaTruckSpeedQMod.Config.SeaTruckSpeedModifier}");
            }
        }
    }
}
