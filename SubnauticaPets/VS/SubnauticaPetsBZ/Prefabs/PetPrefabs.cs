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

            PenglingBabyPrefab.Register();
            PengwingAdultPrefab.Register();
            PinnacaridPrefab.Register();
            SnowstalkerBabyPrefab.Register();
            TrivalveBluePrefab.Register();
            TrivalveYellowPrefab.Register();
        }
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
                    PrefabConfigUtilsBelowZero.ConfigureSwimming(obj);
                    PrefabConfigUtils.ConfigureSkyApplier(obj);
                    PrefabConfigUtils.ConfigureAnimator(obj, false);
                    obj.DestroyComponentsInChildren<CreatureDeath>();
                    PrefabConfigUtils.AddPetComponent(obj);
                    obj.name = "PenglingBabyPet";
                    LogUtils.LogDebug(LogArea.Prefabs, $"Done modifying {Info.TechType}");
                };

                prefab.SetGameObject(cloneTemplate);

                // Set the recipe, depends on whether in "Adventure" or "Creative" mode.
                RecipeData recipe = null;
                if (SubnauticaPetsPlugin.ModConfig.ModMode == ModMode.Adventure)
                {
                    recipe = new RecipeData(
                        new Ingredient(TechType.Gold, 1),
                        new Ingredient(PetDnaPrefabs.PenglingBabyDnaPrefab.Info.TechType, 5));
                }
                else
                {
                    recipe = new RecipeData(new Ingredient(TechType.Titanium, 1));
                }
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
                    PrefabConfigUtilsBelowZero.ConfigureSwimming(obj);
                    PrefabConfigUtils.ConfigureSkyApplier(obj);
                    PrefabConfigUtils.ConfigureAnimator(obj, false);
                    obj.DestroyComponentsInChildren<CreatureDeath>();
                    PrefabConfigUtils.AddPetComponent(obj);
                    obj.name = "PengwingAdultPet";
                    LogUtils.LogDebug(LogArea.Prefabs, $"Done modifying {Info.TechType}");
                };

                prefab.SetGameObject(cloneTemplate);

                // Set the recipe, depends on whether in "Adventure" or "Creative" mode.
                RecipeData recipe = null;
                if (SubnauticaPetsPlugin.ModConfig.ModMode == ModMode.Adventure)
                {
                    recipe = new RecipeData(
                    new Ingredient(TechType.Gold, 1),
                    new Ingredient(PetDnaPrefabs.PengwingAdultDnaPrefab.Info.TechType, 3));
                }
                else
                {
                    recipe = new RecipeData(new Ingredient(TechType.Titanium, 1));
                }
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
                    PrefabConfigUtilsBelowZero.ConfigureSwimming(obj);
                    PrefabConfigUtils.ConfigureSkyApplier(obj);
                    PrefabConfigUtils.ConfigureAnimator(obj, false);
                    PrefabConfigUtils.AddPetComponent(obj);
                    obj.name = "PinnacaridPet";
                    obj.SetActive(false);
                    LogUtils.LogDebug(LogArea.Prefabs, $"Done modifying {Info.TechType}");
                };

                prefab.SetGameObject(cloneTemplate);

                // Set the recipe, depends on whether in "Adventure" or "Creative" mode.
                RecipeData recipe = null;
                if (SubnauticaPetsPlugin.ModConfig.ModMode == ModMode.Adventure)
                {
                    recipe = new RecipeData(
                    new Ingredient(TechType.Gold, 1),
                    new Ingredient(PetDnaPrefabs.PinnacaridDnaPrefab.Info.TechType, 3));
                }
                else
                {
                    recipe = new RecipeData(new Ingredient(TechType.Titanium, 1));
                }
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
                    PrefabConfigUtilsBelowZero.ConfigureSwimming(obj);
                    PrefabConfigUtils.ConfigureSkyApplier(obj);
                    PrefabConfigUtils.ConfigureAnimator(obj, false);
                    PrefabConfigUtilsBelowZero.ConfigureMovement(obj);
                    PrefabConfigUtilsBelowZero.CleanNavUpMesh(obj);
                    PrefabConfigUtils.AddPetComponent(obj);
                    obj.name = "SnowstalkerBabyPet";
                    LogUtils.LogDebug(LogArea.Prefabs, $"Done modifying {Info.TechType}");
                };

                prefab.SetGameObject(cloneTemplate);

                // Set the recipe, depends on whether in "Adventure" or "Creative" mode.
                RecipeData recipe = null;
                if (SubnauticaPetsPlugin.ModConfig.ModMode == ModMode.Adventure)
                {
                    recipe = new RecipeData(
                   new Ingredient(TechType.Gold, 1),
                   new Ingredient(PetDnaPrefabs.SnowstalkerBabyDnaPrefab.Info.TechType, 3));
                }
                else
                {
                    recipe = new RecipeData(new Ingredient(TechType.Titanium, 1));
                }
                CraftingGadget crafting = prefab.SetRecipe(recipe);
                prefab.Register();
            }
        }

        private static void ConfigureTrivalve(GameObject obj, PrefabInfo Info, string objName, string classId)
        {
            obj.SetActive(false);
            PrefabConfigUtils.AddTechTag(obj, Info.TechType);
            GameObject modelGameObject = obj.GetComponentInChildren<Animator>(true).gameObject;
            PrefabConfigUtils.AddVFXFabricating(obj, null, -0.2f, 0.5f, new Vector3(0.0f, 0.0f, 0.0f), 1.0f, new Vector3(0.0f, 0.0f, 0.0f));
            PrefabUtils.AddConstructable(obj, Info.TechType, ConstructableFlags.Inside, modelGameObject);
            obj.DestroyComponentsInChildren<Pickupable>();
            // obj.DisableComponentsInChildren<LargeWorldEntity>();
            PrefabConfigUtils.AddPetHandTarget(obj);
            PrefabConfigUtilsBelowZero.ConfigureSwimming(obj);
            PrefabConfigUtils.ConfigureSkyApplier(obj);
            PrefabConfigUtils.ConfigureAnimator(obj, false);
            PrefabConfigUtils.AddPetComponent(obj);
            obj.name = objName;
            obj.SetActive(false);
            LogUtils.LogDebug(LogArea.Prefabs, $"Done modifying {Info.TechType}");

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
                    ConfigureTrivalve(obj, Info, "TrivalveBluePet", "TrivalveBluePet");
                };

                prefab.SetGameObject(cloneTemplate);

                // Set the recipe, depends on whether in "Adventure" or "Creative" mode.
                RecipeData recipe = null;
                if (SubnauticaPetsPlugin.ModConfig.ModMode == ModMode.Adventure)
                {
                    recipe = new RecipeData(
                   new Ingredient(TechType.Gold, 1),
                   new Ingredient(PetDnaPrefabs.TrivalveBlueDnaPrefab.Info.TechType, 3));
                }
                else
                {
                    recipe = new RecipeData(new Ingredient(TechType.Titanium, 1));
                }
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
                    ConfigureTrivalve(obj, Info, "TrivalveYellowPet", "TrivalveYellowPet");
                };

                prefab.SetGameObject(cloneTemplate);

                // Set the recipe, depends on whether in "Adventure" or "Creative" mode.
                RecipeData recipe = null;
                if (SubnauticaPetsPlugin.ModConfig.ModMode == ModMode.Adventure)
                {
                    recipe = new RecipeData(
                    new Ingredient(TechType.Gold, 1),
                    new Ingredient(PetDnaPrefabs.TrivalveYellowDnaPrefab.Info.TechType, 3));
                }
                else
                {
                    recipe = new RecipeData(new Ingredient(TechType.Titanium, 1));
                }
                CraftingGadget crafting = prefab.SetRecipe(recipe);
                prefab.Register();
            }
        }
    }
}