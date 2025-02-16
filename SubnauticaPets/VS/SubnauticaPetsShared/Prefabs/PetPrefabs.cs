using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using Nautilus.Utility;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.Prefabs.PetDnaPrefabs;

// https://github.com/LeeTwentyThree/Nautilus/blob/master/Nautilus/Documentation/resources/BZ-PrefabPaths.json
// https://github.com/LeeTwentyThree/Nautilus/blob/master/Nautilus/Documentation/resources/SN1-PrefabPaths.json

namespace DaftAppleGames.SubnauticaPets.Prefabs
{
    public static class PetPrefabs
    {
        /// <summary>
        /// Set up all the Pet Prefabs
        /// </summary>
        public static void RegisterAll()
        {

#if SUBNAUTICA
            AlienRobotPrefab.Register();
            BloodCrawlerPrefab.Register();
            CaveCrawlerPrefab.Register();
            CrabSquidPrefab.Register();
#endif

#if SUBNAUTICAZERO
            PenglingBabyPrefab.Register();
            PengwingAdultPrefab.Register();
            PinnacaridPrefab.Register();
            SnowstalkerBabyPrefab.Register();
            TrivalveBluePrefab.Register();
            TrivalveYellowPrefab.Register();
#endif
        }
#if SUBNAUTICAZERO
        /// <summary>
        /// Pengling Baby
        /// </summary>
        public static class PenglingBabyPrefab
        {
            // Init PrefabInfo
            public static PrefabInfo Info;

            /// <summary>
            /// Set up the Pet Prefab
            /// </summary>
            public static void Register()
            {
                Info = PrefabInfo
                    .WithTechType("PenglingBabyPet", null, null, unlockAtStart: true)
                    .WithIcon(ModUtils.GetSpriteFromAssetBundle("PenglingBabyTexture"));

                CustomPrefab prefab = new CustomPrefab(Info);
                CloneTemplate cloneTemplate = new CloneTemplate(Info, "807fbbb3-aced-45cd-aba8-db3fb1188f1f");

                // Modify the cloned model
                cloneTemplate.ModifyPrefab += obj =>
                {
                    obj.SetActive(false);
                    PrefabConfigUtils.AddTechTag(obj, Info.TechType);
                    GameObject modelGameObject = obj.GetComponentInChildren<Animator>(true).gameObject;
                    PrefabConfigUtils.AddVFXFabricating(obj, null, -0.2f, 0.5f, new Vector3(0.0f, 0.0f, 0.0f), 1.0f, new Vector3(0.0f, 0.0f, 0.0f));
                    PrefabUtils.AddConstructable(obj, Info.TechType, ConstructableFlags.Inside, modelGameObject);
                    obj.DestroyComponentsInChildren<Pickupable>();
                    PrefabConfigUtils.AddPetHandTarget(obj);
                    PrefabConfigUtils.ConfigureSwimming(obj);
                    PrefabConfigUtils.ConfigureSkyApplier(obj);
                    PrefabConfigUtils.ConfigureAnimator(obj, false);
                    PrefabConfigUtils.ConfigureSwimming(obj);
                    obj.DestroyComponentsInChildren<CreatureDeath>();
                    PrefabConfigUtils.AddPetComponent(obj);
                    obj.name = "PenglingBabyPet";
                    LogUtils.LogDebug(LogArea.Prefabs, $"Done modifying {Info.TechType}");
                };

                prefab.SetGameObject(cloneTemplate);

                // Set the recipe
                RecipeData recipe = new RecipeData(
                new Ingredient(TechType.Gold, 1),
                new Ingredient(PetDnaPrefabs.PenglingBabyDnaPrefab.Info.TechType, 5));

                CraftingGadget crafting = prefab.SetRecipe(recipe);
                prefab.Register();
            }
        }

        /// <summary>
        /// Pengwing Adult
        /// </summary>
        public static class PengwingAdultPrefab
        {
            // Init PrefabInfo
            public static PrefabInfo Info;

