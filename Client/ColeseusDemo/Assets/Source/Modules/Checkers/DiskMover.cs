using ColyseusDemo.Multiplayer;
using ColyseusDemo.SendTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    public class DiskMover : MonoBehaviour
    {
        [SerializeField] private List<Disk> _allDisks = new List<Disk>();
        [SerializeField] private Map _map;

        private List<Disk> _sideDisks = new List<Disk>();
        private MoveInfo _moveInfo = new MoveInfo();
        private Disk _takenDisk;

        public event Action MoveMade;

        private void Awake()
        {
            for (int i = 0; i < _allDisks.Count; i++)
                _allDisks[i].SetId(i);
        }

        internal void SetSideDisks(bool isWhitePlayer)
        {
            var availableDisks = from Disk disk in _allDisks
                                 where disk.IsWhite == isWhitePlayer
                                 select disk;

            _sideDisks = availableDisks.ToList();
        }

        internal bool TryTakeDisk(Disk disk)
        {
            bool isCorrectSideDisk = _sideDisks.Contains(disk);

            if (isCorrectSideDisk)
                _takenDisk = disk;
            else
                _takenDisk = null;

            return isCorrectSideDisk;
        }

        internal void DropDisk()
        {
            if (_takenDisk.gameObject.TryGetComponent<Renderer>(out Renderer renderer))
                renderer.material.color = _takenDisk.DefaultColor;

            _takenDisk = null;
        }

        internal void MovePlayerDisk(MapSquare targetMapSquare)
        {
            MoveDisk(_takenDisk, targetMapSquare);
            SendMoveMessage(targetMapSquare);
        }

        internal void MoveEnemyDisk(string jsonMoveData)
        {
            MoveInfo moveInfo = JsonUtility.FromJson<MoveInfo>(jsonMoveData);

            int diskId = moveInfo.id;
            int targetMapWidthPosition = moveInfo.targetMapWidthPosition;
            int targetMapLengthPosition = moveInfo.targetMapLengthPosition;

            Disk movableDisk = _allDisks[diskId];
            MapSquare targetMapSquare = _map.GetMapSquare(targetMapWidthPosition, targetMapLengthPosition);

            MoveDisk(movableDisk, targetMapSquare);
        }

        private void MoveDisk(Disk disk, MapSquare targetMapSquare)
        {
            float targetXPosition = targetMapSquare.DiskPlace.x;
            float targetZPosition = targetMapSquare.DiskPlace.z;
            Transform diskTransform = disk.transform;

            disk.CurrentMapSquare.Free();
            targetMapSquare.Occupy(disk.IsWhite);

            diskTransform.position = new Vector3(targetXPosition, diskTransform.position.y, targetZPosition);
            disk.SetCurrentMapSquare(targetMapSquare);
            
            MoveMade?.Invoke();
        }

        private void SendMoveMessage(MapSquare targetMapSquare)
        {
            _moveInfo.id = _takenDisk.Id;
            SetCoordinates(targetMapSquare);

            MultiplayerManager.Instance.TrySendMessage(MessagesNames.Move, _moveInfo);
        }

        private void SetCoordinates(MapSquare targetMapSquare)
        {
            _moveInfo.targetMapWidthPosition = targetMapSquare.WidthPosition;
            _moveInfo.targetMapLengthPosition = targetMapSquare.LengthPosition;
        }
    }
}