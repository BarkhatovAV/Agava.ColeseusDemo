using System.Collections.Generic;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    [RequireComponent(typeof(Camera))]
    public class Selector : MonoBehaviour
    {
        [SerializeField] private DiskMover _disksMover;
        [SerializeField] private MoveRules _moveRules;
        [SerializeField] private Color _availableSquareColor;
        [SerializeField] private Color _selectedSquareColor;
        [SerializeField] private Color _takenDiskColor;

        private Camera _camera;
        private Ray _ray;
        private RaycastHit _raycastHit;
        private GameObject _selectedObject;
        private List<MapSquare> _availableSquares = new List<MapSquare>();
        private MapSquare _highlightedMapSquare;
        private MapSquare _selectedMapSquare;
        private Disk _selectedDisk;
        private bool _isDraggingMode = false;
        private bool _isCorrectDisk = false;
        private bool _isCorrectMapSquare = false;

        private void Awake() =>
            _camera = GetComponent<Camera>();

        private void Update()
        {
            _ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _raycastHit))
            {
                _selectedObject = _raycastHit.collider.gameObject;

                if (_selectedObject.TryGetComponent<MapSquare>(out MapSquare mapSquare))
                    TryHighlightMapSquare(mapSquare);

                if (Input.GetMouseButtonDown(0))
                {
                    if (_isDraggingMode)
                    {
                        if (_selectedObject.TryGetComponent<MapSquare>(out MapSquare selectedMapSquare))
                        {
                            if (TrySelectMapSquare(selectedMapSquare))
                                _disksMover.MoveDisk(_selectedMapSquare);

                            DropSelectedDisk();
                        }
                        else if (_selectedObject.TryGetComponent<Disk>(out Disk selectedDisk))
                        {
                            TrySelectDisk(selectedDisk);
                        }
                        else
                        {
                            DropSelectedDisk();
                        }
                    }
                    else
                    {
                        if (_selectedObject.TryGetComponent<Disk>(out Disk selectedDisk))
                            TrySelectDisk(selectedDisk);
                    }
                }
            }
        }

        private bool TrySelectDisk(Disk selectedDisk)
        {
            _isCorrectDisk = _disksMover.TryTakeDisk(selectedDisk);

            if (_isCorrectDisk)
            {
                if (_selectedDisk != selectedDisk && _selectedDisk != null)
                    DropSelectedDisk();

                SelectDisk(selectedDisk);

                return true;
            }
            else
            {
                if (_selectedDisk != null)
                {
                    UnhighlightDisk(_selectedDisk);
                    _selectedDisk = null;
                }

                return false;
            }
        }

        private void SelectDisk(Disk disk)
        {
            _selectedDisk = disk;
            HighlightDisk(_selectedDisk);

            _availableSquares = _moveRules.GetAvailableSquares(disk.CurrentMapSquare);
            HighlightAvailableSquares();

            _isDraggingMode = true;
        }

        private void DropSelectedDisk()
        {
            UnhighlightDisk(_selectedDisk);
            UnhighlightAvailableSquares();

            _disksMover.DropDisk();
            _selectedDisk = null;

            _isDraggingMode = false;
        }

        private void HighlightDisk(Disk selectedDisk)
        {
            if (selectedDisk.gameObject.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
                renderer.material.color = _takenDiskColor;
        }

        private void UnhighlightDisk(Disk selectedDisk)
        {
            if (selectedDisk.gameObject.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
                renderer.material.color = selectedDisk.DefaultColor;
        }

        private bool TrySelectMapSquare(MapSquare mapSquare)
        {
            _isCorrectMapSquare = _availableSquares.Contains(mapSquare);

            if (_isCorrectMapSquare)
            {
                _selectedMapSquare = mapSquare;
                UnhighlightAvailableSquares();
                _isDraggingMode = false;
            }

            return _isCorrectMapSquare;
        }

        private bool TryHighlightMapSquare(MapSquare mapSquare)
        {
            if (_availableSquares.Contains(mapSquare))
            {
                _highlightedMapSquare = mapSquare;
                HighlightMapSquare(_highlightedMapSquare);

                return true;
            }
            else
            {
                UnhighlightMapSquare();
                _highlightedMapSquare = null;

                return false;
            }
        }

        private void HighlightMapSquare(MapSquare mapSquare)
        {
            if (mapSquare.gameObject.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
                renderer.material.color = _selectedSquareColor;
        }

        private void UnhighlightMapSquare()
        {
            if (_highlightedMapSquare != null)
            {
                if (_highlightedMapSquare.gameObject.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
                    renderer.material.color = _availableSquareColor;
            }
        }

        private void HighlightAvailableSquares()
        {
            foreach (MapSquare square in _availableSquares)
            {
                if (square.gameObject.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
                    renderer.material.color = _availableSquareColor;
            }
        }

        private void UnhighlightAvailableSquares()
        {
            foreach (MapSquare square in _availableSquares)
            {
                if (square.gameObject.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
                    renderer.material.color = square.DefaultColor;
            }

            _availableSquares.Clear();
        }
    }
}