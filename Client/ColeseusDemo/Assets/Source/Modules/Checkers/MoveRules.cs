using System.Collections.Generic;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    public class MoveRules : MonoBehaviour
    {
        private const string RightDirectionName = "right";
        private const string LeftDirectionName = "left";

        private const int WhiteLeftWidthIndicator = -1;
        private const int WhiteLeftLengthIndicator = 1;
        private const int WhiteRightWidthIndicator = -1;
        private const int WhiteRightLengthIndicator = -1;

        private const int BlackLeftWidthIndicator = -1;
        private const int BlackLeftLengthIndicator = -1;
        private const int BlackRightWidthIndicator = 1;
        private const int BlackRightLengthIndicator = -1;

        [SerializeField] private Map _map;

        private MapSquare _questionDiskPosition;

        private bool _isWhiteDisk => _questionDiskPosition.IsWhiteOccupied;

        internal List<MapSquare> GetAvailableSquares(MapSquare questionDiskPosition)
        {
            _questionDiskPosition = questionDiskPosition;

            List<MapSquare> availableSquares = new List<MapSquare>();
            MapSquare tempMapSquare;

            if (TryGetAdjacentSquare(out tempMapSquare, RightDirectionName))
                availableSquares.Add(tempMapSquare);

            if (TryGetAdjacentSquare(out tempMapSquare, LeftDirectionName))
                availableSquares.Add(tempMapSquare);

            return availableSquares;
        }

        private bool TryGetAdjacentSquare(out MapSquare adjacentSquare, string directionName)
        {
            int directionIndicator;
            int widthPosition = -1;
            int lengthPosition = -1;

            if (_isWhiteDisk)
                directionIndicator = 1;
            else
                directionIndicator = -1;

            if (directionName == RightDirectionName)
            {
                widthPosition = _questionDiskPosition.WidthPosition + WhiteRightWidthIndicator * directionIndicator;
                lengthPosition = _questionDiskPosition.LengthPosition + WhiteRightLengthIndicator * directionIndicator;
            }
            else if (directionName == LeftDirectionName)
            {
                widthPosition = _questionDiskPosition.WidthPosition + WhiteLeftWidthIndicator * directionIndicator;
                lengthPosition = _questionDiskPosition.LengthPosition + WhiteLeftLengthIndicator * directionIndicator;
            }

            if (IsMapSquareExist(widthPosition, lengthPosition))
            {
                MapSquare mapSquare = _map.GetMapSquare(widthPosition, lengthPosition);

                if (IsMapSquareOccupied(mapSquare))
                {
                    Debug.Log($"Занят! Ширина: {widthPosition}, длина: {lengthPosition}, , имя: {mapSquare.gameObject.name}");
                    adjacentSquare = null;

                    return false;
                }
                else
                {
                    Debug.Log($"Длина: {lengthPosition}, ширина: {widthPosition}");
                    adjacentSquare = mapSquare;

                    return true;
                }
            }

            adjacentSquare = null;
            return false;
        }

        private bool IsMapSquareOccupied(MapSquare mapSquare)
        {
            if (mapSquare.IsOccupied)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsMapSquareExist(int widthPosition, int lengthPosition) =>
            widthPosition > 0 && lengthPosition > 0 && widthPosition < 8 && lengthPosition < 8;

        private bool TryCutDown()
        {
            Debug.Log("Пытается срубить");
            return true;
        }
    }
}