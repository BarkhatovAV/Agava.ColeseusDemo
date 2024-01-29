using UnityEngine;

namespace ColyseusDemo.Checkers
{
    internal class DiskPlacer
    {
        private Material _whiteDiskMaterial;
        private Material _blackDiskMaterial;
        private float _appearingSpeed;
        private float _liftingHeight;
        private int _currentId = 0;

        internal DiskPlacer(Material whiteDiskMaterial, Material blackDiskMaterial, float appearingSpeed, float liftingHeight)
        {
            _whiteDiskMaterial = whiteDiskMaterial;
            _blackDiskMaterial = blackDiskMaterial;
            _appearingSpeed = appearingSpeed;
            _liftingHeight = liftingHeight;
        }

        internal void PlaceDisk(TempDisk disk, TempSquare mapSquare, bool isWhiteDisk)
        {
            if (isWhiteDisk)
                disk.Construct(mapSquare, isWhiteDisk, _currentId, _whiteDiskMaterial);
            else
                disk.Construct(mapSquare, isWhiteDisk, _currentId, _blackDiskMaterial);

            Vector3 mapSquarePosition = mapSquare.DiskPlace;
            Vector3 diskStartPosition = new Vector3(mapSquarePosition.x, mapSquarePosition.y + _liftingHeight, mapSquarePosition.z);
            disk.transform.position = diskStartPosition;

            disk.gameObject.SetActive(true);

            disk.SmoothlyMove(disk.transform, mapSquarePosition, _appearingSpeed);

            _currentId++;
        }
    }
}