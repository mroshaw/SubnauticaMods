using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using Nautilus.Utility;
using UnityEngine;

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
            AlienRobotPrefab.Register();
            BloodCrawlerPrefab.Register();
            CaveCrawlerPrefab.Register();
            CrabSquidPrefab.Register();
        }

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
                    .WithIcon(CustomAssetBundleUtils.GetObjectFromAssetBundle<Sprite>("AlienRobotTexture") as Sprite);

                CustomPrefab prefab = new CustomPrefab(Info);
                CloneTemplate cloneTemplate = new CloneTemplate(Info, "4fae8fa4-0280-43bd-bcf1-f3cba97eed77");

                // Modify the cloned model
                cloneTemplate.ModifyPrefab += obj =>
                {
                    obj.SetActive(false);
                    PrefabConfigUtils.AddTechTag(obj, Info.TechType);
                    obj.DestroyComponentsInChildren<LargeWorldEntity>();
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

                // Set the recipe, depends on whether in "Adventure" or "Creative" mode.
                RecipeData recipe = null;
                if (SubnauticaPetsPlugin.ModConfig.ModMode == ModMode.Adventure)
                {
                    recipe = new RecipeData(
                    new CraftData.Ingredient(TechType.Gold, 1),
                    new CraftData.Ingredient(TechType.CopperWire, 1),
                    new CraftData.Ingredient(TechType.ComputerChip, 1),
                    new CraftData.Ingredient(TechType.Titanium, 2),
                    new CraftData.Ingredient(PetDnaPrefabs.AlienRobotDnaPrefab.Info.TechType, 3));
                }
                else
                {
                    recipe = new RecipeData(new CraftData.Ingredient(TechType.Titanium, 1));
                }

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
                    .WithIcon(CustomAssetBundleUtils.GetObjectFromAssetBundle<Sprite>("BloodCrawlerTexture") as Sprite);

                CustomPrefab prefab = new CustomPrefab(Info);
                CloneTemplate cloneTemplate = new CloneTemplate(Info, "830a8fa0-d92d-4683-a193-7531e6968042");

                // Modify the cloned model
                cloneTemplate.ModifyPrefab += obj =>
                {
                    obj.SetActive(false);
                    PrefabConfigUtils.AddTechTag(obj, Info.TechType);
                    obj.DestroyComponentsInChildren<LargeWorldEntity>();
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

                // Set the recipe, depends on whether in "Adventure" or "Creative" mode.
                RecipeData recipe = null;
                if (SubnauticaPetsPlugin.ModConfig.ModMode == ModMode.Adventure)
                {
                    recipe = new RecipeData(
                    new CraftData.Ingredient(TechType.Gold, 1),
                    new CraftData.Ingredient(TechType.AcidMushroom, 1),
                    new CraftData.Ingredient(TechType.Salt, 1),
                    new CraftData.Ingredient(PetDnaPrefabs.BloodCrawlerDnaPrefab.Info.TechType, 3));
                }
                else
                {
                    recipe = new RecipeData(new CraftData.Ingredient(TechType.Titanium, 1));
                }
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
                    .WithIcon(CustomAssetBundleUtils.GetObjectFromAssetBundle<Sprite>("CaveCrawlerTexture") as Sprite);

                CustomPrefab prefab = new CustomPrefab(Info);
                CloneTemplate cloneTemplate = new CloneTemplate(Info, "3e0a11f1-e2b2-4c4f-9a8e-0b0a77dcc065");

                // Modify the cloned model
                cloneTemplate.ModifyPrefab += obj =>
                {
                    obj.SetActive(false);
                    PrefabConfigUtils.AddTechTag(obj, Info.TechType);
                    obj.DestroyComponentsInChildren<LargeWorldEntity>();
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

                // Set the recipe, depends on whether in "Adventure" or "Creative" mode.
                RecipeData recipe = null;
                if (SubnauticaPetsPlugin.ModConfig.ModMode == ModMode.Adventure)
                {
                    recipe = new RecipeData(
                    new CraftData.Ingredient(TechType.Gold, 1),
                    new CraftData.Ingredient(TechType.Sulphur, 1),
                    new CraftData.Ingredient(TechType.Salt, 1),
                    new CraftData.Ingredient(PetDnaPrefabs.CaveCrawlerDnaPrefab.Info.TechType, 3));
                }
                else
                {
                    recipe = new RecipeData(new CraftData.Ingredient(TechType.Titanium, 1));
                }
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
                    .WithIcon(CustomAssetBundleUtils.GetObjectFromAssetBundle<Sprite>("CrabSquidTexture") as Sprite);

                CustomPrefab prefab = new CustomPrefab(Info);
                CloneTemplate cloneTemplate = new CloneTemplate(Info, "4c2808fe-e051-44d2-8e64-120ddcdc8abb");

                // Modify the cloned model
                cloneTemplate.ModifyPrefab += obj =>
                {
                    obj.SetActive(false);
                    PrefabConfigUtils.AddTechTag(obj, Info.TechType);
                    obj.DestroyComponentsInChildren<LargeWorldEntity>();
                    GameObject modelGameObject = obj.GetComponentInChildren<Animator>(true).gameObject;
                    PrefabConfigUtils.AddVFXFabricating(obj, null, -0.2f, 1.2f, new Vector3(0.0f, 0.0f, 0.0f), 0.07f,
                        new Vector3(0.0f, 0.0f, 0.0f));
                    PrefabUtils.AddConstructable(obj, Info.TechType, ConstructableFlags.Inside, modelGameObject);
                    obj.DestroyComponentsInChildren<Pickupable>();
                    obj.DestroyComponentsInChildren<EMPAttack>();
                    obj.DestroyComponentsInChildren<AggressiveWhenSeeTarget>();
                    obj.DestroyComponentsInChildren<MeleeAttack>();
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

                // Set the recipe, depends on whether in "Adventure" or "Creative" mode.
                RecipeData recipe = null;
                if (SubnauticaPetsPlugin.ModConfig.ModMode == ModMode.Adventure)
                {
                    recipe = new RecipeData(
                    new CraftData.Ingredient(TechType.Gold, 1),
                    new CraftData.Ingredient(TechType.JellyPlant, 1),
                    new CraftData.Ingredient(TechType.Salt, 1),
                    new CraftData.Ingredient(PetDnaPrefabs.CrabSquidDnaPrefab.Info.TechType, 3));
                }
                else
                {
                    recipe = new RecipeData(new CraftData.Ingredient(TechType.Titanium, 1));
                }
                CraftingGadget crafting = prefab.SetRecipe(recipe);
                prefab.Register();
            }
        }
    }
}