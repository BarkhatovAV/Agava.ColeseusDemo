using System.Collections;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    [RequireComponent(typeof(Renderer))]
    internal class TempDisk : MonoBehaviour
    {
        internal TempSquare CurrentMapSquare { get; private set; }
        internal bool IsWhite { get; private set; }
        internal int Id { get; private set; }
        internal int WidthPosition { get; private set; }
        internal int LengthPosition { get; private set; }
        internal Material DefaultMaterial { get; private set; }

        private Coroutine _coroutine;

        internal void Construct(TempSquare currentMapSquare, bool isWhite, int id, Material material)
        {
            CurrentMapSquare = currentMapSquare;
            IsWhite = isWhite;
            Id = id;
            DefaultMaterial = material;

            SetMaterial(material);
            SetCurrentMapSquare(currentMapSquare);
        }

        internal void SetCurrentMapSquare(TempSquare currentMapSquare)
        {
            CurrentMapSquare = currentMapSquare;

            WidthPosition = currentMapSquare.WidthPosition;
            LengthPosition = currentMapSquare.LengthPosition;
            currentMapSquare.Occupy(IsWhite);
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