using DaftAppleGames.SubnauticaPets.MonoBehaviours;
using DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets;
using DaftAppleGames.SubnauticaPets.Utils;
using HarmonyLib;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.Patches
{
    /// <summary>
    /// Patches Creature methods, to ensure we add the Pet component
    /// to creatures as they are loaded.
    /// </summary>
    [HarmonyPatch(typeof(Creature))]
    internal class CreaturePatches
    {
        /// <summary>
        /// Patches the Player Awake method with prefix code.
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(Creature.Start))]
        [HarmonyPostfix]
        public static void Start_PostFix(Creature __instance)
        {
            // If already a pet, move on
            if (__instance.GetComponent<Pet>())
            {
                return;
            }
            // Get the creatures PrefabId
            string loadedPrefabId = __instance.gameObject.GetComponent<PrefabIdentifier>().Id;

            // Check the Saver to see if it's got this on record
            // If so, add the Pet component
            PetSaver.PetDetails petDetails = Saver.GetPetDetailsWithPrefabId(loadedPrefabId);
            if (petDetails != null)
            {
                Pet pet = PetUtils.AddPetComponent(__instance.gameObject, petDetails.PetType, petDetails.PetName);
                pet.PetSaverDetails = petDetails;
                __instance.gameObject.name = $"{__instance.gameObject.name}_Pet";
            }
        }

        [HarmonyPatch(nameof(Creature.OnKill))]
        [HarmonyPostfix]
        public static void OnKill_PostFix(Creature __instance)
        {
            // Check if the dead creature is a Pet
            Pet pet = __instance.GetComponent<Pet>();
            if (pet)
            {
                Saver.UnregisterPet(pet);
                Saver.RemovePetFromList(pet);
            }
        }
    }
}