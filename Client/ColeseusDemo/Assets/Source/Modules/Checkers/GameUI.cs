using ColyseusDemo.Multiplayer;
using TMPro;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    internal class GameUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _playerLoginPlace;
        [SerializeField] private TMP_Text _enemyLoginPlace;
        [SerializeField] private TMP_Text _idText;

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

        internal void Construct(CheckersPlayer checkersPlayer, MultiplayerManager multiplayerManager)
        {
            _checkersPlayer = checkersPlayer;

            SetPlayerLogin();

            _checkersPlayer.EnemyFound += SetEnemyLogin;
            multiplayerManager.RoomFound += SetIdText;
        }

        internal void SetIdText(string sessionId) =>
            _idText.text = "Session Id: " + sessionId;

        private void SetPlayerLogin() =>
            _playerLoginPlace.text = _checkersPlayer.Login;

        private void SetEnemyLogin(string login) =>
            _enemyLoginPlace.text = login;
    }
}