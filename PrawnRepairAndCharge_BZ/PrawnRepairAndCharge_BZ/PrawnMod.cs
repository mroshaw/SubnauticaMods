using System;
using HarmonyLib;
using SMLHelper.V2;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger = QModManager.Utility.Logger;

namespace PrawnRepairAndCharge_BZ
{
    class PrawnMod
    {
        [HarmonyPatch(typeof(Exosuit))]
        [HarmonyPatch("OnDockedChanged")]
        internal class ExoSuitDock
        {
            [HarmonyPostfix]
            public static void Prefix(Exosuit __instance, bool docked, Vehicle.DockType dockType)
            {
                Logger.Log(Logger.Level.Info, $"In pre fix {docked}");

                // Get the current charge and damager levels

            }
        }
    }
}
