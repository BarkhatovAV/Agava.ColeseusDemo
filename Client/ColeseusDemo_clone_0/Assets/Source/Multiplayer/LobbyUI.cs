using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ColyseusDemo.Multiplayer
{
    public class LobbyUI : MonoBehaviour
    {
        public void InputLogin(string login)
        {
            PlayerSettings.Instance.SetLogin(login);
        }

        public void ClickConnect()
        {
            if (string.IsNullOrEmpty(PlayerSettings.Instance.Login)) return;

            SceneManager.LoadScene("Game");
        }
    }
}
