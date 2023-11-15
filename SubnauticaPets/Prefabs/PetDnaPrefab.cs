#if SUBNAUTICA
using DaftAppleGames.SubnauticaPets.Mono.Pets.Subnautica;
#endif
#if SUBNAUTICAZERO
using DaftAppleGames.SubnauticaPets.Mono.Pets.BelowZero;
#endif
using DaftAppleGames.SubnauticaPets.Mono.Pets;
using DaftAppleGames.SubnauticaPets.Mono.Pets.Custom;
using DaftAppleGames.SubnauticaPets.Mono.Utils;
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
    internal class PetDnaPrefab
    {
        // Static references for consumers
        public static string DnaModelObjectName = "DNASampleTube";
        public static GameObject DnaModelPrefab;
        
        // Ency keys
        private static readonly string PetDnaEncyPath = "Lifeforms/Fauna";
        private static readonly string PetDnaEncyKey = "PetDna";
        // Asset Bundle refs
        private static readonly string PetDnaMainImageTexture = "PetDnaDataBankMainImageTexture";
        private static readonly string PetDnaPopupImageTexture = "PetDnaDataBankPopupImageTexture";

        /// <summary>
        /// Register all prefabs
        /// </summary>
        public static void InitPetDnaPrefabs()
        {
            // Get and init the DNA model prefab
            SetDnaModelObjectPrefab();

            // Init DNA spawn prefabs
            CatDnaPrefab.Init();

#if SUBNAUTICA
            AlienRobotDnaPrefab.Init();
            BloodCrawlerDnaPrefab.Init();
            CaveCrawlerDnaPrefab.Init();
            CrabSquidDnaPrefab.Init();
#endif
#if SUBNAUTICAZERO
            PenglingAdultDnaPrefab.Init();
            PenglingBabyDnaPrefab.Init();
            PinnicaridDnaPrefab.Init();
            SnowstalkerBabyDnaPrefab.Init();
            TrivalveBlueDnaPrefab.Init();
            TrivalveYellowDnaPrefab.Init();
#endif
            ConfigureDataBank();
        }

        public static class CatDnaPrefab
        {
            public static PrefabInfo Info;

            public static void Init()
            {
                Info = InitPrefab(CatPet.DnaClassId,
                    null,
                    null,
                    CatPet.DnaTextureName,
                    CatPet.PetObjectColor,
                    CatPet.LootDistributionBiomeData);
            }
        }
#if SUBNAUTICAZERO
        public static class PenglingAdultDnaPrefab
        {
            public static PrefabInfo Info;

            public static void Init()
            {
                Info = InitPrefab(PenglingAdultPet.DnaClassId,
                    null,
                    null,
                    PenglingAdultPet.DnaTextureName,
                    PenglingAdultPet.PetObjectColor,
                    PenglingAdultPet.LootDistributionBiomeData);
            }
        }

        public static class PenglingBabyDnaPrefab
        {
            public static PrefabInfo Info;

            public static void Init()
            {
                Info = InitPrefab(PenglingBabyPet.DnaClassId,
                    null,
                    null,
                    PenglingBabyPet.DnaTextureName,
                    PenglingBabyPet.PetObjectColor,
                    PenglingBabyPet.LootDistributionBiomeData);
            }
        }

        public static class PinnicaridDnaPrefab
        {
            public static PrefabInfo Info;

            public static void Init()
            {
                Info = InitPrefab(PinnicaridPet.DnaClassId,
                    null,
                    null,
                    PinnicaridPet.DnaTextureName,
                    PinnicaridPet.PetObjectColor,
                    PinnicaridPet.LootDistributionBiomeData);
            }
        }

        public static class SnowstalkerBabyDnaPrefab
        {
            public static PrefabInfo Info;

            public static void Init()
            {
                Info = InitPrefab(SnowStalkerBabyPet.DnaClassId,
                    null,
                    null,
                    SnowStalkerBabyPet.DnaTextureName,
                    SnowStalkerBabyPet.PetObjectColor,
                    SnowStalkerBabyPet.LootDistributionBiomeData);
            }
        }

        public static class TrivalveBlueDnaPrefab
        {
            public static PrefabInfo Info;

            public static void Init()
            {
                Info = InitPrefab(TrivalveBluePet.DnaClassId,
                    null,
                    null,
                    TrivalveBluePet.DnaTextureName,
                    TrivalveBluePet.PetObjectColor,
                    TrivalveBluePet.LootDistributionBiomeData);
            }
        }

        public static class TrivalveYellowDnaPrefab
        {
            public static PrefabInfo Info;

            public static void Init()
            {
                Info = InitPrefab(TrivalveYellowPet.DnaClassId,
                    null,
                    null,
                    TrivalveYellowPet.DnaTextureName,
                    TrivalveYellowPet.PetObjectColor,
                    TrivalveYellowPet.LootDistributionBiomeData);
            }
        }
#endif

#if SUBNAUTICA
        public static class AlienRobotDnaPrefab
        {
            public static PrefabInfo Info;

            public static void Init()
            {
                Info = InitPrefab(AlienRobotPet.DnaClassId,
                    null,
                    null,
                    AlienRobotPet.DnaTextureName,
                    AlienRobotPet.PetObjectColor,
                    AlienRobotPet.LootDistributionBiomeData);
            }
        }

        public static class BloodCrawlerDnaPrefab
        {
            public static PrefabInfo Info;

            public static void Init()
            {
                Info = InitPrefab(BloodCrawlerPet.DnaClassId,
                    null,
                    null,
                    BloodCrawlerPet.DnaTextureName,
                    BloodCrawlerPet.PetObjectColor,
                    BloodCrawlerPet.LootDistributionBiomeData);
            }
        }

        public static class CaveCrawlerDnaPrefab
        {
            public static PrefabInfo Info;

            public static void Init()
            {
                Info = InitPrefab(CaveCrawlerPet.DnaClassId,
                    null,
                    null,
                    CaveCrawlerPet.DnaTextureName,
                    CaveCrawlerPet.PetObjectColor,
                    CaveCrawlerPet.LootDistributionBiomeData);
            }
        }

        public static class CrabSquidDnaPrefab
        {
            public static PrefabInfo Info;

            public static void Init()
            {
                Info = InitPrefab(CrabSquidPet.DnaClassId,
                    null,
                    null,
                    CrabSquidPet.DnaTextureName,
                    CrabSquidPet.PetObjectColor,
                    CrabSquidPet.LootDistributionBiomeData);
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
        /// <returns></returns>
        private static PrefabInfo InitPrefab(string classId, string displayName, string description, string textureName,
            Color color, LootDistributionData.BiomeData[] lootBiome)
        {
            LogUtils.LogDebug(LogArea.Prefabs, $"PetDnaPrefab: Init Prefab for {classId}...");

            PrefabInfo prefabInfo = PrefabInfo
                .WithTechType(classId, displayName, description, unlockAtStart: true)
                .WithIcon(ModUtils.GetSpriteFromAssetBundle(textureName));

            CustomPrefab clonePrefab = new CustomPrefab(prefabInfo);

            CloneTemplate cloneTemplate = new CloneTemplate(clonePrefab.Info, TechType.Titanium)
            {
                ModifyPrefab = prefab =>
                {
                    LogUtils.LogDebug(LogArea.Prefabs, $"PetDnaPrefab: InitPrefab is setting the model for {clonePrefab.Info.ClassID}... using {DnaModelPrefab.name}");
                    GameObject newModel = Object.Instantiate(DnaModelPrefab);
                    newModel.name = "newmodel";
                    // Add new model
                    LogUtils.LogDebug(LogArea.Prefabs, $"PetDnaPrefab: InitPrefab is setting the model for {prefab.name} to {newModel.name}...");
                    newModel.transform.SetParent(prefab.transform);
                    newModel.transform.localPosition = new Vector3(0, 0, 0);
                    newModel.transform.localRotation = new Quaternion(0, 0, 0, 0);

                    MaterialUtils.ApplySNShaders(newModel);
                    // Set model color
                    newModel.FindChild("Ends").GetComponent<MeshRenderer>().material.color = color;

                    GameObject dnaGameObject = newModel.transform.Find("DNA").gameObject;
                    RotateModel rotateModel = dnaGameObject.AddComponent<RotateModel>();
                    rotateModel.RotationSpeed = 0.1f;

                    PrefabUtils.AddBasicComponents(prefab, clonePrefab.Info.ClassID, clonePrefab.Info.TechType, LargeWorldEntity.CellLevel.Medium);
                    ResourceTracker resourceTracker = PrefabUtils.AddResourceTracker(prefab, TechType.None);

                    // Disable the old model
                    prefab.FindChild("model").SetActive(false);
                    LogUtils.LogDebug(LogArea.Prefabs, $"PetDnaPrefab: InitPrefab is setting the model for {prefab.name} to {newModel.name}. Done.");
                    // Add PetDna component
                    LogUtils.LogDebug(LogArea.Prefabs, "PetDnaPrefab: InitPrefab adding PetDna component...");
                    prefab.AddComponent<PetDna>();
                    LogUtils.LogDebug(LogArea.Prefabs, "PetDnaPrefab: InitPrefab adding PetDna component... Done.");
                }
            };
            clonePrefab.SetGameObject(cloneTemplate);
            clonePrefab.SetSpawns(lootBiome);
            LogUtils.LogDebug(LogArea.Prefabs, $"PetDnaPrefab: Registering {classId}...");
            clonePrefab.Register();
            LogUtils.LogDebug(LogArea.Prefabs, $"PetDnaPrefab: Init Prefab for {classId}. Done.");
            return prefabInfo;
        }

        /// <summary>
        /// Get's and inits the DNA model object from the Asset Bundle
        /// </summary>
        /// <returns></returns>
        private static void SetDnaModelObjectPrefab()
        {
            LogUtils.LogDebug(LogArea.Prefabs, "PetDnaPrefab: GetDnaModelObjectPrefab is locating model in Asset Bundle...");
            GameObject prefab = ModUtils.GetGameObjectPrefabFromAssetBundle(DnaModelObjectName);
            if (prefab == null)
            {
                LogUtils.LogDebug(LogArea.Prefabs, $"PetDnaPrefab: GetDnaModelObjectPrefab couldn't find {DnaModelObjectName}");
                return;
            }
            LogUtils.LogDebug(LogArea.Prefabs, "PetDnaPrefab: GetDnaModelObjectPrefab is updating material shader properties...");
            MaterialUtils.ApplySNShaders(prefab);
            DnaModelPrefab = prefab;
            LogUtils.LogDebug(LogArea.Prefabs, "PetDnaPrefab: GetDnaModelObjectPrefab is done.");
        }

        /// <summary>
        /// Adds all DataBank entries
        /// </summary>
        public static void ConfigureDataBank()
        {
            LogUtils.LogDebug(LogArea.Prefabs, "PetDatabankEntries: Setting up Databank...");
            // Pet DNA
            ModUtils.ConfigureDatabankEntry(PetDnaEncyKey, PetDnaEncyPath, PetDnaMainImageTexture, PetDnaPopupImageTexture);
            SetDnaPickupGoals(PetDnaEncyKey);
            LogUtils.LogDebug(LogArea.Prefabs, "PetDatabankEntries: Setting up Databank... Done.");
        }

        /// <summary>
        /// Sets up goals based on collection of DNA samples
        /// </summary>
        /// <param name="encyKey"></param>
        private static void SetDnaPickupGoals(string encyKey)
        {
            LogUtils.LogDebug(LogArea.Prefabs, "DatabankEntries: Setting up ItemGoals...");
#if SUBNAUTICA
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal(encyKey, Story.GoalType.Encyclopedia, AlienRobotDnaPrefab.Info.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal(encyKey, Story.GoalType.Encyclopedia, BloodCrawlerDnaPrefab.Info.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal(encyKey, Story.GoalType.Encyclopedia, CaveCrawlerDnaPrefab.Info.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal(encyKey, Story.GoalType.Encyclopedia, CrabSquidDnaPrefab.Info.TechType);
#endif
#if SUBNAUTICAZERO
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal(encyKey, Story.GoalType.Encyclopedia, PenglingAdultDnaPrefab.Info.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal(encyKey, Story.GoalType.Encyclopedia, PenglingBabyDnaPrefab.Info.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal(encyKey, Story.GoalType.Encyclopedia, PinnicaridDnaPrefab.Info.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal(encyKey, Story.GoalType.Encyclopedia, SnowstalkerBabyDnaPrefab.Info.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal(encyKey, Story.GoalType.Encyclopedia, TrivalveYellowDnaPrefab.Info.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal(encyKey, Story.GoalType.Encyclopedia, TrivalveBlueDnaPrefab.Info.TechType);
#endif
            LogUtils.LogDebug(LogArea.Prefabs, "DatabankEntries: Setting up ItemGoals... Done.");
        }

    }
}
