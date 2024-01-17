using UnityEngine;

namespace ColyseusDemo.Multiplayer
{
    public class PlayerSettings : MonoBehaviour
    {
        //Для чего статичный модификатор?
        public static PlayerSettings Instance { get; private set; }

        public string Login { get; private set; }

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return; //А почему именно return, а не блок else для кода далее?
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        internal void SetLogin(string login)
        {
            Login = login;
        }
    }
}