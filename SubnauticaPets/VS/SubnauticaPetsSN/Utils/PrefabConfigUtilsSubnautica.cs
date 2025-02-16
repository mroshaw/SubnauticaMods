using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Utils
{
    internal class PrefabConfigUtilsSubnautica
    {
        /// <summary>
        /// Destroy the EmpAttack component
        /// </summary>
        public static void DestroyEmpAttack(GameObject targetGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "DestroyEmpAttack started...");
            ModUtils.DestroyComponentsInChildren<EMPAttack>(targetGameObject);
            LogUtils.LogDebug(LogArea.PetConfigUtils, "DestroyEmpAttack done.");
        }

        /// <summary>
        /// Destroy the AttackLastTarget component
        /// </summary>
        public static void DestroyAttackLastTarget(GameObject targetGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "DestroyAttackLastTarget started...");
            ModUtils.DestroyComponentsInChildren<AttackLastTarget>(targetGameObject);
            LogUtils.LogDebug(LogArea.PetConfigUtils, "DestroyAttackLastTarget done.");
        }
    }
}
