using CreaturePetMod_SN.MonoBehaviours;
using HarmonyLib;
using static CreaturePetMod_SN.CreaturePetMod_SNPlugin;

namespace CreaturePetMod_SN.Patches
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
            // Get the creatures PrefabId
            string loadedPrefabId = __instance.gameObject.GetComponent<PrefabIdentifier>().Id;

            // Check the Saver to see if it's got this on record
            // If so, add the Pet component
            PetSaver.PetDetails petDetails = Saver.GetPetDetailsWithPrefabId(loadedPrefabId);
            if (petDetails != null)
            {
                Pet pet = __instance.gameObject.AddComponent<Pet>();
                pet.PetName = petDetails.PetName;
                pet.PetCreatureType = petDetails.PetType;
                pet.PetSaverDetails = petDetails;
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
                Saver.RemovePet(pet);
            }
        }

    }
}