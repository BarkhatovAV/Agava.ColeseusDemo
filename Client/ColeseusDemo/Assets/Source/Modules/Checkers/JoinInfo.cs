using UnityEngine;

namespace ColyseusDemo.Checkers
{
    [CreateAssetMenu(fileName = "JoinInfo", menuName = "Info/Create new JoinInfo")]
    public class JoinInfo : ScriptableObject
    {
        public bool IsIdJoining { get; private set; } = false;
        public string JoinId { get; private set; }

        public void SetId(string id)
        {
            JoinId = id;
            IsIdJoining = true;
        }

        public void CleanInfo()
        {
            JoinId = null;
            IsIdJoining = false;
        }
    }
}