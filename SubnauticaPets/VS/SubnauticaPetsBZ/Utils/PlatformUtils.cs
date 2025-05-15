using DaftAppleGames.SubnauticaPets.Prefabs;

namespace DaftAppleGames.SubnauticaPets.Utils
{
    /// <summary>
    /// Static Utility methods specific to the game platform
    /// In this case, BELOW ZERO.
    /// </summary>
    internal static class PlatformUtils
    {
        internal static bool IsTechTypePet(TechType techType)
        {
            return (techType == PetPrefabs.PenglingBabyPrefab.Info.TechType || techType == PetPrefabs.PengwingAdultPrefab.Info.TechType ||
                    techType == PetPrefabs.SnowstalkerBabyPrefab.Info.TechType || techType == PetPrefabs.PinnacaridPrefab.Info.TechType ||
                    techType == PetPrefabs.TrivalveYellowPrefab.Info.TechType || techType == PetPrefabs.TrivalveBluePrefab.Info.TechType ||
                    techType == CustomPetPrefabs.CatPetPrefab.Info.TechType || techType == CustomPetPrefabs.DogPetPrefab.Info.TechType ||
                    techType == CustomPetPrefabs.RabbitPetPrefab.Info.TechType || techType == CustomPetPrefabs.SealPetPrefab.Info.TechType ||
                    techType == CustomPetPrefabs.WalrusPetPrefab.Info.TechType || techType == CustomPetPrefabs.FoxPetPrefab.Info.TechType);
        }
    }
}