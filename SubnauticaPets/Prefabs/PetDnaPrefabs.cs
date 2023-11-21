using DaftAppleGames.SubnauticaPets.Mono.Pets;
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Utility;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Prefabs
{
    /// <summary>
    /// Static class for creating the "Pet DNA" collectable objects
    /// </summary>
    internal class PetDnaPrefabs
    {
        /// <summary>
        /// Register all prefabs
        /// </summary>
        public static void RegisterAll()
        {
            // Get the DNA model prefab from the Asset Bundle
            GameObject dnaModelPrefab = ModUtils.GetGameObjectPrefabFromAssetBundle("DNASampleTube");
            MaterialUtils.ApplySNShaders(dnaModelPrefab);

            // Register DNA spawn prefabs
            CatDnaPrefab.Register(dnaModelPrefab);

#if SUBNAUTICA
            AlienRobotDnaPrefab.Register(dnaModelPrefab);
            BloodCrawlerDnaPrefab.Register(dnaModelPrefab);
            CaveCrawlerDnaPrefab.Register(dnaModelPrefab);
            CrabSquidDnaPrefab.Register(dnaModelPrefab);
#endif
#if SUBNAUTICAZERO
            PengwingAdultDnaPrefab.Register(dnaModelPrefab);
            PenglingBabyDnaPrefab.Register(dnaModelPrefab);
            PinnacaridDnaPrefab.Register(dnaModelPrefab);
            SnowstalkerBabyDnaPrefab.Register(dnaModelPrefab);
            TrivalveBlueDnaPrefab.Register(dnaModelPrefab);
            TrivalveYellowDnaPrefab.Register(dnaModelPrefab);
#endif
            ConfigureDataBank();
        }

        /// <summary>
        /// Cat DNA Prefab
        /// </summary>
        public static class CatDnaPrefab
        {
            public static PrefabInfo Info;

            /// <summary>
            /// Register Cat DNA
            /// </summary>
            /// <param name="dnaModelGameObject"></param>
            public static void Register(GameObject dnaModelGameObject)
            {
                Info = RegisterDnaPrefab("CatPetDna", null, null, "CatTexture", Color.grey,
                    new LootDistributionData.BiomeData[]
                    {
                        new LootDistributionData.BiomeData
                            { biome = BiomeType.Kelp_TechSite, count = 4, probability = 0.6f },
                        new LootDistributionData.BiomeData
                            { biome = BiomeType.GrandReef_TechSite, count = 5, probability = 0.8f },
                        new LootDistributionData.BiomeData
                            { biome = BiomeType.SafeShallows_TechSite, count = 5, probability = 0.4f },
                    }, dnaModelGameObject);
            }
        }
#if SUBNAUTICAZERO
        /// <summary>
        /// Pengwing Adult DNA
        /// </summary>
        public static class PengwingAdultDnaPrefab
        {
            public static PrefabInfo Info;

            /// <summary>
            /// Registers Pengwing Adult DNA
            /// </summary>
            /// <param name="dnaModelGameObject"></param>
            public static void Register(GameObject dnaModelGameObject)
            {
                Info = RegisterDnaPrefab("PengwingAdultPetDna", null, null, "PengwingAdultDnaStrandTexture", Color.grey,
                    new LootDistributionData.BiomeData[] {
                        new LootDistributionData.BiomeData { biome = BiomeType.Kelp_TechSite, count = 4, probability = 0.6f},
                        new LootDistributionData.BiomeData { biome = BiomeType.CrashZone_Sand, count = 5, probability = 0.8f},
                        new LootDistributionData.BiomeData { biome = BiomeType.GrassyPlateaus_TechSite, count = 5, probability = 0.4f},
                        new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_TechSite, count = 8, probability = 0.5f},
                    }, dnaModelGameObject);
            }
        }

        /// <summary>
        /// Pengling Baby DNA
        /// </summary>
        public static class PenglingBabyDnaPrefab
        {
            public static PrefabInfo Info;

            /// <summary>
            /// Register Pengling Baby DNA
            /// </summary>
            /// <param name="dnaModelGameObject"></param>
            public static void Register(GameObject dnaModelGameObject)
            {
                Info = RegisterDnaPrefab("PenglingBabyPetDna", null, null, "PenglingBabyDnaStrandTexture", Color.magenta,
                    new LootDistributionData.BiomeData[] {
                        new LootDistributionData.BiomeData { biome = BiomeType.Kelp_TechSite, count = 4, probability = 0.6f},
                        new LootDistributionData.BiomeData { biome = BiomeType.CrashZone_Sand, count = 5, probability = 0.8f},
                        new LootDistributionData.BiomeData { biome = BiomeType.GrassyPlateaus_TechSite, count = 5, probability = 0.4f},
                        new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_TechSite, count = 8, probability = 0.5f},
                    }, dnaModelGameObject);
            }
        }

        /// <summary>
        /// Pinnacarid DNA
        /// </summary>
        public static class PinnacaridDnaPrefab
        {
            public static PrefabInfo Info;

            /// <summary>
            /// Register Pinnacarid DNA
            /// </summary>
            /// <param name="dnaModelGameObject"></param>
            public static void Register(GameObject dnaModelGameObject)
            {
                Info = RegisterDnaPrefab("PinnacaridPetDna", null, null, "PinnacaridDnaStrandTexture", Color.blue,
                    new LootDistributionData.BiomeData[] {
                        new LootDistributionData.BiomeData { biome = BiomeType.Kelp_TechSite, count = 4, probability = 0.6f},
                        new LootDistributionData.BiomeData { biome = BiomeType.CrashZone_Sand, count = 5, probability = 0.8f},
                        new LootDistributionData.BiomeData { biome = BiomeType.GrassyPlateaus_TechSite, count = 5, probability = 0.4f},
                        new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_TechSite, count = 8, probability = 0.5f},
                    }, dnaModelGameObject);
            }
        }

        /// <summary>
        /// Snowstalker Baby DNA
        /// </summary>
        public static class SnowstalkerBabyDnaPrefab
        {
            public static PrefabInfo Info;

            /// <summary>
            /// Register Snowstalker Baby DNA
            /// </summary>
            /// <param name="dnaModelGameObject"></param>
            public static void Register(GameObject dnaModelGameObject)
            {
                Info = RegisterDnaPrefab("SnowstalkerBabyPetDna", null, null, "SnowstalkerBabyDnaStrandTexture", Color.white,
                    new LootDistributionData.BiomeData[] {
                        new LootDistributionData.BiomeData { biome = BiomeType.Kelp_TechSite, count = 4, probability = 0.6f},
                        new LootDistributionData.BiomeData { biome = BiomeType.CrashZone_Sand, count = 5, probability = 0.8f},
                        new LootDistributionData.BiomeData { biome = BiomeType.GrassyPlateaus_TechSite, count = 5, probability = 0.4f},
                        new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_TechSite, count = 8, probability = 0.5f},
                    }, dnaModelGameObject);
            }
        }

        /// <summary>
        /// Trivalve Blue DNA
        /// </summary>
        public static class TrivalveBlueDnaPrefab
        {
            public static PrefabInfo Info;

            /// <summary>
            /// Register Trivalve Blue DNA
            /// </summary>
            /// <param name="dnaModelGameObject"></param>
            public static void Register(GameObject dnaModelGameObject)
            {
                Info = RegisterDnaPrefab("TrivalveBluePetDna", null, null, "TrivalveBlueDnaStrandTexture", Color.blue,
                    new LootDistributionData.BiomeData[] {
                        new LootDistributionData.BiomeData { biome = BiomeType.Kelp_TechSite, count = 4, probability = 0.6f},
                        new LootDistributionData.BiomeData { biome = BiomeType.CrashZone_Sand, count = 5, probability = 0.8f},
                        new LootDistributionData.BiomeData { biome = BiomeType.GrassyPlateaus_TechSite, count = 5, probability = 0.4f},
                        new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_TechSite, count = 8, probability = 0.5f},
                    }, dnaModelGameObject);
            }
        }

        /// <summary>
        /// Trivalve Yellow DNA
        /// </summary>
        public static class TrivalveYellowDnaPrefab
        {
            public static PrefabInfo Info;

            /// <summary>
            /// Register Trivalve Yellow DNA
            /// </summary>
            /// <param name="dnaModelGameObject"></param>
            public static void Register(GameObject dnaModelGameObject)
            {
                Info = RegisterDnaPrefab("TrivalveYellowPetDna", null, null, "TrivalveYellowDnaStrandTexture", Color.yellow,
                    new LootDistributionData.BiomeData[] {
                        new LootDistributionData.BiomeData { biome = BiomeType.Kelp_TechSite, count = 4, probability = 0.6f},
                        new LootDistributionData.BiomeData { biome = BiomeType.CrashZone_Sand, count = 5, probability = 0.8f},
                        new LootDistributionData.BiomeData { biome = BiomeType.GrassyPlateaus_TechSite, count = 5, probability = 0.4f},
                        new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_TechSite, count = 8, probability = 0.5f},
                    }, dnaModelGameObject);
            }
        }