            /// <summary>
            /// Set up the Pengwing Adult Prefab
            /// </summary>
            public static void Register()
            {
                Info = PrefabInfo
                    .WithTechType("PengwingAdultPet", null, null, unlockAtStart: true)
                    .WithIcon(ModUtils.GetSpriteFromAssetBundle("PengwingAdultTexture"));

                CustomPrefab prefab = new CustomPrefab(Info);
                CloneTemplate cloneTemplate = new CloneTemplate(Info, "74ded0e7-d394-4703-9e53-4384b37f9433");

                // Modify the cloned model
                cloneTemplate.ModifyPrefab += obj =>
                {
                    obj.SetActive(false);
                    PrefabConfigUtils.AddTechTag(obj, Info.TechType);
                    GameObject modelGameObject = obj.GetComponentInChildren<Animator>(true).gameObject;
                    PrefabConfigUtils.AddVFXFabricating(obj, null, -0.2f, 0.5f, new Vector3(0.0f, 0.0f, 0.0f), 1.0f, new Vector3(0.0f, 0.0f, 0.0f));
                    PrefabUtils.AddConstructable(obj, Info.TechType, ConstructableFlags.Inside, modelGameObject);
                    PrefabConfigUtils.UpdatePickupable(obj, false);
                    PrefabConfigUtils.AddPetHandTarget(obj);
                    PrefabConfigUtils.ConfigureSwimming(obj);
                    PrefabConfigUtils.ConfigureSkyApplier(obj);
                    PrefabConfigUtils.ConfigureAnimator(obj, false);
                    PrefabConfigUtils.ConfigureSwimming(obj);
                    obj.DestroyComponentsInChildren<CreatureDeath>();
                    PrefabConfigUtils.AddPetComponent(obj);
                    obj.name = "PengwingAdultPet";
                    LogUtils.LogDebug(LogArea.Prefabs, $"Done modifying {Info.TechType}");
                };

                prefab.SetGameObject(cloneTemplate);

                // Set the recipe
                RecipeData recipe = new RecipeData(
                    new Ingredient(TechType.Gold, 1),
                    new Ingredient(PetDnaPrefabs.PengwingAdultDnaPrefab.Info.TechType, 5));

                CraftingGadget crafting = prefab.SetRecipe(recipe);
                prefab.Register();
            }
        }

        /// <summary>
        /// Pinnacarid
        /// </summary>
        public static class PinnacaridPrefab
        {
            // Init PrefabInfo
            public static PrefabInfo Info;

            /// <summary>
            /// Set up the Pinnacarid Prefab
            /// </summary>
            public static void Register()
        {
            Info = PrefabInfo
                .WithTechType("PinnacaridPet", null, null, unlockAtStart: true)
                .WithIcon(ModUtils.GetSpriteFromAssetBundle("PinnacaridTexture"));

            CustomPrefab prefab = new CustomPrefab(Info);
            CloneTemplate cloneTemplate = new CloneTemplate(Info, "f9eccfe2-a06f-4c06-bc57-01c2e50ffbe8");

            // Modify the cloned model
            cloneTemplate.ModifyPrefab += obj =>
            {
                obj.SetActive(false);
                PrefabConfigUtils.AddTechTag(obj, Info.TechType);
                GameObject modelGameObject = obj.GetComponentInChildren<Animator>(true).gameObject;
                PrefabConfigUtils.AddVFXFabricating(obj, null, -0.2f, 0.5f, new Vector3(0.0f, 0.0f, 0.0f), 1.0f, new Vector3(0.0f, 0.0f, 0.0f));
                PrefabUtils.AddConstructable(obj, Info.TechType, ConstructableFlags.Inside, modelGameObject);
                obj.DestroyComponentsInChildren<Pickupable>();
                PrefabConfigUtils.AddPetHandTarget(obj);
                PrefabConfigUtils.ConfigureSwimming(obj);
                PrefabConfigUtils.ConfigureSkyApplier(obj);
                PrefabConfigUtils.ConfigureAnimator(obj, false);
                PrefabConfigUtils.AddPetComponent(obj);
                obj.name = "PinnacaridPet";
                obj.SetActive(false);
                LogUtils.LogDebug(LogArea.Prefabs, $"Done modifying {Info.TechType}");
            };

            prefab.SetGameObject(cloneTemplate);

                // Set the recipe
                RecipeData recipe = new RecipeData(
                    new Ingredient(TechType.Gold, 1),
                    new Ingredient(PetDnaPrefabs.PinnacaridDnaPrefab.Info.TechType, 5));

                CraftingGadget crafting = prefab.SetRecipe(recipe);
            prefab.Register();
        }
    }

