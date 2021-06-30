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
                Logger.Log(Logger.Level.Debug, "In Exosuit.OnDockedChanged");
                if (docked)
                {
                    Logger.Log(Logger.Level.Debug, $"Dock Change at: {dockType}, Docked is: {docked}");

                    // If we're docked in MoonPool or SeaTruck, and option is enabled, make the modifications
                    if ((dockType == Vehicle.DockType.Base && QMod.Config.EnableMoonPool) || (dockType == Vehicle.DockType.Seatruck && QMod.Config.EnableSeaTruck))
                    {
                        Vehicle baseInstance = (Vehicle)__instance;
                        // Get current and max health
                        float currentHealth = __instance.liveMixin.health;
                        float maxHealth = __instance.liveMixin.maxHealth;
                        Logger.Log(Logger.Level.Debug, $"Current health: {currentHealth}, Max health: {maxHealth}");
                        float deltaHealth = maxHealth - currentHealth;

                        // Top up health
                        __instance.liveMixin.AddHealth(deltaHealth);
                        Logger.Log(Logger.Level.Debug, $"Added health delta: {deltaHealth}");

                        // Get current charge and max charge
                        baseInstance.energyInterface.GetValues(out float currentCharge, out float currentCapacity);
                        Logger.Log(Logger.Level.Debug, $"Current charge: {currentCharge}, Max charge: {currentCapacity}");
                        float deltaPower = currentCapacity - currentCharge;

                        // Top up charge
                        baseInstance.AddEnergy(deltaPower);
                        Logger.Log(Logger.Level.Debug, $"Added power delta: {deltaPower}");
                    }
                }
            }
        }
    }
}
