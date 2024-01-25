using ColyseusDemo.Multiplayer;
using System;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    public class CheckersPlayer : MonoBehaviour
    {
        [SerializeField] private DiskMover _disksMover;
        [SerializeField] private Selector _selector;

        public bool IsWhitePlayer { get; private set; }

        private MultiplayerManager _multiplayerManager;
        private PlayerSettings _playerSettings;
        private Player _enemy;
        private bool IsTurnReady;

        public event Action LoginSet;
        public event Action<string> EnemyFound;

        public string Login => _playerSettings.Login;

        private void OnEnable() =>
            _disksMover.MoveMade += SetIsTurnReady;

        private void OnDisable()
        {
            _multiplayerManager.SideDetermined -= SetSide;
            _disksMover.MoveMade -= SetIsTurnReady;
            _multiplayerManager.EnemyFound -= SetEnemy;
            _multiplayerManager.DiskMoved -= _disksMover.MoveEnemyDisk;
        }

        public void Construct(MultiplayerManager multiplayerManager, PlayerSettings playerSettings)
        {
            _playerSettings = playerSettings;
            LoginSet?.Invoke();

            _multiplayerManager = multiplayerManager;

            SetSide(true);
            _multiplayerManager.SideDetermined += SetSide;
            _multiplayerManager.EnemyFound += SetEnemy;
            _multiplayerManager.DiskMoved += _disksMover.MoveEnemyDisk;
        }

        private void SetSide(bool isWhitePlayer)
        {
            IsWhitePlayer = isWhitePlayer;
            IsTurnReady = isWhitePlayer;

            _disksMover.SetSideDisks(isWhitePlayer);
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
    }
}