        /// <summary>
        /// Snowstalker Baby
        /// </summary>
        public static class SnowstalkerBabyPrefab
        {
            // Init PrefabInfo
            public static PrefabInfo Info;

        /// <summary>
        /// Set up the Snowstalker Baby Prefab
        /// </summary>
        public static void Register()
        {
            Info = PrefabInfo
                .WithTechType("SnowstalkerBabyPet", null, null, unlockAtStart: true)
                .WithIcon(ModUtils.GetSpriteFromAssetBundle("SnowstalkerBabyTexture"));

            CustomPrefab prefab = new CustomPrefab(Info);
            CloneTemplate cloneTemplate = new CloneTemplate(Info, "78d3dbce-856f-4eba-951c-bd99870554e2");

            // Modify the cloned model
            cloneTemplate.ModifyPrefab += obj =>
            {
                obj.SetActive(false);
                PrefabConfigUtils.AddTechTag(obj, Info.TechType);
                GameObject modelGameObject = obj.GetComponentInChildren<Animator>(true).gameObject;
                PrefabConfigUtils.AddVFXFabricating(obj, null, -0.2f, 0.8f, new Vector3(0.0f, 0.0f, 0.0f), 1.0f, new Vector3(0.0f, 0.0f, 0.0f));
                PrefabUtils.AddConstructable(obj, Info.TechType, ConstructableFlags.Inside, modelGameObject);
                obj.DestroyComponentsInChildren<Pickupable>();
                PrefabConfigUtils.AddPetHandTarget(obj);
                PrefabConfigUtils.ConfigureSwimming(obj);
                PrefabConfigUtils.ConfigureSkyApplier(obj);
                PrefabConfigUtils.ConfigureAnimator(obj, false);
                PrefabConfigUtils.ConfigureMovement(obj);
                PrefabConfigUtils.CleanNavUpMesh(obj);
                PrefabConfigUtils.AddPetComponent(obj);
                obj.name = "SnowstalkerBabyPet";
                LogUtils.LogDebug(LogArea.Prefabs, $"Done modifying {Info.TechType}");
            };

            prefab.SetGameObject(cloneTemplate);

                // Set the recipe
                RecipeData recipe = new RecipeData(
                    new Ingredient(TechType.Gold, 1),
                    new Ingredient(PetDnaPrefabs.SnowstalkerBabyDnaPrefab.Info.TechType, 5));

                CraftingGadget crafting = prefab.SetRecipe(recipe);
            prefab.Register();
        }
    }

        /// <summary>
        /// Trivalve Blue
        /// </summary>
        public static class TrivalveBluePrefab
        {
            // Init PrefabInfo
            public static PrefabInfo Info;

        /// <summary>
        /// Set up the Trivalve Blue Prefab
        /// </summary>
        public static void Register()
        {
            Info = PrefabInfo
                .WithTechType("TrivalveBluePet", null, null, unlockAtStart: true)
                .WithIcon(ModUtils.GetSpriteFromAssetBundle("TrivalveBlueTexture"));

            CustomPrefab prefab = new CustomPrefab(Info);
            CloneTemplate cloneTemplate = new CloneTemplate(Info, "f5a2317f-6116-4fc6-8e81-824fd8ba9684");

            // Modify the cloned model
            cloneTemplate.ModifyPrefab += obj =>
            {
                obj.SetActive(false);
                PrefabConfigUtils.AddTechTag(obj, Info.TechType);
                GameObject modelGameObject = obj.GetComponentInChildren<Animator>(true).gameObject;
                PrefabConfigUtils.AddVFXFabricating(obj, null, -0.2f, 0.5f, new Vector3(0.0f, 0.0f, 0.0f), 1.0f, new Vector3(0.0f, 0.0f, 0.0f));
                PrefabUtils.AddConstructable(obj, Info.TechType, ConstructableFlags.Inside, modelGameObject);
                obj.DestroyComponentsInChildren<Pickupable>();
                PrefabConfigUtils.AddPetHandTarget(obj);
                PrefabConfigUtils.ConfigureSwimming(obj);
                PrefabConfigUtils.ConfigureSkyApplier(obj);
                PrefabConfigUtils.ConfigureAnimator(obj, false);
                PrefabConfigUtils.AddPetComponent(obj);
                obj.name = "TrivalveBluePet";
                obj.SetActive(false);
                LogUtils.LogDebug(LogArea.Prefabs, $"Done modifying {Info.TechType}");
            };

            prefab.SetGameObject(cloneTemplate);

                // Set the recipe
                RecipeData recipe = new RecipeData(
                    new Ingredient(TechType.Gold, 1),
                    new Ingredient(PetDnaPrefabs.TrivalveBlueDnaPrefab.Info.TechType, 5));

                CraftingGadget crafting = prefab.SetRecipe(recipe);
            prefab.Register();
        }
    }

