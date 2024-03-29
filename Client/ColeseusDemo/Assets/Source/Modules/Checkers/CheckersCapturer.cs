using ColyseusDemo.Multiplayer;
using ColyseusDemo.SendTypes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    internal class CheckersCapturer : MonoBehaviour
    {
        [SerializeField] private MapGenerator _mapGenerator;
        [SerializeField] private DisksStorage _disksStorage;
        [SerializeField] private float _diskMoveSpeed;

        private CaptureInfo _captureInfo = new CaptureInfo();
        private CaptureRules _captureRules;

        internal event Action<List<Square>> CaptureContinue;
        internal event Action CaptureIsOver;

        internal void Construct(CaptureRules captureRules)
        {
            _captureRules = captureRules;
        }

        internal void CapturePlayersDisks(string jsonCaptureData)
        {
            CaptureInfo captureInfo = JsonUtility.FromJson<CaptureInfo>(jsonCaptureData);

            Disk capturingDisk;
            Square targetSquare;
            int diskId = captureInfo.id;

            if (_disksStorage.TryGetDisk(out capturingDisk, diskId))
            {
                List<int> widthWayPoints = captureInfo.widthWayPoints;
                List<int> lengthWayPoints = captureInfo.lengthWayPoints;

                for (int i = 0; i < widthWayPoints.Count; i++)
                {
                    if (_mapGenerator.TryGetSquare(out targetSquare, widthWayPoints[i], lengthWayPoints[i]))
                        Capture(capturingDisk, targetSquare);
                    else
                        Debug.LogError("Error: An attempt to move a disk to a non-existent square");
                }
            }
            else
            {
                Debug.LogError("Error: An attempt to move a disk with a non-existent index");
            }

            CaptureIsOver?.Invoke();
        }

        internal void CaptureEnemyDisk(Disk capturingDisk, Square targetSquare)
        {
            Capture(capturingDisk, targetSquare);
            AddWayPoint(targetSquare.WidthPosition, targetSquare.LengthPosition);

            AttemptContinueCapture(capturingDisk);
        }

        private void AttemptContinueCapture(Disk capturingDisk)
        {
            List<Square> availableSquares = _captureRules.GetAvailableSquares(capturingDisk.CurrentSquare);

            if (availableSquares.Count <= 0)
            {
                SendCutDownMessage(capturingDisk);

                CaptureIsOver?.Invoke();
            }
            else
            {
                CaptureContinue?.Invoke(availableSquares);
            }
        }

        private void Capture(Disk capturingDisk, Square targetSquare)
        {
            capturingDisk.JumpTo(targetSquare.DiskPlace);

            if (TryGetEnemyDisk(out Disk enemyDisk, capturingDisk.CurrentSquare, targetSquare))
                DisableDisk(enemyDisk);
            else
                Debug.LogError("Error: An attempt to disable non-existant disk");

            _mapGenerator.SetNewDiskPlanPosition(capturingDisk, targetSquare);
        }

        private void DisableDisk(Disk enemyDisk)
        {
            Square enemySquare = enemyDisk.CurrentSquare;
            _mapGenerator.FreeSquare(enemySquare);

            enemyDisk.gameObject.SetActive(false);
        }

        private bool TryGetEnemyDisk(out Disk enemyDisk, Square currentDiskSquare, Square targetSquare)
        {
            int currentPositionWidth = currentDiskSquare.WidthPosition;
            int currentPositionLength = currentDiskSquare.LengthPosition;
            int targetPositionWidth = targetSquare.WidthPosition;
            int targetPositionLength = targetSquare.LengthPosition;

            int enemyPositionWidth = (targetPositionWidth + currentPositionWidth) / 2;
            int enemyPositionLength = (targetPositionLength + currentPositionLength) / 2;

            _mapGenerator.TryGetDisk(out enemyDisk, enemyPositionWidth, enemyPositionLength);

            return enemyDisk != null;
        }

        private void AddWayPoint(int widthPosition, int lengthPosition)
        {
            _captureInfo.widthWayPoints.Add(widthPosition);
            _captureInfo.lengthWayPoints.Add(lengthPosition);
        }

        private void SendCutDownMessage(Disk capturingDisk)
        {
            _captureInfo.id = capturingDisk.Id;
            MultiplayerManager.Instance.TrySendMessage(MessagesNames.Capture, _captureInfo);

            _captureInfo = new CaptureInfo();
        }
    }
}
