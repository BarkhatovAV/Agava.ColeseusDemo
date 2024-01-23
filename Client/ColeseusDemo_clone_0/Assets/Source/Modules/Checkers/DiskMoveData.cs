using System;
using System.Collections.Generic;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    [Serializable]
    public class DiskMoveData
    {
        public int DiskIndex;
        public List<Vector3> DiskPath;
    }
}