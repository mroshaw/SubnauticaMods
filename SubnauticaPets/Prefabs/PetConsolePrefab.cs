using DaftAppleGames.SubnauticaPets.Mono.BaseParts;
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using UnityEngine;
using static CraftData;
using Nautilus.Handlers;

namespace DaftAppleGames.SubnauticaPets.Prefabs
{
    /// <summary>
    /// Static class for creating the Pet Console prefab
    /// </summary>
    internal static class PetConsolePrefab
    {
        // Pubic PrefabInfo, for anything that needs it
        public static PrefabInfo Info { get; } = PrefabInfo
            .WithTechType(PrefabClassId, null, null, unlockAtStart: false)
            .WithIcon(ModUtils.GetSpriteFromAssetBundle(PetConsoleIconTexture));

        // Prefab class Id
        private const string PrefabClassId = "PetConsole";

        // Asset Bundle refs for Databank
        private const string PetConsoleMainImageTexture = "PetConsoleDataBankMainImageTexture";
        private const string PetConsolePopupImageTexture = "PetConsoleDataBankPopupImageTexture";

        // Asset Bundle refs for icon
        private const string PetConsoleIconTexture = "PetConsoleIconTexture";

        // Databank key constants
        public const string PetConsoleEncyPath = "Tech/Habitats";
        public const string PetConsoleEncyKey = "PetConsole";

        /// <summary>
        /// Makes the new Pet Console available for use.
        /// </summary>
        public static void InitPetConsole()
        {
            CustomPrefab consolePrefab = new CustomPrefab(Info);

            // We'll use the PictureFrame as a template
            PrefabTemplate consoleTemplate = new CloneTemplate(consolePrefab.Info, TechType.PictureFrame)
            {
                // Reconfigure the prefab, once it's been created
                ModifyPrefab = ConfigureConsoleComponents
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

            // Set up Databank
            SetupDatabank();

            // Set up the scanning and fragment unlocks
            LogUtils.LogDebug(LogArea.Prefabs, "PetConsolePrefab: Setting up scanner entry...");
            LogUtils.LogDebug(LogArea.Prefabs, $"PetConsolePrefab: Info is: {consolePrefab.Info.TechType}");

            consolePrefab.SetUnlock(Info.TechType)
                .WithAnalysisTech(ModUtils.GetSpriteFromAssetBundle(PetConsolePopupImageTexture), null, null)
                .WithPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule);

            LogUtils.LogDebug(LogArea.Prefabs, "PetConsoleFPrefab: Setting up scanner entry... Done.");
            consolePrefab.Register();
        }

        /// <summary>
        /// Set up Databank Entries
        /// </summary>
        private static void SetupDatabank()
        {
                // Set up Databank
                LogUtils.LogDebug(LogArea.Prefabs, "PetConsoleFragmentPrefab: Setting up Databank entry...");
                PDAHandler.AddEncyclopediaEntry(PetConsoleEncyKey, PetConsoleEncyPath, null, null,
                    ModUtils.GetTexture2DFromAssetBundle(PetConsoleMainImageTexture),
                    ModUtils.GetSpriteFromAssetBundle(PetConsolePopupImageTexture));
                LogUtils.LogDebug(LogArea.Prefabs, "PetConsoleFragmentPrefab: Setting up Databank entry... Done.");
        }

        /// <summary>
        /// Set up our custom "Console" component, replacing the default one
        /// </summary>
        /// <param name="consoleGameObject"></param>
        private static void ConfigureConsoleComponents(GameObject consoleGameObject)
        {
            // Add the PetConsole component, which will provision the UI
            consoleGameObject.SetActive(false);
            LogUtils.LogDebug(LogArea.Prefabs, "PetConsolePrefab: Adding PetConsole component...");
            PetConsole petConsole = consoleGameObject.AddComponent<PetConsole>();
            LogUtils.LogDebug(LogArea.Prefabs, "PetConsolePrefab: Adding PetConsole component... Done.");
        }
    }
}
