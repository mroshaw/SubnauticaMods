#if SUBNAUTICA
using DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets.Subnautica;
#endif
#if SUBNAUTICAZERO
using DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets.BelowZero;
#endif
using System.Collections.Generic;
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using UnityEngine;
using Nautilus.Assets.Gadgets;
using Nautilus.Handlers;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

using DaftAppleGames.SubnauticaPets.MonoBehaviours;
using Nautilus.Utility;

namespace DaftAppleGames.SubnauticaPets.CustomObjects
{
    /// <summary>
    /// Static class for creating the "Pet DNA" collectable objects
    /// </summary>
    internal class PetDnaPrefab
    {
        // Static references for consumers
        public static string DnaModelObjectName = "DNAModel";
        public static GameObject DnaModelPrefab = null;

        /// <summary>
        /// Register all prefabs
        /// </summary>
        public static void InitPetPrefabs()
        {
            // Get and init the DNA model prefab
            SetDnaModelObjectPrefab();
#if SUBNAUTICA
            // Cave Crawler
            CaveCrawlerPet.DnaBuildablePrefabInfo = InitPrefab(CaveCrawlerPet.DnaClassId,
                null,
                null,
                CaveCrawlerPet.DnaTextureName,
                CaveCrawlerPet.PetObjectColor,
                CaveCrawlerPet.LootDistributionBiomeData);
            // Blood Crawler
            BloodCrawlerPet.DnaBuildablePrefabInfo = InitPrefab(BloodCrawlerPet.DnaClassId,
                null,
                null,
                BloodCrawlerPet.DnaTextureName,
                BloodCrawlerPet.PetObjectColor,
                BloodCrawlerPet.LootDistributionBiomeData);
            // Crab Squid
            CrabSquidPet.DnaBuildablePrefabInfo = InitPrefab(CrabSquidPet.DnaClassId,
                null,
                null,
                CrabSquidPet.DnaTextureName,
                CrabSquidPet.PetObjectColor,
                CrabSquidPet.LootDistributionBiomeData);
            // Alien Robot
            AlienRobotPet.DnaBuildablePrefabInfo = InitPrefab(AlienRobotPet.DnaClassId,
                null,
                null,
                AlienRobotPet.DnaTextureName,
                AlienRobotPet.PetObjectColor,
                AlienRobotPet.LootDistributionBiomeData);
#endif
#if SUBNAUTICAZERO
            // Snow Stalker Baby
            SnowStalkerBabyPet.DnaBuildablePrefabInfo = InitPrefab(SnowStalkerBabyPet.DnaClassId,
                null,
                null,
                SnowStalkerBabyPet.DnaTextureName,
                SnowStalkerBabyPet.PetObjectColor,
                SnowStalkerBabyPet.LootDistributionBiomeData);
            // Pengling Adult
            PenglingAdultPet.DnaBuildablePrefabInfo = InitPrefab(PenglingAdultPet.DnaClassId,
                null,
                null,
                PenglingAdultPet.DnaTextureName,
                PenglingAdultPet.PetObjectColor,
                PenglingAdultPet.LootDistributionBiomeData);
            // Pengling Baby
            PenglingBabyPet.DnaBuildablePrefabInfo = InitPrefab(PenglingBabyPet.DnaClassId,
                null,
                null,
                PenglingBabyPet.DnaTextureName,
                PenglingBabyPet.PetObjectColor,
                PenglingBabyPet.LootDistributionBiomeData);
            // Pinnicarid
            PinnicaridPet.DnaBuildablePrefabInfo = InitPrefab(PinnicaridPet.DnaClassId,
                null,
                null,
                PinnicaridPet.DnaTextureName,
                PinnicaridPet.PetObjectColor,
                PinnicaridPet.LootDistributionBiomeData);
            // Yellow Trivalve
            TrivalveYellowPet.DnaBuildablePrefabInfo = InitPrefab(TrivalveYellowPet.DnaClassId,
                null,
                null,
                TrivalveYellowPet.DnaTextureName,
                TrivalveYellowPet.PetObjectColor,
                TrivalveYellowPet.LootDistributionBiomeData);
            // Blue Trivalve
            TrivalveBluePet.DnaBuildablePrefabInfo = InitPrefab(TrivalveBluePet.DnaClassId,
                null,
                null,
                TrivalveBluePet.DnaTextureName,
                TrivalveBluePet.PetObjectColor,
                TrivalveBluePet.LootDistributionBiomeData);
#endif
            SetFixedSpawns();
        }

