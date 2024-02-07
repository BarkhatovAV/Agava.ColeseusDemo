using System.Collections.Generic;

namespace ColyseusDemo.Checkers
{
    internal class MoveRules : Rules
    {
        internal override List<Square> GetAvailableSquares(Square currentDiskPosition)
        {
            CurrentDiskPosition = currentDiskPosition;

            DetermineDeltas();
            FillAvailableSquares();

            return AvailableSquares;
        }

        protected override void DetermineDeltas()
        {
            if (IsCurrentDiskWhite)
            {
                WidthDeltas = AdjecentSquaresDeltas.WhiteWidthDeltas;
                LengthDeltas = AdjecentSquaresDeltas.WhiteLengthDeltas;
            }
            else
            {
                WidthDeltas = AdjecentSquaresDeltas.BlackWidthDeltas;
                LengthDeltas = AdjecentSquaresDeltas.BlackLengthDeltas;
            }
        }

        protected override void FillAvailableSquares()
        {
            Square availableMapSquare;

            for (int i = 0; i < WidthDeltas.Count; i++)
            {
                TryGetAvailableMapSquare(out availableMapSquare, CurrentDiskPosition.WidthPosition + WidthDeltas[i], CurrentDiskPosition.LengthPosition + LengthDeltas[i]);

                if (availableMapSquare != null)
                    AvailableSquares.Add(availableMapSquare);
            }
        }
    }
}