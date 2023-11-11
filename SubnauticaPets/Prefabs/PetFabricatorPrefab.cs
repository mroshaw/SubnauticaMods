#if SUBNAUTICA
using DaftAppleGames.SubnauticaPets.Mono.Pets.Subnautica;
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
using static CraftData;
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
        public static PrefabInfo PrefabInfo;

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
            PrefabInfo fabricatorPrefabInfo = PrefabInfo
                .WithTechType(PrefabClassId, null, null, unlockAtStart: false)
                .WithIcon(ModUtils.GetSpriteFromAssetBundle(PetFabricatorIconTexture));
            
            CustomPrefab fabricatorPrefab = new CustomPrefab(fabricatorPrefabInfo);
            FabricatorGadget fabGadget = fabricatorPrefab.CreateFabricator(out CraftTree.Type treeType)
                // Add our Pet Buildables
#if SUBNAUTICA
                .AddCraftNode(CaveCrawlerPet.BuildablePrefabInfo.TechType)
                .AddCraftNode(BloodCrawlerPet.BuildablePrefabInfo.TechType)
                .AddCraftNode(CrabSquidPet.BuildablePrefabInfo.TechType)
                .AddCraftNode(AlienRobotPet.BuildablePrefabInfo.TechType);
#endif
#if SUBNAUTICAZERO
                .AddCraftNode(PenglingBabyPet.BuildablePrefabInfo.TechType)
                .AddCraftNode(PenglingAdultPet.BuildablePrefabInfo.TechType)
                .AddCraftNode(SnowStalkerBabyPet.BuildablePrefabInfo.TechType)
                .AddCraftNode(PinnicaridPet.BuildablePrefabInfo.TechType)
                .AddCraftNode(TrivalveYellowPet.BuildablePrefabInfo.TechType)
                .AddCraftNode(TrivalveBluePet.BuildablePrefabInfo.TechType);
#endif
            // Add Cat pet, if it's been enabled.
            if (ModConfig.EnableCat)
            {
                Log.LogDebug("PetFabricatorPrefab: Cat enabled, adding to Fabricator...");
                fabGadget.AddCraftNode(CatPet.BuildablePrefabInfo.TechType);
            }

            FabricatorTemplate fabPrefab = new FabricatorTemplate(fabricatorPrefab.Info, treeType)
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
                    new Ingredient(CrabSquidPet.DnaBuildablePrefabInfo.TechType, 1),
                    new Ingredient(AlienRobotPet.DnaBuildablePrefabInfo.TechType, 1),
                    new Ingredient(BloodCrawlerPet.DnaBuildablePrefabInfo.TechType, 1),
                    new Ingredient(CaveCrawlerPet.DnaBuildablePrefabInfo.TechType, 1)
#endif
#if SUBNAUTICAZERO
                    new Ingredient(SnowStalkerBabyPet.DnaBuildablePrefabInfo.TechType, 1),
                    new Ingredient(PenglingAdultPet.DnaBuildablePrefabInfo.TechType, 1),
                    new Ingredient(PenglingBabyPet.DnaBuildablePrefabInfo.TechType, 1),
                    new Ingredient(PinnicaridPet.DnaBuildablePrefabInfo.TechType, 1),
                    new Ingredient(TrivalveBluePet.DnaBuildablePrefabInfo.TechType, 1),
                    new Ingredient(TrivalveYellowPet.DnaBuildablePrefabInfo.TechType, 1),
#endif
                }
            };

            // Set the recipe
            fabricatorPrefab.SetRecipe(recipe);

            // Set up databank
            SetupDatabank();

            // Set up the scanning and fragment unlocks
            fabricatorPrefab.SetUnlock(PetFabricatorFragmentPrefab.PrefabInfo.TechType, 3)
                .WithPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule)
                .WithAnalysisTech(ModUtils.GetSpriteFromAssetBundle(PetFabricatorPopupImageTexture), null, null);
            fabricatorPrefab.Register();
            
            PrefabInfo = fabricatorPrefab.Info;
        }

        /// <summary>
        /// Set up Databank Entries
        /// </summary>
        private static void SetupDatabank()
        {
            // Set up Databank
            Log.LogDebug("PetConsoleFragmentPrefab: Setting up Databank entry...");
            PDAHandler.AddEncyclopediaEntry(PetFabricatorEncyKey, PetFabricatorEncyPath, null, null,
                ModUtils.GetTexture2DFromAssetBundle(PetFabricatorMainImageTexture),
                ModUtils.GetSpriteFromAssetBundle(PetFabricatorPopupImageTexture));
            Log.LogDebug("PetConsoleFragmentPrefab: Setting up Databank entry... Done.");
        }

        /// <summary>
        /// Set up our custom "Workbench" component, replacing the default one
        /// </summary>
        /// <param name="fabricatorGameObject"></param>
        private static void ConfigureFabComponents(GameObject fabricatorGameObject)
        {
            Log.LogDebug("PetFabricatorUtils: Adding PetSpawner component...");
            PetSpawner newPetSpawner = fabricatorGameObject.AddComponent<PetSpawner>();
            Log.LogDebug("PetFabricatorUtils: Adding PetSpawner component... Done.");

            Log.LogDebug("PetFabricatorUtils: Adding PetFabricator component...");
            PetFabricator newPetFabricator = fabricatorGameObject.AddComponent<PetFabricator>();
            Log.LogDebug("PetFabricatorUtils: Adding PetFabricator component... Done.");
        }
    }
}