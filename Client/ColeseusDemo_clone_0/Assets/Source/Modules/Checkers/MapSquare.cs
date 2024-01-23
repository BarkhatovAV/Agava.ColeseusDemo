using UnityEngine;

namespace ColyseusDemo.Checkers
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(MeshRenderer))]
    public class MapSquare : MonoBehaviour
    {
        [field: SerializeField] public bool IsWhiteBusy { get; private set; }

        public int MapWidthPosition { get; private set; }
        public int MapLengthPosition { get; private set; }
        public bool IsAvailable { get; private set; }

        [SerializeField] private Transform _diskPlace;
        [SerializeField] private Material _brightedMaterial;
        [SerializeField] private Material _accessibilityMaterial;
        [SerializeField] private bool _isBusy;
        [SerializeField] private bool _isWhiteBusy;

        private MeshRenderer _meshRenderer;
        private Material _defaultMaterial;

        public bool IsBusy => _isBusy;
        public Vector3 DiskPlace => _diskPlace.transform.position;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _defaultMaterial = _meshRenderer.material;
        }

        public void SetMapPosition(int mapWidthPosition, int mapLengthPosition)
        {
            MapWidthPosition = mapWidthPosition;
            MapLengthPosition = mapLengthPosition;
        }

        public void SetBusyStatus(bool isBusy) =>
            _isBusy = isBusy;

        public void SetIsWhiteBusy(bool isWhiteBusy) =>
            _isWhiteBusy = isWhiteBusy;

        public void SetAvailable(bool value)
        {
            IsAvailable = value;

            if (IsAvailable)
                _meshRenderer.material = _accessibilityMaterial;
            else
                _meshRenderer.material = _defaultMaterial;
        }

        public void Highlight() =>
            _meshRenderer.material = _brightedMaterial;

        public void Unhighlight() =>
            _meshRenderer.material = _accessibilityMaterial;
    }
}