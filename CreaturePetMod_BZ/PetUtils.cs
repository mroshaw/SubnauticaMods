using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Logger = QModManager.Utility.Logger;

namespace CreaturePetMod_BZ
{
    /// <summary>
    /// Selection of Static methods to manage Pets
    /// </summary>
    class PetUtils
    {

        // Selection of animations on petting
        //  { "peck", "flutter" };
        //  { "shudder", "call", "trip" };
        //  { "chilling", "standUpSniff", "standUpHowl", "dryFur" };
        private static string[] PenglingAdultAnims = { "peck", "flutter" };
        private static string[] PenglingBabyAnims = { "shudder", "call" };
        private static string[] SnowStalkerBabyAnims = { "dryFur" };

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
        /// Returns a random animation for the pet
        /// </summary>
        /// <param name="petType"></param>
        /// <returns></returns>
        public static string GetRandomAnim(PetCreatureType petType)
        {
            System.Random random = new System.Random();
            int index;
            switch (petType)
            {
                case PetCreatureType.PenglingAdult:
                    index = random.Next(PenglingAdultAnims.Count());
                    return PenglingAdultAnims[index];
                case PetCreatureType.PenglingBaby:
                    index = random.Next(PenglingBabyAnims.Count());
                    return PenglingBabyAnims[index];
                case PetCreatureType.SnowstalkerBaby:
                    index = random.Next(SnowStalkerBabyAnims.Count());
                    return SnowStalkerBabyAnims[index];
                default:
                    return "";
            }
        }
    }
}
