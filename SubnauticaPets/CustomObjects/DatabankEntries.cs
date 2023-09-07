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
        private static string PdaPath = "Lifeforms/Fauna";
        private static string PetDnaKey = "PetDna";
        
        // Asset bundle names
        private static string MainImageTextureName = "PetDnaDataBankMainImageTexture";
        private static string PopupImageTextureName = "PetDnaDataBankPopupImageTexture";
        /// <summary>
        /// Adds all DataBank entries
        /// </summary>
        public static void ConfigureDataBank()
        {
            Log.LogDebug("DatabankEntries: Setting up Databank...");
            ConfigureDnaDatabankEntry();
            Log.LogDebug("DatabankEntries: Setting up Databank... Done.");
        }

        /// <summary>
        /// Adds the Pet DNA entry to the Databank
        /// </summary>
        private static void ConfigureDnaDatabankEntry()
        {
            Log.LogDebug("DatabankEntries: Setting up PetDNA entry...");
            PDAHandler.AddEncyclopediaEntry(PetDnaKey, PdaPath, null, null,
                ModUtils.GetTexture2DFromAssetBundle(MainImageTextureName),
                ModUtils.GetSpriteFromAssetBundle(PopupImageTextureName));
            Log.LogDebug("DatabankEntries: Setting up ItemGoals...");
#if SUBNAUTICA
            StoryGoalHandler.RegisterItemGoal(PetDnaKey, Story.GoalType.Encyclopedia, AlienRobotPet.DnaBuildablePrefabInfo.TechType);
            StoryGoalHandler.RegisterItemGoal(PetDnaKey, Story.GoalType.Encyclopedia, BloodCrawlerPet.DnaBuildablePrefabInfo.TechType);
            StoryGoalHandler.RegisterItemGoal(PetDnaKey, Story.GoalType.Encyclopedia, CaveCrawlerPet.DnaBuildablePrefabInfo.TechType);
            StoryGoalHandler.RegisterItemGoal(PetDnaKey, Story.GoalType.Encyclopedia, CrabSquidPet.DnaBuildablePrefabInfo.TechType);
#endif
#if SUBNAUTICAZERO
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal(PetDnaKey, Story.GoalType.Encyclopedia, PenglingAdultPet.DnaBuildablePrefabInfo.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal(PetDnaKey, Story.GoalType.Encyclopedia, PenglingBabyPet.DnaBuildablePrefabInfo.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal(PetDnaKey, Story.GoalType.Encyclopedia, PinnicaridPet.DnaBuildablePrefabInfo.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal(PetDnaKey, Story.GoalType.Encyclopedia, SnowStalkerBabyPet.DnaBuildablePrefabInfo.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal(PetDnaKey, Story.GoalType.Encyclopedia, TrivalveYellowPet.DnaBuildablePrefabInfo.TechType);
            Nautilus.Handlers.StoryGoalHandler.RegisterItemGoal(PetDnaKey, Story.GoalType.Encyclopedia, TrivalveBluePet.DnaBuildablePrefabInfo.TechType);
#endif
            Log.LogDebug("DatabankEntries: Setting up ItemGoals... Done.");
            Log.LogDebug("DatabankEntries: Setting up PetDNA entry... Done.");
        }

        /// <summary>
        /// Called to "trigger" the Databank entry
        /// </summary>
        public static void AddPetDnaDataBankEntry()
        {
#if SUBNAUTICAZERO
            PDAEncyclopedia.Add(PetDnaKey, true, true);
#endif

#if SUBNAUTICA
            PDAEncyclopedia.Add(PetDnaKey, true);
#endif
        }
    }
}
