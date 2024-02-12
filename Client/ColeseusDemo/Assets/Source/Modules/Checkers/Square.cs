using DG.Tweening;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    [RequireComponent(typeof(Renderer))]
    internal class Square : MonoBehaviour
    {
        internal bool IsOccupied { get; private set; } = false;
        internal bool IsWhiteOccupied { get; private set; } = false;
        internal bool IsBlackOccupied { get; private set; } = false;
        internal int WidthPosition { get; private set; }
        internal int LengthPosition { get; private set; }
        internal Material DefaultMaterial { get; private set; }

        [SerializeField] private Transform _diskPlace;
        [SerializeField] private float _floatDownDuration;

        internal Vector3 DiskPlace => _diskPlace.transform.position;

        internal void Construct(int widthPosition, int lengthPosition)
        {
            WidthPosition = widthPosition;
            LengthPosition = lengthPosition;
        }

        internal void FloatDown(Vector3 target)
        {
            transform.DOMove(target, _floatDownDuration)
                .SetEase(Ease.InQuad);
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

        internal Vector3 GetTargetPosition(Transform movableObject)
        {
            float targetXPosition = DiskPlace.x;
            float targetZPosition = DiskPlace.z;
            Vector3 currentPosition = movableObject.position;

            return new Vector3(targetXPosition, currentPosition.y, targetZPosition);
        }

        private void SetMaterial(Material material) =>
            GetComponent<Renderer>().material = material;
    }
}