using HarmonyLib;
using static DaftAppleGames.PrawnSuitRepairAndCharge_BZ.PrawnSuitRepairAndChargePluginBz;
namespace DaftAppleGames.PrawnSuitRepairAndCharge_BZ
{
    class PrawnSuitRepairAndChargePatches
    {
        [HarmonyPatch(typeof(Exosuit))]
        internal class ExosuitPatch
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(Exosuit.OnDockedChanged))]
            public static void OnDockedChanged_Postfix(Exosuit __instance, bool docked, Vehicle.DockType dockType)
            {
                Log.LogDebug("In Exosuit.OnDockedChanged");
                if (!docked)
                {
                    return;
                }

                Log.LogDebug($"Dock Change at: {dockType}, Docked is: {docked}");

                // If no options are checked, do nothing more
                if(!ConfigFile.EnableInMoonPool && !ConfigFile.EnableInSeaTruck)
                {
                    Log.LogDebug($"All options disabled. Nothing to do!");
                    return;
                }

                Dockable dockable = __instance.GetComponent<Dockable>();
                if(!dockable)
                {
                    Log.LogDebug($"Couldn't find Dockable!");
                    return;
                }

                // Calculate the deficits
                float powerToAdd = CalculatePowerDeficit(__instance);
                float healthToAdd = CalculateHealthDeficit(__instance);

                // MoonPool Dock
                if (dockType == Vehicle.DockType.Base && ConfigFile.EnableInMoonPool)
                {
                    Log.LogDebug($"Docked in MoonPool...");

                    // Repair and charge, no cost to the Base
                    RepairVehicle(__instance, healthToAdd);
                    ChargeVehicle(__instance, powerToAdd);
                    ErrorMessage.AddMessage($"Prawn Suit repaired and charged!");

                    Log.LogDebug($"Docked in MoonPool... Done!");
                }

                // SeaTruck Dock
                if (dockType == Vehicle.DockType.Seatruck && ConfigFile.EnableInSeaTruck)
                {
                    Log.LogDebug($"Docked in SeaTruck...");

                    // If consuming SeaTruck power to charge, calculate what we can draw and ensure we subtract that from the SeaTruck
                    if (ConfigFile.ConsumeSeaTruckPower)
                    {
                        // Get the SeaTruckDockingBay that we're docked to
                        SeaTruckDockingBay seaTruckDockingBay = dockable.bay as SeaTruckDockingBay;
                        Log.LogDebug($"Found SeaTruckDockingBay: {seaTruckDockingBay.name}");

                        // Get the amount of Energy left in the SeaTruck (found via the Relay attached to the DockingBay)
                        float currentSeaTruckPower = GetSeaTruckPower(seaTruckDockingBay);

                        // Draw the power from the SeaTruck
                        float powerToRepair = healthToAdd * ConfigFile.SeaTruckPowerUseRepairModifier;
                        Log.LogDebug($"Power required to repair ({healthToAdd} x {ConfigFile.SeaTruckPowerUseRepairModifier}): {powerToRepair}");

                        float powerToCharge = powerToAdd * ConfigFile.SeaTruckPowerUseChargeModifier;
                        Log.LogDebug($"Power required to charge ({powerToAdd} x {ConfigFile.SeaTruckPowerUseChargeModifier}): {powerToCharge}");

                        float totalPowerRequired = powerToRepair + powerToCharge;
                        Log.LogDebug($"Total power required: {totalPowerRequired}");

                        if(totalPowerRequired > currentSeaTruckPower)
                        {
                            ErrorMessage.AddMessage($"Prawn Suit not recharged! Insufficient Seatruck power!");
                            return;
                        }

                        // Remove the required power from the SeaTruck
                        int totalPowerInt = (int)RemoveSeatruckPower(seaTruckDockingBay, totalPowerRequired);
                        ErrorMessage.AddMessage($"Process consumed {totalPowerInt} energy.");
                    }

                    // Repair and charge
                    RepairVehicle(__instance, healthToAdd);
                    ChargeVehicle(__instance, powerToAdd);

                    ErrorMessage.AddMessage($"Prawn Suit repaired and charged!");

                    Log.LogDebug($"Docked in SeaTruck... Done!");
                }
            }

            /// <summary>
            /// Remove the specified amount of power from the given SeaTruck
            /// </summary>
            private static float RemoveSeatruckPower(SeaTruckDockingBay seaTruckDockingBay, float powerToRemove)
            {
                PowerRelay powerRelay = seaTruckDockingBay.relay;
                IPowerInterface powerInterface = powerRelay.GetComponent<IPowerInterface>();
                powerInterface.ConsumeEnergy(powerToRemove, out float amountConsumed);
                Log.LogDebug($"Removed SeaTruck power: {powerToRemove}");
                return amountConsumed;
            }

            /// <summary>
            /// Get the current power of the specified Seatruck
            /// </summary>
            private static float GetSeaTruckPower(SeaTruckDockingBay seaTruckDockingBay)
            {
                PowerRelay powerRelay = seaTruckDockingBay.relay;
                float currentPower = powerRelay.GetPower();
                Log.LogDebug($"Current SeaTruck power: {currentPower}");
                return currentPower;
            }

            /// <summary>
            /// Calculate how much energy is required to "top up"
            /// </summary>
            private static float CalculatePowerDeficit(Vehicle vehicleInstance)
            {
                // Get current charge and max charge
                vehicleInstance.energyInterface.GetValues(out float currentCharge, out float currentCapacity);
                float powerDelta = currentCapacity - currentCharge;
                Log.LogDebug($"Current Prawn Suit charge: {currentCharge}, Max charge: {currentCapacity}, Charge delta: {powerDelta}");
                return powerDelta;
            }

            /// <summary>
            /// Calculate how much health is needed for maximum
            /// </summary>
            private static float CalculateHealthDeficit(Vehicle vehicleInstance)
            {
                float currentHealth = vehicleInstance.liveMixin.health;
                float maxHealth = vehicleInstance.liveMixin.maxHealth;
                float healthDelta = maxHealth - currentHealth;
                Log.LogDebug($"Current Prawn Suit health: {currentHealth}, Max health: {maxHealth}, Health delta: {healthDelta}");
                return healthDelta;
            }

            /// <summary>
            /// Repair the PrawnSuit by the specified amount
            /// </summary>
            private static void RepairVehicle(Vehicle vehicleInstance, float healthToAdd)
            {
                // Top up health
                vehicleInstance.liveMixin.AddHealth(healthToAdd);
                Log.LogDebug($"Added health to Prawn Suit: {healthToAdd}");
            }

            /// <summary>
            /// Charge the PrawnSuit by the specified amount
            /// </summary>
            private static void ChargeVehicle(Vehicle vehicleInstance, float powerToAdd)
            {
                vehicleInstance.AddEnergy(powerToAdd);
                Log.LogDebug($"Added power to Prawn Suit: {powerToAdd}");
            }
        }
    }
}