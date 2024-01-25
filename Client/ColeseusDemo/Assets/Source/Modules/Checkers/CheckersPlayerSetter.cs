using ColyseusDemo.Multiplayer;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    public class CheckersPlayerSetter : MonoBehaviour
    {
        [SerializeField] private CheckersPlayer _checkersPlayer;
        [SerializeField] private Transform _blackPlayerPlace;

        private MultiplayerManager _multiplayerManager;

        private void OnEnable() =>
            _multiplayerManager.SideDetermined += SetCamera;

        private void OnDisable() =>
            _multiplayerManager.SideDetermined -= SetCamera;

        public void Construct(MultiplayerManager multiplayerManager) =>
            _multiplayerManager = multiplayerManager;

        private void SetCamera(bool isWhiteSide)
        {
            if (!isWhiteSide)
            {
                _checkersPlayer.transform.position = _blackPlayerPlace.position;
                _checkersPlayer.transform.rotation = _blackPlayerPlace.rotation;
            }
        }
    }
}