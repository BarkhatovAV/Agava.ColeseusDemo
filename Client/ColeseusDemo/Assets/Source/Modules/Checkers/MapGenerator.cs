using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    internal class MapGenerator : MonoBehaviour
    {
        internal const int MapWidth = 8;
        internal const int MapLength = 8;

        [SerializeField] private List<Square> _squares = new List<Square>();
        [SerializeField] private List<Disk> _whiteDisks = new List<Disk>();
        [SerializeField] private List<Disk> _blackDisks = new List<Disk>();
        [SerializeField] private List<int> _whiteDisksPositions = new List<int>();
        [SerializeField] private List<int> _blackDisksPositions = new List<int>();
        [SerializeField] private WaitForSeconds _timeBetweenDiskAvailable = new WaitForSeconds(0.1f);
        [SerializeField] private WaitForSeconds _timeBetweenSquareAvailable = new WaitForSeconds(0.04f);

        private Square[,] _mapPlan = new Square[MapWidth, MapLength];
        private Disk[,] _disksPlan = new Disk[MapWidth, MapLength];
        private CheckersPlayer _checkersPlayer;
        private MapPlacer _mapPlacer;
        private DiskPlacer _diskPlacer;
        private Coroutine _mapPlaceCoroutine;

        public event Action<bool> DisksPlaced;

        private void OnEnable()
        {
            if (_checkersPlayer != null)
                _checkersPlayer.SideDetermined += PlaceMap;
        }

        private void OnDisable()
        {
            if (_checkersPlayer != null)
                _checkersPlayer.SideDetermined -= PlaceMap;
        }

        internal void Construct(CheckersPlayer checkersPlayer, MapPlacer mapPlacer, DiskPlacer diskPlacer)
        {
            _checkersPlayer = checkersPlayer;
            _mapPlacer = mapPlacer;
            _diskPlacer = diskPlacer;

            _checkersPlayer.SideDetermined += PlaceMap;
        }

        internal bool TryGetSquare(out Square square, int widthPosition, int lengthPosition)
        {
            bool isSquareExist = IsCorrectPosition(widthPosition, lengthPosition);

            if (isSquareExist)
                square = _mapPlan[widthPosition, lengthPosition];
            else
                square = null;

            return isSquareExist;
        }

        internal bool TryGetDisk(out Disk disk, int widthPosition, int lengthPosition)
        {
            bool isSquateExist = TryGetSquare(out Square mapSquare, widthPosition, lengthPosition);

            if (isSquateExist && mapSquare.IsOccupied)
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
            if (_mapPlaceCoroutine == null)
                _mapPlaceCoroutine = StartCoroutine(FillMapPlan(isWhite));
        }

        private IEnumerator FillMapPlan(bool isWhitePlayer)
        {
            int squareCount = 0;
            Square tempSquare;

            for (int i = 0; i < MapWidth; i++)
            {
                for (int j = 0; j < MapLength; j++)
                {
                    tempSquare = _squares[squareCount];

                    tempSquare.Construct(i, j);
                    _mapPlan[i, j] = tempSquare;
                    _mapPlacer.PlaceMapSquare(tempSquare);

                    squareCount++;

                    yield return _timeBetweenSquareAvailable;
                }
            }

            yield return StartCoroutine(FillDisksPlan(isWhitePlayer));
        }

        private IEnumerator FillDisksPlan(bool isWhitePlayer)
        {
            Disk disk;
            Square square;
            int widthPosition;
            int lengthPosition;
            int squareIndex;

            for (int i = 0; i < _blackDisks.Count; i++)
            {
                disk = _blackDisks[i];

                squareIndex = _blackDisksPositions[i];
                square = _squares[squareIndex];

                widthPosition = square.WidthPosition;
                lengthPosition = square.LengthPosition;

                _disksPlan[widthPosition, lengthPosition] = disk;
                _diskPlacer.PlaceDisk(disk, square, false);

                yield return _timeBetweenDiskAvailable;
            }

            for (int i = 0; i < _whiteDisks.Count; i++)
            {
                disk = _whiteDisks[i];

                squareIndex = _whiteDisksPositions[i];
                square = _squares[squareIndex];

                widthPosition = square.WidthPosition;
                lengthPosition = square.LengthPosition;

                _disksPlan[widthPosition, lengthPosition] = disk;
                _diskPlacer.PlaceDisk(disk, square, true);

                yield return _timeBetweenDiskAvailable;
            }

            DisksPlaced?.Invoke(isWhitePlayer);
        }
    }
}