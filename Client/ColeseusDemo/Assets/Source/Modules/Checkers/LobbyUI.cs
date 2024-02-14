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
        [SerializeField] private TMP_InputField _gameIdInputField;
        [SerializeField] private Button _connectButton;
        [SerializeField] private Button _connectDyIdButton;
        [SerializeField] private JoinInfo _joinInfo;

        private PlayerSettings _playerSettings;

        private void OnEnable()
        {
            _loginInputField.onEndEdit.AddListener(InputLogin);
            _connectButton.onClick.AddListener(LoadGame);

            _connectDyIdButton.onClick.AddListener(SetIdInfo);
            _connectDyIdButton.onClick.AddListener(LoadGame);
        }

        private void OnDisable()
        {
            _loginInputField.onEndEdit.RemoveListener(InputLogin);
            _connectButton.onClick.RemoveListener(LoadGame);

            _connectDyIdButton.onClick.RemoveListener(SetIdInfo);
            _connectDyIdButton.onClick.RemoveListener(LoadGame);
        }

        private void InputLogin(string login) =>
            _playerSettings.SetLogin(login);

        public void Construct(PlayerSettings playerSettings)
        {
            _playerSettings = playerSettings;
        }

        private void SetIdInfo() =>
            _joinInfo.SetId(_gameIdInputField.text);

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