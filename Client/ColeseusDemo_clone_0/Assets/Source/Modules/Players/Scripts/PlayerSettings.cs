using System;
using ColyseusDemo.Multiplayer;

namespace ColyseusDemo.Players
{
    [Serializable]
    public class PlayerSettings
    {
        public string Login { get; private set; }
        public bool IsTurnReady { get; private set; }
        public bool IsWhitePlayer { get; private set; }

        private MultiplayerManager _multiplayerManager;
        private Player _enemy;

        public event Action EnemyFound;

        public Player Enemy => _enemy;

        public PlayerSettings(MultiplayerManager multiplayerManager)
        {
            _multiplayerManager = multiplayerManager;

            _multiplayerManager.EnemyFound += SetEnemy;
            _multiplayerManager.SideDefined += SetIsWhitePlayer;
        }

        public void SetLogin(string login) =>
            Login = login;

        public void SetIsWhitePlayer(bool value) =>
            IsWhitePlayer = value;

        private void SetEnemy(Player enemy)
        {
            _enemy = enemy;
            _enemy.OnIsTurnReadyChange(GetTurn);

            EnemyFound?.Invoke();
        }

        private void GetTurn(bool currentValue, bool previousValue) =>
            IsTurnReady = currentValue;
    }
}