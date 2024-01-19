using ColyseusDemo.Multiplayer;
using ColyseusDemo.Players;
using UnityEngine;

namespace ColyseusDemo.TurnEnding
{
    public class TurnEnder : MonoBehaviour
    {
        private void OnEnable() =>
            MultiplayerManager.Instance.TurnEnded += GetTurn;

        private void OnDisable() =>
            MultiplayerManager.Instance.TurnEnded -= GetTurn;

        private void GetTurn(string jsonTurnEndedData)
        {
            int turnIndicator = JsonUtility.FromJson<int>(jsonTurnEndedData);
            bool isPlayerRoomOwner = PlayerSettings.CurrentPlayerSettings.IsWhitePlayer;
            bool isPlayerSpawnReady = turnIndicator == 0 && isPlayerRoomOwner;

            //_spawner.SetSpawnReadyStatus(isPlayerSpawnReady);
        }
    }
}