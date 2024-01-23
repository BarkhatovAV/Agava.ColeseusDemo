using ColyseusDemo.Multiplayer;
using ColyseusDemo.SendTypes;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    [RequireComponent(typeof(Collider))]
    public class Disk : MonoBehaviour
    {
        [field: SerializeField] public bool IsWhite { get; private set; }
        [field: SerializeField] public MapSquare CurrentMapSquare;

        [SerializeField] private int Id;
        [SerializeField] private Material _draggingMaterial;

        private MeshRenderer _meshRenderer;
        private Material _defaultMaterial;
        private MoveInfo _currentWayInfo = new MoveInfo();

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _defaultMaterial = _meshRenderer.material;
            _currentWayInfo.id = Id;
        }

        internal void MoveTo(MapSquare nextMapSquare)
        {
            CurrentMapSquare.SetBusyStatus(false);

            CurrentMapSquare = nextMapSquare;
            CurrentMapSquare.SetBusyStatus(true);
            CurrentMapSquare.SetIsWhiteBusy(IsWhite);

            transform.position = new Vector3(CurrentMapSquare.DiskPlace.x, transform.position.y, CurrentMapSquare.DiskPlace.z);

            //SendMoveMessage(nextMapSquare);
        }

        internal void Drag() =>
            _meshRenderer.material = _draggingMaterial;

        internal void Undrag() =>
            _meshRenderer.material = _defaultMaterial;

        internal void SendMoveMessage(MapSquare targetMapSquare)
        {
            AddWayPoints(targetMapSquare);
            MultiplayerManager.Instance.TrySendMessage(MessagesNames.Move, _currentWayInfo);
        }

        private void AddWayPoints(MapSquare targetMapSquare)
        {
            _currentWayInfo.targetMapWidthPosition = targetMapSquare.MapWidthPosition;
            _currentWayInfo.targetMapLengthPosition = targetMapSquare.MapLengthPosition;
        }
    }
}