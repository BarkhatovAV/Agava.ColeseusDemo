using ColyseusDemo.Multiplayer;
using System.Collections.Generic;
using UnityEngine;

namespace ColyseusDemo.ChessMovement
{
    public partial class ChessSpawner : MonoBehaviour
    {
        [SerializeField] private ChessPiece _chessPrefab;

        private float _positionX = -8.75f;
        private bool _isSpawnReady;

        private void OnEnable() =>
            MultiplayerManager.Instance.UnitSpawned += SpawnBall;

        private void OnDisable() =>
            MultiplayerManager.Instance.UnitSpawned -= SpawnBall;

        public void SetBall()
        {
            if (_isSpawnReady)
            {
                Vector3 ballSpawnPosition = new Vector3(_positionX, 0, 6);

                SetNewBall(ballSpawnPosition);
                SendSpawn(_chessPrefab.name, ballSpawnPosition);
            }
        }

        private void SetNewBall(Vector3 ballSpawnPosition)
        {
            _positionX += 2.5f;
            Instantiate(_chessPrefab, ballSpawnPosition, Quaternion.identity);
        }

        private void SendSpawn(string id, Vector3 spawnPoint)
        {
            Dictionary<string, object> data = new Dictionary<string, object>()
            {   
                {"id", id},
                {"x", spawnPoint.x},
                {"z", spawnPoint.z},
            };

            MultiplayerManager.Instance.TrySendMessage(MessagesNames.Spawn, data);
        }

        private void SpawnBall(string jsonSpawnData)
        {
            SpawnData spawnData = JsonUtility.FromJson<SpawnData>(jsonSpawnData);

            Vector3 spawnPoint = new Vector3(spawnData.x, 0, spawnData.z);
            bool isEnemy = spawnData.sessionID != MultiplayerManager.Instance.ClientID;

            if (isEnemy)
                spawnPoint *= -1;

            SpawnChess(spawnPoint);
        }

        private void SpawnChess(in Vector3 spawnPoint) =>
            Instantiate(_chessPrefab, spawnPoint, Quaternion.identity);

        internal void SetSpawnReadyStatus(bool value) =>
            _isSpawnReady = value;
    }
}