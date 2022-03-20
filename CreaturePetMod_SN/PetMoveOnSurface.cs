using UnityEngine;
using Logger = QModManager.Utility.Logger;

    class PetMoveOnSurface : MoveOnSurface
    {
        /// <summary>
        /// Evaluate the liklihood of performing the action
        /// </summary>
        /// <param name="creature"></param>
        /// <returns></returns>
        public override float Evaluate(Creature creature)
        {
            // This is all Pets can do for now!
                return 1.0f;
        }

        /// <summary>
        /// Perform the actions
        /// </summary>
        /// <param name="creature"></param>
        /// <param name="deltaTime"></param>
        public override void Perform(Creature creature, float deltaTime)
        {
            /*
            // Check that our pet is on the floor. If not, move it there!
            Vector3 petPosition = this.gameObject.transform.position;

            if (Physics.Raycast(petPosition, Vector3.down, out RaycastHit hit, 10.0f))
            {
                Logger.Log(Logger.Level.Debug, $"Moving Creature to ground...");
                petPosition.y = hit.point.y;
                this.gameObject.transform.position = petPosition;
            }
            */
            base.Perform(creature, deltaTime);
    }
}
