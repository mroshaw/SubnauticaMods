using System.Collections;
using DaftAppleGames.SubnauticaPets.Mono.Pets;
using DaftAppleGames.SubnauticaPets.Utils;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Mono.Utils
{
    // TODO Review this file and update to your own requirements, or remove it altogether if not required
    /// <summary>
    /// Template MonoBehaviour class. Use this to add new functionality and behaviours to
    /// the game.
    /// </summary>
    internal class FixSkyApplier : MonoBehaviour
    {
        public GameObject _parentBaseGameObject;
        private SkyApplier _skyApplier;
        private Pet _pet;

        /// <summary>
        /// Grab the SkyApplier
        /// </summary>
        public void Awake()
        {
            _skyApplier = GetComponent<SkyApplier>();
            _pet = GetComponent<Pet>();
            _parentBaseGameObject = _pet.ParentBaseGameObject;
        }

        /// <summary>
        /// Check to see if object parent has changed
        /// </summary>
        public void Update()
        {
            // Check to see if unparented
            if (transform.parent != _parentBaseGameObject.transform && transform.parent.gameObject.GetComponent<LargeWorldEntityCell>()!=null)
            {
                LogUtils.LogDebug(LogArea.MonoPets, "FixSkyApplier: Something changed the parent transform. Resetting.");
                transform.SetParent(_parentBaseGameObject.transform);
                StartCoroutine(FixSkyApplierAsync());
            }
        }

        /// <summary>
        /// Fix SkyApplier, once we can find it
        /// </summary>
        /// <returns></returns>
        private IEnumerator FixSkyApplierAsync()
        {
            while (_skyApplier == null)
            {
                LogUtils.LogDebug(LogArea.MonoPets, "FixSkyApplier: Waiting for SkyApplier...");
                _skyApplier = GetComponent<SkyApplier>();
                yield return null;
            }
            LogUtils.LogDebug(LogArea.MonoPets, "FixSkyApplier: Resetting SkyApplier...");

            PrefabConfigUtils.ConfigureSkyApplier(this.gameObject);
        }
    }
}
