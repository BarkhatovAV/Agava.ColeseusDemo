using System;
using System.Collections.Generic;
using Colyseus;
using TMPro;
using UnityEngine;

namespace ColyseusDemo.Multiplayer
{
    public class MultiplayerManager : ColyseusManager<MultiplayerManager>
    {
        [SerializeField] private TMP_Text _playerLogin;
        [SerializeField] private TMP_Text _enemyLogin;

        private const string GameRoomName = "state_handler";
        private const string SpawnName = "spawn";
        private const string TurnReady = "turnReady";

        private ColyseusRoom<State> _room;

        public event Action<string> OnSpawnUnit;

        public string ClientID => _room == null ? "" : _room.SessionId;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            InitializeClient();
            Connection();
        }

        private void OnDestroy()
        {
            base.OnDestroy();
            _room.OnStateChange -= OnChange;
        }

        private async void Connection()
        {
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {"login", PlayerSettings.Instance.Login }
            };

            _room = await client.JoinOrCreate<State>(GameRoomName, data);
            _room.OnMessage<string>(SpawnName, jsonSpawnData => OnSpawnUnit?.Invoke(jsonSpawnData));
            _room.OnStateChange += OnChange;
        }

        private void OnChange(State state, bool isFirstState) //ќбновл€етс€ при изменении состони€ комнаты
        {
            //if (isFirstState == false)
            //    return;

            state.players.ForEach((key, player) =>
            {
                print(player.login);

                if (key == _room.SessionId)
                    CreatePlayer(player);
                else
                    CreateEnemy(key, player);
            });
        }

        private void CreateEnemy(string key, Player player)
        {
            _enemyLogin.text = player.login;
        }

        private void CreatePlayer(Player player)
        {
            _playerLogin.text = player.login;
        }

        public void SendMessage(string key, Dictionary<string, object> data)
        {
            _room.Send(key, data);
        }

        public void SendMessage(string key, int value) 
        {
            _room.Send(key, value);
        }

        public void Leave()
        {
            _room?.Leave();
            _room = null;
        }
    }
}