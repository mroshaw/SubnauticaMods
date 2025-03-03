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
                    "CatMeow", "ee6fe9db-b9d4-49f6-97a8-88cbb9cc89a8",
                    Info.TechType, PetDnaPrefabs.CatDnaPrefab.Info.TechType, true);
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
                    .WithTechType("DogBark", null, null, unlockAtStart: true)
                    .WithIcon(CustomAssetBundleUtils.GetObjectFromAssetBundle<Sprite>("DogTexture") as Sprite);
                PrefabConfigUtilsPlatform.RegisterCustomPet(Info, "PetDog", "PetDog",
                    "DogBark","4aa816f0-3889-47e1-94d2-e90fc22b98a1",
                    Info.TechType, PetDnaPrefabs.CatDnaPrefab.Info.TechType);
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
                    "RabbitSqueak", "4d514051-b2a9-436e-b60e-62132b38ffaf",
                    Info.TechType, PetDnaPrefabs.CatDnaPrefab.Info.TechType);
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
                    "SealBark", "383daa32-dc30-4bfa-af63-402c460a7252",
                    Info.TechType, PetDnaPrefabs.CatDnaPrefab.Info.TechType);
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
                    "WalrusSound", "8282e0cc-a7f0-4d0b-8ff7-8d3193526bcd", Info.TechType, PetDnaPrefabs.CatDnaPrefab.Info.TechType);
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
                    "FoxSound", "5bfc2ea5-db66-4f62-902a-143005c08865",
                    Info.TechType, PetDnaPrefabs.CatDnaPrefab.Info.TechType);
            }
        }
    }
}