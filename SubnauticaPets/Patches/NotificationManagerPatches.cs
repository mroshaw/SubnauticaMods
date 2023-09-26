#if DEBUGNOTIFICATIONS
using HarmonyLib;
using System.Collections.Generic;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.Patches
{
    [HarmonyPatch(typeof(NotificationManager))]
    internal class NotificationManagerPatches
    {
        [HarmonyPatch(nameof(NotificationManager.Add), new[] {typeof (NotificationManager.Group), typeof(string), typeof(float), typeof(float) })]
        [HarmonyPostfix]
        public static void Add_Postfix(NotificationManager.Group group, string key, float duration, float timeLeft)
        {
            Log.LogDebug($"NotificationManager: Add called with group {group} and key {key}...");
        }

        [HarmonyPatch(nameof(NotificationManager.NotifyAdd))]
        [HarmonyPostfix]
        public static void NotifyAdd_Postfix(NotificationManager.NotificationId id, NotificationManager.NotificationData notification)
        {
            Log.LogDebug($"NotificationManager: NotifyAdd called with notificationId {id} and notificationData {notification}...");
            INotificationTarget notificationTarget;
            if (NotificationManager._main.targets.TryGetValue(id, out notificationTarget))
            {
                Log.LogDebug($"NotificationManager: Notification target is: {notificationTarget}...");
            }
            foreach (KeyValuePair<INotificationListener, HashSet<NotificationManager.NotificationId>> keyValuePair in NotificationManager._main.listeners)
            {
                INotificationListener key = keyValuePair.Key;
                HashSet<NotificationManager.NotificationId> value = keyValuePair.Value;
                if (value.Contains(id) || value.Contains(new NotificationManager.NotificationId(id.group, string.Empty)))
                {
                    Log.LogDebug($"NotificationManager: Calling key.OnAdd with {id.group}, {id.key}");
                }
            }
        }
    }
}
#endif