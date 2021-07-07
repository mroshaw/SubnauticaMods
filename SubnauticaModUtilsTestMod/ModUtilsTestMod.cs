using HarmonyLib;
using Logger = QModManager.Utility.Logger;
using UnityEngine;
using System.Collections.Generic;
using System;
using SubnauticaModUtils;

namespace SubnauticaModUtilsTestMod
{
    public class ModUtilsTestMod
    {
        [HarmonyPatch(typeof(Player))]
        [HarmonyPatch("Update")]
        internal class TestAudio
        {
            [HarmonyPostfix]
            public static void Update()
            {
                if (Input.GetKeyUp(KeyCode.Quote))
                {
                    Logger.Log(Logger.Level.Debug, $"Toggle keypress detected");
                    AudioUtilsBZ.Test();
                }
            }
        }
    }
}