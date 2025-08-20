using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Pets
{
    internal enum PetHappiness { Ecstatic, Happy, Neutral, Sad, Devastated, Dead }
    
    public class Pet : MonoBehaviour
    {
       
        private const string EyesName = "Eyes";
        
        internal Transform Eyes { get; private set; }

        private void Awake()
        {
            Eyes = transform.Find("Eyes").transform;
        }

        internal bool IsDead => false;

        internal PetHappiness Happiness => PetHappiness.Happy;
    }
}