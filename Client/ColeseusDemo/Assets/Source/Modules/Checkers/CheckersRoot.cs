using ColyseusDemo.Multiplayer;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    public class CheckersRoot : MonoBehaviour
    {
        [SerializeField] private MapGenerator _mapGenerator;
        [SerializeField] private Selector _selector;
        [SerializeField] private CheckersCapturer _checkersCapturer;
        [SerializeField] private CheckersMover _checkersMover;
        [SerializeField] private CameraSetter _cameraSetter;
        [SerializeField] private GameUI _gameUI;

        [SerializeField] private Material _whiteSquareMaterial;
        [SerializeField] private Material _blackSquareMaterial;
        [SerializeField] private Material _whiteDiskMaterial;
        [SerializeField] private Material _blackDiskMaterial;

        [SerializeField] private float _squareLiftingHeight = 3f;
        [SerializeField] private float _diskLiftingHeight = 3f;

        [SerializeField] private GameObject _squarePrefab;
        [SerializeField] private Vector3 _startMapPosition;

        private CheckersPlayer _checkersPlayer;
        private MultiplayerManager _multiplayerManager;
        private PlayerSettings _playerSettings;
        private MapPlacer _mapPlacer;
        private DiskPlacer _diskPlacer;
        private AdjecentSquaresDeltas _adjecentSquaresDeltas;
        private CaptureRules _captureRules;
        private MoveRules _moveRules;

        public void Construct(MultiplayerManager multiplayerManager, PlayerSettings playerSettings)
        {
            _multiplayerManager = multiplayerManager;
            _playerSettings = playerSettings;

            _checkersPlayer = new CheckersPlayer(multiplayerManager, _selector, _checkersCapturer, _checkersMover, playerSettings);

            _adjecentSquaresDeltas = new AdjecentSquaresDeltas();
            _captureRules = new CaptureRules(_adjecentSquaresDeltas, _mapGenerator);
            _moveRules = new MoveRules(_adjecentSquaresDeltas, _mapGenerator);

            _mapPlacer = new MapPlacer(MapGenerator.MapWidth, _startMapPosition, _squarePrefab, _whiteSquareMaterial, _blackSquareMaterial, _squareLiftingHeight);
            _diskPlacer = new DiskPlacer(_whiteDiskMaterial, _blackDiskMaterial, _diskLiftingHeight);

            _selector.Construct(_captureRules, _moveRules);
            _mapGenerator.Construct(_checkersPlayer, _mapPlacer, _diskPlacer);
            _checkersCapturer.Construct(_captureRules);
            _cameraSetter.Construct(_checkersPlayer);
            _gameUI.Construct(_checkersPlayer);
        }
    }
}