using System;
using System.Collections.Generic;
using Colyseus;
using Colyseus.Schema;

namespace ColyseusDemo.Multiplayer
{
    public class MultiplayerManager : ColyseusManager<MultiplayerManager>
    {
        private ColyseusRoom<State> _room;

        public event Action<Player> EnemyFound;
        public event Action<Player> PlayerFound;
        public event Action<string> DiskMoved;

        public string ClientID => _room == null ? "" : _room.SessionId;
        public string SessionId => _room.SessionId;

        public void FindGame(string login)
        {
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

            SubscribeMessages();
        }

        private void SubscribeMessages()
        {
            _room.OnMessage<string>(MessagesNames.Move, jsonMovedData => DiskMoved?.Invoke(jsonMovedData));

            //_room.OnJoin += OnRoomJoined;
            _room.OnStateChange += OnStateChanged;
        }

        private void OnStateChanged(State state, bool isFirstState) =>
            FindEnemy();

        private void FindEnemy()
        {
            MapSchema<Player> players = _room.State.players;

            players.ForEach((key, player) =>
            {
                if (key != _room.SessionId)
                    EnemyFound?.Invoke(player);
                else
                    PlayerFound?.Invoke(player);
            });
        }
    }
}