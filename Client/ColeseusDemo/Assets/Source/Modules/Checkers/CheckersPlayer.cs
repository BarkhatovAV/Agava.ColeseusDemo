using ColyseusDemo.Multiplayer;
using System;

namespace ColyseusDemo.Checkers
{
    public class CheckersPlayer
    {
        public bool IsWhitePlayer { get; private set; }

        private MultiplayerManager _multiplayerManager;
        private Selector _selector;
        private CheckersCapturer _checkersCapturer;
        private CheckersMover _checkersMover;
        private PlayerSettings _playerSettings;
        private Player _player;
        private Player _enemy;
        private bool _isTurnReady;

        public event Action<string> EnemyFound;

        internal event Action<bool> SideDetermined;

        public string Login => _playerSettings.Login;

        internal CheckersPlayer(MultiplayerManager multiplayerManager, Selector selector, CheckersCapturer checkersCapturer, CheckersMover checkersMover, PlayerSettings playerSettings)
        {
            _multiplayerManager = multiplayerManager;
            _selector = selector;
            _checkersCapturer = checkersCapturer;
            _checkersMover = checkersMover;
            _playerSettings = playerSettings;

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            _multiplayerManager.EnemyFound += SetEnemy;
            _multiplayerManager.PlayerFound += SetPlayer;
            _multiplayerManager.DiskMoved += _checkersMover.MoveEnemyDisk;
            _multiplayerManager.CutDowned += _checkersCapturer.CapturePlayersDisks;
            _checkersCapturer.CaptureIsOver += SetIsTurnReady;
            _checkersMover.MoveMade += SetIsTurnReady;
        }

        private void SetIsTurnReady()
        {
            _isTurnReady = !_isTurnReady;
            _selector.enabled = _isTurnReady;
        }

        private void SetEnemy(Player enemy)
        {
            _enemy = enemy;
            EnemyFound?.Invoke(_enemy.login);
        }

        private void SetPlayer(Player player)
        {
            _player = player;
            _player.OnIsWhitePlayerChange(SetSide);
        }

        private void SetSide(bool currentValue, bool previousValue)
        {
            IsWhitePlayer = currentValue;
            _isTurnReady = IsWhitePlayer;
            _player.isWhitePlayer = IsWhitePlayer;

            SideDetermined?.Invoke(IsWhitePlayer);
            _selector.enabled = IsWhitePlayer;
        }
    }
}