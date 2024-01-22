using ColyseusDemo.MenuUI;
using ColyseusDemo.Multiplayer;
using ColyseusDemo.Players;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ColyseusDemo.Game
{
    public class GameRoot : MonoBehaviour
    {
        private const string GameSceneName = "Game";

        [SerializeField] private MultiplayerManager _multiplayerManager;
        [SerializeField] private LobbyUI _lobbyUI;

        private PlayerSettings _playerSettings;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            _playerSettings = new PlayerSettings();

            ConstructObjects();
        }

        private void OnEnable() =>
            SceneManager.sceneLoaded += OnSceneLoaded;

        private void OnDisable() =>
            SceneManager.sceneLoaded -= OnSceneLoaded;

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == GameSceneName)
                _multiplayerManager.FindGame(_playerSettings.Login);
        }

        private void ConstructObjects()
        {
            _lobbyUI.Construct(_playerSettings);
        }
    }
}