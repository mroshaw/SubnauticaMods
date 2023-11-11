using DaftAppleGames.SubnauticaPets.Mono.BaseParts;
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
    /// Static class for creating the Pet Console prefab
    /// </summary>
    internal static class PetConsolePrefab
    {
        // Pubic PrefabInfo, for anything that needs it
        public static PrefabInfo PrefabInfo;

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
            PrefabInfo consolePrefabInfo = PrefabInfo
                .WithTechType(PrefabClassId, null, null, unlockAtStart: false)
                .WithIcon(ModUtils.GetSpriteFromAssetBundle(PetConsoleIconTexture));

            CustomPrefab consolePrefab = new CustomPrefab(consolePrefabInfo);

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
            Log.LogDebug("PetConsolePrefab: Setting up scanner entry...");
            Log.LogDebug($"PetConsolePrefab: Info is: {consolePrefab.Info.TechType}");
            
            consolePrefab.SetUnlock(consolePrefabInfo.TechType)
                .WithPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule)
                .WithAnalysisTech(ModUtils.GetSpriteFromAssetBundle(PetConsolePopupImageTexture), null, null);

            Log.LogDebug("PetConsoleFPrefab: Setting up scanner entry... Done.");

            consolePrefab.Register();

            PrefabInfo = consolePrefab.Info;
        }

        /// <summary>
        /// Set up Databank Entries
        /// </summary>
        private static void SetupDatabank()
        {
                // Set up Databank
                Log.LogDebug("PetConsoleFragmentPrefab: Setting up Databank entry...");
                PDAHandler.AddEncyclopediaEntry(PetConsoleEncyKey, PetConsoleEncyPath, null, null,
                    ModUtils.GetTexture2DFromAssetBundle(PetConsoleMainImageTexture),
                    ModUtils.GetSpriteFromAssetBundle(PetConsolePopupImageTexture));
                Log.LogDebug("PetConsoleFragmentPrefab: Setting up Databank entry... Done.");
        }

        /// <summary>
        /// Set up our custom "Console" component, replacing the default one
        /// </summary>
        /// <param name="consoleGameObject"></param>
        private static void ConfigureConsoleComponents(GameObject consoleGameObject)
        {
            // Add the PetConsole component, which will provision the UI
            Log.LogDebug("PetConsolePrefab: Adding PetConsole component...");
            PetConsole petConsole = consoleGameObject.AddComponent<PetConsole>();
            Log.LogDebug("PetConsolePrefab: Adding PetConsole component... Done.");
        }
    }
}
