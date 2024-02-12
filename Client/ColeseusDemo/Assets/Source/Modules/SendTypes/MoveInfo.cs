using System;

namespace ColyseusDemo.SendTypes
{
    [Serializable]
    public class MoveInfo
    {
        public string sessionID;
        public int id;
        public int targetMapWidthPosition;
        public int targetMapLengthPosition;
    }
}