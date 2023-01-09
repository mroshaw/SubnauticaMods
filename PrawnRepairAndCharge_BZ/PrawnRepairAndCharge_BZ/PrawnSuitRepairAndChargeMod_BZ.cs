using HarmonyLib;

namespace Mroshaw.PrawnSuitRepairAndCharge_BZ
{
    class PrawnSuitRepairAndChargeMod_BZ
    {
        [HarmonyPatch(typeof(Exosuit))]
        internal class Exosuit_Patch
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(Exosuit.OnDockedChanged))]
            public static void OnDockedChanged_Prefix(Exosuit __instance, bool docked, Vehicle.DockType dockType)
            {
                PrawnSuitRepairAndChargePlugin_BZ.Log.LogDebug("In Exosuit.OnDockedChanged");
                if (docked)
                {
                    PrawnSuitRepairAndChargePlugin_BZ.Log.LogDebug($"Dock Change at: {dockType}, Docked is: {docked}");

                    // If we're docked in MoonPool or SeaTruck, and option is enabled, make the modifications
                    if ((dockType == Vehicle.DockType.Base && PrawnSuitRepairAndChargePlugin_BZ.EnableInMoonPool.Value) ||
                        (dockType == Vehicle.DockType.Seatruck && PrawnSuitRepairAndChargePlugin_BZ.EnableInSeaTruck.Value))
                    {
                        Vehicle baseInstance = (Vehicle)__instance;
                        // Get current and max health
                        float currentHealth = __instance.liveMixin.health;
                        float maxHealth = __instance.liveMixin.maxHealth;
                        PrawnSuitRepairAndChargePlugin_BZ.Log.LogDebug($"Current health: {currentHealth}, Max health: {maxHealth}");
                        float deltaHealth = maxHealth - currentHealth;

                        // Top up health
                        __instance.liveMixin.AddHealth(deltaHealth);
                        PrawnSuitRepairAndChargePlugin_BZ.Log.LogDebug($"Added health delta: {deltaHealth}");

                        // Get current charge and max charge
                        baseInstance.energyInterface.GetValues(out float currentCharge, out float currentCapacity);
                        PrawnSuitRepairAndChargePlugin_BZ.Log.LogDebug($"Current charge: {currentCharge}, Max charge: {currentCapacity}");
                        float deltaPower = currentCapacity - currentCharge;

                        // Top up charge
                        baseInstance.AddEnergy(deltaPower);
                        PrawnSuitRepairAndChargePlugin_BZ.Log.LogDebug($"Added power delta: {deltaPower}");
                    }
                }
            }
        }
    }
}
