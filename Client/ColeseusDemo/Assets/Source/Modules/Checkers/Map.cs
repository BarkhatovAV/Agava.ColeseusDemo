using System.Collections.Generic;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    internal class Map : MonoBehaviour
    {
        private const int MapWidth = 8;
        private const int MapLength = 8;

        [SerializeField] private List<MapSquare> _mapSquares = new List<MapSquare>();

        private MapSquare[,] _mapPlan = new MapSquare[MapWidth, MapLength];
        private List<MapSquare> _availableMapSquares = new List<MapSquare>();

        private void Awake() =>
            FillMapPlan();

        private void FillMapPlan()
        {
            int squareCounter = 0;

            for (int i = 0; i < MapWidth; i++)
            {
                for (int j = 0; j < MapLength; j++)
                {
                    _mapPlan[i, j] = _mapSquares[squareCounter];
                    _mapSquares[squareCounter].SetMapPosition(i, j);

                    squareCounter++;
                }
            }
        }

        internal MapSquare GetTargetMapSquare(int mapWidthPosition, int mapLengthPosition) =>
            _mapPlan[mapWidthPosition, mapLengthPosition];

        internal void DetermineAvailableMapSquares(MapSquare currentMapSquare, bool isWhiteDisk)
        {
            MapSquare leftTempMapSquare;
            MapSquare rightTempMapSquare;

            if (isWhiteDisk)
            {
                leftTempMapSquare = TryFoundWay(currentMapSquare, 1, 1, isWhiteDisk);
                rightTempMapSquare = TryFoundWay(currentMapSquare, -1, 1, isWhiteDisk);

                if (leftTempMapSquare != null)
                    _availableMapSquares.Add(leftTempMapSquare);

                if(rightTempMapSquare != null)
                    _availableMapSquares.Add(rightTempMapSquare);
            }
            else
            {
                leftTempMapSquare = TryFoundWay(currentMapSquare, -1, -1, isWhiteDisk);
                rightTempMapSquare = TryFoundWay(currentMapSquare, 1, -1, isWhiteDisk);

                if (leftTempMapSquare != null)
                    _availableMapSquares.Add(leftTempMapSquare);

                if (rightTempMapSquare != null)
                    _availableMapSquares.Add(rightTempMapSquare);
            }

            foreach(MapSquare availableMapSquare in _availableMapSquares)
                availableMapSquare.SetAvailable(true);
        }

        internal void OnDiskMoved()
        {
            foreach (MapSquare availableMapSquare in _availableMapSquares)
                availableMapSquare.SetAvailable(false);

            _availableMapSquares.Clear();
        }

        private MapSquare TryFoundWay(MapSquare currentMapSquare, int widthMoveModifier, int lenghtMoveModifier,  bool isWhite)
        {
            int widthPosition = currentMapSquare.MapWidthPosition + widthMoveModifier;
            int lengthPosition = currentMapSquare.MapLengthPosition + lenghtMoveModifier;

            MapSquare tempMapSquare = _mapPlan[widthPosition, lengthPosition];

            if(tempMapSquare != null)
            {
                if (tempMapSquare.IsBusy)
                {
                    if (tempMapSquare.IsWhiteBusy && isWhite)
                        return null;
                    else
                    {
                        MapSquare tempMapSquare2 = _mapPlan[widthPosition * 2, lengthPosition * 2];

                        if (tempMapSquare2.IsBusy)
                            return null;
                        else
                            return tempMapSquare2;
                    }
                }
                else
                    return tempMapSquare;
            }
            else
            {
                return null;
            }
        }
    }
}