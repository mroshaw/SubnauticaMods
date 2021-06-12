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
    class PrawnSuitMod
    {
        [HarmonyPatch(typeof(Exosuit))]
        [HarmonyPatch("OnDockedChanged")]
        internal class PrawnSuitDocked
        {
            [HarmonyPrefix]
            public static void Prefix(Exosuit __instance, bool docked, Vehicle.DockType dockType)
            {
                Logger.Log(Logger.Level.Info, $"In pre fix OnDockedChange, docked is: {docked}");
                if (docked)
                {
                    // Charge and Repair as appropriate
                    if ((dockType == Vehicle.DockType.Base && QMod.Config.RepairEnableMoonPool) || dockType == Vehicle.DockType.Seatruck && QMod.Config.RepairEnableSeaTruck)
                    {
                        // Get current health
                        float currentHealth = __instance.liveMixin.GetHealthFraction();
                        Logger.Log(Logger.Level.Info, $"Current health fraction: {currentHealth}");
                        // Top up health
                        __instance.liveMixin.AddHealth(100.00F - currentHealth);
                        currentHealth = __instance.liveMixin.GetHealthFraction();
                        Logger.Log(Logger.Level.Info, $"New health fraction: {currentHealth}");
                    }
                    if ((dockType == Vehicle.DockType.Base && QMod.Config.ChargingEnableMoonPool) || dockType == Vehicle.DockType.Seatruck && QMod.Config.ChargingEnableSeaTruck)
                    {
                        // Get current charge
                        __instance.energyInterface.GetValues(out float currentCharge, out float currentCapacity);
                        Logger.Log(Logger.Level.Info, $"Current charge: {currentCharge}, current capacity {currentCapacity}, delta: {currentCapacity - currentCharge}");
                        __instance.AddEnergy(currentCapacity - currentCharge);
                    }
                }
            }
        }
    }
}
