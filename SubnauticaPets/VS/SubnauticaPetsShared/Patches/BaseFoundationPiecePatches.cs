using HarmonyLib;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Patches
{
    [HarmonyPatch(typeof(BaseFoundationPiece))]
    internal class BaseFoundationPiecePatches
    {
        /// <summary>
        /// Patches the Start method, adding a special collider to the Moon Pool to stop pets falling in
        /// </summary>
        [HarmonyPatch(nameof(BaseFoundationPiece.Start))]
        [HarmonyPostfix]
        public static void Start_Postfix(BaseFoundationPiece __instance)
        {
            if (__instance.gameObject.name != "BaseMoonpool(Clone)")
            {
                return;
            }
            
            // Below Zero
            Transform poolColliderTransform = __instance.transform.Find("blockfish");

            int layer;
            if (poolColliderTransform)
            {
                layer = poolColliderTransform.gameObject.layer;
            }
            else
            {
                // Subnautica
                poolColliderTransform = __instance.transform.Find("entrance");
                layer =  LayerMask.NameToLayer("OnlyVehicle");
            }

            if (!poolColliderTransform)
            {
                LogUtils.LogError(LogArea.Patches, $"Could not patch MoonPool on {__instance.gameObject.name}! Couldn't find pool collider transform!");
            }
            
            BoxCollider fishCollider = poolColliderTransform.GetComponent<BoxCollider>();
            
            GameObject petColliderGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            petColliderGameObject.name = "petcollider";
            petColliderGameObject.layer = layer;
            petColliderGameObject.tag = poolColliderTransform.gameObject.tag;
            
            petColliderGameObject.transform.SetParent(__instance.transform);
            petColliderGameObject.transform.position = fishCollider.transform.position + new Vector3(0, -1f, 0);
            petColliderGameObject.transform.rotation = fishCollider.transform.rotation;
            petColliderGameObject.transform.localScale = fishCollider.size + (new Vector3(0, 2f, 0));

            Object.Destroy(petColliderGameObject.GetComponent<MeshRenderer>());
            Object.Destroy(petColliderGameObject.GetComponent<MeshFilter>());
        }
    }
}