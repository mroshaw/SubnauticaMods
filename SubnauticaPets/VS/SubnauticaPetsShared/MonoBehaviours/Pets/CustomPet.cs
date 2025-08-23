namespace DaftAppleGames.SubnauticaPets.Pets
{
    public class CustomPet : Creature
    {
        private Pet _pet;

        public override void Start()
        {
            base.Start();
            _pet = GetComponent<Pet>();
        }
    }
}