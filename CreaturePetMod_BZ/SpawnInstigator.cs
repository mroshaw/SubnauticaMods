using HarmonyLib;
using Logger = QModManager.Utility.Logger;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace CreaturePetMod_BZ
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
                // Check for "Spawn Pet" keypress
                if (Input.GetKeyUp(QMod.Config.SpawnPetKey))
                {
                    Logger.Log(Logger.Level.Debug, $"Spawn keypress detected");
                    PetSpawner.SpawnCreaturePet();

                }
            }
        }
    }
}
