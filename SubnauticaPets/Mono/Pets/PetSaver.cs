using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DaftAppleGames.SubnauticaPets.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.SubnauticaPets.Mono.Pets
{

    /// <summary>
    /// MonoBehaviour class to save and load active Pets
    /// </summary>
    public class PetSaver : MonoBehaviour
    {
        public List<Pet> PetList = new ();

        /// <summary>
        /// Abstract instance stub for UnityEvent
        /// </summary>
        public class OnPetRegisteredEvent : UnityEvent<Pet>
        {
        }

        /// <summary>
        /// Abstract instance stub for UnityEvent 
        /// </summary>
        public class OnPetUnregisteredEvent : UnityEvent<Pet>
        {
        }

        public UnityEvent<Pet> PetRegisteredEvent = new OnPetRegisteredEvent(); 
        public UnityEvent<Pet> PetUnregisteredEvent = new OnPetUnregisteredEvent();

        /// <summary>
        /// Register a new Pet to the HashList
        /// </summary>
        /// <param name="pet"></param>
        public void RegisterPet(Pet pet)
        {
            PetList ??= new List<Pet>();
            PetList.Add(pet);
            PetRegisteredEvent?.Invoke(pet);
        }

        /// <summary>
        /// Initialise the Saver
        /// </summary>
        public void Init()
        {
            PetList = new();
        }

        /// <summary>
        /// Remove a Pet from the HashList
        /// </summary>
        /// <param name="pet"></param>
        public void UnregisterPet(Pet pet)
        {
            PetList.Remove(pet);
            PetUnregisteredEvent.Invoke(pet);
        }

        /// <summary>
        /// Iterate the Pet List and kill
        /// </summary>
        public void KillAllPets()
        {
            foreach (Pet pet in PetList.ToArray())
            {
                pet.Kill();
            }
        }

        /// <summary>
        /// Creates a HashSet of current pets, suitable using in a save game
        /// </summary>
        /// <returns></returns>
        public HashSet<PetDetails> GetPetListAsHashSet()
        {
            HashSet<PetDetails> hashSet = new();

            PetList ??= new List<Pet>();

            foreach (Pet pet in PetList)
            {
                PetDetails newPetDetails = new PetDetails(pet.PrefabId, pet.PetName, pet.PetTypeString);
                hashSet.Add(newPetDetails);
            }
            return hashSet;
        }

        /// <summary>
        /// Internal PetDetails class, used to store "minimum" attributes for a pet
        /// so we can serialize and deserialize for saving and loading pet data
        /// </summary>
        public class PetDetails
        {
            public string PrefabId { get; }
            public string PetName { get; set; }
            public string PetType {get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="prefabId"></param>
            /// <param name="petName"></param>
            /// <param name="petType"></param>
            public PetDetails(string prefabId, string petName, string petType)
            {
                PrefabId = prefabId;
                PetName = petName;
                PetType = petType;
            }
        }

        /// <summary>
        /// Load and update Pets
        /// </summary>
        public void LoadData()
        {
            StartCoroutine(WaitForDataLoad());
        }

        /// <summary>
        /// Wait for the world to settle, then init pets
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitForDataLoad()
        {
            LargeWorldStreamer streamer = null;

            while (streamer == null)
            {
                streamer = FindObjectOfType<LargeWorldStreamer>();
                yield return new WaitForEndOfFrame();
            }

            while (!streamer.IsWorldSettled())
            {
                yield return new WaitForEndOfFrame();
            }
            FixPetLoadData();
        }

        /// <summary>
        /// Iterate through and Init pets loaded, once scene is loaded
        /// </summary>
        private void FixPetLoadData()
        {
            LogUtils.LogDebug(LogArea.MonoUtils, $"Loading Pet Data...");
            foreach (Pet pet in FindObjectsOfType<Pet>())
            {
                pet.LoadPetData();
            }
        }
    }
}
