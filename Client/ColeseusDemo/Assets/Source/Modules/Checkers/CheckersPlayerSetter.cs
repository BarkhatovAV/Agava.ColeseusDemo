using UnityEngine;

namespace ColyseusDemo.Checkers
{
    internal class CheckersPlayerSetter : MonoBehaviour
    {
        [SerializeField] private CheckersPlayer _checkersPlayer;
        [SerializeField] private Transform _blackPlayerPlace;

        private void OnEnable() =>
            _checkersPlayer.SideDetermined += SetCamera;

        private void OnDisable() =>
            _checkersPlayer.SideDetermined -= SetCamera;

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