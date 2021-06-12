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
    class SeaTruckMod
    {
        [HarmonyPatch(typeof(Dockable))]
        [HarmonyPatch("OnDockingComplete")]
        internal class SeaTruckDocked
        {
            [HarmonyPrefix]
            public static void Prefix(Dockable __instance)
            {
                SeaTruckSegment truckSegment = __instance.truckSegment;

                if (truckSegment != null && truckSegment.IsFront())
                {
                    Logger.Log(Logger.Level.Info, $"In prefix SeaTruckSegment_");
                    // Charge and Repair as appropriate
                    // Get current health
                    float currentHealth = truckSegment.liveMixin.GetHealthFraction();
                    Logger.Log(Logger.Level.Info, $"Current health fraction: {currentHealth}");
                    // Top up health
                    truckSegment.liveMixin.AddHealth(100.00F - currentHealth);
                    currentHealth = truckSegment.liveMixin.GetHealthFraction();
                    Logger.Log(Logger.Level.Info, $"New health fraction: {currentHealth}");

                    // Get current charge
                     __instance.GetEnergy(out float currentCharge, out float maxCharge);

                    float delta = maxCharge - currentCharge;
                    Logger.Log(Logger.Level.Info, $"Current charge: {currentCharge}, current capacity {maxCharge}, delta: {delta}");
                    __instance.AddEnergy(delta, out float newCharge);
                    Logger.Log(Logger.Level.Info, $"New charge: {newCharge}");
                }
            }
        }
    }
}
