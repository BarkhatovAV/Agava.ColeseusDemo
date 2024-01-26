using UnityEngine;

namespace ColyseusDemo.Checkers
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Renderer))]
    public class MapSquare : MonoBehaviour
    {
        [field: SerializeField] public bool IsWhiteOccupied { get; private set; }
        [field: SerializeField] public bool IsBlackOccupied { get; private set; }
        [field: SerializeField] public bool IsOccupied { get; private set; }

        public int WidthPosition { get; private set; }
        public int LengthPosition { get; private set; }
        public Color DefaultColor { get; private set; }

        [SerializeField] private Transform _diskPlace;

        public Vector3 DiskPlace => _diskPlace.transform.position;

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