using System.Collections.Generic;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    internal abstract class Rules : MonoBehaviour
    {
        [SerializeField] protected MapGenerator MapGenerator;

        protected AdjecentSquaresDeltas AdjecentSquaresDeltas;
        protected List<Square> AvailableSquares = new List<Square>();
        protected List<int> WidthDeltas;
        protected List<int> LengthDeltas;
        protected Square CurrentDiskPosition;

        protected bool IsCurrentDiskWhite => CurrentDiskPosition.IsWhiteOccupied;

        private void Awake()
        {
            AdjecentSquaresDeltas = new AdjecentSquaresDeltas();
        }

        internal abstract List<Square> GetAvailableSquares(Square currentDiskPosition);

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