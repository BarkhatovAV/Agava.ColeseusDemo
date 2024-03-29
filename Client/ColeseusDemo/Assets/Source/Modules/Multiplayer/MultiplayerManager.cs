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
        public event Action<string> CutDowned;
        public event Action<string> RoomFound;

        public string ClientID => _room == null ? "" : _room.SessionId;
        public string SessionId => _room.SessionId;

        public void FindGame(string login)
        {
            InitializeClient();
            ConnectClient(login);
        }

        public void FindGameByID(string login, string sessionId)
        {
            InitializeClient();
            ConnectClient(login, sessionId);
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
            RoomFound?.Invoke(SessionId);

            SubscribeMessages();
        }

        private async void ConnectClient(string playerLogin, string sessionId)
        {
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {MessagesNames.Login, playerLogin }
            };

            _room = await client.JoinById<State>(sessionId);
            RoomFound?.Invoke(SessionId);

            SubscribeMessages();
        }

        private void SubscribeMessages()
        {
            _room.OnMessage<string>(MessagesNames.Move, jsonMovedData => DiskMoved?.Invoke(jsonMovedData));
            _room.OnMessage<string>(MessagesNames.Capture, jsonCutDownData => CutDowned?.Invoke(jsonCutDownData));

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
                if (key == _room.SessionId)
                    PlayerFound?.Invoke(player);
                else
                    EnemyFound?.Invoke(player);
            });
        }
    }
}