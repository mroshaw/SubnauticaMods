using System.Reflection;
using HarmonyLib;
using QModManager.API.ModLoading;
using Logger = QModManager.Utility.Logger;

namespace SeaTruckFishScoop_BZ
{
    [QModCore]
    public static class QMod
    {
        [QModPatch]
        public static void Patch()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string id = "mroshaw_" + executingAssembly.GetName().Name;
            Logger.Log(Logger.Level.Info, "Patching " + id);
            new Harmony(id).PatchAll(executingAssembly);
            Logger.Log(Logger.Level.Info, "Patched successfully!");
        }
    }
}
