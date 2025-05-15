using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Pets
{
    public class CustomPet : Creature
    {
        internal bool makeRandomSounds = true;
        internal float minDelayBetweenSounds = 20.0f;
        internal float maxDelayBetweenSounds = 120.0f;

        private float _timeToNextSound;
        private float _timeToPlaySoundCounter;

        private Pet _pet;

        public override void Start()
        {
            base.Start();
            _pet = GetComponent<Pet>();
            ResetTimeToSound();
        }

        private void Update()
        {
            if (!makeRandomSounds)
            {
                return;
            }

            if (_timeToPlaySoundCounter >= _timeToNextSound)
            {
                _pet.PlaySound();
                ResetTimeToSound();
            }
            else
            {
                _timeToPlaySoundCounter += Time.deltaTime;
            }
        }

        private void ResetTimeToSound()
        {
            _timeToPlaySoundCounter = 0.0f;
            _timeToNextSound = Random.Range(minDelayBetweenSounds, maxDelayBetweenSounds);
        }
    }
}