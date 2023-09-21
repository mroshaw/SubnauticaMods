#if SUBNAUTICA
using DaftAppleGames.SubnauticaPets.Pets.Subnautica;
#endif

#if SUBNAUTICAZERO
using DaftAppleGames.SubnauticaPets.Pets.BelowZero;
#endif

using DaftAppleGames.SubnauticaPets.Pets.Custom;
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using Nautilus.Utility;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;
using Object = UnityEngine.Object;

namespace DaftAppleGames.SubnauticaPets.Pets
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
            CatBuildable.Register();

#if SUBNAUTICA
            CaveCrawlerBuildable.Register();
            BloodCrawlerBuildable.Register();
            CrabSquidBuildable.Register();
            AlienRobotBuildable.Register();
#endif
#if SUBNAUTICAZERO
            PenglingBabyBuildable.Register();
            PenglingAdultBuildable.Register();
            SnowStalkerBabyBuildable.Register();
            PinnicaridBuildable.Register();
            TrivalveYellowBuildable.Register();
            TrivalveBluePetBuildable.Register();
#endif
        }

        /// <summary>
        /// Generic method to return the PrefabInfo for a given set of pet details
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="displayName"></param>
        /// <param name="description"></param>
        /// <param name="textureName"></param>
        /// <returns></returns>
        public static PrefabInfo CreateBuildablePrefabInfo(string classId, string displayName, string description, string textureName)
        {
            Log.LogDebug($"PetBuildablePrefab: CreateBuildablePrefabInfo with classId {classId}.");
            PrefabInfo prefabInfo = PrefabInfo
                .WithTechType(classId, displayName, description)
                // Set the icon
                .WithIcon(ModUtils.GetSpriteFromAssetBundle(textureName));

            return prefabInfo;
        }

        /// <summary>
        /// Register with Fabricator, using a custom object model
        /// </summary>
        /// <param name="prefabInfo"></param>
        /// <param name="cloneTemplateClassGuid"></param>
        /// <param name="cloneModelPrefabName"></param>
        /// <param name="modelGameObjectName"></param>
        /// <param name="recipe"></param>
        /// <param name="modelScale"></param>
        /// <param name="vfxMinOffset"></param>
        /// <param name="vfxMaxOffset"></param>
        public static void RegisterPrefabInfoWithPrefab(PrefabInfo prefabInfo, string cloneTemplateClassGuid, string cloneModelPrefabName,
            string modelGameObjectName,
            RecipeData recipe, Vector3 modelScale,
            float vfxMinOffset, float vfxMaxOffset)
        {
            // Create prefab
            CustomPrefab prefab = new CustomPrefab(prefabInfo);

            CloneTemplate cloneTemplate = new CloneTemplate(prefabInfo, cloneTemplateClassGuid);
            // modify the cloned model:
            cloneTemplate.ModifyPrefab += obj =>
            {
                // Set constructable flags
                ConstructableFlags constructableFlags = ConstructableFlags.Inside;

                // Get instance for new model
                GameObject customPrefabInstance = ModUtils.GetGameObjectInstanceFromAssetBundle(cloneModelPrefabName);
                Log.LogDebug($"Created prefab instance from {cloneModelPrefabName}");
                MaterialUtils.ApplySNShaders(customPrefabInstance);

                // Find the object that holds the model for the fabricator
                GameObject modelGameObject = null;

                Log.LogDebug($"PetBuildablePrefab: RegisterPrefabInfo looking for prefab model for {obj.name}...");
                // First, find the Animator
                Animator animator = obj.GetComponentInChildren<Animator>(true);
                if (animator == null)
                {
                    Log.LogError($"PetBuildableInfo: RegisterPrefabInfo can't find Animator in {obj.name}"!);
                }
                else
                {
                    Log.LogDebug("PetBuildableInfo: Swapping model game objects...");
                    GameObject animatorGameObject = animator.gameObject;
                    Log.LogDebug($"PetBuildableInfo: Using parent {animatorGameObject.transform.parent.gameObject.name} of {animatorGameObject.name}...");
                    customPrefabInstance.transform.SetParent(animatorGameObject.transform.parent);
                    customPrefabInstance.transform.localPosition = new Vector3(0, 0, 0);
                    customPrefabInstance.transform.localRotation = new Quaternion(0, 0, 0, 0);
                    Log.LogDebug($"PetBuildableInfo: {customPrefabInstance.name} has been re-parented.");

                    modelGameObject = animatorGameObject.transform.parent.gameObject;
                    Object.Destroy(animatorGameObject);
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

                // Add all components necessary for it to be built:
                PrefabUtils.AddConstructable(obj, prefabInfo.TechType, constructableFlags, modelGameObject);
            };
            // Assign the created clone model to the prefab itself:
            prefab.SetGameObject(cloneTemplate);
            // Set recipe
            prefab.SetRecipe(recipe);
            // Register it into the game:
            prefab.Register();
        }
        /// <summary>
        /// Generic method to register PetInfo
        /// </summary>
        /// <param name="prefabInfo"></param>
        /// <param name="cloneTemplateClassGuid"></param>
        /// <param name="modelGameObjectName"></param>
        /// <param name="recipe"></param>
        /// <param name="modelScale"></param>
        /// <param name="vfxMinOffset"></param>
        /// <param name="vfxMaxOffset"></param>
        /// <returns></returns>
        public static void RegisterPrefabInfoWithGuid(PrefabInfo prefabInfo, string cloneTemplateClassGuid,
            string modelGameObjectName,
            RecipeData recipe, Vector3 modelScale,
            float vfxMinOffset, float vfxMaxOffset)
        {
            // Create prefab
            CustomPrefab prefab = new CustomPrefab(prefabInfo);

            CloneTemplate cloneTemplate = new CloneTemplate(prefabInfo, cloneTemplateClassGuid);
            // modify the cloned model:
            cloneTemplate.ModifyPrefab += obj =>
            {
                // Set constructable flags
                ConstructableFlags constructableFlags = ConstructableFlags.Inside;

                // Find the object that holds the model for the fabricator
                GameObject modelGameObject = null;

                Log.LogDebug($"PetBuildablePrefab: RegisterPrefabInfo looking for prefab model for {obj.name}...");
                // First, find the Animator
                Animator animator = obj.GetComponentInChildren<Animator>(true);
                if (animator == null)
                {
                    Log.LogError($"PetBuildableInfo: RegisterPrefabInfo can't find Animator in {obj.name}"!);
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

                    /*
                    // Look for the first MeshRender in the child - this will work as the model
                    SkinnedMeshRenderer renderer = animatorGameObject.GetComponentInChildren<SkinnedMeshRenderer>();
                    if (renderer == null)
                    {
                        Log.LogError($"PetBuildableInfo: RegisterPrefabInfo can't find SkinnedMeshRenderer in {animatorGameObject.name}"!);
                    }
                    else
                    {
                        Log.LogDebug($"PetBuildablePrefab: RegisterPrefabInfo looking for prefab model for {prefabInfo.TechType}. Found {modelGameObject.name}. Done.");
                        modelGameObject = renderer.gameObject;
                    }
                    */
                }

                /*
                GameObject model = obj.transform.Find(modelGameObjectName).gameObject;
                if (!model)
                {
                    Log.LogDebug($"PetBuildableUtils: RegisterPrefabInfo cannot find object model {modelGameObjectName} in prefab!");
                }
                else
                {
                    Log.LogDebug($"PetBuildableUtils: RegisterPrefabInfo {modelGameObjectName} found model on {model.name}");
                }
                */

                // add all components necessary for it to be built:
                PrefabUtils.AddConstructable(obj, prefabInfo.TechType, constructableFlags, modelGameObject);
            };
            // Assign the created clone model to the prefab itself:
            prefab.SetGameObject(cloneTemplate);
            // Set recipe
            prefab.SetRecipe(recipe);
            // Register it into the game:
            prefab.Register();
        }

        // Custom Pets
        public static class CatBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(CatPet.ClassId, null, null, CatPet.IconTextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfoWithPrefab(Info, CatPet.PrefabGuid, CatPet.CustomPrefabName ,CatPet.ModelName, CatPet.GetRecipeData(),
                    CatPet.ModelScale, CatPet.VfxMinOffset, CatPet.VfxMaxOffset);
                CatPet.BuildablePrefabInfo = Info;
            }
        }

