using UnityEngine;
using static DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{
    internal enum CellType { Start, End, NavCell, Route }

    internal class CellVisualiser : MonoBehaviour
    {
        private NavCell _navCell;
        private CellType _cellType;

        private GameObject _visualiserSphere;

        internal void CreateOrUpdate(NavCell newNavCell, CellType cellType, Transform parentContainer)
        {
            _cellType = cellType;
            _navCell = newNavCell;
            CreateOrUpdateVisualiserSphere();
            transform.position = newNavCell.Position;
            if (parentContainer)
            {
                gameObject.transform.SetParent(parentContainer, true);
            }
        }

        private void CreateOrUpdateVisualiserSphere()
        {
            if (_visualiserSphere)
            {
                Destroy(_visualiserSphere);
            }
            CreateNewVisualiserSphere();
        }

        private void CreateNewVisualiserSphere()
        {
            switch (_cellType)
            {
                case CellType.Start:
                    _visualiserSphere = CreateSphere(3.0f, Color.white);
                    break;
                case CellType.End:
                    _visualiserSphere = CreateSphere(3.0f, Color.blue);
                    break;
                case CellType.NavCell:
                    _visualiserSphere = CreateSphere(0.5f, _navCell.HasColliders ? Color.red : Color.green);
                    break;
                case CellType.Route:
                    _visualiserSphere = CreateSphere(2.0f, Color.black);
                    break;
                default:
                    Log.LogDebug("CellVisualiser: Unknown CellType");
                    break;
            }
        }

        private GameObject CreateSphere(float radius, Color color)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Destroy(sphere.GetComponent<Collider>());
            sphere.transform.SetParent(gameObject.transform, false);
            sphere.transform.localPosition = Vector3.zero;
            sphere.transform.localScale = new Vector3(radius, radius, radius);
            sphere.GetComponent<Renderer>().material.color = color;

            return sphere;
        }
    }
}