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
    class PlayerInputMod
    {
        [HarmonyPatch(typeof(Player))]
        [HarmonyPatch("Update")]
        internal class KeyboardInput
        {
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
                    AquariumsMod.ToggleFishScoop(QMod.Config.EnableFishScoop);
                }

                // Check for "purge aquariums" keypress
                if (Input.GetKeyUp(QMod.Config.ReleaseFishKey))
                {
                    Logger.Log(Logger.Level.Debug, "Purge keypress detected");
                    // Only allow when pilotinbg Seatruck
                    if (!Player.main.IsPilotingSeatruck())
                    {
                        Logger.Log(Logger.Level.Debug, "Purge: Not piloting. Abort.");
                        return;
                    }
                    Logger.Log(Logger.Level.Debug, "Attempting to purge Aquariums...");
                    AquariumsMod.PurgeAllFish();
                    Logger.Log(Logger.Level.Debug, "Aquariums purged!");
                }
            }

        }
    }
}
