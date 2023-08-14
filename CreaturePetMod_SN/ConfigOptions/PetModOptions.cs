using System.Collections.Generic;
using System.Linq;
using DaftAppleGames.CreaturePetModSn.MonoBehaviours.Pets;
using Nautilus.Handlers;
using Nautilus.Options;
using static DaftAppleGames.CreaturePetModSn.CreaturePetModSnPlugin;

namespace DaftAppleGames.CreaturePetModSn.ConfigOptions
{
    /// <summary>
    /// Class to create and manage Nautilus mod options
    /// </summary>
    internal class PetModOptions : ModOptions
    {
        private ModChoiceOption<PetCreatureType> _petTypeChoiceOption;
        private ModChoiceOption<PetName> _petNameChoiceOption;
        private ModKeybindOption _spawnKeybindOption;
        private ModKeybindOption _spawnModifierKeybindOption;
        private ModKeybindOption _killAllKeybindOption;
        private ModKeybindOption _killAllModifierKeybindOption;
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
            // Set up KeyboardShortcut entry for Spawn
            _spawnKeybindOption = SpawnKeyboardShortcutConfig.ToModKeybindOption();
            _spawnModifierKeybindOption = SpawnKeyboardShortcutModifierConfig.ToModKeybindOption();

            // Set up KeyboardShortcut entry for Kill All
            _killAllKeybindOption = KillAllKeyboardShortcutConfig.ToModKeybindOption();
            _killAllModifierKeybindOption = KillAllKeyboardShortcutModifierConfig.ToModKeybindOption();

            // Set up choice / enum option for Pet Creature Type
            _petTypeChoiceOption = PetCreatureTypeConfig.ToModChoiceOption();
            _petNameChoiceOption = PetNameConfig.ToModChoiceOption();

            // Set up debug options
            _skipSpawnObstacleCheckOption = SkipSpawnObstacleCheckConfig.ToModToggleOption();

            // Set up changed listeners
            _petTypeChoiceOption.OnChanged += PetTypeOnChanged;
            _petNameChoiceOption.OnChanged += PetNameOnChanged;
            _spawnKeybindOption.OnChanged += SpawnKeyCodeOnChanged;
            _spawnModifierKeybindOption.OnChanged += SpawnModifierKeyCodeOnChanged;
            _killAllKeybindOption.OnChanged += KillAllKeyCodeOnChanged;
            _killAllModifierKeybindOption.OnChanged += KillAllModifierKeyCodeOnChanged;
            _skipSpawnObstacleCheckOption.OnChanged += SpawnObstacleCheckOnChanged;

            // Add new items to the mod options
            AddItem(_petTypeChoiceOption);
            AddItem(_petNameChoiceOption);
            AddItem(_spawnKeybindOption);
            AddItem(_spawnModifierKeybindOption);
            AddItem(_killAllKeybindOption);
            AddItem(_killAllModifierKeybindOption);
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
        /// Handle Nautilus options changed for Pet Type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void PetTypeOnChanged(object sender, ChoiceChangedEventArgs<PetCreatureType> eventArgs)
        {
            PetCreatureTypeConfig.Value = eventArgs.Value;
        }

        /// <summary>
        /// Handle Nautilus options changed for Pet Name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void PetNameOnChanged(object sender, ChoiceChangedEventArgs<PetName> eventArgs)
        {
            PetNameConfig.Value = eventArgs.Value;
        }

        /// <summary>
        /// Handle Nautilus options changed for Spawn key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void SpawnKeyCodeOnChanged(object sender, KeybindChangedEventArgs eventArgs)
        {
            SpawnKeyboardShortcutConfig.Value = eventArgs.Value;
        }

        /// <summary>
        /// Handle Nautilus options changed for Spawn Modifier key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void SpawnModifierKeyCodeOnChanged(object sender, KeybindChangedEventArgs eventArgs)
        {
            SpawnKeyboardShortcutModifierConfig.Value = eventArgs.Value;
        }

        /// <summary>
        /// Handle Nautilus options changed for Kill All key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void KillAllKeyCodeOnChanged(object sender, KeybindChangedEventArgs eventArgs)
        {
            KillAllKeyboardShortcutConfig.Value = eventArgs.Value;
        }

        /// <summary>
        /// Handle Nautilus options changed for Kill All modifier key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void KillAllModifierKeyCodeOnChanged(object sender, KeybindChangedEventArgs eventArgs)
        {
            KillAllKeyboardShortcutModifierConfig.Value = eventArgs.Value;
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
