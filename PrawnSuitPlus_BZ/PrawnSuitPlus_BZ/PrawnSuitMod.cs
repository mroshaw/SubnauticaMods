using System;
using HarmonyLib;
using SMLHelper.V2;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger = QModManager.Utility.Logger;
using UnityEngine;

namespace PrawnSuitPlus_BZ
{
    class PrawnSuitMod
    {
        /// <summary>
        /// Dock and Charge
        /// </summary>
        [HarmonyPatch(typeof(Exosuit))]
        [HarmonyPatch("OnDockedChanged")]
        internal class ExoSuitDockAndCharge
        {
            [HarmonyPrefix]
            public static void Prefix(Exosuit __instance, bool docked, Vehicle.DockType dockType)
            {
                Logger.Log(Logger.Level.Debug, $"ExoSuitDockAndCharge");
                if (docked)
                {
                    // Charge and Repair as appropriate
                    if ((dockType == Vehicle.DockType.Base && QMod.Config.RepairEnableMoonPool) || dockType == Vehicle.DockType.Seatruck && QMod.Config.RepairEnableSeaTruck)
                    {
                        // Get current health
                        float currentHealth = __instance.liveMixin.GetHealthFraction();
                        Logger.Log(Logger.Level.Debug, $"Current health fraction: {currentHealth}");
                        // Top up health
                        __instance.liveMixin.AddHealth(100.00F - currentHealth);
                        currentHealth = __instance.liveMixin.GetHealthFraction();
                        Logger.Log(Logger.Level.Debug, $"New health fraction: {currentHealth}");
                    }
                    if ((dockType == Vehicle.DockType.Base && QMod.Config.ChargingEnableMoonPool) || dockType == Vehicle.DockType.Seatruck && QMod.Config.ChargingEnableSeaTruck)
                    {
                        // Get current charge
                        __instance.energyInterface.GetValues(out float currentCharge, out float currentCapacity);
                        Logger.Log(Logger.Level.Debug, $"Current charge: {currentCharge}, current capacity {currentCapacity}, delta: {currentCapacity - currentCharge}");
                        __instance.AddEnergy(currentCapacity - currentCharge);
                    }
                }
            }
        }
        /// <summary>
        /// PowerDrill
        /// </summary>
        [HarmonyPatch(typeof(LiveMixin))]
        [HarmonyPatch("TakeDamage")]
        internal class DamageModifier
        {
            [HarmonyPrefix]
            public static void Prefix(LiveMixin __instance, ref float originalDamage, DamageType type)
            {
                if (type == DamageType.Drill)
                {
                    Logger.Log(Logger.Level.Info, $"ExoSuitPowerDrill");
                    float additionalDamage = QMod.Config.DrillArmDamageModifier;
                    float totalDamage = originalDamage + additionalDamage;
                    if (__instance.health > additionalDamage + 1.0F)
                    {
                        Logger.Log(Logger.Level.Debug, $"Total Damage: {totalDamage}");
                        originalDamage -= totalDamage;
                    }
                }
                if (QMod.Config.Invincible)
                {
                    Logger.Log(Logger.Level.Info, $"ExoSuitInvincibility");
                    // Check to see if this is our Prawn Suit
                    if (__instance.GetType().BaseType == typeof(Exosuit))
                    {
                        Logger.Log(Logger.Level.Info, $"Prawn Suit Damage set to 0");
                        originalDamage = 0;
                    }
                }
            }
        }
        /// <summary>
        /// Super Thrust
        /// </summary>
        [HarmonyPatch(typeof(Exosuit))]
        [HarmonyPatch("ApplyJumpForce")]
        internal class ExoSuitJump
        {
            [HarmonyPrefix]
            public static void Prefix(Exosuit __instance)
            {
                // Calculate and apply the new thrust value
                Logger.Log(Logger.Level.Debug, $"ExoSuitThrust");
                float thrustModifier = QMod.Config.ThrustModifier;
                Logger.Log(Logger.Level.Debug, $"Adding force modifier: {thrustModifier}");
                __instance.useRigidbody.AddForce(Vector3.up * (__instance.jumpJetsUpgraded ? (7f * (thrustModifier - 1f)) : (5f * (thrustModifier - 1f))), ForceMode.VelocityChange);

            }
        }
    }
}
