using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Logger = QModManager.Utility.Logger;

namespace CreaturePetMod_BZ
{
    class PetUtils
    {
        /// <summary>
        /// Determine if a Creature is a pet
        /// </summary>
        /// <param name="creaturePet"></param>
        /// <returns></returns>
        internal static bool IsCreaturePet(Creature creaturePet)
        {
            return (creaturePet.GetComponentInParent<CreaturePet>());
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
    }
}
