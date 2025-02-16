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

            PengwingAdultDnaPrefab.Register(dnaModelPrefab);
            PenglingBabyDnaPrefab.Register(dnaModelPrefab);
            PinnacaridDnaPrefab.Register(dnaModelPrefab);
            SnowstalkerBabyDnaPrefab.Register(dnaModelPrefab);
            TrivalveBlueDnaPrefab.Register(dnaModelPrefab);
            TrivalveYellowDnaPrefab.Register(dnaModelPrefab);
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

                        new LootDistributionData.BiomeData { biome = BiomeType.ArcticKelp_SeamonkeyNest2, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.ArcticKelp_SeamonkeyNest4, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_Crevice_SeamonkeyNest2, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_Crevice_SeamonkeyNest4, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_ShipWreck_Open, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.MargArea_Ground, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.PurpleVents_ShipWreck_Ground, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.ShipWreck1_Open, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.ShipWreck3_Open, count = FindCount, probability = FindProbability},


                    }, dnaModelGameObject);
            }
        }
        /// <summary>
        /// Pengwing Adult DNA
        /// </summary>
        public static class PengwingAdultDnaPrefab
        {
            public static PrefabInfo Info;
            private const int FindCount = 1;
            private const float FindProbability = 0.6f;

            /// <summary>
            /// Registers Pengwing Adult DNA
            /// </summary>
            /// <param name="dnaModelGameObject"></param>
            public static void Register(GameObject dnaModelGameObject)
            {
                Info = RegisterDnaPrefab("PengwingAdultPetDna", null, null, "PengwingAdultDnaStrandTexture", Color.grey,
                    new LootDistributionData.BiomeData[] {
                        new LootDistributionData.BiomeData { biome = BiomeType.ArcticKelp_SeamonkeyNest1, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.ArcticKelp_SeamonkeyNest3, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.ArcticKelp_SeamonkeyNest5, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_Crevice_SeamonkeyNest1, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_Crevice_SeamonkeyNest3, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_Crevice_SeamonkeyNest5, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_ShipWreck_Ground, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.PurpleVents_ShipWreck_Open, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.ShipWreck2_Open, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.ShipWreck3_Open, count = FindCount, probability = FindProbability},

                    }, dnaModelGameObject);
            }
        }

        /// <summary>
        /// Pengling Baby DNA
        /// </summary>
        public static class PenglingBabyDnaPrefab
        {
            public static PrefabInfo Info;
            private const int FindCount = 1;
            private const float FindProbability = 0.3f;

            /// <summary>
            /// Register Pengling Baby DNA
            /// </summary>
            /// <param name="dnaModelGameObject"></param>
            public static void Register(GameObject dnaModelGameObject)
            {
                Info = RegisterDnaPrefab("PenglingBabyPetDna", null, null, "PenglingBabyDnaStrandTexture", Color.magenta,
                    new LootDistributionData.BiomeData[] {
                        new LootDistributionData.BiomeData { biome = BiomeType.ArcticKelp_SeamonkeyNest1, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.ArcticKelp_SeamonkeyNest2, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.ArcticKelp_SeamonkeyNest4, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.GlacialBasin_BikeCrashSite, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_Crevice_SeamonkeyNest2, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_Crevice_SeamonkeyNest4, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_Crevice_SeamonkeyNest5, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_ShipWreck_Open, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.MargArea_Ground, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.PurpleVents_ShipWreck_Ground, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.ShipWreck1_Open, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.ShipWreck3_Open, count = FindCount, probability = FindProbability},

                    }, dnaModelGameObject);
            }
        }

        /// <summary>
        /// Snowstalker Baby DNA
        /// </summary>
        public static class SnowstalkerBabyDnaPrefab
        {
            public static PrefabInfo Info;
            private const int FindCount = 1;
            private const float FindProbability = 0.3f;

            /// <summary>
            /// Register Snowstalker Baby DNA
            /// </summary>
            /// <param name="dnaModelGameObject"></param>
            public static void Register(GameObject dnaModelGameObject)
            {
                Info = RegisterDnaPrefab("SnowstalkerBabyPetDna", null, null, "SnowstalkerBabyDnaStrandTexture", Color.white,
                    new LootDistributionData.BiomeData[] {
                        new LootDistributionData.BiomeData { biome = BiomeType.ArcticKelp_SeamonkeyNest1, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.ArcticKelp_SeamonkeyNest3, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.ArcticKelp_SeamonkeyNest5, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.GlacialBasin_BikeCrashSite, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_Crevice_SeamonkeyNest1, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_Crevice_SeamonkeyNest3, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_Crevice_SeamonkeyNest5, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_ShipWreck_Ground, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.PurpleVents_ShipWreck_Open, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.ShipWreck2_Open, count = FindCount, probability = FindProbability},


                    }, dnaModelGameObject);
            }
        }

        /// <summary>
        /// Trivalve Blue DNA
        /// </summary>
        public static class TrivalveBlueDnaPrefab
        {
            public static PrefabInfo Info;
            private const int FindCount = 1;
            private const float FindProbability = 0.3f;

            /// <summary>
            /// Register Trivalve Blue DNA
            /// </summary>
            /// <param name="dnaModelGameObject"></param>
            public static void Register(GameObject dnaModelGameObject)
            {
                Info = RegisterDnaPrefab("TrivalveBluePetDna", null, null, "TrivalveBlueDnaStrandTexture", Color.blue,
                    new LootDistributionData.BiomeData[] {
                        new LootDistributionData.BiomeData { biome = BiomeType.ArcticKelp_SeamonkeyNest1, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.ArcticKelp_SeamonkeyNest2, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.ArcticKelp_SeamonkeyNest4, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.GlacialBasin_BikeCrashSite, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_Crevice_SeamonkeyNest1, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_Crevice_SeamonkeyNest2, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_Crevice_SeamonkeyNest4, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_ShipWreck_Open, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.MargArea_Ground, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.PurpleVents_ShipWreck_Ground, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.ShipWreck1_Open, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.ShipWreck3_Open, count = FindCount, probability = FindProbability},


                    }, dnaModelGameObject);
            }
        }

        /// <summary>
        /// Trivalve Yellow DNA
        /// </summary>
        public static class TrivalveYellowDnaPrefab
        {
            public static PrefabInfo Info;
            private const int FindCount = 1;
            private const float FindProbability = 0.3f;

            /// <summary>
            /// Register Trivalve Yellow DNA
            /// </summary>
            /// <param name="dnaModelGameObject"></param>
            public static void Register(GameObject dnaModelGameObject)
            {
                Info = RegisterDnaPrefab("TrivalveYellowPetDna", null, null, "TrivalveYellowDnaStrandTexture", Color.yellow,
                    new LootDistributionData.BiomeData[] {
                        new LootDistributionData.BiomeData { biome = BiomeType.ArcticKelp_SeamonkeyNest1, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.ArcticKelp_SeamonkeyNest3, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.ArcticKelp_SeamonkeyNest5, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.GlacialBasin_BikeCrashSite, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_Crevice_SeamonkeyNest1, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_Crevice_SeamonkeyNest3, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_Crevice_SeamonkeyNest5, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_ShipWreck_Ground, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.MargArea_Ground, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.PurpleVents_ShipWreck_Open, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.ShipWreck2_Open, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.ShipWreck3_Open, count = FindCount, probability = FindProbability},

                    }, dnaModelGameObject);
            }
        }

        /// <summary>
        /// Pinnacarid DNA
        /// </summary>
        public static class PinnacaridDnaPrefab
        {
            public static PrefabInfo Info;
            private const int FindCount = 1;
            private const float FindProbability = 0.3f;

            /// <summary>
            /// Register Pinnacarid DNA
            /// </summary>
            /// <param name="dnaModelGameObject"></param>
            public static void Register(GameObject dnaModelGameObject)
            {
                Info = RegisterDnaPrefab("PinnacaridPetDna", null, null, "PinnacaridDnaStrandTexture", Color.blue,
                    new LootDistributionData.BiomeData[] {
                        new LootDistributionData.BiomeData { biome = BiomeType.ArcticKelp_SeamonkeyNest2, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.ArcticKelp_SeamonkeyNest3, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.ArcticKelp_SeamonkeyNest5, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.GlacialBasin_BikeCrashSite, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_Crevice_SeamonkeyNest1, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_Crevice_SeamonkeyNest3, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_Crevice_SeamonkeyNest5, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.LilyPads_ShipWreck_Ground, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.PurpleVents_ShipWreck_Open, count = FindCount, probability = FindProbability},
                        new LootDistributionData.BiomeData { biome = BiomeType.ShipWreck2_Open, count = FindCount, probability = FindProbability},

                    }, dnaModelGameObject);
            }
        }


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
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal("PetDna", Story.GoalType.Encyclopedia, PengwingAdultDnaPrefab.Info.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal("PetDna", Story.GoalType.Encyclopedia, PenglingBabyDnaPrefab.Info.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal("PetDna", Story.GoalType.Encyclopedia, PinnacaridDnaPrefab.Info.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal("PetDna", Story.GoalType.Encyclopedia, SnowstalkerBabyDnaPrefab.Info.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal("PetDna", Story.GoalType.Encyclopedia, TrivalveYellowDnaPrefab.Info.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal("PetDna", Story.GoalType.Encyclopedia, TrivalveBlueDnaPrefab.Info.TechType);
        }
    }
}
