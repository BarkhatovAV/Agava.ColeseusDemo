using System.Collections.Generic;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    public class TurnReferee : MonoBehaviour
    {
        private List<Player> _players = new List<Player>();

        internal void Addplayer(Player player)
        {
            _players.Add(player);
            //player.OnIsTurnReadyChange()
        }

        private void SetTurnReady()
        {

        }
    }
}