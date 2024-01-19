using UnityEngine;

namespace ColyseusDemo.ChessMovement
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(MeshRenderer))]
    public class ChessSquare : MonoBehaviour
    {
        [SerializeField] private Transform _chessPieceSpawnPlace;
        [SerializeField] private Material _brightedMaterial;

        private MeshRenderer _meshRenderer;
        private Material _defaultMaterial;

        public Transform ChessPieceSpawnPlace => _chessPieceSpawnPlace;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _defaultMaterial = _meshRenderer.material;
        }

        public void Highlight() =>
            _meshRenderer.material = _brightedMaterial;

        public void Unhighlight() =>
            _meshRenderer.material = _defaultMaterial;
    }
}