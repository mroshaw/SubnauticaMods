#if SUBNAUTICA
using DaftAppleGames.SubnauticaPets.Pets.Subnautica;
#endif
#if SUBNAUTICAZERO
using DaftAppleGames.SubnauticaPets.Pets.BelowZero;
#endif
using DaftAppleGames.SubnauticaPets.Pets;
using DaftAppleGames.SubnauticaPets.Pets.Custom;
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using UnityEngine;
using static CraftData;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;
namespace DaftAppleGames.SubnauticaPets.BaseParts
{
    /// <summary>
    /// Static utilities class for creating the new Pet Fabricator
    /// </summary>
    internal static class PetFabricatorPrefab
    {
        /// <summary>
        /// Makes the new Pet Fabricator available for use.
        /// </summary>
        public static void InitPetFabricator()
        {
            CustomPrefab customFab = new CustomPrefab("PetFabricator",
                null,
                null,
                ModUtils.GetSpriteFromAssetBundle(PetFabricator.PetFabricatorIconTexture));

            FabricatorGadget fabGadget = customFab.CreateFabricator(out CraftTree.Type treeType)
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

            FabricatorTemplate fabPrefab = new FabricatorTemplate(customFab.Info, treeType)
            {
                FabricatorModel = FabricatorTemplate.Model.Workbench,
                ModifyPrefab = ConfigureFabComponents

            };
            customFab.SetGameObject(fabPrefab);

            // Define the recipe
            RecipeData recipe = new RecipeData
            {
                craftAmount = 1,
                Ingredients =
                {
                    new Ingredient(TechType.Titanium, 1),
                    new Ingredient(TechType.Nickel, 1),
                    new Ingredient(TechType.Copper, 1),
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
            customFab.SetRecipe(recipe);
            customFab.SetUnlock(PetFabricatorFragmentPrefab.PrefabInfo.TechType, 1);
            customFab.SetPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule);
            customFab.Register();
            
            PetFabricator.PrefabInfo = customFab.Info;
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