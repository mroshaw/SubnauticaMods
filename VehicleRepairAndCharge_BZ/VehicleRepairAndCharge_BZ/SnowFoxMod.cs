using System;
using HarmonyLib;
using SMLHelper.V2;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger = QModManager.Utility.Logger;

namespace VehicleRepairAndCharge_BZ
{
    class SnowFoxMod
    {
        [HarmonyPatch(typeof(Hoverbike))]
        [HarmonyPatch("DockToHoverpad")]
        internal class HoverbikeDocked
        {
            [HarmonyPrefix]
            public static void Prefix(Hoverbike __instance)
            {
                Logger.Log(Logger.Level.Info, $"In prefix Hoverbike_DockToHoverpad");
                // Charge and Repair as appropriate
                // Get current health
                float currentHealth = __instance.liveMixin.GetHealthFraction();
                Logger.Log(Logger.Level.Info, $"Current health fraction: {currentHealth}");
                // Top up health
                __instance.liveMixin.AddHealth(100.00F - currentHealth);
                currentHealth = __instance.liveMixin.GetHealthFraction();
                Logger.Log(Logger.Level.Info, $"New health fraction: {currentHealth}");

                // Get current charge
                float currentCharge = __instance.energyMixin.energy;
                float maxCharge = __instance.energyMixin.maxEnergy;
                float delta = maxCharge - currentCharge;
                Logger.Log(Logger.Level.Info, $"Current charge: {currentCharge}, current capacity {maxCharge}, delta: {delta}");
                __instance.energyMixin.AddEnergy(delta);
            }
        }
    }
}
