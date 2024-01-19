using ColyseusDemo.ChessMovement;
using UnityEngine;

namespace ColyseusDemo.ChessSelection
{
    [RequireComponent(typeof(Camera))]
    internal class ChessSelector : MonoBehaviour
    {
        private Camera _camera;
        private Ray _ray;
        private RaycastHit _raycastHit;
        private ChessSquare _selectedChessSquare;

        private void Awake() =>
            _camera = GetComponent<Camera>();

        private void Update() =>
            HighlightChess();

        private bool HighlightChess()
        {
            _ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _raycastHit))
            {
                if (_raycastHit.collider.TryGetComponent<ChessSquare>(out ChessSquare chessSquare))
                {
                    chessSquare.Highlight();

                    if (_selectedChessSquare != chessSquare)
                    {
                        _selectedChessSquare.Unhighlight();
                        _selectedChessSquare = chessSquare;
                    }
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