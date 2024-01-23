using ColyseusDemo.Checkers;
using TMPro;
using UnityEngine;

namespace ColyseusDemo.UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _playerLoginPlace;
        [SerializeField] private TMP_Text _enemyLoginPlace;

        private PlayerSettings _playerSettings;

        private void OnDisable()
        {
            _playerSettings.EnemyFound -= SetEnemyLogin;
        }

        public void Construct(PlayerSettings playerSettings)
        {
            _playerSettings = playerSettings;

            SetPlayerLogin();

            _playerSettings.EnemyFound += SetEnemyLogin;
        }

        private void SetPlayerLogin() =>
            _playerLoginPlace.text = _playerSettings.Login;

        private void SetEnemyLogin() =>
            _enemyLoginPlace.text = _playerSettings.Enemy.login;
    }
}