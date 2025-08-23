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
            
            // List all layers
            /*
            for (int i = 0; i < 32; i++)
            {
                string name = LayerMask.LayerToName(i);
                if (!string.IsNullOrEmpty(name))
                {
                    LogUtils.LogDebug(LogArea.MonoBaseParts, $"Layer {i}: {name}");
                }
            }
            */
            
            Transform blockFish = __instance.transform.Find("blockfish");
            BoxCollider fishCollider = blockFish.GetComponent<BoxCollider>();
            
            GameObject debugBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
            debugBox.name = "petcollider";
            debugBox.layer = blockFish.gameObject.layer;
            debugBox.tag = blockFish.gameObject.tag;
            
            debugBox.transform.SetParent(__instance.transform);
            debugBox.transform.position = fishCollider.transform.position + new Vector3(0, -1f, 0);
            debugBox.transform.rotation = fishCollider.transform.rotation;
            debugBox.transform.localScale = fishCollider.size + (new Vector3(0, 2f, 0));

            Object.Destroy(debugBox.GetComponent<MeshRenderer>());
            Object.Destroy(debugBox.GetComponent<MeshFilter>());
        }
    }
}