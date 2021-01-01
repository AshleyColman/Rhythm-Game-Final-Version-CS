using UnityEngine;
using System.Collections;

namespace Gameplay
{
    public sealed class HitobjectFollower : MonoBehaviour
    {
        #region Constants
        private readonly Vector3 scaleTo = new Vector3(1.25f, 1.25f, 1f);
        #endregion

        #region Constants
        private const float TimeToReachTarget = 0.25f;
        #endregion

        #region Private Fields
        private Transform cachedTransform;

        private float timer = 0f;

        private Vector3 targetPosition = Vector3.zero; // delete

        private HitobjectManager hitobjectManager;
        private GameManager gameManager;
        #endregion

        #region Public Methods
        public void ResetPositionTimer()
        {
            timer = 0;
        }

        public void TrackMoveToPosition()
        {
            StopCoroutine("MoveToPositionCoroutine");
            StartCoroutine(MoveToPositionCoroutine());
        }

        public void PlayRhythmTween()
        {
            LeanTween.cancel(cachedTransform.gameObject);
            cachedTransform.localScale = Vector3.one;

            LeanTween.scale(cachedTransform.gameObject, scaleTo, 0.1f).setLoopPingPong(1);
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            hitobjectManager = MonoBehaviour.FindObjectOfType<HitobjectManager>();
            gameManager = MonoBehaviour.FindObjectOfType<GameManager>();
            cachedTransform = this.gameObject.transform;
        }

        private IEnumerator MoveToPositionCoroutine()
        {
            while (gameManager.GameplayStarted == true)
            {
                if (hitobjectManager.CurrentHittableObject != null && 
                    hitobjectManager.CurrentHittableObject.gameObject.activeSelf == true)
                {
                    timer += Time.deltaTime / TimeToReachTarget;
                    cachedTransform.localPosition = Vector3.Lerp(cachedTransform.localPosition,
                      hitobjectManager.CurrentHittableObject.CachedTransform.localPosition, timer);
                }
                yield return null;
            }
            yield return null;
        }
        #endregion
    }
}
