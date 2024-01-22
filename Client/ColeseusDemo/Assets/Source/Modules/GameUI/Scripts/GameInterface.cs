using ColyseusDemo.ChessMovement;
using ColyseusDemo.Multiplayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ColyseusDemo.GameUI
{
    //������ ��������� � ������, � �� � Multiplayer'�
    public class GameInterface : MonoBehaviour
    {
        [SerializeField] private MultiplayerManager _multiplayerManager;
        [SerializeField] private ChessSpawner _spawner;
        [SerializeField] private TMP_Text _playerLoginPlace;
        [SerializeField] private TMP_Text _enemyLoginPlace;
        [SerializeField] private Button _skipButton;
        [SerializeField] private Button _setBallButton;

        private void OnEnable()
        {
            _multiplayerManager.PlayerConnected += SetPlayersLogins;
            _setBallButton.onClick.AddListener(_spawner.SetBall);
            //��� ������ ���� �������� ������, ������� �������� ��������� � ���, ��� ����� �������� ��� � ������ _endTurn
        }

        private void OnDisable() 
        {
            _multiplayerManager.PlayerConnected -= SetPlayersLogins;
            _setBallButton.onClick.RemoveListener(_spawner.SetBall);
        }

        private void SetPlayersLogins(string playerLogin, string enemyLogin)
        {
            _playerLoginPlace.text = playerLogin;
            _enemyLoginPlace.text = enemyLogin;
        }
    }
}