#endif

#if SUBNAUTICA
        /// <summary>
        /// Alien Robot DNA
        /// </summary>
        public static class AlienRobotDnaPrefab
        {
            public static PrefabInfo Info;

            /// <summary>
            /// Register Alien Robot DNA
            /// </summary>
            /// <param name="dnaModelGameObject"></param>
            public static void Register(GameObject dnaModelGameObject)
            {
                Info = RegisterDnaPrefab("AlienRobotPetDna", null, null, "AlienRobotDnaStrandTexture", Color.grey,
                    new LootDistributionData.BiomeData[] {
                        new LootDistributionData.BiomeData { biome = BiomeType.Kelp_TechSite, count = 4, probability = 1.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.Kelp_TechSite_Scattered, count = 4, probability = 1.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.Kelp_TechSite_Barrier, count = 4, probability = 1.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.GrandReef_TechSite, count = 4, probability = 1.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.GrandReef_TechSite_Scattered, count = 4, probability = 1.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.GrandReef_TechSite_Barrier, count = 4, probability = 1.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_TechSite, count = 4, probability = 1.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_TechSite_Scattered, count = 4, probability = 1.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_TechSite_Barrier, count = 4, probability = 1.0f},
                    }, dnaModelGameObject);
            }
        }

        /// <summary>
        /// Blood Crawler DNA
        /// </summary>
        public static class BloodCrawlerDnaPrefab
        {
            public static PrefabInfo Info;

            /// <summary>
            /// Register Blood Crawler DNA
            /// </summary>
            /// <param name="dnaModelGameObject"></param>
            public static void Register(GameObject dnaModelGameObject)
            {
                Info = RegisterDnaPrefab("BloodCrawlerPetDna", null, null, "BloodCrawlerDnaStrandTexture", Color.red,
                    new LootDistributionData.BiomeData[] {
                        new LootDistributionData.BiomeData { biome = BiomeType.CrashZone_Sand, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.CrashZone_Rock, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.Dunes_TechSite, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.Dunes_TechSite_Scatter, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.Dunes_TechSite_Barrier, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.Mountains_TechSite, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.Mountains_TechSite_Scatter, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.SeaTreaderPath_TechSite, count = 4, probability = 2.0f},
                    }, dnaModelGameObject);
            }
        }

        public static class CaveCrawlerDnaPrefab
        {
            public static PrefabInfo Info;

            public static void Register(GameObject dnaModelGameObject)
            {
                Info = RegisterDnaPrefab("CaveCrawlerPetDna", null, null, "CaveCrawlerDnaStrandTexture", Color.cyan,
                    new LootDistributionData.BiomeData[] {
                        new LootDistributionData.BiomeData { biome = BiomeType.Kelp_TechSite, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.Kelp_TechSite_Barrier, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.Kelp_TechSite_Scattered, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.CrashZone_Sand, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.GrassyPlateaus_TechSite, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.GrassyPlateaus_TechSite_Barrier, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.GrassyPlateaus_TechSite_Scattered, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_TechSite, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_TechSite_Barrier, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_TechSite_Scattered, count = 4, probability = 2.0f},
                    }, dnaModelGameObject);
            }
        }

        /// <summary>
        /// Crab Squid DNA
        /// </summary>
        public static class CrabSquidDnaPrefab
        {
            public static PrefabInfo Info;

            /// <summary>
            /// Register Crab Squid DNA
            /// </summary>
            /// <param name="dnaModelGameObject"></param>
            public static void Register(GameObject dnaModelGameObject)
            {
                Info = RegisterDnaPrefab("CrabSquidPetDna", null, null, "CrabSquidDnaStrandTexture", Color.blue,
                    new LootDistributionData.BiomeData[] {
                        new LootDistributionData.BiomeData { biome = BiomeType.CrashZone_Sand, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.KooshZone_TechSite, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.KooshZone_TechSite_Barrier, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.KooshZone_TechSite_Scatter, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.Mountains_TechSite, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.Mountains_TechSite_Scatter, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.Mountains_TechSite_Barrier, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.SparseReef_Techsite, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.SparseReef_Techsite_Scatter, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.SparseReef_Techsite_Barrier, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.UnderwaterIslands_TechSite, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.UnderwaterIslands_TechSite_Scatter, count = 4, probability = 2.0f},
                        new LootDistributionData.BiomeData { biome = BiomeType.UnderwaterIslands_TechSite_Barrier, count = 4, probability = 2.0f},
                    }, dnaModelGameObject);
            }
        }
