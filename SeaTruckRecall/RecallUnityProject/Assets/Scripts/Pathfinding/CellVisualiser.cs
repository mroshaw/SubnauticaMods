using UnityEngine;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{
    public class CellVisualiser : MonoBehaviour
    {
        public NavCell navCell;

        public void Init(NavCell newNavCell)
        {
            navCell = newNavCell;
            CreateVisualiserSphere();
        }

        private void CreateVisualiserSphere()
        {
            gameObject.transform.position = navCell.Position;
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Destroy(sphere.GetComponent<Collider>());
            sphere.transform.SetParent(gameObject.transform, false);
            sphere.transform.localPosition = Vector3.zero;
            sphere.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            sphere.GetComponent<Renderer>().material.color = navCell.hasColliders ? Color.red : Color.green;
        }
    }
}