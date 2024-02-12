using System.Collections.Generic;

namespace ColyseusDemo.Checkers
{
    internal class AdjecentSquaresDeltas
    {
        private const int FrontLeftWidthIndicator = -1;
        private const int FrontLeftLengthIndicator = -1;

        private const int FrontRightWidthIndicator = -1;
        private const int FrontRightLengthIndicator = 1;

        private const int BackLeftWidthIndicator = 1;
        private const int BackLeftLengthIndicator = -1;

        private const int BackRightWidthIndicator = 1;
        private const int BackRightLengthIndicator = 1;

        public readonly List<int> WidthDeltas = new List<int>();
        public readonly List<int> LengthDeltas = new List<int>();

        public readonly List<int> WhiteWidthDeltas = new List<int>();
        public readonly List<int> WhiteLengthDeltas = new List<int>();

        public readonly List<int> BlackWidthDeltas = new List<int>();
        public readonly List<int> BlackLengthDeltas = new List<int>();

        internal AdjecentSquaresDeltas()
        {
            FillWidthDeltasList();
            FillLengthDeltasList();

            FillWhiteWidthDeltasList();
            FillWhiteLengthDeltasList();

            FillBlackWidthDeltasList();
            FillBlackLengthDeltasList();
        }

        private void FillWidthDeltasList()
        {
            WidthDeltas.Add(FrontLeftWidthIndicator);
            WidthDeltas.Add(FrontRightWidthIndicator);
            WidthDeltas.Add(BackLeftWidthIndicator);
            WidthDeltas.Add(BackRightWidthIndicator);
        }

        private void FillLengthDeltasList()
        {
            LengthDeltas.Add(FrontLeftLengthIndicator);
            LengthDeltas.Add(FrontRightLengthIndicator);
            LengthDeltas.Add(BackLeftLengthIndicator);
            LengthDeltas.Add(BackRightLengthIndicator);
        }

        private void FillWhiteWidthDeltasList()
        {
            WhiteWidthDeltas.Add(FrontLeftWidthIndicator);
            WhiteWidthDeltas.Add(FrontRightWidthIndicator);
        }

        private void FillWhiteLengthDeltasList()
        {
            WhiteLengthDeltas.Add(FrontLeftLengthIndicator);
            WhiteLengthDeltas.Add(FrontRightLengthIndicator);
        }

        private void FillBlackWidthDeltasList()
        {
            BlackWidthDeltas.Add(BackLeftWidthIndicator);
            BlackWidthDeltas.Add(BackRightWidthIndicator);
        }

        private void FillBlackLengthDeltasList()
        {
            BlackLengthDeltas.Add(BackLeftLengthIndicator);
            BlackLengthDeltas.Add(BackRightLengthIndicator);
        }
    }
}