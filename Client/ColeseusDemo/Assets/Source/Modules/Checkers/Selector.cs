using System.Collections.Generic;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    [RequireComponent(typeof(Camera))]
    internal class Selector : MonoBehaviour
    {
        [SerializeField] private CheckersCapturer _checkersCapturer;
        [SerializeField] private CheckersMover _checkersMover;
        [SerializeField] private CaptureRules _captureRules;
        [SerializeField] private MoveRules _moveRules;
        [SerializeField] private DisksStorage _disksStorage;
        [SerializeField] private Highlighter _highlighter;

        private List<Square> _availableSquares = new List<Square>();
        private Camera _camera;
        private Ray _ray;
        private RaycastHit _raycastHit;
        private GameObject _raycastedObject;
        private Disk _selectedDisk;
        private Disk _raycastedDisk;
        private Square _raycastedSquare;
        private bool _isCaptureMode = false;
        private bool _isDisk = false;
        private bool _isSquare = false;

        private void Awake() =>
            _camera = GetComponent<Camera>();

        private void OnEnable()
        {
            _checkersCapturer.CaptureContinue += ContinueCapture;
            _checkersCapturer.CaptureIsOver += DisableCaptureMode;
        }

        private void OnDisable()
        {
            _checkersCapturer.CaptureContinue += ContinueCapture;
            _checkersCapturer.CaptureIsOver -= DisableCaptureMode;
        }

        private void Update()
        {
            if (TryCastRay())
            {
                _isDisk = _raycastedObject.TryGetComponent<Disk>(out _raycastedDisk);
                _isSquare = _raycastedObject.TryGetComponent<Square>(out _raycastedSquare);

                if (_isSquare)
                    TryHighlightSquare(_raycastedSquare);

                if (Input.GetMouseButtonDown(0))
                {
                    if (_isCaptureMode)
                        OnCaptureMode();
                    else
                        OnMoveMode();
                }
            }
        }

        private void ContinueCapture(List<Square> availableSquares)
        {
            _availableSquares = availableSquares;
            _highlighter.HighlightAvailableSquares(_availableSquares);
        }

        private void DisableCaptureMode()
        {
            _isCaptureMode = false;
            DropCurrentDisk();
        }

        private bool TryCastRay()
        {
            _ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _raycastHit))
            {
                _raycastedObject = _raycastHit.collider.gameObject;

                return true;
            }
            else
            {
                return false;
            }
        }

        private void OnCaptureMode()
        {
            if (_isSquare && _selectedDisk != null)
                TrySelectSquare(_raycastedSquare);
        }

        private void OnMoveMode()
        {
            if (_isSquare && _selectedDisk != null)
            {
                TrySelectSquare(_raycastedSquare);
                DropCurrentDisk();
            }
            else if (_isDisk)
            {
                TrySelectDisk(_raycastedDisk);
            }
        }

        private void TryHighlightSquare(Square selectedSquare)
        {
            if (IsCorrectSquare(selectedSquare))
                _highlighter.HighlightSquare(selectedSquare);
            else
                _highlighter.UnhighlightSquare();
        }

        private bool TrySelectDisk(Disk selectedDisk)
        {
            bool isCorrectDisk = _disksStorage.IsCorrectDisk(selectedDisk);

            if (isCorrectDisk)
            {
                if (_selectedDisk != selectedDisk && _selectedDisk != null)
                    DropCurrentDisk();

                SelectDisk(selectedDisk);
            }

            return isCorrectDisk;
        }

        private void SelectDisk(Disk selectedDisk)
        {
            _selectedDisk = selectedDisk;
            _highlighter.HighlightDisk(_selectedDisk);

            _availableSquares = _captureRules.GetAvailableSquares(selectedDisk.CurrentSquare);

            if (_availableSquares.Count == 0)
                _availableSquares = _moveRules.GetAvailableSquares(selectedDisk.CurrentSquare);
            else
                _isCaptureMode = true;

            _highlighter.HighlightAvailableSquares(_availableSquares);
        }

        private bool TrySelectSquare(Square selectedSquare)
        {
            bool isCorrectSquare = IsCorrectSquare(selectedSquare);

            if (isCorrectSquare)
            {
                if (_captureRules.IsCutDown(_selectedDisk, selectedSquare))
                {
                    ClearAvailableSquares();
                    _checkersCapturer.CaptureEnemyDisk(_selectedDisk, selectedSquare);
                }
                else
                {
                    _checkersMover.MovePlayerDisk(_selectedDisk, selectedSquare);
                }
            }

            return isCorrectSquare;
        }

        private bool IsCorrectSquare(Square square) =>
            _availableSquares.Contains(square);

        private void DropCurrentDisk()
        {
            ClearAvailableSquares();

            if (_selectedDisk != null)
                _highlighter.UnhighlightDisk();

            _selectedDisk = null;
        }

        private void ClearAvailableSquares()
        {
            _highlighter.UnhighlightAvailableSquares();
            _availableSquares.Clear();
        }
    }
}