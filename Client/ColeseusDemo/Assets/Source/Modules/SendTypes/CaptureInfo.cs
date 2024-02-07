using System.Collections.Generic;

namespace ColyseusDemo.SendTypes
{
    public class CaptureInfo
    {
        public string sessionID;
        public int id;
        public List<int> cutDisksIndexes = new List<int>();
        public List<int> widthWayPoints = new List<int>();
        public List<int> lengthWayPoints = new List<int>();
    }
}