using UnityEngine;

namespace ColyseusDemo.Checkers
{
    [RequireComponent(typeof(Camera))]
    public class Selector : MonoBehaviour
    {
        [SerializeField] private Map _map;

        private PlayerSettings _playerSettings;
        private Camera _camera;
        private Ray _ray;
        private RaycastHit _raycastHit;
        private Disk _selectedDisk;
        private MapSquare _selectedMapSquare;
        private bool _isDraggingMode = false;

        private void Awake() =>
            _camera = GetComponent<Camera>();

        private void Update()
        {
            if (_playerSettings.IsTurnReady)
            {
                _ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (_isDraggingMode)
                {
                    HighlightChessSquare();

                    if (Input.GetMouseButtonDown(0))
                    {
                        if (_selectedDisk != null && _selectedMapSquare != null)
                        {
                            _selectedDisk.MoveTo(_selectedMapSquare);
                            _selectedDisk.SendMoveMessage(_selectedMapSquare);
                            _playerSettings.PassTurn();
                        }

                        //if(_raycastHit.collider.gameObject.TryGetComponent<MapSquare>(out MapSquare selectedMapSquare))
                        //{
                        //    if(selectedMapSquare == _selectedMapSquare)
                        //    {
                        //        _map.OnDiskMoved();
                        //        _selectedMapSquare.SetAvailable(false);
                        //        _selectedDisk.MoveTo(_selectedMapSquare);
                        //    }
                        //}
                        //else
                        //{
                        //    DropDisk();
                        //    TryDragDisk();
                        //}
                    }
                }

                if (Input.GetMouseButtonDown(0))
                    TryDragDisk();
            }
        }

        public void SetPlayerSetting(PlayerSettings playerSettings)
        {
            _playerSettings = playerSettings;
        }

        private bool TryDragDisk()
        {
            if (Physics.Raycast(_ray, out _raycastHit))
            {
                if (_raycastHit.collider.gameObject.TryGetComponent<Disk>(out Disk selectedDisk))
                {
                    if (_selectedDisk != null)
                        _selectedDisk.Undrag();

                    _selectedDisk = selectedDisk;
                    _selectedDisk.Drag();
                    _isDraggingMode = true;

                    _map.DetermineAvailableMapSquares(_selectedDisk.CurrentMapSquare, true);

                    return true;
                }
                else
                {
                    DropDisk();

                    return false;
                }
            }

            return false;
        }

        private void DropDisk()
        {
            //_selectedDisk.Undrag();
            _selectedDisk = null;
            _isDraggingMode = false;
        }

        private void HighlightChessSquare()
        {
            if (Physics.Raycast(_ray, out _raycastHit))
            {
                if (_raycastHit.collider.TryGetComponent<MapSquare>(out MapSquare mapSquare) && mapSquare.IsAvailable)
                {
                    _selectedMapSquare = mapSquare;
                    _selectedMapSquare.Highlight();
                }
                else if (_selectedMapSquare != null)
                {
                    _selectedMapSquare.Unhighlight();
                    _selectedMapSquare = null;
                }
            }
        }
    }
}