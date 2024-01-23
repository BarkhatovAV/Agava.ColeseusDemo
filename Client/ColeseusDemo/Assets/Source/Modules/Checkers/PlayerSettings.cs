using System;
using ColyseusDemo.Multiplayer;

namespace ColyseusDemo.Checkers
{
    [Serializable]
    public class PlayerSettings
    {
        public string Login { get; private set; }
        public bool IsTurnReady => _player.isTurnReady;
        public bool IsWhitePlayer { get; private set; }

        private MultiplayerManager _multiplayerManager;
        private Player _player;
        private Player _enemy;

        public event Action EnemyFound;

        public Player Enemy => _enemy;

        public PlayerSettings(MultiplayerManager multiplayerManager)
        {
            _multiplayerManager = multiplayerManager;

            _multiplayerManager.EnemyFound += SetEnemy;
            _multiplayerManager.PlayerFound += SetPlayer;
            _multiplayerManager.SideDefined += SetIsWhitePlayer;
        }

        public void SetLogin(string login) =>
            Login = login;

        public void SetIsWhitePlayer(bool value)
        {
            IsWhitePlayer = value;

            if (IsWhitePlayer)
                GetTurn(false, true);
            else
                PassTurn();
        }

        internal void PassTurn() =>
            _player.isTurnReady = false;

        private void SetEnemy(Player enemy)
        {
            _enemy = enemy;
            _enemy.OnIsTurnReadyChange(GetTurn);

            EnemyFound?.Invoke();
        }

        private void SetPlayer(Player player)
        {
            _player = player;
        }

        private void GetTurn(bool currentValue, bool previousValue)
        {
            if(_enemy.isTurnReady == false)
                _player.isTurnReady = true;
        }
    }
}