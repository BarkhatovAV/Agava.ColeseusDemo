using UnityEngine;

namespace ColyseusDemo.Checkers
{
    [RequireComponent(typeof(Collider))]
    internal class Disk : MonoBehaviour
    {
        [field: SerializeField] internal bool IsWhite { get; private set; }
        [field: SerializeField] internal MapSquare CurrentMapSquare { get; private set; }

        internal int Id { get; private set; }
        internal Color DefaultColor { get; private set; }

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