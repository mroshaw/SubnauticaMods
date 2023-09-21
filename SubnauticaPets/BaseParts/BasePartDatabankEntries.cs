#if SUBNAUTICA
using DaftAppleGames.SubnauticaPets.Pets.Subnautica;
#endif
#if SUBNAUTICAZERO
using DaftAppleGames.SubnauticaPets.Pets.BelowZero;
#endif
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Handlers;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.BaseParts
{
    /// <summary>
    /// Static class for defining and adding PDA entries
    /// for Pets and DNA
    /// </summary>
    internal static class BasePartDatabankEntries
    {
        // Pet Fabricator
        // Ency keys
        private static readonly string PetFabricatorEncyKey = "PetFabricator";
        private static readonly string PetFabricatorEncyPath = "Tech/Habitats";
        // Asset Bundle refs
        private static readonly string PetFabricatorMainImageTexture = "PetFabricatorDataBankMainImageTexture";
        private static readonly string PetFabricatorPopupImageTexture = "PetFabricatorDataBankPopupImageTexture";

        // Pet Console
        // Ency keys
        private static readonly string PetConsoleEncyPath = "Tech/Habitats";
        private static readonly string PetConsoleEncyKey = "PetConsole";
        // Asset Bundle refs
        private static readonly string PetConsoleMainImageTexture = "PetConsoleDataBankMainImageTexture";
        private static readonly string PetConsolePopupImageTexture = "PetConsoleDataBankPopupImageTexture";

        /// <summary>
        /// Adds all DataBank entries
        /// </summary>
        public static void ConfigureDataBank()
        {
            Log.LogDebug("BasePartDatabankEntries: Setting up Databank...");
            // Pet Fabricator
            ModUtils.ConfigureDatabankEntry(PetFabricatorEncyKey, PetFabricatorEncyPath, PetFabricatorMainImageTexture, PetFabricatorPopupImageTexture);
            SetScanGoals(PetFabricatorEncyKey, PetFabricatorFragmentPrefab.PrefabInfo.TechType);

            // Pet Console
            ModUtils.ConfigureDatabankEntry(PetConsoleEncyKey, PetConsoleEncyPath, PetConsoleMainImageTexture, PetConsolePopupImageTexture);
            SetScanGoals(PetConsoleEncyKey, PetConsoleFragmentPrefab.PrefabInfo.TechType);

            Log.LogDebug("BasePartDatabankEntries: Setting up Databank... Done.");
        }


        /// <summary>
        /// Sets up goals based on collection of DNA samples
        /// </summary>
        /// <param name="encyKey"></param>
        private static void SetDnaPickupGoals(string encyKey)
        {
            Log.LogDebug("DatabankEntries: Setting up ItemGoals...");
#if SUBNAUTICA
            StoryGoalHandler.RegisterItemGoal(encyKey, Story.GoalType.Encyclopedia, AlienRobotPet.DnaBuildablePrefabInfo.TechType);
            StoryGoalHandler.RegisterItemGoal(encyKey, Story.GoalType.Encyclopedia, BloodCrawlerPet.DnaBuildablePrefabInfo.TechType);
            StoryGoalHandler.RegisterItemGoal(encyKey, Story.GoalType.Encyclopedia, CaveCrawlerPet.DnaBuildablePrefabInfo.TechType);
            StoryGoalHandler.RegisterItemGoal(encyKey, Story.GoalType.Encyclopedia, CrabSquidPet.DnaBuildablePrefabInfo.TechType);
#endif
#if SUBNAUTICAZERO
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal(encyKey, Story.GoalType.Encyclopedia, PenglingAdultPet.DnaBuildablePrefabInfo.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal(encyKey, Story.GoalType.Encyclopedia, PenglingBabyPet.DnaBuildablePrefabInfo.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal(encyKey, Story.GoalType.Encyclopedia, PinnicaridPet.DnaBuildablePrefabInfo.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal(encyKey, Story.GoalType.Encyclopedia, SnowStalkerBabyPet.DnaBuildablePrefabInfo.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal(encyKey, Story.GoalType.Encyclopedia, TrivalveYellowPet.DnaBuildablePrefabInfo.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal(encyKey, Story.GoalType.Encyclopedia, TrivalveBluePet.DnaBuildablePrefabInfo.TechType);
#endif
            Log.LogDebug("DatabankEntries: Setting up ItemGoals... Done.");
        }

        /// <summary>
        /// Sets up goals based on scanning of fragment TechTypes
        /// </summary>
        /// <param name="encyKey"></param>
        /// <param name="scanTechType"></param>
        private static void SetScanGoals(string encyKey, TechType scanTechType)
        {
            PDAHandler.AddCustomScannerEntry(scanTechType, 3.0f, true, encyKey);
        }
    }
}
