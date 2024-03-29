using ColyseusDemo.Checkers;
using ColyseusDemo.Multiplayer;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ColyseusDemo.Game
{
    internal class GameRoot : MonoBehaviour
    {
        private const string GameSceneName = "Game";

        [SerializeField] private MultiplayerManager _multiplayerManager;
        [SerializeField] private LobbyUI _lobbyUI;

        private PlayerSettings _playerSettings;
        private CheckersRoot _checkersRoot;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            _playerSettings = new PlayerSettings();

            _lobbyUI.Construct(_playerSettings);
        }

        private void OnEnable() =>
            SceneManager.sceneLoaded += OnSceneLoaded;

        private void OnDisable() =>
            SceneManager.sceneLoaded -= OnSceneLoaded;

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            switch (scene.name)
            {
                case GameSceneName:
                    PrepareGameScene();
                    break;
            }
        }

        private void PrepareGameScene()
        {
            _multiplayerManager.FindGame(_playerSettings.Login);

            _checkersRoot = FindObjectOfType<CheckersRoot>();
            _checkersRoot.Construct(_multiplayerManager, _playerSettings);
        }
    }
}