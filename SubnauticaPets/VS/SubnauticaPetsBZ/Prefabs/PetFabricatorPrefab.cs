using DaftAppleGames.SubnauticaPets.BaseParts;
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;

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
                .WithIcon(ModUtils.GetSpriteFromAssetBundle("PetFabricatorIconTexture"));

            CustomPrefab fabricatorPrefab = new CustomPrefab(Info);

            FabricatorGadget fabGadget = fabricatorPrefab.CreateFabricator(out CraftTree.Type treeType)
                .AddCraftNode(PetPrefabs.PenglingBabyPrefab.Info.TechType)
                .AddCraftNode(PetPrefabs.PengwingAdultPrefab.Info.TechType)
                .AddCraftNode(PetPrefabs.PinnacaridPrefab.Info.TechType)
                .AddCraftNode(PetPrefabs.SnowstalkerBabyPrefab.Info.TechType)
                .AddCraftNode(PetPrefabs.TrivalveBluePrefab.Info.TechType)
                .AddCraftNode(PetPrefabs.TrivalveYellowPrefab.Info.TechType)
                ;

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

            // Define the recipe for the new Fabricator, depends on whether in "Adventure" or "Creative" mode.
            RecipeData recipe = null;
            if (SubnauticaPetsPlugin.ModConfig.ModMode == ModMode.Adventure)
            {
                recipe = new RecipeData(
                    new Ingredient(TechType.Titanium, 5),
                    new Ingredient(TechType.ComputerChip, 1),
                    new Ingredient(TechType.CopperWire, 2),
                    new Ingredient(PetDnaPrefabs.PengwingAdultDnaPrefab.Info.TechType, 1),
                    new Ingredient(PetDnaPrefabs.PengwingAdultDnaPrefab.Info.TechType, 1),
                    new Ingredient(PetDnaPrefabs.PengwingAdultDnaPrefab.Info.TechType, 1),
                    new Ingredient(PetDnaPrefabs.PengwingAdultDnaPrefab.Info.TechType, 1));
            }
            else
            {
                // Only costs 1 titanium in "Easy" mode
                recipe = new RecipeData(new Ingredient(TechType.Titanium, 1));
            }

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
