using UnityEngine;

namespace ColyseusDemo.Checkers
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Renderer))]
    internal class MapSquare : MonoBehaviour
    {
        [field: SerializeField] internal bool IsWhiteOccupied { get; private set; }
        [field: SerializeField] internal bool IsBlackOccupied { get; private set; }
        [field: SerializeField] internal bool IsOccupied { get; private set; }

        internal int WidthPosition { get; private set; }
        internal int LengthPosition { get; private set; }
        internal Color DefaultColor { get; private set; }

        [SerializeField] private Transform _diskPlace;

        internal Vector3 DiskPlace => _diskPlace.transform.position;

        private void Awake()
        {
            Renderer renderer = GetComponent<Renderer>();
            DefaultColor = renderer.material.color;
        }

        internal void SetMapPosition(int mapWidthPosition, int mapLengthPosition)
        {
            WidthPosition = mapWidthPosition;
            LengthPosition = mapLengthPosition;
        }

        internal void Occupy(bool isWhiteOccupied)
        {
            IsOccupied = true;
            IsWhiteOccupied = isWhiteOccupied;
            IsBlackOccupied = !isWhiteOccupied;
        }

        internal void Free()
        {
            IsOccupied = false;
            IsWhiteOccupied = false;
            IsBlackOccupied = false;
        }
    }
}