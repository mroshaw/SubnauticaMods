using DaftAppleGames.SubnauticaPets.Pets;
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
            AlienRobotDnaPrefab.Register(dnaModelPrefab);
            BloodCrawlerDnaPrefab.Register(dnaModelPrefab);
            CaveCrawlerDnaPrefab.Register(dnaModelPrefab);
            CrabSquidDnaPrefab.Register(dnaModelPrefab);

            ConfigureDataBank();
        }

        /// <summary>
        /// Cat DNA Prefab
        /// </summary>
        public static class CatDnaPrefab
        {
            public static PrefabInfo Info;
            private const int FindCount = 1;
            private const float FindProbability = 0.3f;

            /// <summary>
            /// Register Cat DNA
            /// </summary>
            /// <param name="dnaModelGameObject"></param>
            public static void Register(GameObject dnaModelGameObject)
            {
                Info = RegisterDnaPrefab("CatPetDna", null, null, "CatTexture", Color.grey,
                    new LootDistributionData.BiomeData[]
                    {

                        new LootDistributionData.BiomeData { biome = BiomeType.InactiveLavaZone_Corridor_Floor, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.InactiveLavaZone_Corridor_Floor_Far, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.InactiveLavaZone_LavaPit_Floor, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.JellyshroomCaves_CaveFloor, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.UnderwaterIslands_TechSite, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.UnderwaterIslands_TechSite_Scatter, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.UnderwaterIslands_ValleyFloor, count = FindCount, probability = FindProbability},

                        new LootDistributionData.BiomeData { biome = BiomeType.CrashZone_Sand, count = FindCount, probability = FindProbability},

                    }, dnaModelGameObject);
            }
        }
        /// <summary>
        /// Alien Robot DNA
        /// </summary>
        public static class AlienRobotDnaPrefab
        {
            public static PrefabInfo Info;
            private const int FindCount = 1;
            private const float FindProbability = 0.3f;

            /// <summary>
            /// Register Alien Robot DNA
            /// </summary>
            /// <param name="dnaModelGameObject"></param>
            public static void Register(GameObject dnaModelGameObject)
            {
                Info = RegisterDnaPrefab("AlienRobotPetDna", null, null, "AlienRobotDnaStrandTexture", Color.grey,
                    new LootDistributionData.BiomeData[] {
                    new LootDistributionData.BiomeData { biome = BiomeType.ActiveLavaZone_Chamber_Floor_Far, count = FindCount, probability = FindProbability},
                    new LootDistributionData.BiomeData { biome = BiomeType.ActiveLavaZone_Falls_Floor, count = FindCount, probability = FindProbability},
                    new LootDistributionData.BiomeData { biome = BiomeType.ActiveLavaZone_Falls_Floor_Far, count = FindCount, probability = FindProbability},
                    new LootDistributionData.BiomeData { biome = BiomeType.BloodKelp_CaveFloor, count = FindCount, probability = FindProbability},
                    new LootDistributionData.BiomeData { biome = BiomeType.BloodKelp_TechSite_Scatter, count = FindCount, probability = FindProbability},
                    new LootDistributionData.BiomeData { biome = BiomeType.Dunes_TechSite, count = FindCount, probability = FindProbability},
                    new LootDistributionData.BiomeData { biome = BiomeType.Dunes_TechSite_Scatter, count = FindCount, probability = FindProbability},
                    new LootDistributionData.BiomeData { biome = BiomeType.GrandReef_TechSite_Barrier, count = FindCount, probability = FindProbability},
                    new LootDistributionData.BiomeData { biome = BiomeType.GrassyPlateaus_TechSite, count = FindCount, probability = FindProbability},
                    new LootDistributionData.BiomeData { biome = BiomeType.Kelp_CaveFloor, count = FindCount, probability = FindProbability},
                    new LootDistributionData.BiomeData { biome = BiomeType.Kelp_TechSite, count = FindCount, probability = FindProbability},
                    new LootDistributionData.BiomeData { biome = BiomeType.KooshZone_CaveFloor, count = FindCount, probability = FindProbability},
                    new LootDistributionData.BiomeData { biome = BiomeType.LostRiverCorridor_LakeFloor, count = FindCount, probability = FindProbability},
                    new LootDistributionData.BiomeData { biome = BiomeType.LostRiverJunction_LakeFloor, count = FindCount, probability = FindProbability},
                    new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_TechSite_Scattered, count = FindCount, probability = FindProbability},

                    new LootDistributionData.BiomeData { biome = BiomeType.CrashZone_Rock, count = FindCount, probability = FindProbability},


                    }, dnaModelGameObject);
            }
        }

        /// <summary>
        /// Blood Crawler DNA
        /// </summary>
        public static class BloodCrawlerDnaPrefab
        {
            public static PrefabInfo Info;
            private const int FindCount = 1;
            private const float FindProbability = 0.3f;

            /// <summary>
            /// Register Blood Crawler DNA
            /// </summary>
            /// <param name="dnaModelGameObject"></param>
            public static void Register(GameObject dnaModelGameObject)
            {
                Info = RegisterDnaPrefab("BloodCrawlerPetDna", null, null, "BloodCrawlerDnaStrandTexture", Color.red,
                    new LootDistributionData.BiomeData[] {
                        new LootDistributionData.BiomeData { biome = BiomeType.BloodKelp_TrenchFloor, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.BonesField_LakePit_Floor, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.BonesField_Lake_Floor, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.Canyon_Lake_Floor, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.GrandReef_TechSite_Scattered, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.GrandReef_TechSite_Scattered_Crate_Obsolete, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.KooshZone_TechSite, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.Mountains_CaveFloor, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.Mountains_IslandCaveFloor, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.Mountains_TechSite, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.MushroomForest_CaveFloor, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.MushroomForest_GiantTreeInteriorFloor, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_TechSite_Scattered_Crate_Obsolete, count = FindCount, probability = FindProbability},

                        new LootDistributionData.BiomeData { biome = BiomeType.CrashZone_TrenchSand, count = FindCount, probability = FindProbability},
                    }, dnaModelGameObject);
            }
        }

        public static class CaveCrawlerDnaPrefab
        {
            public static PrefabInfo Info;
            private const int FindCount = 1;
            private const float FindProbability = 0.3f;
            public static void Register(GameObject dnaModelGameObject)
            {
                Info = RegisterDnaPrefab("CaveCrawlerPetDna", null, null, "CaveCrawlerDnaStrandTexture", Color.cyan,
                    new LootDistributionData.BiomeData[] {
                        new LootDistributionData.BiomeData { biome = BiomeType.Dunes_CaveFloor, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.GhostTree_LakePit_Floor, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.GhostTree_Lake_Floor, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.GrandReef_CaveFloor, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.GrassyPlateaus_CaveFloor, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.GrassyPlateaus_TechSite_Scattered, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.Kelp_TechSite_Scattered, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.MushroomForest_TechSite, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.PrisonAquarium_CaveFloor, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_CaveFloor, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_TechSite, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.SeaTreaderPath_CaveFloor, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.SeaTreaderPath_TechSite_Scatter, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.SkeletonCave_Lake_Floor, count = FindCount, probability = FindProbability},

                        new LootDistributionData.BiomeData { biome = BiomeType.CrashZone_TrenchRock, count = FindCount, probability = FindProbability},

                    }, dnaModelGameObject);
            }
        }

        /// <summary>
        /// Crab Squid DNA
        /// </summary>
        public static class CrabSquidDnaPrefab
        {
            public static PrefabInfo Info;
            private const int FindCount = 1;
            private const float FindProbability = 0.3f;

            /// <summary>
            /// Register Crab Squid DNA
            /// </summary>
            /// <param name="dnaModelGameObject"></param>
            public static void Register(GameObject dnaModelGameObject)
            {
                Info = RegisterDnaPrefab("CrabSquidPetDna", null, null, "CrabSquidDnaStrandTexture", Color.blue,
                    new LootDistributionData.BiomeData[] {
                        new LootDistributionData.BiomeData { biome = BiomeType.InactiveLavaZone_CastleChamber_Floor, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.InactiveLavaZone_CastleTunnel_Floor, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.InactiveLavaZone_Chamber_Floor, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.InactiveLavaZone_Chamber_Floor_Far, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.KooshZone_TechSite_Scatter, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.Mountains_TechSite_Scatter, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.SeaTreaderPath_TechSite, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.SparseReef_CaveFloor, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.SparseReef_DeepFloor, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.SparseReef_Techsite, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.SparseReef_Techsite_Scatter, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.TreeCove_LakeFloor, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.UnderwaterIslands_IslandCaveFloor, count = FindCount, probability = FindProbability},

                        new LootDistributionData.BiomeData { biome = BiomeType.CrashZone_TechSite_Obsolete, count = FindCount, probability = FindProbability},

                    }, dnaModelGameObject);
            }
        }

        /// <summary>
        /// Generic method to set up the DNA prefabs
        /// </summary>
        private static PrefabInfo RegisterDnaPrefab(string classId, string displayName, string description, string textureName,
            Color color, LootDistributionData.BiomeData[] lootBiome, GameObject dnaModelPrefab)
        {
            LogUtils.LogDebug(LogArea.Prefabs, $"PetDnaPrefab: Register Prefab for {classId}...");

            PrefabInfo prefabInfo = PrefabInfo
                .WithTechType(classId, displayName, description, unlockAtStart: true)
                .WithIcon(ModUtils.GetSpriteFromAssetBundle(textureName));

            CustomPrefab clonePrefab = new CustomPrefab(prefabInfo);

            CloneTemplate cloneTemplate = new CloneTemplate(clonePrefab.Info, TechType.Quartz)
            {
                ModifyPrefab = obj =>
                {
                    obj.SetActive(false);
                    // Disable the old model
                    GameObject modelGameObject = obj.GetComponentInChildren<MeshRenderer>(true).gameObject;
                    modelGameObject.SetActive(false);

                    GameObject newModel = Object.Instantiate(dnaModelPrefab);
                    newModel.name = "newmodel";
                    // Add new model
                    newModel.transform.SetParent(obj.transform);
                    newModel.transform.localPosition = new Vector3(0, 0, 0);
                    newModel.transform.localRotation = new Quaternion(0, 0, 0, 0);

                    // obj.FindChild("Quartz_small").SetActive(false);

                    MaterialUtils.ApplySNShaders(newModel);

                    // Configure Prefab
                    PrefabUtils.AddBasicComponents(obj, clonePrefab.Info.ClassID, clonePrefab.Info.TechType, LargeWorldEntity.CellLevel.VeryFar);
                    PrefabUtils.AddResourceTracker(obj, TechType.None);
                    PrefabConfigUtils.SetMeshRenderersColor(newModel, "Ends", color);
                    PrefabConfigUtils.AddRotateModel(newModel, "DNA");
                    // PrefabConfigUtils.AddRigidBody(obj);
                    // PrefabConfigUtils.AddFreezeOnSettle(obj);
                    // PrefabConfigUtils.AddDnaCapsuleCollider(obj);
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

            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal("PetDna", Story.GoalType.Encyclopedia, CatDnaPrefab.Info.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal("PetDna", Story.GoalType.Encyclopedia, AlienRobotDnaPrefab.Info.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal("PetDna", Story.GoalType.Encyclopedia, BloodCrawlerDnaPrefab.Info.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal("PetDna", Story.GoalType.Encyclopedia, CaveCrawlerDnaPrefab.Info.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal("PetDna", Story.GoalType.Encyclopedia, CrabSquidDnaPrefab.Info.TechType);

        }
    }
}
