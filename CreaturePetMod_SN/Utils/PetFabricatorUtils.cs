using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using static CraftData;

namespace DaftAppleGames.CreaturePetModSn.Utils
{
    /// <summary>
    /// Static utilities class for creating the new Pet Fabricator
    /// </summary>
    internal static class PetFabricatorUtils
    {
        /// <summary>
        /// Makes the new Pet Fabricator available for use.
        /// </summary>
        public static void InitPetFabricator()
        {
            CustomPrefab customFab = new CustomPrefab("PetFab", "Pet Fabricator", "A special fabricator for replicating pets!",
                SpriteManager.Get(TechType.Fabricator));

            customFab.CreateFabricator(out CraftTree.Type treeType)
                // Add our Pet Buildables
                .AddCraftNode(PetBuildableUtils.CaveCrawlerPetBuildablePrefabInfo.TechType)
                .AddCraftNode(PetBuildableUtils.BloodCrawlerPetBuildablePrefabInfo.TechType)
                .AddCraftNode(PetBuildableUtils.CrabSquidPetBuildablePrefabInfo.TechType)
                .AddCraftNode(PetBuildableUtils.AlienRobotBuildablePefabInfo.TechType);

            FabricatorTemplate fabPrefab = new FabricatorTemplate(customFab.Info, treeType)
            {
                FabricatorModel = FabricatorTemplate.Model.Workbench
            };
            customFab.SetGameObject(fabPrefab);
            /*
             * This is what the aforementioned json object will look like as a RecipeData object.
             * You may use the CustomPrefab.SetRecipe() to set the recipe to a RecipeData object.
             */
            RecipeData recipe = new RecipeData
            {
                craftAmount = 1,
                Ingredients =
                {
                    new Ingredient(TechType.Titanium, 1),
                    new Ingredient(TechType.Nickel, 1),
                    new Ingredient(TechType.Copper, 1)
                }
            };

            /*
             * Set the recipe.
             */
            customFab.SetRecipe(recipe);

            customFab.SetUnlock(TechType.Workbench)
                .WithPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule);

            customFab.Register();
        }
    }
}
