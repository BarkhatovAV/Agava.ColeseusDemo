namespace ColyseusDemo.Players
{
    public class PlayerSettings
    {
        public string Login { get; private set; }
        public bool IsTurnReady { get; private set; }

        private Player _enemy;

        public void SetLogin(string login) =>
            Login = login;

        public void SetEnemy(Player enemy)
        {
            _enemy = enemy;
            _enemy.OnIsTurnReadyChange(GetTurn);
        }

        private void GetTurn(bool currentValue, bool previousValue) =>
            IsTurnReady = currentValue;
    }
}