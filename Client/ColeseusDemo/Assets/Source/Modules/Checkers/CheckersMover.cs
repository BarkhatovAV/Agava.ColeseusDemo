using ColyseusDemo.Multiplayer;
using ColyseusDemo.SendTypes;
using System;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    internal class CheckersMover : MonoBehaviour
    {
        [SerializeField] private DisksStorage _disksStorage;
        [SerializeField] private MapGenerator _mapGenerator;
        [SerializeField] private float _diskMoveSpeed;

        private MoveInfo _moveInfo = new MoveInfo();

        internal event Action MoveMade;

        internal void MovePlayerDisk(Disk disk, Square targetSquare)
        {
            MoveDisk(disk, targetSquare);
            SendMoveMessage(disk, targetSquare);
        }

        internal void MoveEnemyDisk(string jsonMoveData)
        {
            MoveInfo moveInfo = JsonUtility.FromJson<MoveInfo>(jsonMoveData);

            int diskId = moveInfo.id;
            int targetMapWidthPosition = moveInfo.targetMapWidthPosition;
            int targetMapLengthPosition = moveInfo.targetMapLengthPosition;

            if (_disksStorage.TryGetDisk(out Disk movableDisk, diskId))
            {
                _mapGenerator.TryGetSquare(out Square targetSquare, targetMapWidthPosition, targetMapLengthPosition);
                MoveDisk(movableDisk, targetSquare);
            }
            else
            {
                Debug.LogError("Error: An attempt to move a disk with a non-existent index");
            }
        }

        private void MoveDisk(Disk disk, Square targetSquare)
        {
            Transform diskTransform = disk.transform;
            Vector3 targetPosition = targetSquare.GetTargetPosition(diskTransform);

            _mapGenerator.SetNewDiskPlanPosition(disk, targetSquare);
            disk.MoveTo(targetPosition);

            MoveMade?.Invoke();
        }

        private void SendMoveMessage(Disk movingDisk, Square targetMapSquare)
        {
            PrepareMoveInfo(movingDisk, targetMapSquare);
            MultiplayerManager.Instance.TrySendMessage(MessagesNames.Move, _moveInfo);
        }

        private void PrepareMoveInfo(Disk movingDisk, Square targetSquare)
        {
            _moveInfo.id = movingDisk.Id;
            _moveInfo.targetMapWidthPosition = targetSquare.WidthPosition;
            _moveInfo.targetMapLengthPosition = targetSquare.LengthPosition;
        }
    }
}