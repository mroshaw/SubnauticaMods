#if SUBNAUTICA
using DaftAppleGames.SubnauticaPets.Mono.Pets.Subnautica;
using Ingredient = CraftData.Ingredient;
#endif
#if SUBNAUTICAZERO
using DaftAppleGames.SubnauticaPets.Mono.Pets.BelowZero;
#endif

using DaftAppleGames.SubnauticaPets.Mono.Pets.Custom;
using DaftAppleGames.SubnauticaPets.Mono.Utils;
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using Nautilus.Utility;
using UnityEngine;
using Object = UnityEngine.Object;
using Nautilus.Handlers;

namespace DaftAppleGames.SubnauticaPets.Prefabs
{
    /// <summary>
    /// This static class provides methods to create our "Buildable Pets", via the
    /// new Pet Fabricator.
    /// </summary>
    public static class PetBuildablePrefab
    {
        /// <summary>
        /// Initialise all Pet buildables
        /// </summary>
        public static void InitPetBuildables()
        {
            // Custom pets
            CatBuildable.Init();

#if SUBNAUTICA
            CaveCrawlerBuildable.Init();
            BloodCrawlerBuildable.Init();
            CrabSquidBuildable.Init();
            AlienRobotBuildable.Init();
#endif
#if SUBNAUTICAZERO
            PenglingBabyBuildable.Init();
            PenglingAdultBuildable.Init();
            SnowStalkerBabyBuildable.Init();
            PinnicaridBuildable.Init();
            TrivalveYellowBuildable.Init();
            TrivalveBluePetBuildable.Init();
#endif
        }
        

        /// <summary>
        /// Generic method to register PetInfo
        /// </summary>
        /// <param name="prefabInfo"></param>
        /// <param name="cloneTemplateClassGuid"></param>
        /// <param name="modelGameObjectName"></param>
        /// <param name="cloneModelPrefabName"></param>
        /// <param name="recipe"></param>
        /// <param name="modelScale"></param>
        /// <param name="vfxMinOffset"></param>
        /// <param name="vfxMaxOffset"></param>
        /// <returns></returns>
        public static void MakeBuildable(PrefabInfo prefabInfo, string cloneTemplateClassGuid,
            string cloneModelPrefabName,
            RecipeData recipe, Vector3 modelScale,
            float vfxMinOffset, float vfxMaxOffset, string modelGameObjectName = "")
        {
            // Create prefab
            CustomPrefab prefab = new CustomPrefab(prefabInfo);

            GameObject customModelInstance;

            // Derive custom model
            if (!string.IsNullOrEmpty(modelGameObjectName))
            {
                customModelInstance = ModUtils.GetGameObjectInstanceFromAssetBundle(cloneModelPrefabName, true);
                LogUtils.LogDebug(LogArea.Prefabs, $"Created prefab instance from {cloneModelPrefabName}");
                MaterialUtils.ApplySNShaders(customModelInstance);
            }

            CloneTemplate cloneTemplate = new CloneTemplate(prefabInfo, cloneTemplateClassGuid);

            // modify the cloned model:
            cloneTemplate.ModifyPrefab += obj =>
            {
                // Set constructable flags
                ConstructableFlags constructableFlags = ConstructableFlags.Inside;

                // Find the object that holds the model for the fabricator
                GameObject modelGameObject = null;

                LogUtils.LogDebug(LogArea.Prefabs, $"PetBuildablePrefab: RegisterPrefabInfo looking for prefab model for {obj.name}...");
                // First, find the Animator
                Animator animator = obj.GetComponentInChildren<Animator>(true);
                if (animator == null)
                {
                    LogUtils.LogError($"PetBuildableInfo: RegisterPrefabInfo can't find Animator in {obj.name}"!);
                }
                else
                {
                    GameObject animatorGameObject = animator.gameObject;
                    modelGameObject = animatorGameObject;

                    // Set the model scale when it's used by the fabricator
                    ScaleOnStart scaleOnStart = modelGameObject.AddComponent<ScaleOnStart>();
                    scaleOnStart.Scale = modelScale;

                    // Add Fabricator VFX
                    VFXFabricating fabVfx = modelGameObject.AddComponent<VFXFabricating>();
                    fabVfx.localMinY = vfxMinOffset; // -0.2f
                    fabVfx.localMaxY = vfxMaxOffset; // 0.5f
                    fabVfx.posOffset = new Vector3(0.0f, 0.0f, 0.0f);
                    fabVfx.eulerOffset = new Vector3(0.0f, 0.0f, 0.0f);
                    fabVfx.scaleFactor = 1.0f;
                    // We'll take this opportunity to disable this for the ghost. We'll re-enable when spawned
                    animator.enabled = false;
                }

                // add all components necessary for it to be built:
                PrefabUtils.AddConstructable(obj, prefabInfo.TechType, constructableFlags, modelGameObject);
            };
            // Assign the created clone model to the prefab itself:
            prefab.SetGameObject(cloneTemplate);
            // Set recipe
            LogUtils.LogDebug(LogArea.Prefabs, $"PetBuildablePrefab: Setting up recipe with {recipe.ingredientCount} ingredients for {prefab.Info.TechType}...");
            CraftingGadget crafting = prefab.SetRecipe(recipe);
            // Register it into the game:
            prefab.Register();
        }

