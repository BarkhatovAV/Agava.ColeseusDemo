using System.Collections;
using UnityEngine;

namespace ColyseusDemo.Checkers
{
    internal class SmoothMover : MonoBehaviour
    {
        private const float PermissibleMovementInaccuracy = 0.01f;

        private Coroutine _coroutine;

        internal void StartMoving(Transform movableObject, Vector3 target, float moveSpeed)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(FloatDown(movableObject, target, moveSpeed));
        }

        private IEnumerator FloatDown(Transform movableObject, Vector3 target, float moveSpeed)
        {
            while (movableObject.position.y > target.y + 0.05f)
            {
                movableObject.position = Vector3.Lerp(movableObject.position, target, moveSpeed * Time.deltaTime);

                yield return null;
            }

            movableObject.position = target;
        }
    }
}