        /// <summary>
        /// Trivalve Blue
        /// </summary>
        public static class TrivalveYellowPrefab
        {
            // Init PrefabInfo
            public static PrefabInfo Info;

            /// <summary>
            /// Set up the Trivalve Blue Prefab
            /// </summary>
            public static void Register()
            {
                Info = PrefabInfo
                    .WithTechType("TrivalveYellowPet", null, null, unlockAtStart: true)
                    .WithIcon(ModUtils.GetSpriteFromAssetBundle("TrivalveYellowTexture"));

                CustomPrefab prefab = new CustomPrefab(Info);
                CloneTemplate cloneTemplate = new CloneTemplate(Info, "e8f2bfd4-49c6-45d1-a029-489b492515a9");

                // Modify the cloned model
                cloneTemplate.ModifyPrefab += obj =>
                {
                    obj.SetActive(false);
                    PrefabConfigUtils.AddTechTag(obj, Info.TechType);
                    GameObject modelGameObject = obj.GetComponentInChildren<Animator>(true).gameObject;
                    PrefabConfigUtils.AddVFXFabricating(obj, null, -0.2f, 0.5f, new Vector3(0.0f, 0.0f, 0.0f), 1.0f, new Vector3(0.0f, 0.0f, 0.0f));
                    PrefabUtils.AddConstructable(obj, Info.TechType, ConstructableFlags.Inside, modelGameObject);
                    obj.DestroyComponentsInChildren<Pickupable>();
                    PrefabConfigUtils.AddPetHandTarget(obj);
                    PrefabConfigUtils.ConfigureSwimming(obj);
                    PrefabConfigUtils.ConfigureSkyApplier(obj);
                    PrefabConfigUtils.ConfigureAnimator(obj, false);
                    PrefabConfigUtils.AddPetComponent(obj);
                    obj.name = "TrivalveYellowPet";
                    obj.SetActive(false);
                    LogUtils.LogDebug(LogArea.Prefabs, $"Done modifying {Info.TechType}");
                };

                prefab.SetGameObject(cloneTemplate);

                // Set the recipe
                RecipeData recipe = new RecipeData(
                    new Ingredient(TechType.Gold, 1),
                    new Ingredient(PetDnaPrefabs.TrivalveYellowDnaPrefab.Info.TechType, 5));

                CraftingGadget crafting = prefab.SetRecipe(recipe);
                prefab.Register();
            }
        }

#endif

#if SUBNAUTICA
        /// <summary>
        /// Alien Robot
        /// </summary>
        public static class AlienRobotPrefab
        {
            // Init PrefabInfo
            public static PrefabInfo Info;

            /// <summary>
            /// Register Alien Robot
            /// </summary>
            public static void Register()
            {
                Info = PrefabInfo
                    .WithTechType("AlienRobotPet", null, null, unlockAtStart: true)
                    .WithIcon(ModUtils.GetSpriteFromAssetBundle("AlienRobotTexture"));

                CustomPrefab prefab = new CustomPrefab(Info);
                CloneTemplate cloneTemplate = new CloneTemplate(Info, "4fae8fa4-0280-43bd-bcf1-f3cba97eed77");

                // Modify the cloned model
                cloneTemplate.ModifyPrefab += obj =>
                {
                    obj.SetActive(false);
                    PrefabConfigUtils.AddTechTag(obj, Info.TechType);
                    GameObject modelGameObject = obj.GetComponentInChildren<Animator>(true).gameObject;
                    PrefabConfigUtils.AddVFXFabricating(obj, null, -0.2f, 1.2f, new Vector3(0.0f, 0.0f, 0.0f), 1.0f,
                        new Vector3(0.0f, 0.0f, 0.0f));
                    PrefabUtils.AddConstructable(obj, Info.TechType, ConstructableFlags.Inside, modelGameObject);
                    PrefabConfigUtils.AddPetHandTarget(obj);
                    obj.DestroyComponentsInChildren<Pickupable>();
                    PrefabConfigUtils.ConfigureSkyApplier(obj);
                    PrefabConfigUtils.ConfigureAnimator(obj, false);
                    PrefabConfigUtils.AddPetComponent(obj);
                    obj.name = "AlienRobotPet";
                    obj.SetActive(false);
                    LogUtils.LogDebug(LogArea.Prefabs, $"Done modifying {Info.TechType}");
                };

                prefab.SetGameObject(cloneTemplate);

                // Set the recipe
                RecipeData recipe = new RecipeData(
                    new CraftData.Ingredient(TechType.Gold, 1),
                    new CraftData.Ingredient(TechType.CopperWire, 1),
                    new CraftData.Ingredient(TechType.ComputerChip, 1),
                    new CraftData.Ingredient(TechType.Titanium, 2),
                    new CraftData.Ingredient(PetDnaPrefabs.AlienRobotDnaPrefab.Info.TechType, 3));

                CraftingGadget crafting = prefab.SetRecipe(recipe);
                prefab.Register();
            }
        }

