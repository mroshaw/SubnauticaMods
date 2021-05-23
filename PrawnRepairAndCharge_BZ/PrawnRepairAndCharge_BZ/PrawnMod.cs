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
            [HarmonyPrefix]
            public static void Prefix(Exosuit __instance, bool docked, Vehicle.DockType dockType)
            {
                Logger.Log(Logger.Level.Debug, $"In pre fix {docked}");
                if (docked)
                {
                    // Get current charge
                    if ((dockType == Vehicle.DockType.Base && QMod.Config.EnableMoonPool) || (dockType == Vehicle.DockType.Seatruck && QMod.Config.EnableSeaTruck))
                    {
                        // Get current health
                        float currentHealth = __instance.liveMixin.GetHealthFraction();
                        Logger.Log(Logger.Level.Debug, $"Current health fraction: {currentHealth}");
                        // Top up health
                        __instance.liveMixin.AddHealth(100.00F - currentHealth);
                        currentHealth = __instance.liveMixin.GetHealthFraction();
                        Logger.Log(Logger.Level.Debug, $"New health fraction: {currentHealth}");
                        try
                        {
                            // Get current charge
                            __instance.energyInterface.GetValues(out float currentCharge, out float currentCapacity);
                            Logger.Log(Logger.Level.Debug, $"Current charge: {currentCharge}, current capacity {currentCapacity}, delta: {currentCapacity - currentCharge}");
                            __instance.AddEnergy(currentCapacity - currentCharge);
                        }
                        catch(Exception e)
                        {
                            Logger.Log(Logger.Level.Debug, $"Bugger: {e}");
                        }
                    }
                }
            }
        }
    }
}
