using System;

namespace ColyseusDemo.ChessMovement
{
    public partial class ChessSpawner
    {
        [Serializable]
        public class SpawnData
        {
            public string sessionID;
            public string id;
            public float x;
            public float z;
        }
    }
}