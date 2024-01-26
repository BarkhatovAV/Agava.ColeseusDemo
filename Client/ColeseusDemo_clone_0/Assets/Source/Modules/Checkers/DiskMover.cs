using ColyseusDemo.Multiplayer;
using ColyseusDemo.SendTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    public class DiskMover : MonoBehaviour
    {
        private const float PermissibleMovementInaccuracy = 0.01f;

        [SerializeField] private List<Disk> _allDisks = new List<Disk>();
        [SerializeField] private Map _map;
        [SerializeField] private float _diskMoveSpeed;

        private List<Disk> _sideDisks = new List<Disk>();
        private MoveInfo _moveInfo = new MoveInfo();
        private Coroutine _moveCoroutine;

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

        internal bool IsCorrectDisk(Disk disk) =>
            _sideDisks.Contains(disk);

        internal void MovePlayerDisk(Disk disk, MapSquare targetMapSquare)
        {
            MoveDisk(disk, targetMapSquare);
            SendMoveMessage(disk, targetMapSquare);
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
            Vector3 targetPosition = new Vector3(targetXPosition, diskTransform.position.y, targetZPosition);

            disk.CurrentMapSquare.Free();
            targetMapSquare.Occupy(disk.IsWhite);

            if (_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);

            _moveCoroutine = StartCoroutine(SmoothlyMove(diskTransform, targetPosition)); 
            //diskTransform.position = new Vector3(targetXPosition, diskTransform.position.y, targetZPosition);
            disk.SetCurrentMapSquare(targetMapSquare);

            MoveMade?.Invoke();
        }

        private void SendMoveMessage(Disk movingDisk, MapSquare targetMapSquare)
        {
            _moveInfo.id = movingDisk.Id;
            SetCoordinates(targetMapSquare);

            MultiplayerManager.Instance.TrySendMessage(MessagesNames.Move, _moveInfo);
        }

        private void SetCoordinates(MapSquare targetMapSquare)
        {
            _moveInfo.targetMapWidthPosition = targetMapSquare.WidthPosition;
            _moveInfo.targetMapLengthPosition = targetMapSquare.LengthPosition;
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