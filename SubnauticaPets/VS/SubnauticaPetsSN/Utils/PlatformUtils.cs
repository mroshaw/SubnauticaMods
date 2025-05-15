using DaftAppleGames.SubnauticaPets.Prefabs;

namespace DaftAppleGames.SubnauticaPets
{
    /// <summary>
    /// Static Utility methods specific to the game platform
    /// In this case, the original Subnautica.
    /// </summary>
    internal static class PlatformUtils
    {
        /// <summary>
        /// Static method to determine if the given TechType is a Pet or not
        /// </summary>
        internal static bool IsPetTechType(TechType techType)
        {
            return (techType == PetPrefabs.AlienRobotPrefab.Info.TechType || techType == PetPrefabs.CaveCrawlerPrefab.Info.TechType ||
                    techType == PetPrefabs.BloodCrawlerPrefab.Info.TechType || techType == PetPrefabs.CrabSquidPrefab.Info.TechType ||
                    techType == CustomPetPrefabs.CatPetPrefab.Info.TechType || techType == CustomPetPrefabs.DogPetPrefab.Info.TechType ||
                    techType == CustomPetPrefabs.RabbitPetPrefab.Info.TechType || techType == CustomPetPrefabs.SealPetPrefab.Info.TechType ||
                    techType == CustomPetPrefabs.WalrusPetPrefab.Info.TechType || techType == CustomPetPrefabs.FoxPetPrefab.Info.TechType);
        }
    }
}