        /// <summary>
        /// Blood Crawler
        /// </summary>
        public static class BloodCrawlerPrefab
        {
            // Init PrefabInfo
            public static PrefabInfo Info;

            /// <summary>
            /// Register Blood Crawler
            /// </summary>
            public static void Register()
            {
                Info = PrefabInfo
                    .WithTechType("BloodCrawlerPet", null, null, unlockAtStart: true)
                    .WithIcon(ModUtils.GetSpriteFromAssetBundle("BloodCrawlerTexture"));

                CustomPrefab prefab = new CustomPrefab(Info);
                CloneTemplate cloneTemplate = new CloneTemplate(Info, "830a8fa0-d92d-4683-a193-7531e6968042");

                // Modify the cloned model
                cloneTemplate.ModifyPrefab += obj =>
                {
                    obj.SetActive(false);
                    PrefabConfigUtils.AddTechTag(obj, Info.TechType);
                    GameObject modelGameObject = obj.GetComponentInChildren<Animator>(true).gameObject;
                    PrefabConfigUtils.AddVFXFabricating(obj, null, -0.2f, 1.2f, new Vector3(0.0f, 0.0f, 0.0f), 0.3f,
                        new Vector3(0.0f, 0.0f, 0.0f));
                    PrefabUtils.AddConstructable(obj, Info.TechType, ConstructableFlags.Inside, modelGameObject);
                    PrefabConfigUtils.AddScaleOnStart(obj, 0.3f);
                    PrefabConfigUtils.AddPetHandTarget(obj);
                    obj.DestroyComponentsInChildren<Pickupable>();
                    PrefabConfigUtils.ConfigureSkyApplier(obj);
                    PrefabConfigUtils.ConfigureAnimator(obj, false);
                    PrefabConfigUtils.AddPetComponent(obj);
                    obj.name = "BloodCrawlerPet";
                    obj.SetActive(false);
                    LogUtils.LogDebug(LogArea.Prefabs, $"Done modifying {Info.TechType}");
                };

                prefab.SetGameObject(cloneTemplate);

                // Set the recipe
                RecipeData recipe = new RecipeData(
                    new CraftData.Ingredient(TechType.Gold, 1),
                    new CraftData.Ingredient(TechType.AcidMushroom, 1),
                    new CraftData.Ingredient(TechType.Salt, 1),
                    new CraftData.Ingredient(PetDnaPrefabs.BloodCrawlerDnaPrefab.Info.TechType, 3));

                CraftingGadget crafting = prefab.SetRecipe(recipe);
                prefab.Register();
            }
        }

        /// <summary>
        /// Cave Crawler
        /// </summary>
        public static class CaveCrawlerPrefab
        {
            // Init PrefabInfo
            public static PrefabInfo Info;

