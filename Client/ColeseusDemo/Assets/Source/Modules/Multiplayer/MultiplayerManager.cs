using System;
using System.Collections.Generic;
using Colyseus;
using ColyseusDemo.Players;
using UnityEngine;

namespace ColyseusDemo.Multiplayer
{
    public class MultiplayerManager : ColyseusManager<MultiplayerManager>
    {
        private ColyseusRoom<State> _room;

        public event Action<string> UnitSpawned;
        public event Action<string> TurnEnded;
        public event Action<string, string> PlayerConnected;

        public string ClientID => _room == null ? "" : _room.SessionId;

        private void Awake()
        {
            base.Awake();
            FindGame();
        }

        //private void OnEnable()
        //{
        //    _room.OnJoin += SetLoggins;
        //}

        private void OnDisable()
        {
            _room.OnJoin -= SetLoggins;
        }

        public void FindGame()
        {
            //Это в gameRoot Awake
            DontDestroyOnLoad(gameObject);
            InitializeClient();
            ConnectClient();
            //SubscribeMessages();
        }

        public bool TrySendMessage(string key, object message)
        {
            bool isCorrectMessage = MessagesNames.DetermineMessageCorrectness(key, message);

            if (isCorrectMessage)
            {
                _room.Send(key, message);

                return true;
            }
            else
            {
                return false;
            }
        }

        public void Leave()
        {
            _room?.Leave();
            _room = null;
        }

        private async void ConnectClient()
        {
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {MessagesNames.Login, PlayerSettings.CurrentPlayerSettings.Login }
            };

            _room = await client.JoinOrCreate<State>(StatesNames.GameRoomName, data);
            SubscribeMessages();
        }

        private void SubscribeMessages()
        {
            Debug.Log(_room == null);
            _room.OnMessage<string>(MessagesNames.Spawn, jsonSpawnData => UnitSpawned?.Invoke(jsonSpawnData));
            _room.OnMessage<string>(MessagesNames.TurnEnded, jsonTurnEndedData => TurnEnded?.Invoke(jsonTurnEndedData));
            _room.OnJoin += SetLoggins;
        }

        private void SetLoggins()
        {
            Debug.Log("dfgsdfg");
            string playerLogin = "";
            string enemyLogin = "";

            State currentState = _room.State;

            currentState.players.ForEach((key, player) =>
            {
                print(player.login);

                if (key == _room.SessionId)
                    playerLogin = player.login;
                else
                    enemyLogin = player.login;
            });

            PlayerConnected?.Invoke(playerLogin, enemyLogin);
        }
    }
}