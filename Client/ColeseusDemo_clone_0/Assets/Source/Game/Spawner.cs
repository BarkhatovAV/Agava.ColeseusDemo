using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI;
using ColyseusDemo.Multiplayer;
using Colyseus;

namespace ColyseusDemo.Game
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _gameObject;

        private float _posX = -8.75f;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
            MultiplayerManager.Instance.OnSpawnUnit += Spawn;
        }

        private void OnDestroy()
        {
            MultiplayerManager.Instance.OnSpawnUnit -= Spawn;
        }

        private void OnClick()
        {
            Vector3 pos = new Vector3(_posX, 0, 6);
            _posX += 2.5f;
            Instantiate(_gameObject, pos, Quaternion.identity);
            SendSpawn(_gameObject.name, pos);
        }

        private void SendSpawn(string id, Vector3 spawnPoint)
        {
            Dictionary<string, object> data = new Dictionary<string, object>()
            {   
                {"id", id},
                {"x", spawnPoint.x},
                {"z", spawnPoint.z},
            };

            MultiplayerManager.Instance.SendMessage("spawn", data);
        }

        private void Spawn(string jsonSpawnData)
        {
            SpawnData spawnData = JsonUtility.FromJson<SpawnData>(jsonSpawnData);

            Vector3 spawnPoint = new Vector3(spawnData.x, 0, spawnData.z);
            bool isEnemy = spawnData.sessionID != MultiplayerManager.Instance.ClientID;

            if (isEnemy)
            {
                spawnPoint *= -1;
            }

            Spawn(spawnData.id, spawnPoint, isEnemy);
        }

        private void Spawn(string id, in Vector3 spawnPoint, bool isEnemy)
        {
            Instantiate(_gameObject, spawnPoint, Quaternion.identity);
        }

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
