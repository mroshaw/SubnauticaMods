using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets;

namespace DaftAppleGames.SubnauticaPets.Prefabs
{
    internal static class CustomPetPrefabs
    {
        internal static void RegisterAll()
        {
            CatPetPrefab.Register();
            DogPetPrefab.Register();
            RabbitPetPrefab.Register();
            SealPetPrefab.Register();
            WalrusPetPrefab.Register();
            FoxPetPrefab.Register();
        }

        // Cat
        internal static class CatPetPrefab
        {
            // Init PrefabInfo
            internal static PrefabInfo Info;

            /// <summary>
            /// Register Cat
            /// </summary>
            internal static void Register()
            {
                Info = PrefabInfo
                    .WithTechType("CatPet", null, null, unlockAtStart: true)
                    .WithIcon(ModUtils.GetSpriteFromAssetBundle("CatTexture"));
                PrefabConfigUtilsPlatform.RegisterCustomPet(Info, "PetCat", "PetCat", Info.TechType, PetDnaPrefabs.CatDnaPrefab.Info.TechType, true);
            }
        }

        internal static class DogPetPrefab
        {
            // Init PrefabInfo
            internal static PrefabInfo Info;

            /// <summary>
            /// Register Cat
            /// </summary>
            internal static void Register()
            {
                Info = PrefabInfo
                    .WithTechType("DogPet", null, null, unlockAtStart: true)
                    .WithIcon(ModUtils.GetSpriteFromAssetBundle("DogTexture"));
                PrefabConfigUtilsPlatform.RegisterCustomPet(Info, "PetDog", "PetDog", Info.TechType, PetDnaPrefabs.CatDnaPrefab.Info.TechType);
            }
        }

        internal static class RabbitPetPrefab
        {
            // Init PrefabInfo
            internal static PrefabInfo Info;

            /// <summary>
            /// Register Cat
            /// </summary>
            internal static void Register()
            {
                Info = PrefabInfo
                    .WithTechType("RabbitPet", null, null, unlockAtStart: true)
                    .WithIcon(ModUtils.GetSpriteFromAssetBundle("RabbitTexture"));
                PrefabConfigUtilsPlatform.RegisterCustomPet(Info, "PetRabbit", "PetRabbit", Info.TechType, PetDnaPrefabs.CatDnaPrefab.Info.TechType);
            }
        }

        internal static class SealPetPrefab
        {
            // Init PrefabInfo
            internal static PrefabInfo Info;

            /// <summary>
            /// Register Cat
            /// </summary>
            internal static void Register()
            {
                Info = PrefabInfo
                    .WithTechType("SealPet", null, null, unlockAtStart: true)
                    .WithIcon(ModUtils.GetSpriteFromAssetBundle("SealTexture"));
                PrefabConfigUtilsPlatform.RegisterCustomPet(Info, "PetSeal", "PetSeal", Info.TechType, PetDnaPrefabs.CatDnaPrefab.Info.TechType);
            }
        }

        internal static class WalrusPetPrefab
        {
            // Init PrefabInfo
            internal static PrefabInfo Info;

            /// <summary>
            /// Register Walrus
            /// </summary>
            internal static void Register()
            {
                Info = PrefabInfo
                    .WithTechType("WalrusPet", null, null, unlockAtStart: true)
                    .WithIcon(ModUtils.GetSpriteFromAssetBundle("WalrusTexture"));
                PrefabConfigUtilsPlatform.RegisterCustomPet(Info, "PetWalrus", "PetWalrus", Info.TechType, PetDnaPrefabs.CatDnaPrefab.Info.TechType);
            }
        }

        // Fox
        internal static class FoxPetPrefab
        {
            // Init PrefabInfo
            internal static PrefabInfo Info;

            /// <summary>
            /// Register Cat
            /// </summary>
            internal static void Register()
            {
                Info = PrefabInfo
                    .WithTechType("FoxPet", null, null, unlockAtStart: true)
                    .WithIcon(ModUtils.GetSpriteFromAssetBundle("FoxTexture"));
                PrefabConfigUtilsPlatform.RegisterCustomPet(Info, "PetFox", "PetFox", Info.TechType, PetDnaPrefabs.CatDnaPrefab.Info.TechType);
            }
        }
    }
}