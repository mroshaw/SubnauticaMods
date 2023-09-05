#if SUBNAUTICA
using DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets.Subnautica;
#endif

#if SUBNAUTICAZERO
using DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets.BelowZero;
#endif

using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using Nautilus.Utility;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;
using static UWE.FreezeTime;

namespace DaftAppleGames.SubnauticaPets.CustomObjects
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
            Log.LogDebug($"PetBuildablePrefab: CreateBuildablePrefabInfo with classId {classId}, displayName {displayName}, description {description}, textureName {textureName}.");
            PrefabInfo prefabInfo = PrefabInfo
                .WithTechType(classId, displayName, description)
                // Set the icon
                .WithIcon(ModUtils.GetSpriteFromAssetBundle(textureName));

            return prefabInfo;
        }

        /// <summary>
        /// Generic method to register PetInfo
        /// </summary>
        /// <param name="prefabInfo"></param>
        /// <param name="cloneTemplateClassGuid"></param>
        /// <param name="modelGameObjectName"></param>
        /// <param name="recipe"></param>
        /// <returns></returns>
        public static void RegisterPrefabInfo(PrefabInfo prefabInfo, string cloneTemplateClassGuid, string modelGameObjectName,
            RecipeData recipe)
        {
            // Create prefab
            CustomPrefab prefab = new CustomPrefab(prefabInfo);
            // Copy the prefab model
            CloneTemplate cloneTemplate = new CloneTemplate(prefabInfo, cloneTemplateClassGuid);
            // modify the cloned model:
            cloneTemplate.ModifyPrefab += obj =>
            {
                // Set constructable flags
                ConstructableFlags constructableFlags = ConstructableFlags.Inside;

                // Find the object that holds the model:
                GameObject model = obj.transform.Find(modelGameObjectName).gameObject;
                if (!model)
                {
                    Log.LogDebug($"PetBuildableUtils: RegisterPrefabInfo cannot find object model {modelGameObjectName} in prefab!");
                }
                else
                {
                    Log.LogDebug($"PetBuildableUtils: RegisterPrefabInfo {modelGameObjectName} found model on {model.name}");
                }
                // add all components necessary for it to be built:
                PrefabUtils.AddConstructable(obj, prefabInfo.TechType, constructableFlags, model);
            };
            // Assign the created clone model to the prefab itself:
            prefab.SetGameObject(cloneTemplate);
            // Set recipe
            prefab.SetRecipe(recipe);
            // Register it into the game:
            prefab.Register();
        }

#if SUBNAUTICA
        /// <summary>
        /// Cave Crawler Buildable
        /// </summary>
        public static class CaveCrawlerBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(CaveCrawlerPet.ClassId, CaveCrawlerPet.DisplayName, CaveCrawlerPet.Description, CaveCrawlerPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfo(Info, CaveCrawlerPet.PrefabGuid, CaveCrawlerPet.ModelName, CaveCrawlerPet.GetRecipeData());
                CaveCrawlerPet.BuildablePrefabInfo = Info;
            }
        }

        /// <summary>
        /// Blood Crawler Buildable
        /// </summary>
        public static class BloodCrawlerBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(BloodCrawlerPet.ClassId, BloodCrawlerPet.DisplayName, BloodCrawlerPet.Description, BloodCrawlerPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfo(Info, BloodCrawlerPet.PrefabGuid, BloodCrawlerPet.ModelName, BloodCrawlerPet.GetRecipeData());
                BloodCrawlerPet.BuildablePrefabInfo = Info;
            }
        }

        /// <summary>
        /// Crab Squid Buildable
        /// </summary>
        public static class CrabSquidBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(CrabSquidPet.ClassId, CrabSquidPet.DisplayName, CrabSquidPet.Description, CrabSquidPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfo(Info, CrabSquidPet.PrefabGuid, CrabSquidPet.ModelName, CrabSquidPet.GetRecipeData());
                CrabSquidPet.BuildablePrefabInfo = Info;
            }
        }

        /// <summary>
        /// Alien Robot Buildable
        /// </summary>
        public static class AlienRobotBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(AlienRobotPet.ClassId, AlienRobotPet.DisplayName, AlienRobotPet.Description, AlienRobotPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfo(Info, AlienRobotPet.PrefabGuid, AlienRobotPet.ModelName, AlienRobotPet.GetRecipeData());
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
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(PenglingBabyPet.ClassId, PenglingBabyPet.DisplayName, PenglingBabyPet.Description, PenglingBabyPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfo(Info, PenglingBabyPet.PrefabGuid, PenglingBabyPet.ModelName, PenglingBabyPet.GetRecipeData());
                PenglingBabyPet.BuildablePrefabInfo = Info;
            }
        }

        /// <summary>
        /// Pengling Adult Buildable
        /// </summary>
        public static class PenglingAdultBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(PenglingAdultPet.ClassId, PenglingAdultPet.DisplayName, PenglingAdultPet.Description, PenglingAdultPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfo(Info, PenglingAdultPet.PrefabGuid, PenglingAdultPet.ModelName, PenglingAdultPet.GetRecipeData());
                PenglingAdultPet.BuildablePrefabInfo = Info;
            }
        }

        /// <summary>
        /// Snow Stalker Baby Buildable
        /// </summary>
        public static class SnowStalkerBabyBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(SnowStalkerBabyPet.ClassId, SnowStalkerBabyPet.DisplayName, SnowStalkerBabyPet.Description, SnowStalkerBabyPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfo(Info, SnowStalkerBabyPet.PrefabGuid, SnowStalkerBabyPet.ModelName, SnowStalkerBabyPet.GetRecipeData());
                SnowStalkerBabyPet.BuildablePrefabInfo = Info;
            }
        }

        /// <summary>
        /// Pinnicarid Buildable
        /// </summary>
        public static class PinnicaridBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(PinnicaridPet.ClassId, PinnicaridPet.DisplayName, PinnicaridPet.Description, PinnicaridPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfo(Info, PinnicaridPet.PrefabGuid, PinnicaridPet.ModelName, PinnicaridPet.GetRecipeData());
                PinnicaridPet.BuildablePrefabInfo = Info;
            }
        }

        /// <summary>
        /// Yellow Trivalve Buildable
        /// </summary>
        public static class TrivalveYellowBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(TrivalveYellowPet.ClassId, TrivalveYellowPet.DisplayName, TrivalveYellowPet.Description, TrivalveYellowPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfo(Info, TrivalveYellowPet.PrefabGuid, TrivalveYellowPet.ModelName, TrivalveYellowPet.GetRecipeData());
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
                TrivalveBluePet.DisplayName, TrivalveBluePet.Description, TrivalveBluePet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfo(Info, TrivalveBluePet.PrefabGuid,
                    TrivalveBluePet.ModelName, TrivalveBluePet.GetRecipeData());
                TrivalveBluePet.BuildablePrefabInfo = Info;
            }
        }
#endif
    }
}