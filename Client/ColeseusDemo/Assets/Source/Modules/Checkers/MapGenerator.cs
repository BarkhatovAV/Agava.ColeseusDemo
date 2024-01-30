using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    internal class MapGenerator : MonoBehaviour
    {
        [SerializeField] private const int MapWidth = 8;
        [SerializeField] private const int MapLength = 8;

        [SerializeField] private List<MapSquare> _mapSquares = new List<MapSquare>();
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

        private MapSquare[,] _mapPlan = new MapSquare[MapWidth, MapLength];
        private Disk[,] _disksPlan = new Disk[MapWidth, MapLength];
        private MapPlacer _mapPlacer;
        private DiskPlacer _diskPlacer;
        private Coroutine _coroutine;

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

        internal MapSquare GetMapSquare(int mapWidthPosition, int mapLengthPosition) =>
            _mapPlan[mapWidthPosition, mapLengthPosition];

        internal Disk GetDisk(int widthPosition, int lengthPosition) =>
            _disksPlan[widthPosition, lengthPosition];

        internal void SetNewDiskPlanPosition(Disk disk, int newWidthPosition, int newLengthPosition)
        {
            _disksPlan[disk.WidthPosition, disk.LengthPosition] = null;
            _disksPlan[newWidthPosition, newLengthPosition] = disk;
        }

        private void PlaceMap(bool isWhite)
        {
            if (_coroutine == null)
                _coroutine = StartCoroutine(FillMapPlan(isWhite));
        }

        private IEnumerator FillMapPlan(bool isWhitePlayer)
        {
            int squareCount = 0;
            MapSquare tempSquare;
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
            MapSquare mapSquare;
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

                yield return new WaitForSeconds(0.1f);
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

                yield return new WaitForSeconds(0.1f);
            }

            DisksPlaced?.Invoke(isWhitePlayer);
        }
    }
}