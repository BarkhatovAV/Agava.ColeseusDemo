using System.Collections.Generic;

namespace ColyseusDemo.Checkers
{
    internal class MoveRules : Rules
    {
        internal MoveRules(AdjecentSquaresDeltas adjecentSquaresDeltas, MapGenerator mapGenerator) : base(adjecentSquaresDeltas, mapGenerator) { }

        internal override List<Square> GetAvailableSquares(Square currentDiskPosition)
        {
            CurrentDiskSquare = currentDiskPosition;

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
            Square availableSquare;

            for (int i = 0; i < WidthDeltas.Count; i++)
            {
                TryGetAvailableMapSquare(out availableSquare, CurrentDiskSquare.WidthPosition + WidthDeltas[i], CurrentDiskSquare.LengthPosition + LengthDeltas[i]);

                if (availableSquare != null)
                    AvailableSquares.Add(availableSquare);
            }
        }
    }
}