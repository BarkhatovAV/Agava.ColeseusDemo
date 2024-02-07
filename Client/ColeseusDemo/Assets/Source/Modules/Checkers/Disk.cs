using System.Collections;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    [RequireComponent(typeof(Renderer))]
    internal class Disk : MonoBehaviour
    {
        internal Square CurrentSquare { get; private set; }
        internal bool IsWhite { get; private set; }
        internal int Id { get; private set; }
        internal int WidthPosition { get; private set; }
        internal int LengthPosition { get; private set; }
        internal Material DefaultMaterial { get; private set; }

        private Coroutine _coroutine;

        internal void Construct(Square currentMapSquare, bool isWhite, Material material)
        {
            CurrentSquare = currentMapSquare;
            IsWhite = isWhite;
            DefaultMaterial = material;

            SetMaterial(material);
            SetCurrentMapSquare(currentMapSquare);
        }

        internal void SetCurrentMapSquare(Square currentMapSquare)
        {
            CurrentSquare = currentMapSquare;

            WidthPosition = currentMapSquare.WidthPosition;
            LengthPosition = currentMapSquare.LengthPosition;
            currentMapSquare.Occupy(IsWhite);
        }

        internal void SmoothlyMove(Transform movableObject, Vector3 target, float moveSpeed) =>
            StartCoroutine(FloatDown(movableObject, target, moveSpeed));

        internal void SetId(int id) =>
            Id = id;

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