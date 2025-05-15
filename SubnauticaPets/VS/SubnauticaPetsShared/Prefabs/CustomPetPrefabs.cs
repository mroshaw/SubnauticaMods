using Nautilus.Assets;
using UnityEngine;

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
                    .WithIcon(CustomAssetBundleUtils.GetObjectFromAssetBundle<Sprite>("CatTexture") as Sprite);
                PrefabConfigUtilsPlatform.RegisterCustomPet(Info, "PetCat", "PetCat",
                    "CatMeow",
                    Info.TechType, PetDnaPrefabs.CatDnaPrefab.Info.TechType);
            }
        }

        internal static class DogPetPrefab
        {
            // Init PrefabInfo
            internal static PrefabInfo Info;

            internal static void Register()
            {
                Info = PrefabInfo
                    .WithTechType("DogPet", null, null, unlockAtStart: true)
                    .WithIcon(CustomAssetBundleUtils.GetObjectFromAssetBundle<Sprite>("DogTexture") as Sprite);
                PrefabConfigUtilsPlatform.RegisterCustomPet(Info, "PetDog", "PetDog",
                    "DogBark",
                    Info.TechType, TechType.None);
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
                    .WithIcon(CustomAssetBundleUtils.GetObjectFromAssetBundle<Sprite>("RabbitTexture") as Sprite);
                PrefabConfigUtilsPlatform.RegisterCustomPet(Info, "PetRabbit", "PetRabbit",
                    "RabbitSqueak",
                    Info.TechType, TechType.None);
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
                    .WithIcon(CustomAssetBundleUtils.GetObjectFromAssetBundle<Sprite>("SealTexture") as Sprite);
                PrefabConfigUtilsPlatform.RegisterCustomPet(Info, "PetSeal", "PetSeal",
                    "SealBark",
                    Info.TechType, TechType.None);
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
                    .WithIcon(CustomAssetBundleUtils.GetObjectFromAssetBundle<Sprite>("WalrusTexture") as Sprite);
                PrefabConfigUtilsPlatform.RegisterCustomPet(Info, "PetWalrus", "PetWalrus",
                    "WalrusSound",
                    Info.TechType, TechType.None);
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
                    .WithIcon(CustomAssetBundleUtils.GetObjectFromAssetBundle<Sprite>("FoxTexture") as Sprite);
                PrefabConfigUtilsPlatform.RegisterCustomPet(Info, "PetFox", "PetFox",
                    "FoxSound",
                    Info.TechType, TechType.None);
            }
        }
    }
}