        // Custom Pets
        public static class CatBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info;
            
            // Register with the game
            public static void Init()
            {
                Info = PrefabInfo
                    .WithTechType(CatPet.ClassId, null, null, unlockAtStart: true)
                    .WithIcon(ModUtils.GetSpriteFromAssetBundle(CatPet.IconTextureName));
                
                MakeBuildable(Info, CatPet.PrefabGuid, CatPet.CustomPrefabName, CatPet.GetRecipeData(),
                    CatPet.ModelScale, CatPet.VfxMinOffset, CatPet.VfxMaxOffset, CatPet.ModelName);
            }
        }

#if SUBNAUTICA
        /// <summary>
        /// Cave Crawler Buildable
        /// </summary>
        public static class CaveCrawlerBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info;

            // Register with the game
            public static void Init()
            {
                Info = PrefabInfo
                    .WithTechType(CaveCrawlerPet.ClassId, null, null, unlockAtStart: true)
                    .WithIcon(ModUtils.GetSpriteFromAssetBundle(CaveCrawlerPet.TextureName));
                MakeBuildable(Info, CaveCrawlerPet.PrefabGuid, CaveCrawlerPet.ModelName, CaveCrawlerPet.GetRecipeData(),
                    CaveCrawlerPet.ModelScale, CaveCrawlerPet.VfxMinOffset, CaveCrawlerPet.VfxMaxOffset);
            }
        }

        /// <summary>
        /// Blood Crawler Buildable
        /// </summary>
        public static class BloodCrawlerBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info;

            // Register with the game
            public static void Init()
            {
                Info = PrefabInfo
                    .WithTechType(BloodCrawlerPet.ClassId, null, null, unlockAtStart: true)
                    .WithIcon(ModUtils.GetSpriteFromAssetBundle(BloodCrawlerPet.TextureName));
                MakeBuildable(Info, BloodCrawlerPet.PrefabGuid, BloodCrawlerPet.ModelName, BloodCrawlerPet.GetRecipeData(),
                    BloodCrawlerPet.ModelScale, BloodCrawlerPet.VfxMinOffset, BloodCrawlerPet.VfxMaxOffset);
            }
        }

        /// <summary>
        /// Crab Squid Buildable
        /// </summary>
        public static class CrabSquidBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info;

            // Register with the game
            public static void Init()
            {
                Info = PrefabInfo
                    .WithTechType(CrabSquidPet.ClassId, null, null, unlockAtStart: true)
                    .WithIcon(ModUtils.GetSpriteFromAssetBundle(CrabSquidPet.TextureName));
                MakeBuildable(Info, CrabSquidPet.PrefabGuid, CrabSquidPet.ModelName, CrabSquidPet.GetRecipeData(),
                    CrabSquidPet.ModelScale, CrabSquidPet.VfxMinOffset, CrabSquidPet.VfxMaxOffset);
            }
        }

        /// <summary>
        /// Alien Robot Buildable
        /// </summary>
        public static class AlienRobotBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info;

            // Register with the game
            public static void Init()
            {
                Info = PrefabInfo
                    .WithTechType(AlienRobotPet.ClassId, null, null, unlockAtStart: true)
                    .WithIcon(ModUtils.GetSpriteFromAssetBundle(AlienRobotPet.TextureName));
                MakeBuildable(Info, AlienRobotPet.PrefabGuid, AlienRobotPet.ModelName, AlienRobotPet.GetRecipeData(),
                    AlienRobotPet.ModelScale, AlienRobotPet.VfxMinOffset, AlienRobotPet.VfxMaxOffset);
            }
        }

#endif

