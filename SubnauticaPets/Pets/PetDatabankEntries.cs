#if SUBNAUTICA
using DaftAppleGames.SubnauticaPets.Pets.Subnautica;
#endif
#if SUBNAUTICAZERO
using DaftAppleGames.SubnauticaPets.Pets.BelowZero;
#endif
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Handlers;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.Pets
{
    /// <summary>
    /// Static class for defining and adding PDA entries
    /// for Pets and DNA
    /// </summary>
    internal static class PetDatabankEntries
    {
        // Pet DNA
        // Ency keys
        private static readonly string PetDnaEncyPath = "Lifeforms/Fauna";
        private static readonly string PetDnaEncyKey = "PetDna";
        // Asset Bundle refs
        private static readonly string PetDnaMainImageTexture = "PetDnaDataBankMainImageTexture";
        private static readonly string PetDnaPopupImageTexture = "PetDnaDataBankPopupImageTexture";

        /// <summary>
        /// Adds all DataBank entries
        /// </summary>
        public static void ConfigureDataBank()
        {
            Log.LogDebug("PetDatabankEntries: Setting up Databank...");
            // Pet DNA
            ModUtils.ConfigureDatabankEntry(PetDnaEncyKey, PetDnaEncyPath, PetDnaMainImageTexture, PetDnaPopupImageTexture);
            SetDnaPickupGoals(PetDnaEncyKey);
            Log.LogDebug("PetDatabankEntries: Setting up Databank... Done.");
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
    }
}
