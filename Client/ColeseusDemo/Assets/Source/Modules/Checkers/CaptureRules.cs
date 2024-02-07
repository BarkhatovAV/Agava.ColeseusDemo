using System;
using System.Collections.Generic;

namespace ColyseusDemo.Checkers
{
    internal class CaptureRules : Rules
    {
        private const int MovingDelta = 1;

        private List<Square> _suitableAdjecentSquares = new List<Square>();
        private Square _adjecentSquare;

        internal CaptureRules(AdjecentSquaresDeltas adjecentSquaresDeltas, MapGenerator mapGenerator) : base(adjecentSquaresDeltas, mapGenerator) { }

        internal bool IsCutDown(Disk disk, Square targetMapSquare)
        {
            int currentPositionWidth = disk.CurrentSquare.WidthPosition;
            int targetPositionWidth = targetMapSquare.WidthPosition;

            return Math.Abs(targetPositionWidth - currentPositionWidth) > MovingDelta;
        }

        internal override List<Square> GetAvailableSquares(Square currentDiskPosition)
        {
            CurrentDiskSquare = currentDiskPosition;

            AvailableSquares.Clear();
            _suitableAdjecentSquares.Clear();

            DetermineDeltas();
            FillSuitableAdjecentSquares();
            FillAvailableSquares();

            return AvailableSquares;
        }

        protected override void DetermineDeltas()
        {
            WidthDeltas = AdjecentSquaresDeltas.WidthDeltas;
            LengthDeltas = AdjecentSquaresDeltas.LengthDeltas;
        }

        protected override void FillAvailableSquares()
        {
            Square availableSquare;
            int widthPosition;
            int lengthPosition;

            for (int i = 0; i < _suitableAdjecentSquares.Count; i++)
            {
                _adjecentSquare = _suitableAdjecentSquares[i];

                widthPosition = _adjecentSquare.WidthPosition + (_adjecentSquare.WidthPosition - CurrentDiskSquare.WidthPosition);
                lengthPosition = _adjecentSquare.LengthPosition + (_adjecentSquare.LengthPosition - CurrentDiskSquare.LengthPosition);

                if (TryGetAvailableMapSquare(out availableSquare, widthPosition, lengthPosition))
                    AvailableSquares.Add(availableSquare);
            }
        }

        private void FillSuitableAdjecentSquares()
        {
            for (int i = 0; i < WidthDeltas.Count; i++)
            {
                if (TryGetSuitableAdjecentSquare(out _adjecentSquare, WidthDeltas[i], LengthDeltas[i]))
                    _suitableAdjecentSquares.Add(_adjecentSquare);
            }
        }

        private bool TryGetSuitableAdjecentSquare(out Square adjecentSquare, int widthDelta, int lengthDelta)
        {
            Square suitableSquare = null;
            int currentDiskWidth = CurrentDiskSquare.WidthPosition;
            int currentDiskLength = CurrentDiskSquare.LengthPosition;

            int adjecentSquareWidth = currentDiskWidth + widthDelta;
            int adjecentSquareLength = currentDiskLength + lengthDelta;

            bool isCorrectSquare = MapGenerator.TryGetSquare(out adjecentSquare, adjecentSquareWidth, adjecentSquareLength);

            if (isCorrectSquare && adjecentSquare.IsOccupied && (IsCurrentDiskWhite != adjecentSquare.IsWhiteOccupied))
                suitableSquare = adjecentSquare;

            return suitableSquare != null;
        }
    }
}