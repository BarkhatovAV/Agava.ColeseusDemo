using ColyseusDemo.Checkers;
using TMPro;
using UnityEngine;

namespace ColyseusDemo.UI
{
    internal class GameUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _playerLoginPlace;
        [SerializeField] private TMP_Text _enemyLoginPlace;
        [SerializeField] private CheckersPlayer _checkersPlayer;

        private void OnEnable()
        {
            _checkersPlayer.EnemyFound += SetEnemyLogin;
            _checkersPlayer.LoginSet += SetPlayerLogin;
        }

        private void OnDisable()
        {
            _checkersPlayer.EnemyFound -= SetEnemyLogin;
            _checkersPlayer.LoginSet -= SetPlayerLogin;
        }

        private void SetPlayerLogin() =>
            _playerLoginPlace.text = _checkersPlayer.Login;

        private void SetEnemyLogin(string login) =>
            _enemyLoginPlace.text = login;
    }
}