#endif
        /// <summary>
        /// Generic method to set up the DNA prefabs
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="displayName"></param>
        /// <param name="description"></param>
        /// <param name="textureName"></param>
        /// <param name="color"></param>
        /// <param name="lootBiome"></param>
        /// <param name="dnaModelPrefab"></param>
        /// <returns></returns>
        private static PrefabInfo RegisterDnaPrefab(string classId, string displayName, string description, string textureName,
            Color color, LootDistributionData.BiomeData[] lootBiome, GameObject dnaModelPrefab)
        {
            LogUtils.LogDebug(LogArea.Prefabs, $"PetDnaPrefab: Register Prefab for {classId}...");

            PrefabInfo prefabInfo = PrefabInfo
                .WithTechType(classId, displayName, description, unlockAtStart: true)
                .WithIcon(ModUtils.GetSpriteFromAssetBundle(textureName));

            CustomPrefab clonePrefab = new CustomPrefab(prefabInfo);

            CloneTemplate cloneTemplate = new CloneTemplate(clonePrefab.Info, TechType.Titanium)
            {
                ModifyPrefab = obj =>
                {
                    obj.SetActive(false);
                    GameObject newModel = Object.Instantiate(dnaModelPrefab);
                    newModel.name = "newmodel";
                    // Add new model
                    newModel.transform.SetParent(obj.transform);
                    newModel.transform.localPosition = new Vector3(0, 0, 0);
                    newModel.transform.localRotation = new Quaternion(0, 0, 0, 0);
                    // Disable the old model
                    obj.FindChild("model").SetActive(false);

                    MaterialUtils.ApplySNShaders(newModel);

                    // Configure Prefab
                    PrefabUtils.AddBasicComponents(obj, clonePrefab.Info.ClassID, clonePrefab.Info.TechType, LargeWorldEntity.CellLevel.Medium);
                    PrefabUtils.AddResourceTracker(obj, TechType.None);
                    PrefabConfigUtils.SetMeshRenderersColor(newModel, "Ends", color);
                    PrefabConfigUtils.AddRotateModel(newModel, "DNA");
                    PrefabConfigUtils.AddRigidBody(obj);
                    PrefabConfigUtils.AddFreezeOnSettle(obj);
                    PrefabConfigUtils.AddDnaCapsuleCollider(obj);
                    PrefabConfigUtils.AddScaleOnStart(obj, 0.4f);
                    obj.AddComponent<PetDna>();


                }
            };
            clonePrefab.SetGameObject(cloneTemplate);
            clonePrefab.SetSpawns(lootBiome);
            clonePrefab.Register();
            return prefabInfo;
        }

        /// <summary>
        /// Adds all DataBank entries
        /// </summary>
        public static void ConfigureDataBank()
        {
            ModUtils.ConfigureDatabankEntry("PetDna", "Lifeforms/Fauna", "PetDnaDataBankMainImageTexture", "PetDnaDataBankPopupImageTexture");
#if SUBNAUTICA
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal("PetDna", Story.GoalType.Encyclopedia, AlienRobotDnaPrefab.Info.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal("PetDna", Story.GoalType.Encyclopedia, BloodCrawlerDnaPrefab.Info.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal("PetDna", Story.GoalType.Encyclopedia, CaveCrawlerDnaPrefab.Info.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal("PetDna", Story.GoalType.Encyclopedia, CrabSquidDnaPrefab.Info.TechType);
#endif
#if SUBNAUTICAZERO
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal("PetDna", Story.GoalType.Encyclopedia, PengwingAdultDnaPrefab.Info.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal("PetDna", Story.GoalType.Encyclopedia, PenglingBabyDnaPrefab.Info.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal("PetDna", Story.GoalType.Encyclopedia, PinnacaridDnaPrefab.Info.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal("PetDna", Story.GoalType.Encyclopedia, SnowstalkerBabyDnaPrefab.Info.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal("PetDna", Story.GoalType.Encyclopedia, TrivalveYellowDnaPrefab.Info.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal("PetDna", Story.GoalType.Encyclopedia, TrivalveBlueDnaPrefab.Info.TechType);
#endif
        }
    }
}
