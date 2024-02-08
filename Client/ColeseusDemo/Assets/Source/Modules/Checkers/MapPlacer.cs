using UnityEngine;

namespace ColyseusDemo.Checkers
{
    internal class MapPlacer
    {
        private const int DirectionModifier = -1;
        private const int EvenNumbersDevisor = 2;

        private int _mapWidth;
        private Vector3 _startMapPosition;
        private GameObject _mapSquarePrefab;
        private Material _whiteMapSquareMaterial;
        private Material _blackMapSquareMaterial;
        private Material _firstMapSquareMaterial;
        private Material _secondMapSquareMaterial;
        private Vector3 _currentPosition;
        private Vector3 _widthDeltaVector;
        private Vector3 _lengthDeltaVector;
        private int _placedSquaresCount = 0;
        private float _liftingHeight;

        internal MapPlacer(int mapWidth, Vector3 startMapPosition, GameObject squarePrefab, Material whiteSquareMaterial, Material blackSquareMaterial, float liftingHeight)
        {
            _mapWidth = mapWidth;
            _startMapPosition = startMapPosition;
            _mapSquarePrefab = squarePrefab;
            _whiteMapSquareMaterial = whiteSquareMaterial;
            _blackMapSquareMaterial = blackSquareMaterial;
            _currentPosition = startMapPosition;
            _liftingHeight = liftingHeight;

            _firstMapSquareMaterial = _whiteMapSquareMaterial;
            _secondMapSquareMaterial = _blackMapSquareMaterial;

            DetermineDeltas();
        }

        internal void PlaceSquare(Square square)
        {
            if (_placedSquaresCount % EvenNumbersDevisor == 0)
                square.SetDefaultMaterial(_firstMapSquareMaterial);
            else
                square.SetDefaultMaterial(_secondMapSquareMaterial);

            Vector3 startMapSquarePosition = new Vector3(_currentPosition.x, _currentPosition.y + _liftingHeight, _currentPosition.z);
            Transform mapSquareTransform = square.transform;
            mapSquareTransform.position = startMapSquarePosition;
            square.gameObject.SetActive(true);

            square.FloatDown(_currentPosition);

            _placedSquaresCount++;

            DetermineNextPosition();
        }

        private void StartNewRow() =>
            _currentPosition = new Vector3 (_currentPosition.x, 0, _startMapPosition.z) + _lengthDeltaVector;

        private void DetermineNextPosition()
        {
            _currentPosition += _widthDeltaVector;

            if (_placedSquaresCount % _mapWidth == 0)
            {
                StartNewRow();
                ReplaceMaterials();
            }
        }

        private void ReplaceMaterials()
        {
            Material tempMaterial = _secondMapSquareMaterial;
            _secondMapSquareMaterial = _firstMapSquareMaterial;
            _firstMapSquareMaterial = tempMaterial;
        }

        private void DetermineDeltas()
        {
            Transform mapSquareTransform = _mapSquarePrefab.gameObject.GetComponent<Transform>();

            DetermineWidthDeltaVector(mapSquareTransform);
            DetermineLengthDeltaVector(mapSquareTransform);
        }

        private void DetermineWidthDeltaVector(Transform mapSquareTransform)
        {
            float zDelta = mapSquareTransform.localScale.z * DirectionModifier;

            _widthDeltaVector = new Vector3(0, 0, zDelta);
        }

        private void DetermineLengthDeltaVector(Transform mapSquareTransform)
        {
            float xDelta = mapSquareTransform.localScale.x * DirectionModifier;

            _lengthDeltaVector = new Vector3(xDelta, 0, 0);
        }
    }
}