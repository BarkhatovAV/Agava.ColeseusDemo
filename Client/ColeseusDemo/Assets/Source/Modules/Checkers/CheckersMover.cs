using ColyseusDemo.Multiplayer;
using ColyseusDemo.SendTypes;
using System;
using System.Collections;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    internal class CheckersMover : MonoBehaviour
    {
        private const float PermissibleMovementInaccuracy = 0.01f;

        [SerializeField] private DisksStorage _disksStorage;
        [SerializeField] private MapGenerator _mapGenerator;
        [SerializeField] private CaptureRules _cutDownRules;
        [SerializeField] private float _diskMoveSpeed;

        private MoveInfo _moveInfo = new MoveInfo();
        private Coroutine _moveCoroutine;

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
                _mapGenerator.TryGetSquare(out Square targetMapSquare, targetMapWidthPosition, targetMapLengthPosition);
                MoveDisk(movableDisk, targetMapSquare);
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
            ShowMove(diskTransform, targetPosition);

            MoveMade?.Invoke();
        }

        private void ShowMove(Transform diskTransform, Vector3 targetPosition)
        {
            if (_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);

            _moveCoroutine = StartCoroutine(SmoothlyMove(diskTransform, targetPosition));
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

        private IEnumerator SmoothlyMove(Transform diskTransform, Vector3 target)
        {
            float distance = (diskTransform.position - target).sqrMagnitude;

            while (distance > PermissibleMovementInaccuracy)
            {
                diskTransform.position = Vector3.Lerp(diskTransform.position, target, _diskMoveSpeed * Time.deltaTime);

                yield return null;
            }
        }
    }
}