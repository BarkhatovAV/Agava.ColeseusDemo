using ColyseusDemo.Players;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ColyseusDemo.MenuUI
{
    public class LobbyUI : MonoBehaviour
    {
        private const string InvalidLoginErrorText = "Error: Invalid login";

        [SerializeField] private TMP_InputField _loginInputField;
        [SerializeField] private Button _connectButton;

        private void OnEnable()
        {
            _loginInputField.onEndEdit.AddListener(InputLogin);
            _connectButton.onClick.AddListener(Connect);
        }

        private void OnDisable()
        {
            _loginInputField.onEndEdit.RemoveListener(InputLogin);
            _connectButton.onClick.RemoveListener(Connect);
        }

        private void InputLogin(string login) =>
            PlayerSettings.CurrentPlayerSettings.SetLogin(login);

        private void Connect()
        {
            bool isInvalidLogin = string.IsNullOrEmpty(PlayerSettings.CurrentPlayerSettings.Login);

            if (isInvalidLogin)
                Debug.LogError(InvalidLoginErrorText);
            else
                SceneManager.LoadScene("Game");
        }
    }
}