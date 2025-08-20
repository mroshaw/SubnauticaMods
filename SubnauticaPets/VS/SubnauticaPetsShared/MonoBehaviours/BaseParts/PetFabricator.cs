using DaftAppleGames.SubnauticaPets.Pets;
using System;
using System.Collections;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.BaseParts
{
    /// <summary>
    /// MonoBehaviour for the Pet Fabricator
    /// </summary>
    internal class PetFabricator : MonoBehaviour
    {
        // This is the base root of the base n which the fabricator was created
        public Base Base { get; set; }

        public string BaseId
        {
            get
            {
                if (Base != null)
                {
                    return Base.GetComponent<PrefabIdentifier>().Id;
                }
                else
                {
                    return "NO BASE!";
                }
            }
        }

        private Vector3 _spawnPoint;
        private GameObject _baseParentGameObject;

        /// <summary>
        /// Initialise the component
        /// </summary>
        private void Start()
        {

            if (transform.parent == null)
            {
                // We're probably in the prefab, so return.
                return;
            }

            CrafterGhostModel ghostModel = GetComponent<CrafterGhostModel>();
            if (ghostModel != null)
            {
                _spawnPoint = ghostModel.itemSpawnPoint.position;
            }

            SetParentBaseObject();
            _baseParentGameObject = gameObject.transform.parent.gameObject;
        }

        private void SetParentBaseObject()
        {
            // Get the BasePart transform
            Base = transform.parent.GetComponent<Base>();
            if (Base)
            {
                LogUtils.LogDebug(LogArea.MonoBaseParts, $"PetFabriactor Start in Base: {Base.gameObject.name}");
            }
            else
            {
                LogUtils.LogDebug(LogArea.MonoBaseParts, $"PetFabriactor Start: Base not found in parent!");
            }
        }

        /// <summary>
        /// Spawn a Pet. Optinal callback action is invoked, if provided, with the spawned GameObject
        /// </summary>
        public void SpawnPet(TechType techType, Action<GameObject> callBack = null)
        {
            StartCoroutine(SpawnPetAsync(techType, callBack));
        }

        /// <summary>
        /// Spawn Pet Async version. Optional callback is invoked at the end of the process, if provided.
        /// </summary>
        private IEnumerator SpawnPetAsync(TechType techType, Action<GameObject> callback = null)
        {
            CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(techType);
            yield return task;
            GameObject prefab = task.GetResult();
            prefab.SetActive(false);
            // Instantiate in the spawn position
            LogUtils.LogDebug(LogArea.MonoBaseParts, $"PetFabricator: Instantiating Pet {techType}");
            GameObject newPetGameObject = Instantiate(prefab, _spawnPoint, Quaternion.identity);

            LogUtils.LogDebug(LogArea.MonoBaseParts, "PetFabricator: Instantiating Pet done!");
            Pet newPet = newPetGameObject.GetComponent<Pet>();
            if (newPet)
            {
                LogUtils.LogDebug(LogArea.MonoBaseParts, "PetFabricator: Setting Pet Name...");
                newPet.PetName = $"Test Subject {SubnauticaPetsPlugin.PetSaver.PetList.Count + 1}";
                LogUtils.LogDebug(LogArea.MonoBaseParts, "PetFabricator: Setting Pet Name... Done.");

                // Tell the pet which base it belongs to and parent the transform
                newPet.Base = Base;
                newPet.transform.SetParent(Base.transform);
                PrefabConfigUtils.ConfigureSkyApplier(newPetGameObject);
            }
            else
            {
                LogUtils.LogError(LogArea.MonoBaseParts, "PetFabricator: Spawned Pet has no Pet component!");
            }
            // Rotate to face the player
            newPetGameObject.transform.LookAt(Player.main.transform.position);
            newPetGameObject.SetActive(true);
            newPet.LoadPetData();

            callback?.Invoke(newPetGameObject);
        }
    }
}