#if SUBNAUTICAZERO
        /// <summary>
        /// Baby Pengling Buildable
        /// </summary>
        public static class PenglingBabyBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info;

            // Register with the game
            public static void Init()
            {
                Info = PrefabInfo
                    .WithTechType(PenglingBabyPet.ClassId, null, null, unlockAtStart: true)
                    .WithIcon(ModUtils.GetSpriteFromAssetBundle(PenglingBabyPet.TextureName));
                MakeBuildable(Info, PenglingBabyPet.PrefabGuid, PenglingBabyPet.ModelName, PenglingBabyPet.GetRecipeData(),
                    PenglingBabyPet.ModelScale, PenglingBabyPet.VfxMinOffset, PenglingBabyPet.VfxMaxOffset);
            }
        }

        /// <summary>
        /// Pengling Adult Buildable
        /// </summary>
        public static class PenglingAdultBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info;

            // Register with the game
            public static void Init()
            {
                Info = PrefabInfo
                    .WithTechType(PenglingAdultPet.ClassId, null, null, unlockAtStart: true)
                    .WithIcon(ModUtils.GetSpriteFromAssetBundle(PenglingAdultPet.TextureName));
                MakeBuildable(Info, PenglingAdultPet.PrefabGuid, PenglingAdultPet.ModelName, PenglingAdultPet.GetRecipeData(),
                    PenglingAdultPet.ModelScale, PenglingAdultPet.VfxMinOffset, PenglingAdultPet.VfxMaxOffset);
            }
        }

        /// <summary>
        /// Snow Stalker Baby Buildable
        /// </summary>
        public static class SnowStalkerBabyBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info;

            // Register with the game
            public static void Init()
            {
                Info = PrefabInfo
                    .WithTechType(SnowStalkerBabyPet.ClassId, null, null, unlockAtStart: true)
                    .WithIcon(ModUtils.GetSpriteFromAssetBundle(SnowStalkerBabyPet.TextureName));
                MakeBuildable(Info, SnowStalkerBabyPet.PrefabGuid, SnowStalkerBabyPet.ModelName, SnowStalkerBabyPet.GetRecipeData(),
                    SnowStalkerBabyPet.ModelScale, SnowStalkerBabyPet.VfxMinOffset, SnowStalkerBabyPet.VfxMaxOffset);
            }
        }

        /// <summary>
        /// Pinnicarid Buildable
        /// </summary>
        public static class PinnicaridBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info;

            // Register with the game
            public static void Init()
            {
                Info = PrefabInfo
                    .WithTechType(PinnicaridPet.ClassId, null, null, unlockAtStart: true)
                    .WithIcon(ModUtils.GetSpriteFromAssetBundle(PinnicaridPet.TextureName));
                MakeBuildable(Info, PinnicaridPet.PrefabGuid, PinnicaridPet.ModelName, PinnicaridPet.GetRecipeData(),
                    PinnicaridPet.ModelScale, PinnicaridPet.VfxMinOffset, PinnicaridPet.VfxMaxOffset);
            }
        }

        /// <summary>
        /// Yellow Trivalve Buildable
        /// </summary>
        public static class TrivalveYellowBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info;

            // Register with the game
            public static void Init()
            {
                Info = PrefabInfo
                    .WithTechType(TrivalveYellowPet.ClassId, null, null, unlockAtStart: true)
                    .WithIcon(ModUtils.GetSpriteFromAssetBundle(TrivalveYellowPet.TextureName));
                MakeBuildable(Info, TrivalveYellowPet.PrefabGuid, TrivalveYellowPet.ModelName, TrivalveYellowPet.GetRecipeData(),
                    TrivalveYellowPet.ModelScale, TrivalveYellowPet.VfxMinOffset, TrivalveYellowPet.VfxMaxOffset);
            }
        }

        /// <summary>
        /// Blue Trivalve Buildable
        /// </summary>
        public static class TrivalveBluePetBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info;

            // Register with the game
            public static void Init()
            {
                Info = PrefabInfo
                    .WithTechType(TrivalveBluePet.ClassId, null, null, unlockAtStart: true)
                    .WithIcon(ModUtils.GetSpriteFromAssetBundle(TrivalveBluePet.TextureName));
                MakeBuildable(Info, TrivalveBluePet.PrefabGuid,
                    TrivalveBluePet.ModelName, TrivalveBluePet.GetRecipeData(), TrivalveBluePet.ModelScale,
                    TrivalveBluePet.VfxMinOffset, TrivalveBluePet.VfxMaxOffset);
            }
        }
#endif
    }
}