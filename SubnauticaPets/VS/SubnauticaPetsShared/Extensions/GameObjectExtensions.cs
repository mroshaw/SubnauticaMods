using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Extensions
{
    /// <summary>
    /// Useful static extension methods to GameObject
    /// </summary>
    internal static class GameObjectExtensions
    {
        /// <summary>
        /// Destroys all child components of a given type
        /// </summary>
        internal static void DestroyComponentsInChildren<T>(this GameObject gameObject)
        {
            LogUtils.LogDebug(LogArea.Utilities, $"ModUtils: Destroying all components of type: {typeof(T)}");
            var components = gameObject.GetComponentsInChildren<T>(true);

            LogUtils.LogDebug(LogArea.Utilities, $"ModUtils: Found {components.Length} instances to destroy");

            // Iterate through all child components and destroy them
            foreach (var component in components)
            {
                Object.Destroy(component as Object);
                LogUtils.LogDebug(LogArea.Utilities, $"ModUtils: Destroyed: {component.GetType()}");
            }

            LogUtils.LogDebug(LogArea.Utilities, $"ModUtils: Destroying all components of type: {typeof(T)}. Done.");
        }

        /// <summary>
        /// Disables all components of given type
        /// </summary>
        internal static void DisableComponentsInChildren<T>(this GameObject gameObject)
        {
            var components = gameObject.GetComponentsInChildren<Behaviour>(true);

            // Iterate through all child components and disable them
            foreach (Behaviour component in components)
            {
                if (component.GetType() == typeof(T))
                {
                    component.enabled = false;
                }
            }

            LogUtils.LogDebug(LogArea.Utilities, $"ModUtils: Disabling all components of type: {typeof(T)}. Done.");
        }

        /// <summary>
        /// Updates all materials on the gameobject that use the oldTextureName to use the bundleTextureName
        /// </summary>
        public static void SetMaterialTexture(this GameObject targetGameObject, string oldTextureName, string bundleTextureName)
        {
            Renderer[] renderers = targetGameObject.GetComponentsInChildren<Renderer>(true);
            Texture texture = CustomAssetBundleUtils.GetObjectFromAssetBundle<Texture>(bundleTextureName) as Texture;

            foreach (Renderer renderer in renderers)
            {
                if (renderer.material.mainTexture.name == oldTextureName)
                {
                    renderer.material.mainTexture = texture;
                }
            }
        }

        /// <summary>
        /// Applies a texture to the material on a GameObject
        /// </summary>
        public static void ApplyNewMeshTexture(this GameObject targetGameObject, string textureName, string gameObjectNameHint)
        {
            Renderer[] renderers = targetGameObject.GetComponentsInChildren<Renderer>();

            if (gameObjectNameHint == "")
            {
                renderers[0].material.mainTexture = CustomAssetBundleUtils.GetObjectFromAssetBundle<Texture2D>(textureName) as Texture2D;
            }
            else
            {
                foreach (Renderer renderer in renderers)
                {
                    if (renderer.gameObject.name == gameObjectNameHint)
                    {
                        renderer.material.mainTexture = CustomAssetBundleUtils.GetObjectFromAssetBundle<Texture2D>(textureName) as Texture2D;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the Layer of the GameObject, and it's children if isIncludeChildren is true
        /// </summary>
        public static void SetLayer(this GameObject targetGameObject, string layerName, bool includeChildren)
        {
            LogUtils.LogDebug(LogArea.Prefabs, $"Layer of {targetGameObject} is currently {LayerMask.LayerToName(targetGameObject.layer)}");
            LogUtils.LogDebug(LogArea.Prefabs, $"Setting Layer of {targetGameObject} to {layerName}");
            targetGameObject.layer = LayerMask.NameToLayer(layerName);
            if (includeChildren)
            {
                foreach (Transform child in targetGameObject.GetComponentsInChildren<Transform>(true))
                {
                    child.gameObject.layer = LayerMask.NameToLayer("Vehicle");
                }
            }
            LogUtils.LogDebug(LogArea.Prefabs, $"Layer of {targetGameObject} is now {LayerMask.LayerToName(targetGameObject.layer)}");
        }
    }
}