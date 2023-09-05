using System.Collections.Generic;
using System.Linq;
using Nautilus.Handlers;
using Nautilus.Options;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.ConfigOptions
{
    /// <summary>
    /// Class to create and manage Nautilus mod options
    /// </summary>
    internal class PetModOptions : ModOptions
    {
        private ModToggleOption _skipSpawnObstacleCheckOption;

        public PetModOptions() : base("Subnautica Pets Options")
        {
            // Register Mod Options
            OptionsPanelHandler.RegisterModOptions(this);
            AddModOptions();
        }

        /// <summary>
        /// Add Mod Options based on existing BepInEx Config
        /// </summary>
        private void AddModOptions()
        {
            // Set up debug options
            _skipSpawnObstacleCheckOption = SkipSpawnObstacleCheckConfig.ToModToggleOption();

            _skipSpawnObstacleCheckOption.OnChanged += SpawnObstacleCheckOnChanged;

            // Add new items to the mod options
            AddItem(_skipSpawnObstacleCheckOption);
        }

        /// <summary>
        /// Override BuildModOptions to handle mapping to BepInEx Config
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="modsTabIndex"></param>
        /// <param name="options"></param>
        public override void BuildModOptions(uGUI_TabbedControlsPanel panel, int modsTabIndex, IReadOnlyCollection<OptionItem> options)
        {
            // Remove Mod Options
            foreach (OptionItem item in options.ToList())
            {
                Log.LogDebug($"PetModOptions: Removing {item}");
                RemoveItem(item.Id);
            }

            // Recreate, in case they've changed in BepInEx Config
            AddModOptions();
            
            base.BuildModOptions(panel, modsTabIndex, options);
        }

        /// <summary>
        /// Handle options changed for Skip Spawn Obstacle Check
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void SpawnObstacleCheckOnChanged(object sender, ToggleChangedEventArgs eventArgs)
        {
            SkipSpawnObstacleCheckConfig.Value = eventArgs.Value;
        }
    }
}