#if SUBNAUTICA
        /// <summary>
        /// Cave Crawler Buildable
        /// </summary>
        public static class CaveCrawlerBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(CaveCrawlerPet.ClassId, null, null, CaveCrawlerPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfoWithGuid(Info, CaveCrawlerPet.PrefabGuid, CaveCrawlerPet.ModelName, CaveCrawlerPet.GetRecipeData(),
                    CaveCrawlerPet.ModelScale, CaveCrawlerPet.VfxMinOffset, CaveCrawlerPet.VfxMaxOffset);
                CaveCrawlerPet.BuildablePrefabInfo = Info;
            }
        }

        /// <summary>
        /// Blood Crawler Buildable
        /// </summary>
        public static class BloodCrawlerBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(BloodCrawlerPet.ClassId, null, null, BloodCrawlerPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfoWithGuid(Info, BloodCrawlerPet.PrefabGuid, BloodCrawlerPet.ModelName, BloodCrawlerPet.GetRecipeData(),
                    BloodCrawlerPet.ModelScale, BloodCrawlerPet.VfxMinOffset, BloodCrawlerPet.VfxMaxOffset);
                BloodCrawlerPet.BuildablePrefabInfo = Info;
            }
        }

        /// <summary>
        /// Crab Squid Buildable
        /// </summary>
        public static class CrabSquidBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(CrabSquidPet.ClassId, null, null, CrabSquidPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfoWithGuid(Info, CrabSquidPet.PrefabGuid, CrabSquidPet.ModelName, CrabSquidPet.GetRecipeData(),
                    CrabSquidPet.ModelScale, CrabSquidPet.VfxMinOffset, CrabSquidPet.VfxMaxOffset);
                CrabSquidPet.BuildablePrefabInfo = Info;
            }
        }

        /// <summary>
        /// Alien Robot Buildable
        /// </summary>
        public static class AlienRobotBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(AlienRobotPet.ClassId, null, null, AlienRobotPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfoWithGuid(Info, AlienRobotPet.PrefabGuid, AlienRobotPet.ModelName, AlienRobotPet.GetRecipeData(),
                    AlienRobotPet.ModelScale, AlienRobotPet.VfxMinOffset, AlienRobotPet.VfxMaxOffset);
                AlienRobotPet.BuildablePrefabInfo = Info;
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
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(PenglingBabyPet.ClassId, null, null, PenglingBabyPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfoWithGuid(Info, PenglingBabyPet.PrefabGuid, PenglingBabyPet.ModelName, PenglingBabyPet.GetRecipeData(),
                    PenglingBabyPet.ModelScale, PenglingBabyPet.VfxMinOffset, PenglingBabyPet.VfxMaxOffset);
                PenglingBabyPet.BuildablePrefabInfo = Info;
            }
        }

        /// <summary>
        /// Pengling Adult Buildable
        /// </summary>
        public static class PenglingAdultBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(PenglingAdultPet.ClassId, null, null, PenglingAdultPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfoWithGuid(Info, PenglingAdultPet.PrefabGuid, PenglingAdultPet.ModelName, PenglingAdultPet.GetRecipeData(),
                    PenglingAdultPet.ModelScale, PenglingAdultPet.VfxMinOffset, PenglingAdultPet.VfxMaxOffset);
                PenglingAdultPet.BuildablePrefabInfo = Info;
            }
        }

        /// <summary>
        /// Snow Stalker Baby Buildable
        /// </summary>
        public static class SnowStalkerBabyBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(SnowStalkerBabyPet.ClassId, null, null, SnowStalkerBabyPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfoWithGuid(Info, SnowStalkerBabyPet.PrefabGuid, SnowStalkerBabyPet.ModelName, SnowStalkerBabyPet.GetRecipeData(),
                    SnowStalkerBabyPet.ModelScale, SnowStalkerBabyPet.VfxMinOffset, SnowStalkerBabyPet.VfxMaxOffset);
                SnowStalkerBabyPet.BuildablePrefabInfo = Info;
            }
        }

        /// <summary>
        /// Pinnicarid Buildable
        /// </summary>
        public static class PinnicaridBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(PinnicaridPet.ClassId, null, null, PinnicaridPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfoWithGuid(Info, PinnicaridPet.PrefabGuid, PinnicaridPet.ModelName, PinnicaridPet.GetRecipeData(),
                    PinnicaridPet.ModelScale, PinnicaridPet.VfxMinOffset, PinnicaridPet.VfxMaxOffset);
                PinnicaridPet.BuildablePrefabInfo = Info;
            }
        }

        /// <summary>
        /// Yellow Trivalve Buildable
        /// </summary>
        public static class TrivalveYellowBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(TrivalveYellowPet.ClassId, null, null, TrivalveYellowPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfoWithGuid(Info, TrivalveYellowPet.PrefabGuid, TrivalveYellowPet.ModelName, TrivalveYellowPet.GetRecipeData(),
                    TrivalveYellowPet.ModelScale, TrivalveYellowPet.VfxMinOffset, TrivalveYellowPet.VfxMaxOffset);
                TrivalveYellowPet.BuildablePrefabInfo = Info;
            }
        }

        /// <summary>
        /// Blue Trivalve Buildable
        /// </summary>
        public static class TrivalveBluePetBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(TrivalveBluePet.ClassId,
                null, null, TrivalveBluePet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfoWithGuid(Info, TrivalveBluePet.PrefabGuid,
                    TrivalveBluePet.ModelName, TrivalveBluePet.GetRecipeData(), TrivalveBluePet.ModelScale,
                    TrivalveBluePet.VfxMinOffset, TrivalveBluePet.VfxMaxOffset);
                TrivalveBluePet.BuildablePrefabInfo = Info;
            }
        }
#endif
    }
}