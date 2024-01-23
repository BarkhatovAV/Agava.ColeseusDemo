using ColyseusDemo.Multiplayer;
using ColyseusDemo.SendTypes;
using System.Collections.Generic;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    [RequireComponent(typeof(Selector))]
    public class DiskMover : MonoBehaviour
    {
        [SerializeField] private List<Disk> _disks = new List<Disk>();
        [SerializeField] private Map _map;

        private void OnEnable() =>
            MultiplayerManager.Instance.DiskMoved += MoveDisk;

        private void OnDisable() =>
            MultiplayerManager.Instance.DiskMoved -= MoveDisk;

        private void MoveDisk(string jsonMoveData)
        {
            MoveInfo moveInfo = JsonUtility.FromJson<MoveInfo>(jsonMoveData);

            int diskId = moveInfo.id;
            Disk movableDisk = _disks[diskId];

            int targetMapWidthPosition = moveInfo.targetMapWidthPosition;
            int targetMapLengthPosition = moveInfo.targetMapLengthPosition;
            MapSquare targetMapSquare = _map.GetTargetMapSquare(targetMapWidthPosition, targetMapLengthPosition);

            movableDisk.MoveTo(targetMapSquare);
        }
    }
}