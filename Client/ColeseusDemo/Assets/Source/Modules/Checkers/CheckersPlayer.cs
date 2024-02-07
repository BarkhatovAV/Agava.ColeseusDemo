using ColyseusDemo.Multiplayer;
using System;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    public class CheckersPlayer : MonoBehaviour
    {
        [SerializeField] private CheckersMover _disksMover;
        [SerializeField] private CheckersCapturer _checkersCapturer;
        [SerializeField] private Selector _selector;

        public bool IsWhitePlayer { get; private set; }

        private MultiplayerManager _multiplayerManager;
        private PlayerSettings _playerSettings;
        private Player _player;
        private Player _enemy;
        private bool IsTurnReady;

        public event Action LoginSet;
        public event Action<string> EnemyFound;

        internal event Action<bool> SideDetermined;

        public string Login => _playerSettings.Login;

        private void OnEnable()
        {
            _disksMover.MoveMade += SetIsTurnReady;
            _checkersCapturer.CaptureIsOver += SetIsTurnReady;
        }

        private void OnDisable()
        {
            _disksMover.MoveMade -= SetIsTurnReady;
            _checkersCapturer.CaptureIsOver -= SetIsTurnReady;
            _multiplayerManager.EnemyFound -= SetEnemy;
            _multiplayerManager.PlayerFound -= SetPlayer;
            _multiplayerManager.DiskMoved -= _disksMover.MoveEnemyDisk;
            _multiplayerManager.CutDowned -= _checkersCapturer.CapturePlayersDisks;
        }

        public void Construct(MultiplayerManager multiplayerManager, PlayerSettings playerSettings)
        {
            _playerSettings = playerSettings;
            LoginSet?.Invoke();

            _multiplayerManager = multiplayerManager;

            _multiplayerManager.EnemyFound += SetEnemy;
            _multiplayerManager.PlayerFound += SetPlayer;
            _multiplayerManager.DiskMoved += _disksMover.MoveEnemyDisk;
            _multiplayerManager.CutDowned += _checkersCapturer.CapturePlayersDisks;
        }

        private void SetIsTurnReady()
        {
            IsTurnReady = !IsTurnReady;
            _selector.enabled = IsTurnReady;
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
            IsTurnReady = IsWhitePlayer;
            _player.isWhitePlayer = IsWhitePlayer;

            SideDetermined?.Invoke(IsWhitePlayer);
            _selector.enabled = IsWhitePlayer;
        }
    }
}