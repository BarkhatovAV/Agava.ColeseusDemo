using ColyseusDemo.ChessMovement;
using UnityEngine;

namespace ColyseusDemo.ChessSquareSelection
{
    [RequireComponent(typeof(Camera))]
    internal class ChessSquareSelector : MonoBehaviour
    {
        private Camera _camera;
        private Ray _ray;
        private RaycastHit _raycastHit;
        private ChessSquare _selectedChessSquare;

        private void Awake() =>
            _camera = GetComponent<Camera>();

        private void Update() =>
            HighlightChessSquare();

        private bool HighlightChessSquare()
        {
            _ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _raycastHit))
            {
                if (_raycastHit.collider.TryGetComponent<ChessSquare>(out ChessSquare chessSquare))
                {
                    chessSquare.Highlight();

                    if (_selectedChessSquare != null && _selectedChessSquare != chessSquare)
                    {
                        _selectedChessSquare.Unhighlight();
                        _selectedChessSquare = chessSquare;
                    }

                    _selectedChessSquare = chessSquare;
                }
                else if (_selectedChessSquare)
                {
                    _selectedChessSquare.Unhighlight();
                    _selectedChessSquare = null;
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}