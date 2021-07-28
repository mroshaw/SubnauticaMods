using HarmonyLib;
using Logger = QModManager.Utility.Logger;
using UnityEngine;
using System.Linq;

namespace CreaturePetMod_BZ
{
    /// <summary>
    /// Contains hooks and classes to manage loading a game with Pets
    /// </summary>
    class LoadGameOverride
    {
        [HarmonyPatch(typeof(Creature))]
        [HarmonyPatch("Start")]
        internal class CreatureLoad
        {
            /// <summary>
            /// This fixes an issue where loading a save game "undoes" our GameObject changes
            /// We lose our "PetTag", so need another way to identify the loaded creature.
            /// </summary>
            /// <param name="__instance"></param>
            [HarmonyPostfix]
            public static void FixBehaviours(Creature __instance)
            {
                // We check against prefab Id to find pets to reconfigure
                string loadedPrefabId = PetUtils.GetCreaturePreFabId(__instance);
                Logger.Log(Logger.Level.Debug, $"Start Creature Prefab Id: {loadedPrefabId}");

                // First up, is this creature a pet? We lose our "PetTag" component
                // to the save game, so need to check another way
                if (PetUtils.IsCreaturePet(__instance))
                {
                    Logger.Log(Logger.Level.Debug, $"Already a pet");
                    return;
                }           

                // If the GUID is in the list, let's reconfigure
                if (QMod.PetPrefabKeyList.Any(loadedPrefabId.Contains))
                {
                    Logger.Log(Logger.Level.Debug, $"Reconfiguring loaded Pet: {__instance.name}");
                    PetBehaviour.ConfigurePetCreature(__instance.gameObject);
                }
            }
        }
    }
}