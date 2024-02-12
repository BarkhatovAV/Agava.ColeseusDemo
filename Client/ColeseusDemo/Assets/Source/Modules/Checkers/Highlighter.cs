using System.Collections.Generic;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    internal class Highlighter : MonoBehaviour
    {
        [SerializeField] private Material _availableSquareMaterial;
        [SerializeField] private Material _selectedSquareMaterial;
        [SerializeField] private Material _takenDiskMaterial;

        private List<Square> _availableMapSquares;
        private Square _highlightedMapSquare;
        private Disk _highlightedDisk;

        internal void HighlightDisk(Disk disk)
        {
            if (disk.gameObject.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
                renderer.material = _takenDiskMaterial;

            _highlightedDisk = disk;
        }

        internal void UnhighlightDisk()
        {
            if (_highlightedDisk.gameObject.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
                renderer.material = _highlightedDisk.DefaultMaterial;
        }

        internal void HighlightSquare(Square mapSquare)
        {
            if (mapSquare.gameObject.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
                renderer.material = _selectedSquareMaterial;

            _highlightedMapSquare = mapSquare;
        }

        internal void UnhighlightSquare()
        {
            if (_highlightedMapSquare != null)
            {
                if (_highlightedMapSquare.gameObject.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
                    renderer.material = _availableSquareMaterial;
            }

            _highlightedMapSquare = null;
        }

        internal void HighlightAvailableSquares(List<Square> availableMapSquares)
        {
            foreach (Square square in availableMapSquares)
            {
                if (square.gameObject.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
                    renderer.material = _availableSquareMaterial;
            }

            _availableMapSquares = availableMapSquares;
        }

        internal void UnhighlightAvailableSquares()
        {
            foreach (Square square in _availableMapSquares)
            {
                if (square.gameObject.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
                    renderer.material = square.DefaultMaterial;
            }

            _availableMapSquares.Clear();
            _highlightedMapSquare = null;
        }
    }
}