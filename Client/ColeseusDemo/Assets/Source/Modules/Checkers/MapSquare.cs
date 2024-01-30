using System.Collections;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    [RequireComponent(typeof(Renderer))]
    internal class MapSquare : MonoBehaviour
    {
        internal bool IsOccupied { get; private set; } = false;
        internal bool IsWhiteOccupied { get; private set; } = false;
        internal bool IsBlackOccupied { get; private set; } = false;
        internal int WidthPosition { get; private set; }
        internal int LengthPosition { get; private set; }
        internal Material DefaultMaterial { get; private set; }

        [SerializeField] private Transform _diskPlace;

        internal Vector3 DiskPlace => _diskPlace.transform.position;

        internal void Construct(int widthPosition, int lengthPosition)
        {
            WidthPosition = widthPosition;
            LengthPosition = lengthPosition;
        }

        internal void SetDefaultMaterial(Material material)
        {
            DefaultMaterial = material;
            SetMaterial(material);
        }

        internal void Occupy(bool isOccupantWhite)
        {
            IsOccupied = true;
            IsWhiteOccupied = isOccupantWhite;
            IsBlackOccupied = !isOccupantWhite;
        }

        internal void Free()
        {
            IsOccupied = false;
            IsWhiteOccupied = false;
            IsBlackOccupied = false;
        }

        internal void SmoothlyMove(Transform movableObject, Vector3 target, float moveSpeed) =>
            StartCoroutine(FloatDown(movableObject, target, moveSpeed));

        private IEnumerator FloatDown(Transform movableObject, Vector3 target, float moveSpeed)
        {
            while (movableObject.position.y > target.y + 0.05f)
            {
                movableObject.position = Vector3.Lerp(movableObject.position, target, moveSpeed * Time.deltaTime);

                yield return null;
            }

            movableObject.position = target;
        }

        private void SetMaterial(Material material) =>
            GetComponent<Renderer>().material = material;
    }
}