        /// <summary>
        /// Generic method to set up the DNA prefabs
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="displayName"></param>
        /// <param name="description"></param>
        /// <param name="sprite"></param>
        /// <param name="color"></param>
        /// <param name="lootBiome"></param>
        /// <returns></returns>
        private static PrefabInfo InitPrefab(string classId, string displayName, string description, string textureName,
            Color color, LootDistributionData.BiomeData lootBiome)
        {
            Log.LogDebug($"PetDnaPrefab: Init Prefab for {classId}...");

            CustomPrefab clonePrefab = new CustomPrefab(
               classId,
               displayName,
               description,
               ModUtils.GetSpriteFromAssetBundle(textureName)
            );

            PrefabTemplate cloneTemplate = new CloneTemplate(clonePrefab.Info, TechType.Titanium)
            {
                ModifyPrefab = prefab =>
                {
                    Log.LogDebug($"PetDnaPrefab: InitPrefab is setting the model for {clonePrefab.Info.ClassID}... using {DnaModelPrefab.name}");
                    GameObject newModel = Object.Instantiate(DnaModelPrefab);
                    // Add new model
                    Log.LogDebug($"PetDnaPrefab: InitPrefab is setting the model for {prefab.name} to {newModel.name}...");
                    newModel.transform.SetParent(prefab.transform);
                    newModel.transform.localPosition = new Vector3(0, 0, 0);
                    newModel.transform.localRotation = new Quaternion(0, 0, 0, 0);
                    // Set model color
                    newModel.FindChild("Ends").GetComponent<MeshRenderer>().material.color = color;
                    // Add the rotate script to the DNA model
                    RotateModel rotateModel = newModel.FindChild("DNA").AddComponent<RotateModel>();
                    rotateModel.RotationSpeed = 0.01f;
                    // Resize
                    prefab.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    // Disable the old model
                    prefab.FindChild("model").SetActive(false);
                    Log.LogDebug($"PetDnaPrefab: InitPrefab is setting the model for {prefab.name} to {newModel.name}. Done.");
                    // Add PetDna component
                    Log.LogDebug($"PetDnaPrefab: InitPrefab adding PetDna component...");
                    prefab.AddComponent<PetDna>();
                    Log.LogDebug($"PetDnaPrefab: InitPrefab adding PetDna component... Done.");
                }
            };
            clonePrefab.SetGameObject(cloneTemplate);
            clonePrefab.SetSpawns(lootBiome);
            Log.LogDebug($"PetDnaPrefab: Registering {classId}...");
            clonePrefab.Register();
            Log.LogDebug($"PetDnaPrefab: Init Prefab for {classId}. Done.");
            return clonePrefab.Info;
        }

        /// <summary>
        /// Get's and inits the DNA model object from the Asset Bundle
        /// </summary>
        /// <returns></returns>
        private static void SetDnaModelObjectPrefab()
        {
            Log.LogDebug("PetDnaPrefab: GetDnaModelObjectPrefab is locating model in Asset Bundle...");
            GameObject prefab = ModUtils.GetGameObjectPrefabFromAssetBundle(DnaModelObjectName);
            if (prefab == null)
            {
                Log.LogDebug($"PetDnaPrefab: GetDnaModelObjectPrefab couldn't find {DnaModelObjectName}");
                return;
            }
            Log.LogDebug("PetDnaPrefab: GetDnaModelObjectPrefab is updating material shader properties...");
            MaterialUtils.ApplySNShaders(prefab);
            DnaModelPrefab = prefab;
            Log.LogDebug("PetDnaPrefab: GetDnaModelObjectPrefab is done.");
        }

        /// <summary>
        /// Spawns Pet DNA in some fixed locations
        /// </summary>
        public static void SetFixedSpawns()
        {
            Log.LogDebug("PetDnaPrefab: SetFixedSpawns adding coordinated spawns...");
            List<SpawnInfo> spawnInfos = new List<SpawnInfo>()
            {
#if SUBNAUTICA
                new SpawnInfo(CaveCrawlerPet.DnaClassId, new Vector3(-131.93f, -19.82f, -246.48f),
                    Vector3.up * 90f),
                new SpawnInfo(AlienRobotPet.DnaClassId, new Vector3(-132.63f, -19.82f, -246.48f),
                Vector3.up * 90f),
                new SpawnInfo(BloodCrawlerPet.DnaClassId, new Vector3(-131.93f, -19.32f, -246.48f),
                    Vector3.up * 90f),
                new SpawnInfo(CrabSquidPet.DnaClassId, new Vector3(-132.63f, -19.32f, -246.48f),
                    Vector3.up * 90f)

#endif
#if SUBNAUTICAZERO
                new SpawnInfo(SnowStalkerBabyPet.DnaClassId, new Vector3(617.56f, -178.31f, -456.59f),
                    Vector3.up * 90f),
                new SpawnInfo(PenglingAdultPet.DnaClassId, new Vector3(616.76f, -177.51f, -455.89f),
                    Vector3.up * 90f),
                new SpawnInfo(PenglingBabyPet.DnaClassId, new Vector3(617.56f, -177.51f, -456.59f),
                    Vector3.up * 90f),
                new SpawnInfo(PinnicaridPet.DnaClassId, new Vector3(616.76f, -177.51f, -455.89f),
                    Vector3.up * 90f),
                new SpawnInfo(TrivalveYellowPet.DnaClassId, new Vector3(618.56f, -178.31f, -456.59f),
                    Vector3.up * 90f),
                new SpawnInfo(TrivalveBluePet.DnaClassId, new Vector3(618.76f, -177.51f, -455.89f),
                    Vector3.up * 90f),
#endif
            };
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawns(spawnInfos);
            Log.LogDebug("PetDnaPrefab: SetFixedSpawns adding coordinated spawns... Done.");
        }
    }
}
