using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Handlers;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;
using static TechStringCache;

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
