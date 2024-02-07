using TMPro;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    internal class GameUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _playerLoginPlace;
        [SerializeField] private TMP_Text _enemyLoginPlace;

        private CheckersPlayer _checkersPlayer;

        private void OnEnable()
        {
            if (_checkersPlayer != null)
                _checkersPlayer.EnemyFound += SetEnemyLogin;
        }

        private void OnDisable()
        {
            if (_checkersPlayer != null)
                _checkersPlayer.EnemyFound -= SetEnemyLogin;
        }

        internal void Construct(CheckersPlayer checkersPlayer)
        {
            _checkersPlayer = checkersPlayer;

            SetPlayerLogin();
            _checkersPlayer.EnemyFound += SetEnemyLogin;
        }

        private void SetPlayerLogin() =>
            _playerLoginPlace.text = _checkersPlayer.Login;

        private void SetEnemyLogin(string login) =>
            _enemyLoginPlace.text = login;
    }
}