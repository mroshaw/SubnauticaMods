using DaftAppleGames.CreaturePetModSn.MonoBehaviours.Pets;
using UnityEngine;

namespace DaftAppleGames.CreaturePetModSn.CustomObjects
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
        /// <returns></returns>
        public static Pet AddPetComponent(GameObject creatureGameObject, PetCreatureType petCreatureType, PetName petName)
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
    }
}
