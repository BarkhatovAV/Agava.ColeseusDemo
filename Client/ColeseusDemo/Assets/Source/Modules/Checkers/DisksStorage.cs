using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    public class DisksStorage : MonoBehaviour
    {
        [SerializeField] private MapGenerator _mapGenerator;
        [SerializeField] private List<Disk> _allDisks = new List<Disk>();

        private List<Disk> _sideDisks = new List<Disk>();

        private void OnEnable() =>
            _mapGenerator.DisksPlaced += SetSideDisks;

        private void OnDisable() =>
            _mapGenerator.DisksPlaced -= SetSideDisks;

        internal bool IsCorrectDisk(Disk disk) =>
            _sideDisks.Contains(disk);

        internal bool TryGetDisk(out Disk disk, int diskId)
        {
            bool isCorrectDisk = diskId >= 0 && diskId < _allDisks.Count;

            if (isCorrectDisk)
                disk = _allDisks[diskId];
            else
                disk = null;

            return isCorrectDisk;
        }

        private void SetSideDisks(bool isWhitePlayer)
        {
            SetDisksIds();

            var availableDisks = from Disk disk in _allDisks
                                 where disk.IsWhite == isWhitePlayer
                                 select disk;

            _sideDisks = availableDisks.ToList();
        }

        private void SetDisksIds()
        {
            for (int i = 0; i < _allDisks.Count; i++)
                _allDisks[i].SetId(i);
        }
    }
}