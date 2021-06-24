using HarmonyLib;
using Logger = QModManager.Utility.Logger;

namespace PrawnRepairAndCharge_BZ
{
    class PrawnSuitMod
    {
        [HarmonyPatch(typeof(Exosuit))]
        [HarmonyPatch("OnDockedChanged")]
        internal class ExoSuitDock
        {
            [HarmonyPrefix]
            public static void Prefix(Exosuit __instance, bool docked, Vehicle.DockType dockType)
            {
                Logger.Log(Logger.Level.Debug, $"In Exosuit.OnDockedChanged");
                if (docked)
                {
                    Logger.Log(Logger.Level.Debug, $"Dock Change at: {dockType}, Docked is: {docked}");

                    // If we're docked in MoonPool or SeaTruck, and option is enabled, make the modifications
                    if ((dockType == Vehicle.DockType.Base && QMod.Config.EnableMoonPool) || (dockType == Vehicle.DockType.Seatruck && QMod.Config.EnableSeaTruck))
                    {
                        Vehicle baseInstance = (Vehicle)__instance;
                        // Get current health
                        float currentHealth = __instance.liveMixin.GetHealthFraction();
                        Logger.Log(Logger.Level.Debug, $"Current health fraction: {currentHealth}");

                        // Top up health
                        float healthDelta = 1F - currentHealth;
                        __instance.liveMixin.AddHealth(healthDelta);
                        Logger.Log(Logger.Level.Debug, $"Health Delta: {healthDelta}");

                        // Get current charge
                        baseInstance.energyInterface.GetValues(out float currentCharge, out float currentCapacity);
                        Logger.Log(Logger.Level.Debug, $"Current charge: {currentCharge}, Current capacity: {currentCapacity}");
                        float powerDelta = currentCapacity - currentCharge;

                        // Top up charge
                        baseInstance.AddEnergy(powerDelta);
                        Logger.Log(Logger.Level.Debug, $"Current charge: {currentCharge}, current capacity {currentCapacity}, delta: {powerDelta}");
                    }
                }
            }
        }
    }
}
