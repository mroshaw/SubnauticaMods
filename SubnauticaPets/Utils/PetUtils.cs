using DaftAppleGames.SubnauticaPets.Mono.Pets;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.Utils
{
    /// <summary>
    /// Static LogUtils.LogDebug(LogArea.Utilities, LogArea.Utilities,  class for common functions and properties to be used within your mod code
    /// </summary>
    internal static class PetUtils
    {
 
        /// <summary>
        /// Find and Kill all pets
        /// </summary>
        public static void KillAllPets()
        {
            foreach (Pet pet in Saver.PetList.ToArray())
            {
                pet.Kill();
            }
        }
    }
}
