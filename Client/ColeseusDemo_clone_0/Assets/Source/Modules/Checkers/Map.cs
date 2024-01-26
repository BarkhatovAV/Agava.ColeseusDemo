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

        private void Awake() =>
            FillMapPlan();

        internal MapSquare GetMapSquare(int mapWidthPosition, int mapLengthPosition) =>
            _mapPlan[mapWidthPosition, mapLengthPosition];

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
    }
}