using System;
using System.Collections.Generic;
using Colyseus;

namespace ColyseusDemo.Multiplayer
{
    public class MultiplayerManager : ColyseusManager<MultiplayerManager>
    {
        private ColyseusRoom<State> _room;

        public event Action<string> UnitSpawned;
        public event Action<string> TurnEnded;
        public event Action<string, string> PlayerConnected;

        public string ClientID => _room == null ? "" : _room.SessionId;

        public void FindGame(string login)
        {
            //DontDestroyOnLoad(gameObject);
            InitializeClient();
            ConnectClient(login);
        }

        public bool TrySendMessage(string key, object message)
        {
            bool isCorrectMessage = MessagesNames.DetermineMessageCorrectness(key, message);

            if (isCorrectMessage)
                _room.Send(key, message);

            return isCorrectMessage;
        }

        public void Leave()
        {
            _room?.Leave();
            _room = null;
        }

        private async void ConnectClient(string playerLogin)
        {
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {MessagesNames.Login, playerLogin }
            };

            _room = await client.JoinOrCreate<State>(StatesNames.GameRoomName, data);
            //По-хорошему нужно это переместить куда-то ещё
            SubscribeMessages();
        }

        private void SubscribeMessages()
        {
            _room.OnMessage<string>(MessagesNames.Spawn, jsonSpawnData => UnitSpawned?.Invoke(jsonSpawnData));
            _room.OnMessage<string>(MessagesNames.TurnEnded, jsonTurnEndedData => TurnEnded?.Invoke(jsonTurnEndedData));

            //_room.OnJoin += SetLoggins;
            _room.OnStateChange += OnStateChanged;
        }

        private void OnStateChanged(State state, bool isFirstState)
        {
            TransferLogins();
        }

        private void TransferLogins()
        {
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