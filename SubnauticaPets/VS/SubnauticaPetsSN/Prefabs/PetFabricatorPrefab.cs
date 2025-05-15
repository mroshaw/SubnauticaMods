using DaftAppleGames.SubnauticaPets.BaseParts;
using DaftAppleGames.SubnauticaPets.Extensions;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Prefabs
{
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
            // Unlock at start if in Creative mode
            Info = PrefabInfo
                .WithTechType("PetFabricator", null, null, unlockAtStart: false)
                .WithIcon(CustomAssetBundleUtils.GetObjectFromAssetBundle<Sprite>("PetFabricatorIconTexture") as Sprite);

            CustomPrefab fabricatorPrefab = new CustomPrefab(Info);

            FabricatorGadget fabGadget = fabricatorPrefab.CreateFabricator(out CraftTree.Type treeType)
                .AddCraftNode(PetPrefabs.AlienRobotPrefab.Info.TechType)
                .AddCraftNode(PetPrefabs.BloodCrawlerPrefab.Info.TechType)
                .AddCraftNode(PetPrefabs.CaveCrawlerPrefab.Info.TechType)
                .AddCraftNode(PetPrefabs.CrabSquidPrefab.Info.TechType);

            // If enabled, add the "bonus pets" to the fabricator
            if (SubnauticaPetsPlugin.ModConfig.EnableBonusPets)
            {
                fabGadget

                .AddCraftNode(CustomPetPrefabs.CatPetPrefab.Info.TechType)
                .AddCraftNode(CustomPetPrefabs.DogPetPrefab.Info.TechType)
                .AddCraftNode(CustomPetPrefabs.RabbitPetPrefab.Info.TechType)
                .AddCraftNode(CustomPetPrefabs.SealPetPrefab.Info.TechType)
                .AddCraftNode(CustomPetPrefabs.WalrusPetPrefab.Info.TechType)
                .AddCraftNode(CustomPetPrefabs.FoxPetPrefab.Info.TechType)
                ;
            }

            FabricatorTemplate fabPrefab = new FabricatorTemplate(Info, treeType)
            {
                FabricatorModel = FabricatorTemplate.Model.Workbench,
                ModifyPrefab = obj =>
                {
                    obj.SetActive(false);
                    obj.AddComponent<PetFabricator>();
                    obj.ApplyNewMeshTexture("PetFabricatorTexture", "");
                    obj.SetActive(false);
                }
            };

            fabricatorPrefab.SetGameObject(fabPrefab);

            // Define the recipe for the new Fabricator, depends on whether in "Adventure" or "Creative" mode.
            RecipeData recipe = null;
            if (SubnauticaPetsPlugin.ModConfig.ModMode == ModMode.Adventure)
            {
                recipe = new RecipeData(
                    new CraftData.Ingredient(TechType.Titanium, 5),
                    new CraftData.Ingredient(TechType.ComputerChip, 1),
                    new CraftData.Ingredient(TechType.CopperWire, 2),
                    new CraftData.Ingredient(PetDnaPrefabs.CrabSquidDnaPrefab.Info.TechType, 1),
                    new CraftData.Ingredient(PetDnaPrefabs.AlienRobotDnaPrefab.Info.TechType, 1),
                    new CraftData.Ingredient(PetDnaPrefabs.BloodCrawlerDnaPrefab.Info.TechType, 1),
                    new CraftData.Ingredient(PetDnaPrefabs.CaveCrawlerDnaPrefab.Info.TechType, 1));
            }
            else
            {
                // Only costs 1 titanium in "Easy" mode
                recipe = new RecipeData(new CraftData.Ingredient(TechType.Titanium, 1));
            }

            // Set the recipe
            fabricatorPrefab.SetRecipe(recipe);

            // Set up the scanning and fragment unlocks
            fabricatorPrefab.SetUnlock(Info.TechType, 3)
                .WithPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule)
                .WithAnalysisTech(CustomAssetBundleUtils.GetObjectFromAssetBundle<Sprite>("PetFabricatorDataBankPopupImageTexture") as Sprite, null,
                    null)
                .WithEncyclopediaEntry("Tech/Habitats",
                    CustomAssetBundleUtils.GetObjectFromAssetBundle<Sprite>("PetFabricatorDataBankPopupImageTexture") as Sprite,
                    CustomAssetBundleUtils.GetObjectFromAssetBundle<Texture2D>("PetFabricatorDataBankMainImageTexture") as Texture2D);
            fabricatorPrefab.Register();
        }
    }
}