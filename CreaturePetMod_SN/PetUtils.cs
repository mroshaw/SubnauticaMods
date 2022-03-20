using System.Linq;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Logger = QModManager.Utility.Logger;

namespace CreaturePetMod_SN
{
    /// <summary>
    /// Selection of Static methods to manage Pets
    /// </summary>
    class PetUtils
    {

           /// <summary>
        /// Determine if a Creature is a pet
        /// </summary>
        /// <param name="creaturePet"></param>
        /// <returns></returns>
        internal static bool IsCreaturePet(Creature creaturePet)
        {
            return creaturePet.GetComponentInParent<CreaturePet>();
        }

        /// <summary>
        /// Gets the GUID Id of the PrefabIdentifier associated with the given Creature
        /// </summary>
        /// <param name="creaturePet"></param>
        /// <returns></returns>
        internal static string GetCreaturePrefabId(Creature creaturePet)
        {
            return creaturePet.GetComponent<PrefabIdentifier>().Id;
        }

        /// <summary>
        /// Determine if the GameObject is a pet
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        internal static bool IsGameGameObjectPet(GameObject gameObject)
        {
            return gameObject.GetComponentInParent<CreaturePet>();
        }

        /// <summary>
        /// Determines whether the given prefab id is in the pet hashset
        /// </summary>
        /// <param name="prefabid"></param>
        /// <returns></returns>
        internal static PetDetails IsPrefabIdInHashSet(string prefabid)
        {
            foreach (PetDetails petDetails in QMod.PetDetailsHashSet)
            {
                if (petDetails.PrefabId == prefabid)
                {
                    return petDetails;
                }
            }
            return null;
        }

    
        /// <summary>
        /// Kills ALL pets. Use with caution!
        /// </summary>
        public static void KillAllPets()
        {

            CreaturePet[] creaturePets = GameObject.FindObjectsOfType<CreaturePet>();
            Logger.Log(Logger.Level.Debug, $"Killing {creaturePets.Count()} pets");
            foreach (CreaturePet creaturePet in creaturePets)
            {
                KillPet(creaturePet);
            }
        }

        /// <summary>
        /// Kill the specified pet
        /// </summary>
        /// <param name="pet"></param>
        public static void KillPet(CreaturePet pet)
        {
            // Get LiveMixIn component and kill
            Creature creature = pet.GetComponentInParent<Creature>();
            LiveMixin liveMixin = pet.GetComponentInParent<LiveMixin>();

            // Kill the Creature
            Logger.Log(Logger.Level.Debug, $"Killing {creature.GetType()} ({pet.GetPetName()})");
            liveMixin.Kill();
        }
    }
}
