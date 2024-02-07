using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    public class CheckersRoot : MonoBehaviour
    {
        [SerializeField] private CheckersPlayer _checkersPlayer;

        [SerializeField] private Material _whiteMapSquareMaterial;
        [SerializeField] private Material _blackMapSquareMaterial;
        [SerializeField] private Material _whiteDiskMaterial;
        [SerializeField] private Material _blackDiskMaterial;

        [SerializeField] private float _mapSquareAppearingSpeed = 8f;
        [SerializeField] private float _diskAppearingSpeed = 8f;
        [SerializeField] private float _mapSquareLiftingHeight = 3f;
        [SerializeField] private float _diskLiftingHeight = 3f;

        private MapPlacer _mapPlacer;
        private DiskPlacer _diskPlacer;
    }
}