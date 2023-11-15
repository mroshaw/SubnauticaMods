#if SUBNAUTICA
using DaftAppleGames.SubnauticaPets.Mono.Pets.Subnautica;
using static CraftData;
#endif
#if SUBNAUTICAZERO
using DaftAppleGames.SubnauticaPets.Mono.Pets.BelowZero;
#endif
using System;

using DaftAppleGames.SubnauticaPets.Mono.BaseParts;
using DaftAppleGames.SubnauticaPets.Mono.Pets;
using DaftAppleGames.SubnauticaPets.Mono.Pets.Custom;
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;
using Nautilus.Handlers;

namespace DaftAppleGames.SubnauticaPets.Prefabs
{
    /// <summary>
    /// Static utilities class for creating the new Pet Fabricator
    /// </summary>
    internal static class PetFabricatorPrefab
    {
        // Pubic PrefabInfo, for anything that needs it
        public static PrefabInfo Info;

        // Prefab Class Id
        private const string PrefabClassId = "PetFabricator";

        // Asset Bundle constants
        private const string PetFabricatorMainImageTexture = "PetFabricatorDataBankMainImageTexture";
        private const string PetFabricatorPopupImageTexture = "PetFabricatorDataBankPopupImageTexture";

        // Asset Bundle refs for icon
        private const string PetFabricatorIconTexture = "PetFabricatorIconTexture";

        // Databank key constants
        public const string PetFabricatorEncyKey = "PetFabricator";
        public const string PetFabricatorEncyPath = "Tech/Habitats";

        /// <summary>
        /// Makes the new Pet Fabricator available for use.
        /// </summary>
        public static void InitPetFabricator()
        {
            Info = PrefabInfo
                .WithTechType(PrefabClassId, null, null, unlockAtStart: false)
                .WithIcon(ModUtils.GetSpriteFromAssetBundle(PetFabricatorIconTexture));

            CustomPrefab fabricatorPrefab = new CustomPrefab(Info);
            FabricatorGadget fabGadget = fabricatorPrefab.CreateFabricator(out CraftTree.Type treeType)
                // Add our Pet Buildables
#if SUBNAUTICA
                .AddCraftNode(PetBuildablePrefab.CaveCrawlerBuildable.Info.TechType)
                .AddCraftNode(PetBuildablePrefab.BloodCrawlerBuildable.Info.TechType)
                .AddCraftNode(PetBuildablePrefab.CrabSquidBuildable.Info.TechType)
                .AddCraftNode(PetBuildablePrefab.AlienRobotBuildable.Info.TechType);
#endif
#if SUBNAUTICAZERO
                .AddCraftNode(PetBuildablePrefab.PenglingBabyBuildable.Info.TechType)
                .AddCraftNode(PetBuildablePrefab.PenglingAdultBuildable.Info.TechType)
                .AddCraftNode(PetBuildablePrefab.SnowStalkerBabyBuildable.Info.TechType)
                .AddCraftNode(PetBuildablePrefab.PinnicaridBuildable.Info.TechType)
                .AddCraftNode(PetBuildablePrefab.TrivalveYellowBuildable.Info.TechType)
                .AddCraftNode(PetBuildablePrefab.TrivalveBluePetBuildable.Info.TechType);
#endif
            // Add Cat pet, if it's been enabled.
            if (ModConfig.EnableCat)
            {
                LogUtils.LogDebug(LogArea.Prefabs, "PetFabricatorPrefab: Cat enabled, adding to Fabricator...");
                fabGadget.AddCraftNode(PetBuildablePrefab.CatBuildable.Info.TechType);
            }

            FabricatorTemplate fabPrefab = new FabricatorTemplate(Info, treeType)
            {
                FabricatorModel = FabricatorTemplate.Model.Workbench,
                ModifyPrefab = ConfigureFabComponents

            };
            fabricatorPrefab.SetGameObject(fabPrefab);

            // Define the recipe
            RecipeData recipe = new RecipeData
            {
                craftAmount = 1,
                Ingredients =
                {
                    new Ingredient(TechType.Titanium, 5),
                    new Ingredient(TechType.ComputerChip, 1),
                    new Ingredient(TechType.CopperWire, 3),
#if SUBNAUTICA
                    new Ingredient(PetDnaPrefab.CrabSquidDnaPrefab.Info.TechType, 1),
                    new Ingredient(PetDnaPrefab.AlienRobotDnaPrefab.Info.TechType, 1),
                    new Ingredient(PetDnaPrefab.BloodCrawlerDnaPrefab.Info.TechType, 1),
                    new Ingredient(PetDnaPrefab.CaveCrawlerDnaPrefab.Info.TechType, 1)
#endif
#if SUBNAUTICAZERO
                    new Ingredient(PetDnaPrefab.SnowstalkerBabyDnaPrefab.Info.TechType, 1),
                    new Ingredient(PetDnaPrefab.PenglingAdultDnaPrefab.Info.TechType, 1),
                    new Ingredient(PetDnaPrefab.PenglingBabyDnaPrefab.Info.TechType, 1),
                    new Ingredient(PetDnaPrefab.PinnicaridDnaPrefab.Info.TechType, 1),
                    new Ingredient(PetDnaPrefab.TrivalveBlueDnaPrefab.Info.TechType, 1),
                    new Ingredient(PetDnaPrefab.TrivalveYellowDnaPrefab.Info.TechType, 1),
#endif
                }
            };

            // Set the recipe
            fabricatorPrefab.SetRecipe(recipe);

            // Set up the scanning and fragment unlocks
            fabricatorPrefab.SetUnlock(Info.TechType, 3)
                .WithPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule)
                .WithAnalysisTech(ModUtils.GetSpriteFromAssetBundle(PetFabricatorPopupImageTexture), null, null)
                .WithEncyclopediaEntry(PetFabricatorEncyPath, ModUtils.GetSpriteFromAssetBundle(PetFabricatorPopupImageTexture),
                    ModUtils.GetTexture2DFromAssetBundle(PetFabricatorMainImageTexture));
            fabricatorPrefab.Register();
        }
        
        /// <summary>
        /// Set up our custom "Workbench" component, replacing the default one
        /// </summary>
        /// <param name="fabricatorGameObject"></param>
        private static void ConfigureFabComponents(GameObject fabricatorGameObject)
        {
            fabricatorGameObject.SetActive(false);

            LogUtils.LogDebug(LogArea.Prefabs, "PetFabricatorPrefab: Adding PetSpawner component...");
            PetSpawner newPetSpawner = fabricatorGameObject.AddComponent<PetSpawner>();
            LogUtils.LogDebug(LogArea.Prefabs, "PetFabricatorPrefab: Adding PetSpawner component... Done.");

            LogUtils.LogDebug(LogArea.Prefabs, "PetFabricatorPrefab: Adding PetFabricator component...");
            PetFabricator newPetFabricator = fabricatorGameObject.AddComponent<PetFabricator>();
            LogUtils.LogDebug(LogArea.Prefabs, "PetFabricatorPrefab: Adding PetFabricator component... Done.");
        }
    }
}