using UnityEngine;

namespace ColyseusDemo.Checkers
{
    internal class DiskPlacer
    {
        private Material _whiteDiskMaterial;
        private Material _blackDiskMaterial;
        private float _liftingHeight;

        internal DiskPlacer(Material whiteDiskMaterial, Material blackDiskMaterial, float liftingHeight)
        {
            _whiteDiskMaterial = whiteDiskMaterial;
            _blackDiskMaterial = blackDiskMaterial;
            _liftingHeight = liftingHeight;
        }

        internal void PlaceDisk(Disk disk, Square square, bool isWhiteDisk)
        {
            if (isWhiteDisk)
                disk.Construct(square, isWhiteDisk, _whiteDiskMaterial);
            else
                disk.Construct(square, isWhiteDisk, _blackDiskMaterial);

            Vector3 squarePosition = square.DiskPlace;
            Vector3 diskStartPosition = new Vector3(squarePosition.x, squarePosition.y + _liftingHeight, squarePosition.z);
            disk.transform.position = diskStartPosition;

            disk.gameObject.SetActive(true);
            disk.FloatDown(squarePosition);
        }
    }
}