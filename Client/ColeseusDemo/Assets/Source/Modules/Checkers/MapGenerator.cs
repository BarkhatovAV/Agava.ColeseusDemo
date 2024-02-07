using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    internal class MapGenerator : MonoBehaviour
    {
        private const int MapWidth = 8;
        private const int MapLength = 8;

        [SerializeField] private List<Square> _mapSquares = new List<Square>();
        [SerializeField] private List<Disk> _whiteDisks = new List<Disk>();
        [SerializeField] private List<Disk> _blackDisks = new List<Disk>();
        [SerializeField] private List<int> _whiteDisksPositions = new List<int>();
        [SerializeField] private List<int> _blackDisksPositions = new List<int>();
        [SerializeField] private CheckersPlayer _checkersPlayer;
        [SerializeField] private Vector3 _startMapPosition;
        [SerializeField] private GameObject _mapSquarePrefab;
        [SerializeField] private Material _whiteMapSquareMaterial;
        [SerializeField] private Material _blackMapSquareMaterial;
        [SerializeField] private Material _whiteDiskMaterial;
        [SerializeField] private Material _blackDiskMaterial;
        [SerializeField] private float _mapSquareAppearingSpeed = 8f;
        [SerializeField] private float _diskAppearingSpeed = 8f;
        [SerializeField] private float _mapSquareLiftingHeight = 3f;
        [SerializeField] private float _diskLiftingHeight = 3f;

        private Square[,] _mapPlan = new Square[MapWidth, MapLength];
        private Disk[,] _disksPlan = new Disk[MapWidth, MapLength];
        private MapPlacer _mapPlacer;
        private DiskPlacer _diskPlacer;
        private Coroutine _coroutine;
        private WaitForSeconds _timeBetweenDiskAvailable = new WaitForSeconds(0.1f);

        public event Action<bool> DisksPlaced;

        private void Awake()
        {
            _mapPlacer = new MapPlacer(MapWidth, _startMapPosition, _mapSquarePrefab, _whiteMapSquareMaterial, _blackMapSquareMaterial, _mapSquareAppearingSpeed, _mapSquareLiftingHeight);
            _diskPlacer = new DiskPlacer(_whiteDiskMaterial, _blackDiskMaterial, _diskAppearingSpeed, _diskLiftingHeight);
        }

        private void OnEnable() =>
            _checkersPlayer.SideDetermined += PlaceMap;

        private void OnDisable() =>
            _checkersPlayer.SideDetermined -= PlaceMap;

        internal bool TryGetSquare(out Square square, int widthPosition, int lengthPosition)
        {
            bool isMapSquareExist = IsCorrectPosition(widthPosition, lengthPosition);

            if (isMapSquareExist)
                square = _mapPlan[widthPosition, lengthPosition];
            else
                square = null;

            return isMapSquareExist;
        }

        internal bool TryGetDisk(out Disk disk, int widthPosition, int lengthPosition)
        {
            bool isMapSquateExist = TryGetSquare(out Square mapSquare, widthPosition, lengthPosition);

            if (isMapSquateExist && mapSquare.IsOccupied)
                disk = _disksPlan[widthPosition, lengthPosition];
            else
                disk = null;

            return disk != null;
        }

        internal void SetNewDiskPlanPosition(Disk disk, Square targetSquare)
        {
            FreeSquare(disk.CurrentSquare);
            _disksPlan[targetSquare.WidthPosition, targetSquare.LengthPosition] = disk;

            disk.SetCurrentMapSquare(targetSquare);
            targetSquare.Occupy(disk.IsWhite);
        }

        internal void FreeSquare(Square square)
        {
            _disksPlan[square.WidthPosition, square.LengthPosition] = null;
            square.Free();
        }

        private bool IsCorrectPosition(int widthPosition, int lengthPosition) =>
            widthPosition >= 0 && lengthPosition >= 0 && widthPosition < MapWidth && lengthPosition < MapLength;

        private void PlaceMap(bool isWhite)
        {
            if (_coroutine == null)
                _coroutine = StartCoroutine(FillMapPlan(isWhite));
        }

        private IEnumerator FillMapPlan(bool isWhitePlayer)
        {
            int squareCount = 0;
            Square tempSquare;
            WaitForSeconds timeBetweenMapSquareAppearence = new WaitForSeconds(0.04f);

            for (int i = 0; i < MapWidth; i++)
            {
                for (int j = 0; j < MapLength; j++)
                {
                    tempSquare = _mapSquares[squareCount];

                    tempSquare.Construct(i, j);
                    _mapPlan[i, j] = tempSquare;
                    _mapPlacer.PlaceMapSquare(tempSquare);

                    squareCount++;

                    yield return timeBetweenMapSquareAppearence;
                }
            }

            yield return StartCoroutine(FillDisksPlan(isWhitePlayer));
        }

        private IEnumerator FillDisksPlan(bool isWhitePlayer)
        {
            Disk disk;
            Square mapSquare;
            int widthPosition;
            int lengthPosition;

            for (int i = 0; i < _blackDisks.Count; i++)
            {
                disk = _blackDisks[i];

                int mapSquareIndex = _blackDisksPositions[i];
                mapSquare = _mapSquares[mapSquareIndex];

                widthPosition = mapSquare.WidthPosition;
                lengthPosition = mapSquare.LengthPosition;

                _disksPlan[widthPosition, lengthPosition] = disk;
                _diskPlacer.PlaceDisk(disk, mapSquare, false);

                yield return _timeBetweenDiskAvailable;
            }

            for (int i = 0; i < _whiteDisks.Count; i++)
            {
                disk = _whiteDisks[i];

                int mapSquareIndex = _whiteDisksPositions[i];
                mapSquare = _mapSquares[mapSquareIndex];

                widthPosition = mapSquare.WidthPosition;
                lengthPosition = mapSquare.LengthPosition;

                _disksPlan[widthPosition, lengthPosition] = disk;
                _diskPlacer.PlaceDisk(disk, mapSquare, true);

                yield return _timeBetweenDiskAvailable;
            }

            DisksPlaced?.Invoke(isWhitePlayer);
        }
    }
}