using UnityEngine;

namespace ColyseusDemo.Checkers
{
    internal class CameraSetter : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _blackCameraPlace;

        private CheckersPlayer _checkersPlayer;

        private void OnEnable()
        {
            if (_checkersPlayer != null)
                _checkersPlayer.SideDetermined += SetCamera;
        }

        private void OnDisable() =>
            _checkersPlayer.SideDetermined -= SetCamera;

        internal void Construct(CheckersPlayer checkersPlayer)
        {
            _checkersPlayer = checkersPlayer;
            _checkersPlayer.SideDetermined += SetCamera;
        }

        private void SetCamera(bool isWhiteSide)
        {
            if (!isWhiteSide)
            {
                _camera.transform.position = _blackCameraPlace.position;
                _camera.transform.rotation = _blackCameraPlace.rotation;
            }
        }
    }
}