using System.Reflection;
using HarmonyLib;
using QModManager.API.ModLoading;

using SMLHelper.V2.Handlers;
using SMLHelper.V2.Interfaces;
using SMLHelper.V2.Json;
using SMLHelper.V2.Options;
using SMLHelper.V2.Options.Attributes;

using UnityEngine.UI;
using Logger = QModManager.Utility.Logger;

namespace SeaTruckSpeedMod_BZ
{
    [QModCore]
    public static class QMod
    {
        /// <summary>
        /// Here, we are setting up a instance of <see cref="Config"/>, which will automatically generate an options menu using
        /// Attributes. The values in this instance will be updated whenever the user changes the corresponding option in the menu.
        /// </summary>
        internal static Config Config { get; } = OptionsPanelHandler.Main.RegisterModOptions<Config>();

        [QModPatch]
        public static void Patch()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var modName = ($"mroshaw_{assembly.GetName().Name}");
            Logger.Log(Logger.Level.Info, $"Patching {modName}");
            Harmony harmony = new Harmony(modName);
            harmony.PatchAll(assembly);
            Logger.Log(Logger.Level.Info, "Patched successfully!");
         }
    }
    /// <summary>
    /// Set up the Mod menu
    /// </summary>
    [Menu("Sea Truck Speed Mod")]
    public class Config : ConfigFile
    {

        /// <summary>
        /// Slider element for float value of the modifier. We'll allow 1.0 (unchanged) to 5.0 (lightening speed)
        /// </summary>
        [Slider("Speed modifier", Format = "{0:F2}", Min = 1.0F, Max = 5.0F, DefaultValue = 1.0F, Step = 0.1F), OnChange(nameof(MyGenericValueChangedEvent))]
        public float SeaTruckSpeedModifier;

        /// <summary>
        /// OnChange event, for debugging for now
        /// </summary>
        private void MyGenericValueChangedEvent(IModOptionEventArgs e)
        {
            Logger.Log(Logger.Level.Debug, "Generic value changed!");
            Logger.Log(Logger.Level.Debug, $"{e.Id}: {e.GetType()}");

            switch (e)
            {
                case KeybindChangedEventArgs keybindChangedEventArgs:
                    Logger.Log(Logger.Level.Debug, keybindChangedEventArgs.KeyName);
                    break;
                case ChoiceChangedEventArgs choiceChangedEventArgs:
                    Logger.Log(Logger.Level.Debug, $"{choiceChangedEventArgs.Index}: {choiceChangedEventArgs.Value}");
                    break;
                case SliderChangedEventArgs sliderChangedEventArgs:
                    Logger.Log(Logger.Level.Debug, sliderChangedEventArgs.Value.ToString());
                    break;
                case ToggleChangedEventArgs toggleChangedEventArgs:
                    Logger.Log(Logger.Level.Debug, toggleChangedEventArgs.Value.ToString());
                    break;
            }
        }
    }
}