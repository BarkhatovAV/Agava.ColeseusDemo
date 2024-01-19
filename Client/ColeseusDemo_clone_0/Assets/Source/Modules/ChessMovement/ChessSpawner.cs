using System;
using System.Collections.Generic;
using ColyseusDemo.Multiplayer;
using UnityEngine;
using UnityEngine.UI;

namespace ColyseusDemo.ChessMovement
{
    //К MultiplayerManager можем обращаться отсюда или в принципе с любого другого класса
    public class ChessSpawner : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _gameObject;

        private float _positionX = -8.75f;
        private bool _isSpawnReady;

        private void OnEnable()
        {
            MultiplayerManager.Instance.UnitSpawned += SpawnBall;
        }

        private void OnDestroy()
        {
            MultiplayerManager.Instance.UnitSpawned -= SpawnBall;
        }

        public void SetBall()
        {
            if (_isSpawnReady)
            {
                Vector3 ballSpawnPosition = new Vector3(_positionX, 0, 6);

                SetNewBall(ballSpawnPosition);
                SendSpawn(_gameObject.name, ballSpawnPosition);
            }
        }

        private void SetNewBall(Vector3 ballSpawnPosition)
        {
            _positionX += 2.5f;
            Instantiate(_gameObject, ballSpawnPosition, Quaternion.identity);
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

            Spawn(spawnPoint);
        }

        private void Spawn(in Vector3 spawnPoint)
        {
            Instantiate(_gameObject, spawnPoint, Quaternion.identity);
        }

        internal void SetSpawnReadyStatus(bool value) =>
            _isSpawnReady = value;

        [Serializable]
        public class SpawnData
        {
            public string sessionID;
            public string id;
            public float x;
            public float z;
        }
    }
}