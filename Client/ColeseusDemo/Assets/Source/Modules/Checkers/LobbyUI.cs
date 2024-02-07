using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ColyseusDemo.Checkers
{
    public class LobbyUI : MonoBehaviour
    {
        private const string InvalidLoginErrorText = "Error: Invalid login";

        [SerializeField] private TMP_InputField _loginInputField;
        [SerializeField] private Button _connectButton;

        private PlayerSettings _playerSettings;

        private void OnEnable()
        {
            _loginInputField.onEndEdit.AddListener(InputLogin);
            _connectButton.onClick.AddListener(LoadGame);
        }

        private void OnDisable()
        {
            _loginInputField.onEndEdit.RemoveListener(InputLogin);
            _connectButton.onClick.RemoveListener(LoadGame);
        }

        private void InputLogin(string login) =>
            _playerSettings.SetLogin(login);

        public void Construct(PlayerSettings playerSettings)
        {
            _playerSettings = playerSettings;
        }

        private void LoadGame()
        {
            string playerLogin = _playerSettings.Login;
            bool isInvalidLogin = string.IsNullOrEmpty(playerLogin);

            if (isInvalidLogin)
                Debug.LogError(InvalidLoginErrorText);
            else
                SceneManager.LoadScene("Game");
        }
    }
}