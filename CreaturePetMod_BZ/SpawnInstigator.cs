using HarmonyLib;
using Logger = QModManager.Utility.Logger;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace CreaturePetMod_BZ
{
    /// <summary>
    /// Mods to enable additonal controls that can be used to spawn pet creatures
    /// </summary>
    class SpawnInstigator
    {
        /// <summary>
        /// Class to manage "spawning" of a new pet
        /// </summary>
        [HarmonyPatch(typeof(Player))]
        [HarmonyPatch("Update")]
        internal class PetSpawner
        {
            [HarmonyPostfix]
            public static void OnSpawn()
            {
                // Check for "Spawn Pet" keypress
                if (Input.GetKeyUp(QMod.Config.SpawnPetKey))
                {
                    Logger.Log(Logger.Level.Debug, $"Spawn keypress detected");
                    CreaturePetMod_BZ.PetSpawner.SpawnCreaturePet();
                    Logger.Log(Logger.Level.Debug, $"Pet spawned!");
                }
            }
        }

        /// <summary>
        /// Class to manage the death of a pet
        /// </summary>
        [HarmonyPatch(typeof(Creature))]
        [HarmonyPatch("OnKill")]
        internal class PetKiller
        {
            [HarmonyPostfix]
            public static void OnKill(Creature __instance)
            {
                // Check to see if the creature is a Pet
                if (PetUtils.IsCreaturePet(__instance))
                {
                    Logger.Log(Logger.Level.Debug, $"Pet death detected");
                    PetDetails petDetails = __instance.GetComponentInParent<CreaturePet>().GetPetDetailsObject();
                    QMod.PetDetailsHashSet.Remove(petDetails);

                    // Stop floating away!
                    __instance.GetComponentInParent<Rigidbody>().mass = 99.0f;

                    // So long, fuzzball.
                    Logger.Log(Logger.Level.Debug, $"Pet removed!");
                    ErrorMessage.AddMessage($"One of your pets has died! Farewell, {petDetails.PetName}!");
                }
            }
        }
    }
}
