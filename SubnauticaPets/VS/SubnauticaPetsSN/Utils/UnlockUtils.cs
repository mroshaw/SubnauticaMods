using DaftAppleGames.SubnauticaPets.Prefabs;
using Story;

namespace DaftAppleGames.SubnauticaPets.Utils
{
    internal static class UnlockUtils
    {
        internal static void UnlockAllIfCreativeMode()
        {
            // If Creative Mode is enabled, ensure Builder elements are unlocked
            if (SubnauticaPetsPlugin.ModConfig.ModMode == ModMode.Creative)
            {
                LogUtils.LogInfo("Creative mode selected. Unlocking Blueprints and PDA entries...");

                // Unlock Builder entries
                UnlockTechType(PetFabricatorPrefab.Info.TechType);
                UnlockTechType(PetConsolePrefab.Info.TechType);

                // Unlock builder PDA entries
                PDAEncyclopedia.AddAndPlaySound("PetFabricator");
                PDAEncyclopedia.AddAndPlaySound("PetConsole");
                UnlockTechType(PetFabricatorFragmentPrefab.Info.TechType);
                UnlockTechType(PetConsoleFragmentPrefab.Info.TechType);

                // Unlock Pet DNA entries
                StoryGoal.Execute("PetDna", Story.GoalType.Encyclopedia);
            }
        }

        private static void UnlockTechType(TechType techType)
        {
            LogUtils.LogInfo($"KnownText UnlockState for '{techType}' is: {KnownTech.GetTechUnlockState(techType)}");
            if (KnownTech.GetTechUnlockState(techType) != TechUnlockState.Available)
            {
                KnownTech.Add(techType, true);
            }
        }
    }
}
