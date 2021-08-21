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
            /// We lose our "creaturePet", so need another way to identify the loaded creature.
            /// </summary>
            /// <param name="__instance"></param>
            [HarmonyPostfix]
            public static void FixBehaviours(Creature __instance)
            {
                // First up, is this creature a pet? We lose our "creaturePet" component
                // to the save game, so need to check another way
                if (PetUtils.IsCreaturePet(__instance))
                {
                    Logger.Log(Logger.Level.Debug, $"Already a pet");
                    return;
                }

                // We check against prefab Id to find pets to reconfigure
                string loadedPrefabId = PetUtils.GetCreaturePrefabId(__instance);

                // If the GUID is in the list, let's reconfigure
                PetDetails petDetails = PetUtils.IsPrefabIdInHashSet(loadedPrefabId);
                if (petDetails != null)
                {
                    Logger.Log(Logger.Level.Debug, $"Start Creature Prefab Id: {loadedPrefabId}");
                    Logger.Log(Logger.Level.Debug, $"Reconfiguring loaded Pet: {__instance.name}");
                    PetBehaviour.ConfigurePetCreature(__instance.gameObject, petDetails);
                    __instance.Start();
                    PetBehaviour.ConfigurePetTraits(__instance);
                }
            }
        }
    }
}