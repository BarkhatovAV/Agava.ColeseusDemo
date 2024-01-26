using UnityEngine;

namespace ColyseusDemo.Checkers
{
    [RequireComponent(typeof(Collider))]
    public class Disk : MonoBehaviour
    {
        [field: SerializeField] public bool IsWhite { get; private set; }
        [field: SerializeField] public MapSquare CurrentMapSquare { get; private set; }

        public int Id { get; private set; }
        public Color DefaultColor { get; private set; }

        private void Awake() =>
            SetDefaultMaterial();

        internal void SetCurrentMapSquare(MapSquare currentMapSquare)
        {
            CurrentMapSquare.Free();
            CurrentMapSquare = currentMapSquare;
        }

        internal void SetId(int id) =>
            Id = id;

        private void SetDefaultMaterial()
        {
            if (gameObject.TryGetComponent<Renderer>(out Renderer renderer))
                DefaultColor = renderer.material.color;
        }
    }
}