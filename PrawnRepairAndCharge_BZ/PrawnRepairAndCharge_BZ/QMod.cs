using System;
using System.Reflection;
using HarmonyLib;
using QModManager.API.ModLoading;
using SMLHelper.V2.Commands;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Interfaces;
using SMLHelper.V2.Json;
using SMLHelper.V2.Options;
using SMLHelper.V2.Options.Attributes;

using UnityEngine;
using UnityEngine.UI;
using Logger = QModManager.Utility.Logger;

namespace PrawnRepairAndCharge_BZ
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

            /// Here we are registering a console command by use of a delegate. The delegate will respond to the "delegatecommand"
            /// command from the dev console, passing values following "delegatecommand" as the correct types, provided they can be
            /// parsed to that type. For example, "delegatecommand foo 3 true" would be a valid command for the
            /// <see cref="MyCommand"/> delegate signature. You can also use Func or Action to define your delegate signatures
            /// if you prefer, and you can also pass a reference to a method that matches this signature.
            /// 
            /// Registered commands must be unique. If another mod has already added the command, your command will be rejected.
            /// 
            /// If the user enters incorrect parameters for a command, they will be notified of the expected parameter types,
            /// both on-screen and in the log.
            /// 
            /// Note that a command can have a return type, but it is not necessary. If it does return any type, it will be printed
            /// both on-screen and in the log.
            ConsoleCommandsHandler.Main.RegisterConsoleCommand<MyCommand>("prawnrepairandcharge", (enabledMoonpool, enabledSeatruck) =>
            {
                return $"Parameters: {enabledMoonpool} {enabledSeatruck}";
            });

            /// Here we are registering all console commands defined
            ConsoleCommandsHandler.Main.RegisterConsoleCommands(typeof(QMod));
        }
        private delegate string MyCommand(bool enabledMoonpool, bool enabledSeatruck);

        /// <summary>
        /// <para>Here, we are using the <see cref="ConsoleCommandAttribute"/> to define a custom console command, which is
        /// registered via our use of <see cref="IConsoleCommandHandler.RegisterConsoleCommands(Type)"/> above.</para>
        /// 
        /// <para>This method will respond to the "attributedcommand" command from the dev console. The command will respect the method
        /// signature of the decorated method, passing values following "attributedcommand" as the correct types, as long as they can be
        /// parsed to that type. For example, "attributedcommand foo 3 true" would be a valid command for this method signature.</para>
        /// 
        /// <para>The decorated method must be both <see langword="public"/> and <see langword="static"/>, or the attribute will
        /// be ignored. <see cref="IConsoleCommandHandler.RegisterConsoleCommand(string, Type, string, Type[])"/> allows for
        /// targeting non-<see langword="public"/> methods (must still be <see langword="static"/>), and uses a
        /// similar syntax to <see cref="HarmonyPatch"/> for defining the target method.</para>
        /// </summary>
        /// <param name="speedModifier"></param>
        /// <returns></returns>
        [ConsoleCommand("seatruckspeed")]
        public static string MyAttributedCommand(bool enabledMoonpool, bool enabledSeatruck)
        {
            return $"Parameters: {enabledMoonpool} {enabledSeatruck}";
        }
    }
    /// <summary>
    /// <para>The <see cref="MenuAttribute"/> allows us to set the title of our options menu in the "Mods" tab.</para>
    /// 
    /// <para>Optionally, we can set the <see cref="MenuAttribute.SaveOn"/> or <see cref="MenuAttribute.LoadOn"/> properties to
    /// customise when the values are saved to or loaded from disk respectively. By default, the values will be saved whenever
    /// they change, and loaded from disk when the game is registered to the options menu, which in this example happens on game
    /// launch and is the recommended setting.</para>
    /// 
    /// <para>Both of these values allow for bitwise combinations of their options, so
    /// <c>[Menu("SMLHelper Example Mod", LoadOn = MenuAttribute.LoadEvents.MenuRegistered | MenuAttribute.LoadEvents.MenuOpened)]</c>
    /// is valid and will result in the values being loaded both on game start and also whenever the menu is opened.</para>
    /// 
    /// <para>We could also specify a <see cref="ConfigFileAttribute"/> here to customise the name of the config file
    /// (defaults to "config") and an optional subfolder for the config file to reside in.</para>
    /// </summary>

    [Menu("Prawn Repair and Charge")]
    public class Config : ConfigFile
    {

        /// <summary>
        [Toggle("Enabled in Moonpool"), OnChange(nameof(MyCheckboxToggleEvent)), OnChange(nameof(MyGenericValueChangedEvent))]
        public bool EnableMoonPool;

        [Toggle("Enabled on Seatruck"), OnChange(nameof(MyCheckboxToggleEvent)), OnChange(nameof(MyGenericValueChangedEvent))]
        public bool EnableSeaTruck;


        private void MyCheckboxToggleEvent(ToggleChangedEventArgs e)
        {
            Logger.Log(Logger.Level.Info, "Checkbox value was changed!");
            Logger.Log(Logger.Level.Info, $"{e.Value}");
        }

        /// <summary>
        /// This method will be called whenever a value with an <see cref="OnChangeAttribute"/> referencing it by name is changed.
        /// In this example, every field above references it, so it will be called whenever any value in this class is changed by the
        /// user via the options menu.
        /// </summary>
        /// <param name="e"><para>The data from the onchange event, passed as the interface <see cref="IModOptionEventArgs"/>.</para>
        /// 
        /// <para>As this particular method is being used as an onchange event for various field types, the usage of the
        /// <see cref="IModOptionEventArgs"/> interface here enables coercion to its original data type for correct handling, as
        /// demonstrated by the <see langword="switch"/> statement below.</para>
        /// 
        /// <para>As with the other events in this example, it is not necessary to define the parameter if you do not need the data
        /// it contains.</para></param>
        private void MyGenericValueChangedEvent(IModOptionEventArgs e)
        {
            Logger.Log(Logger.Level.Info, "Generic value changed!");
            Logger.Log(Logger.Level.Info, $"{e.Id}: {e.GetType()}");

            switch (e)
            {
                case KeybindChangedEventArgs keybindChangedEventArgs:
                    Logger.Log(Logger.Level.Info, keybindChangedEventArgs.KeyName);
                    break;
                case ChoiceChangedEventArgs choiceChangedEventArgs:
                    Logger.Log(Logger.Level.Info, $"{choiceChangedEventArgs.Index}: {choiceChangedEventArgs.Value}");
                    break;
                case SliderChangedEventArgs sliderChangedEventArgs:
                    Logger.Log(Logger.Level.Info, sliderChangedEventArgs.Value.ToString());
                    break;
                case ToggleChangedEventArgs toggleChangedEventArgs:
                    Logger.Log(Logger.Level.Info, toggleChangedEventArgs.Value.ToString());
                    break;
            }
        }
    }
}