            /// <summary>
            /// Register Cave Crawler
            /// </summary>
            public static void Register()
            {
                Info = PrefabInfo
                    .WithTechType("CaveCrawlerPet", null, null, unlockAtStart: true)
                    .WithIcon(ModUtils.GetSpriteFromAssetBundle("CaveCrawlerTexture"));

                CustomPrefab prefab = new CustomPrefab(Info);
                CloneTemplate cloneTemplate = new CloneTemplate(Info, "3e0a11f1-e2b2-4c4f-9a8e-0b0a77dcc065");

                // Modify the cloned model
                cloneTemplate.ModifyPrefab += obj =>
                {
                    obj.SetActive(false);
                    PrefabConfigUtils.AddTechTag(obj, Info.TechType);
                    GameObject modelGameObject = obj.GetComponentInChildren<Animator>(true).gameObject;
                    PrefabConfigUtils.AddVFXFabricating(obj, null, -0.2f, 0.5f, new Vector3(0.0f, 0.0f, 0.0f), 1.0f,
                        new Vector3(0.0f, 0.0f, 0.0f));
                    PrefabUtils.AddConstructable(obj, Info.TechType, ConstructableFlags.Inside, modelGameObject);
                    PrefabConfigUtils.AddPetHandTarget(obj);
                    obj.DestroyComponentsInChildren<Pickupable>();
                    PrefabConfigUtils.ConfigureSkyApplier(obj);
                    PrefabConfigUtils.ConfigureAnimator(obj, false);
                    PrefabConfigUtils.AddPetComponent(obj);
                    obj.name = "CaveCrawlerPet";
                    obj.SetActive(false);
                    LogUtils.LogDebug(LogArea.Prefabs, $"Done modifying {Info.TechType}");
                };

                prefab.SetGameObject(cloneTemplate);

                // Set the recipe
                RecipeData recipe = new RecipeData(
                    new CraftData.Ingredient(TechType.Gold, 1),
                    new CraftData.Ingredient(TechType.Sulphur, 1),
                    new CraftData.Ingredient(TechType.Salt, 1),
                    new CraftData.Ingredient(PetDnaPrefabs.CaveCrawlerDnaPrefab.Info.TechType, 3));
                CraftingGadget crafting = prefab.SetRecipe(recipe);
                prefab.Register();
            }
        }

        /// <summary>
        /// Crab Squid
        /// </summary>
        public static class CrabSquidPrefab
        {
            // Init PrefabInfo
            public static PrefabInfo Info;

            /// <summary>
            /// Register Crab Squid
            /// </summary>
            public static void Register()
            {
                Info = PrefabInfo
                    .WithTechType("CrabSquidPet", null, null, unlockAtStart: true)
                    .WithIcon(ModUtils.GetSpriteFromAssetBundle("CrabSquidTexture"));

                CustomPrefab prefab = new CustomPrefab(Info);
                CloneTemplate cloneTemplate = new CloneTemplate(Info, "4c2808fe-e051-44d2-8e64-120ddcdc8abb");

                // Modify the cloned model
                cloneTemplate.ModifyPrefab += obj =>
                {
                    obj.SetActive(false);
                    PrefabConfigUtils.AddTechTag(obj, Info.TechType);
                    GameObject modelGameObject = obj.GetComponentInChildren<Animator>(true).gameObject;
                    PrefabConfigUtils.AddVFXFabricating(obj, null, -0.2f, 1.2f, new Vector3(0.0f, 0.0f, 0.0f), 0.07f,
                        new Vector3(0.0f, 0.0f, 0.0f));
                    PrefabUtils.AddConstructable(obj, Info.TechType, ConstructableFlags.Inside, modelGameObject);
                    obj.DestroyComponentsInChildren<Pickupable>();
                    PrefabConfigUtils.AddPetHandTarget(obj);
                    PrefabConfigUtils.ConfigureSkyApplier(obj);
                    PrefabConfigUtils.ConfigureAnimator(obj, false);
                    PrefabConfigUtils.AddScaleOnStart(obj, 0.07f);
                    PrefabConfigUtils.ConfigureAnimator(obj, false);
                    PrefabConfigUtils.AddPetComponent(obj);
                    obj.name = "CrabSquidPet";
                    obj.SetActive(false);
                    LogUtils.LogDebug(LogArea.Prefabs, $"Done modifying {Info.TechType}");
                };

                prefab.SetGameObject(cloneTemplate);

                // Set the recipe
                RecipeData recipe = new RecipeData(
                    new CraftData.Ingredient(TechType.Gold, 1),
                    new CraftData.Ingredient(TechType.JellyPlant, 1),
                    new CraftData.Ingredient(TechType.Salt, 1),
                    new CraftData.Ingredient(PetDnaPrefabs.CrabSquidDnaPrefab.Info.TechType, 3));
                CraftingGadget crafting = prefab.SetRecipe(recipe);
                prefab.Register();
            }
        }
#endif
    }
}