using System.Collections.Generic;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    internal class MoveRules : MonoBehaviour
    {
        private const string RightDirectionName = "right";
        private const string LeftDirectionName = "left";

        private const int WhiteLeftWidthIndicator = -1;
        private const int WhiteLeftLengthIndicator = 1;
        private const int WhiteRightWidthIndicator = -1;
        private const int WhiteRightLengthIndicator = -1;

        [SerializeField] private MapGenerator _mapGenerator;

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
                MapSquare mapSquare = _mapGenerator.GetMapSquare(widthPosition, lengthPosition);

                if (mapSquare.IsOccupied)
                {
                    TryCutDown(mapSquare, out adjacentSquare);
                }
                else
                {
                    adjacentSquare = mapSquare;
                }
            }
            else
            {
                adjacentSquare = null;
            }

            return adjacentSquare != null;
        }

        private bool IsMapSquareExist(int widthPosition, int lengthPosition) =>
            widthPosition >= 0 && lengthPosition >= 0 && widthPosition < 8 && lengthPosition < 8;

        private bool TryCutDown(MapSquare occupiedMapSquare, out MapSquare adjacentSquare)
        {
            int questionDiskWidth = _questionDiskPosition.WidthPosition;
            int questionDiskLength = _questionDiskPosition.LengthPosition;
            int occupiedMapSquareWidth = occupiedMapSquare.WidthPosition;
            int occupiedMapSquareLength = occupiedMapSquare.LengthPosition;

            int adjacentSquareWidth = questionDiskWidth + (occupiedMapSquareWidth - questionDiskWidth) * 2;
            int adjacentSquareLength = questionDiskLength + (occupiedMapSquareLength - questionDiskLength) * 2;

            if (IsMapSquareExist(adjacentSquareWidth, adjacentSquareLength))
            {
                MapSquare checkedSquare = _mapGenerator.GetMapSquare(adjacentSquareWidth, adjacentSquareLength);

                if (checkedSquare.IsOccupied)
                {
                    adjacentSquare = null;
                }
                else
                {
                    if (_isWhiteDisk == occupiedMapSquare.IsWhiteOccupied)
                        adjacentSquare = null;
                    else
                        adjacentSquare = checkedSquare;
                }
            }
            else
            {
                adjacentSquare = null;
            }

            return adjacentSquare != null;
        }
    }
}