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
        /// Determine if a GameObject is a pet
        /// </summary>
        /// <param name="parentGameObject"></param>
        /// <returns></returns>
        public static bool IsGameObjectPet(GameObject parentGameObject)
        {
            return (parentGameObject.GetComponentInParent<PetTag>());
        }

        /// <summary>
        /// Determine if a Creature is a pet
        /// </summary>
        /// <param name="parentCreature"></param>
        /// <returns></returns>
        public static bool IsCreaturePet(Creature parentCreature)
        {
            return (parentCreature.GetComponent<PetTag>());
        }

        public static string GetCreaturePreFabId(Creature parentCreature)
        {
            return parentCreature.GetComponent<PrefabIdentifier>().Id;
        }

        public static bool StorePrefabId(GameObject petCreatureGameObject)
        {
            // Get and store the Prefab keys to allow us to reconfigure these again on loading
            Logger.Log(Logger.Level.Debug, $"Getting Prefab Id to add to list");
            PrefabIdentifier prefabIdentifier = petCreatureGameObject.GetComponentInParent<PrefabIdentifier>();
            string prefabId = prefabIdentifier.Id;
            if (string.IsNullOrEmpty(prefabId))
            {
                return false;
            }
            else
            {
                Logger.Log(Logger.Level.Debug, $"Adding Prefab Idenfifier to list: {prefabId}");
                QMod.PetPrefabKeyList.Add(prefabId);
                return true;
            }
        }
    }
}
