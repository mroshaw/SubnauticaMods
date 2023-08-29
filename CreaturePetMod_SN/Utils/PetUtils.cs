using DaftAppleGames.CreaturePetModSn.MonoBehaviours.Pets;
using UnityEngine;
using static DaftAppleGames.CreaturePetModSn.CreaturePetModSnPlugin;

namespace DaftAppleGames.CreaturePetModSn.Utils
{
    /// <summary>
    /// Static utilities class for common functions and properties to be used within your mod code
    /// </summary>
    internal static class PetUtils
    {
        /// <summary>
        /// Adds the specified child pet class component to the given creature GameObject
        /// based on the given PetCreatureType
        /// </summary>
        /// <param name="creatureGameObject"></param>
        /// <param name="petCreatureType"></param>
        /// <param name="petName"></param>
        /// <returns></returns>
        public static Pet AddPetComponent(GameObject creatureGameObject, PetCreatureType petCreatureType, string petName)
        {
            Pet newPet;

            switch (petCreatureType)
            {
                case PetCreatureType.CaveCrawler:
                    newPet = creatureGameObject.AddComponent<CaveCrawlerPet>();
                    break;
                case PetCreatureType.BloodCrawler:
                    newPet = creatureGameObject.AddComponent<BloodCrawlerPet>();
                    break;
                case PetCreatureType.CrabSquid:
                    newPet = creatureGameObject.AddComponent<CrabSquidPet>();
                    break;
                case PetCreatureType.AlienRobot:
                    newPet = creatureGameObject.AddComponent<AlienRobotPet>();
                    break;
                default:
                    return null;
            }

            newPet.PetName = petName;
            newPet.PetCreatureType = petCreatureType;

            return newPet;
        }

        /// <summary>
        /// Find and Kill all pets
        /// </summary>
        public static void KillAllPets()
        {
            foreach (Pet pet in Saver.PetList.ToArray())
            {
                pet.Kill();
            }
        }
    }
}
