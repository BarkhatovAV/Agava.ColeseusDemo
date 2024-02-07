using System.Collections.Generic;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    internal abstract class Rules
    {
        protected AdjecentSquaresDeltas AdjecentSquaresDeltas;
        protected MapGenerator MapGenerator;
        protected List<Square> AvailableSquares = new List<Square>();
        protected List<int> WidthDeltas;
        protected List<int> LengthDeltas;
        protected Square CurrentDiskSquare;

        protected bool IsCurrentDiskWhite => CurrentDiskSquare.IsWhiteOccupied;

        internal abstract List<Square> GetAvailableSquares(Square currentDiskPosition);

        internal Rules(AdjecentSquaresDeltas adjecentSquaresDeltas, MapGenerator mapGenerator)
        {
            AdjecentSquaresDeltas = adjecentSquaresDeltas;
            MapGenerator = mapGenerator;
        }

        protected abstract void FillAvailableSquares();

        protected bool TryGetAvailableMapSquare(out Square availableMapSquare, int widthPosition, int lengthPosition)
        {
            bool isCorrectMapSquare = MapGenerator.TryGetSquare(out availableMapSquare, widthPosition, lengthPosition);

            if (isCorrectMapSquare && availableMapSquare.IsOccupied)
                availableMapSquare = null;

            return availableMapSquare != null;
        }

        protected abstract void DetermineDeltas();
    }
}