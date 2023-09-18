#if SUBNAUTICA
using DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets.Subnautica;
#endif
#if SUBNAUTICAZERO
using DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets.BelowZero;
#endif
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Handlers;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.CustomObjects
{
    /// <summary>
    /// Static class for defining and adding PDA entries
    /// for Pets and DNA
    /// </summary>
    internal static class DatabankEntries
    {
        // Pet DNA
        // Ency keys
        private static readonly string PetDnaEncyPath = "Lifeforms/Fauna";
        private static readonly string PetDnaEncyKey = "PetDna";
        // Asset Bundle refs
        private static readonly string PetDnaMainImageTexture = "PetDnaDataBankMainImageTexture";
        private static readonly string PetDnaPopupImageTexture = "PetDnaDataBankPopupImageTexture";

        // Pet Fabricator
        // Ency keys
        private static readonly string PetFabricatorEncyKey = "PetFabricator";
        private static readonly string PetFabricatorEncyPath = "Tech/Habitats";
        // Asset Bundle refs
        private static readonly string PetFabricatorMainImageTexture = "PetFabricatorDataBankMainImageTexture";
        private static readonly string PetFabriactorPopupImageTexture = "PetFabricatorDataBankPopupImageTexture";

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
            Log.LogDebug("DatabankEntries: Setting up Databank...");
            
            // Pet DNA
            ConfigureDatabankEntry(PetDnaEncyKey, PetDnaEncyPath, PetDnaMainImageTexture, PetDnaPopupImageTexture);
            SetDnaPickupGoals(PetDnaEncyKey);

            // Pet Fabricator
            ConfigureDatabankEntry(PetFabricatorEncyKey, PetFabricatorEncyPath, PetFabricatorMainImageTexture, PetFabriactorPopupImageTexture);
            SetScanGoals(PetFabricatorEncyKey, PetFabricatorFragmentPrefab.PrefabInfo.TechType);

            // Pet Console
            ConfigureDatabankEntry(PetConsoleEncyKey, PetConsoleEncyPath, PetConsoleMainImageTexture, PetConsolePopupImageTexture);
            SetScanGoals(PetConsoleEncyKey, PetConsoleFragmentPrefab.PrefabInfo.TechType);

            Log.LogDebug("DatabankEntries: Setting up Databank... Done.");
        }

        /// <summary>
        /// Can be called to "trigger" the Databank entry manually
        /// </summary>
        public static void TriggerDataBankEntry(string encyKey)
        {
#if SUBNAUTICAZERO
            PDAEncyclopedia.Add(encyKey, true, true);
#endif

#if SUBNAUTICA
            PDAEncyclopedia.Add(encyKey, true);
#endif
        }

        private static void ConfigureDatabankEntry(string encyKey, string encyPath, string mainImageTextureName,
            string popupImageTextureName)
        {
            Log.LogDebug($"DatabankEntries: Setting up {encyKey} entry...");
            PDAHandler.AddEncyclopediaEntry(encyKey, encyPath, null, null,
                ModUtils.GetTexture2DFromAssetBundle(mainImageTextureName),
                ModUtils.GetSpriteFromAssetBundle(popupImageTextureName));

            Log.LogDebug("DatabankEntries: Setting up PetDNA entry... Done.");
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
            PDAHandler.AddCustomScannerEntry(scanTechType, 2.0f, false, encyKey);
        }
    }
}
