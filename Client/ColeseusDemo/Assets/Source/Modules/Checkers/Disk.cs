using DG.Tweening;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    [RequireComponent(typeof(Renderer))]
    internal class Disk : MonoBehaviour
    {
        private const int JumpsCount = 1;

        internal Square CurrentSquare { get; private set; }
        internal bool IsWhite { get; private set; }
        internal int Id { get; private set; }
        internal int WidthPosition { get; private set; }
        internal int LengthPosition { get; private set; }
        internal Material DefaultMaterial { get; private set; }

        [SerializeField] private float _floatDownDuration;
        [SerializeField] private float _moveDuration;
        [SerializeField] float _jumpDuration;
        [SerializeField] float _jumpPower;

        internal void Construct(Square currentSquare, bool isWhite, Material material)
        {
            CurrentSquare = currentSquare;
            IsWhite = isWhite;
            DefaultMaterial = material;

            SetMaterial(material);
            SetCurrentSquare(currentSquare);
        }

        internal void SetCurrentSquare(Square currentMapSquare)
        {
            CurrentSquare = currentMapSquare;

            WidthPosition = currentMapSquare.WidthPosition;
            LengthPosition = currentMapSquare.LengthPosition;
            currentMapSquare.Occupy(IsWhite);
        }

        internal void SetId(int id) =>
            Id = id;

        internal void FloatDown(Vector3 target)
        {
            transform.DOMove(target, _floatDownDuration)
                .SetEase(Ease.InQuad);
        }

        internal void MoveTo(Vector3 target)
        {
            transform.DOMove(target, _moveDuration)
                .SetEase(Ease.InQuad);

        }

        internal void JumpTo(Vector3 target)
        {
            transform.DOJump(target, _jumpPower, JumpsCount, _jumpDuration)
                .SetEase(Ease.InQuad);
        }

        private void SetMaterial(Material material) =>
            GetComponent<Renderer>().material = material;
    }
}