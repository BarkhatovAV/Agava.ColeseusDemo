using UnityEngine;

namespace ColyseusDemo.Players
{
    public class PlayerSettings : MonoBehaviour
    {
        public static PlayerSettings CurrentPlayerSettings { get; private set; }

        public string Login { get; private set; }
        public bool IsWhitePlayer { get; private set; }

        private void Awake()
        {
            if (CurrentPlayerSettings)
            {
                Destroy(gameObject);
            }
            else
            {
                CurrentPlayerSettings = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (CurrentPlayerSettings == this)
                CurrentPlayerSettings = null;
        }

        public void SetLogin(string login) =>
            Login = login;
        
        //Вот тут
        internal void SetWhitePlayerStatus(bool value) =>
            IsWhitePlayer = value;
    }
}