using HarmonyLib;
using Logger = QModManager.Utility.Logger;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace SeaTruckFishScoop_BZ
{
    /// <summary>
    /// Mods to enable additonal controls that can be used within the Seatruck cab
    /// </summary>
    class SeatruckControlsMod
    {
        [HarmonyPatch(typeof(Player))]
        [HarmonyPatch("Update")]
        internal class PurgeAquariums
        {
            // Static FMODAsset for playing sounds
            private static readonly FMODAsset PowerSound = ScriptableObject.CreateInstance<FMODAsset>();

            [HarmonyPostfix]
            public static void Update()
            {
                // Check for "toggle fish scoop" keypress
                if (Input.GetKeyUp(QMod.Config.ToggleFishScoop))
                {
                    Logger.Log(Logger.Level.Debug, $"Toggle keypress detected");
                    // Only toggle when pilotinbg Seatruck
                    if (!Player.main.IsPilotingSeatruck())
                    {
                        Logger.Log(Logger.Level.Debug, "Toggle: Not piloting. Abort.");
                        return;
                    }

                    // Toggle scoope
                    ToggleFishScoop(QMod.Config.EnableFishScoop);
                }
            }
            
            /// <summary>
            /// Plays a sound when the Fuel Scoop is toggled
            /// </summary>
            private static void ToggleFishScoop(bool currentState)
            {

                Logger.Log(Logger.Level.Debug, $"Toggling fish scoop from: {currentState}...");
                QMod.Config.EnableFishScoop = !currentState;
                Logger.Log(Logger.Level.Debug, $"Toggled to: {QMod.Config.EnableFishScoop}");

                if (currentState)
                {
                    ErrorMessage.AddMessage($"Fish scoop DISABLED");                    
                    
                    PowerSound.path = "event:/sub/base/power_off";
                    FMODUWE.PlayOneShot(PowerSound, Player.main.transform.position);
                }
                else
                {
                    ErrorMessage.AddMessage($"Fish scoop ENABLED");
                    PowerSound.path = "event:/sub/cyclops/start";
                    FMODUWE.PlayOneShot(PowerSound, Player.main.transform.position);
                }
            }
        }
    }
}
