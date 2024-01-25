using System;

namespace ColyseusDemo.Checkers
{
    [Serializable]
    public class PlayerSettings
    {
        public string Login { get; private set; }

        public void SetLogin(string login) =>
            Login = login;
    }
}