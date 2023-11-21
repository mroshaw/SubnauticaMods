using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
#if SUBNAUTICA
using Ingredient = CraftData.Ingredient;
#endif
using DaftAppleGames.SubnauticaPets.Mono.BaseParts;
using DaftAppleGames.SubnauticaPets.Mono.Pets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DaftAppleGames.SubnauticaPets.Prefabs
{
    /// <summary>
    /// Static utilities class for common functions and properties to be used within your mod code
    /// </summary>
    internal static class BaseModulePrefabs
    {
        /// <summary>
        /// Register all base modules
        /// </summary>
        public static void RegisterAll()
        {
            PetConsolePrefab.Register();
            PetFabricatorPrefab.Register();
        }

        /// <summary>
        /// Pet Console Prefab
        /// </summary>
        internal static class PetConsolePrefab
        {
            public static PrefabInfo Info;

            /// <summary>
            /// Register Pet Console
            /// </summary>
            public static void Register()
            {
                Info = PrefabInfo
                    .WithTechType("PetConsole", null, null, unlockAtStart: false)
                    .WithIcon(ModUtils.GetSpriteFromAssetBundle("PetConsoleIconTexture"));
                CustomPrefab consolePrefab = new CustomPrefab(Info);

                // We'll use the PictureFrame as a template
                PrefabTemplate consoleTemplate = new CloneTemplate(consolePrefab.Info, TechType.PictureFrame)
                {
                    // Reconfigure the prefab, once it's been created
                    ModifyPrefab = obj =>
                    {
                        obj.SetActive(false);
                        ModUtils.ApplyNewMeshTexture(obj, "PetConsoleTexture", "submarine_Picture_Frame");

                        obj.DestroyComponentsInChildren<PictureFrame>();

                        // Set up the UI
                        PetConsole petConsole = obj.AddComponent<PetConsole>();

                        UiUtils.CreatePetConsoleUi(obj, out Button renameButton, out Button killButton,
                            out Button killAllButton,
                            out Button killAllConfirmButton, out TMP_InputField petNameTextInput,
                            out GameObject petsScrollViewContent, out Button petListButtonTemplate);
                        petConsole.renameButton = renameButton;
                        petConsole.killButton = killButton;
                        petConsole.killAllButton = killAllButton;
                        petConsole.killAllConfirmButton = killAllConfirmButton;
                        petConsole.petNameTextInput = petNameTextInput;
                        petConsole.petListButtonTemplate = petListButtonTemplate;
                        petConsole.petsScrollViewContent = petsScrollViewContent;
                    }
                };
                consolePrefab.SetGameObject(consoleTemplate);

                // Define the recipe for the new Console
                RecipeData recipe = new RecipeData
                {
                    craftAmount = 1,
                    Ingredients =
                    {
                        new Ingredient(TechType.Titanium, 3),
                        new Ingredient(TechType.ComputerChip, 1),
                        new Ingredient(TechType.CopperWire, 2),
                        new Ingredient(TechType.Glass, 1)
                    }
                };

                // Set the recipe.
                consolePrefab.SetRecipe(recipe);

                consolePrefab.SetUnlock(Info.TechType)
                    .WithAnalysisTech(ModUtils.GetSpriteFromAssetBundle("PetConsoleDataBankPopupImageTexture"), null,
                        null)
                    .WithPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule)
                    .WithEncyclopediaEntry("Tech/Habitats",
                        ModUtils.GetSpriteFromAssetBundle("PetConsoleDataBankPopupImageTexture"),
                        ModUtils.GetTexture2DFromAssetBundle("PetConsoleDataBankMainImageTexture"));

                consolePrefab.Register();
            }
        }

        /// <summary>
        /// Static utilities class for creating the new Pet Fabricator
        /// </summary>
        internal static class PetFabricatorPrefab
        {
            // Pubic PrefabInfo, for anything that needs it
            public static PrefabInfo Info;

            /// <summary>
            /// Makes the new Pet Fabricator available for use.
            /// </summary>
            public static void Register()
            {
                Info = PrefabInfo
                    .WithTechType("PetFabricator", null, null, unlockAtStart: false)
                    .WithIcon(ModUtils.GetSpriteFromAssetBundle("PetFabricatorIconTexture"));

                CustomPrefab fabricatorPrefab = new CustomPrefab(Info);

                FabricatorGadget fabGadget = fabricatorPrefab.CreateFabricator(out CraftTree.Type treeType)
#if SUBNAUTICA
                    .AddCraftNode(PetPrefabs.AlienRobotPrefab.Info.TechType)
                    .AddCraftNode(PetPrefabs.BloodCrawlerPrefab.Info.TechType)
                    .AddCraftNode(PetPrefabs.CaveCrawlerPrefab.Info.TechType)
                    .AddCraftNode(PetPrefabs.CrabSquidPrefab.Info.TechType);
#endif
#if SUBNAUTICAZERO
                    .AddCraftNode(PetPrefabs.PenglingBabyPrefab.Info.TechType)
                    .AddCraftNode(PetPrefabs.PengwingAdultPrefab.Info.TechType)
                    .AddCraftNode(PetPrefabs.PinnacaridPrefab.Info.TechType)
                    .AddCraftNode(PetPrefabs.SnowstalkerBabyPrefab.Info.TechType)
                    .AddCraftNode(PetPrefabs.TrivalveBluePrefab.Info.TechType)
                    .AddCraftNode(PetPrefabs.TrivalveYellowPrefab.Info.TechType);


#endif

                FabricatorTemplate fabPrefab = new FabricatorTemplate(Info, treeType)
                {
                    FabricatorModel = FabricatorTemplate.Model.Workbench,
                    ModifyPrefab = obj =>
                    {
                        obj.SetActive(false);
                        obj.AddComponent<PetFabricator>();
                        ModUtils.ApplyNewMeshTexture(obj, "PetFabricatorTexture", "");
                        obj.SetActive(false);
                    }
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
                        new CraftData.Ingredient(PetDnaPrefabs.CrabSquidDnaPrefab.Info.TechType, 1),
                        new CraftData.Ingredient(PetDnaPrefabs.AlienRobotDnaPrefab.Info.TechType, 1),
                        new CraftData.Ingredient(PetDnaPrefabs.BloodCrawlerDnaPrefab.Info.TechType, 1),
                        new CraftData.Ingredient(PetDnaPrefabs.CaveCrawlerDnaPrefab.Info.TechType, 1)
#endif
#if SUBNAUTICAZERO
                        new Ingredient(PetDnaPrefabs.SnowstalkerBabyDnaPrefab.Info.TechType, 1),
                        new Ingredient(PetDnaPrefabs.PengwingAdultDnaPrefab.Info.TechType, 1),
                        new Ingredient(PetDnaPrefabs.PenglingBabyDnaPrefab.Info.TechType, 1),
                        new Ingredient(PetDnaPrefabs.PinnacaridDnaPrefab.Info.TechType, 1),
                        new Ingredient(PetDnaPrefabs.TrivalveBlueDnaPrefab.Info.TechType, 1),
                        new Ingredient(PetDnaPrefabs.TrivalveYellowDnaPrefab.Info.TechType, 1),
#endif
                    }
                };

                // Set the recipe
                fabricatorPrefab.SetRecipe(recipe);

                // Set up the scanning and fragment unlocks
                fabricatorPrefab.SetUnlock(Info.TechType, 3)
                    .WithPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule)
                    .WithAnalysisTech(ModUtils.GetSpriteFromAssetBundle("PetFabricatorDataBankPopupImageTexture"), null,
                        null)
                    .WithEncyclopediaEntry("Tech/Habitats",
                        ModUtils.GetSpriteFromAssetBundle("PetFabricatorDataBankPopupImageTexture"),
                        ModUtils.GetTexture2DFromAssetBundle("PetFabricatorDataBankMainImageTexture"));
                fabricatorPrefab.Register();
            }
        }
    }
}
