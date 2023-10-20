#if SUBNAUTICA
using DaftAppleGames.SubnauticaPets.Mono.Pets.Subnautica;
#endif
#if SUBNAUTICAZERO
using DaftAppleGames.SubnauticaPets.Mono.Pets.BelowZero;
#endif
using DaftAppleGames.SubnauticaPets.Mono.Pets;
using DaftAppleGames.SubnauticaPets.Mono.Pets.Custom;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.Utils
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
                // Custom Types
                case PetCreatureType.Cat:
                    newPet = creatureGameObject.AddComponent<CatPet>();
                    break;
#if SUBNAUTICA
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
#endif
#if SUBNAUTICAZERO
                case PetCreatureType.PenglingBaby:
                    newPet = creatureGameObject.AddComponent<PenglingBabyPet>();
                    break;
                case PetCreatureType.PenglingAdult:
                    newPet = creatureGameObject.AddComponent<PenglingAdultPet>();
                    break;
                case PetCreatureType.SnowstalkerBaby:
                    newPet = creatureGameObject.AddComponent<SnowStalkerBabyPet>();
                    break;
                case PetCreatureType.Pinnicarid:
                    newPet = creatureGameObject.AddComponent<PinnicaridPet>();
                    break;
                case PetCreatureType.BlueTrivalve:
                    newPet = creatureGameObject.AddComponent<TrivalveBluePet>();
                    break;
                case PetCreatureType.YellowTrivalve:
                    newPet = creatureGameObject.AddComponent<TrivalveYellowPet>();
                    break;
                default:
                    return null;
#endif
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
