namespace Menu
{
    using System.Collections;
    using UnityEngine;

    public sealed class FlashCanvasGroup : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] private CanvasGroup canvasGroup = default;

        private IEnumerator playFlashAnimation;
        #endregion

        #region Public Methods
        public void PlayFlashAnimation()
        {
            if (playFlashAnimation != null)
            {
                StopCoroutine(playFlashAnimation);
            }

            playFlashAnimation = PlayFlashAnimationCoroutine();
            StartCoroutine(playFlashAnimation);
        }
        #endregion

        #region Private Methods
        private IEnumerator PlayFlashAnimationCoroutine()
        {
            canvasGroup.alpha = 0f;
            LeanTween.cancel(canvasGroup.gameObject);
            canvasGroup.gameObject.SetActive(true);

            LeanTween.alphaCanvas(canvasGroup, 1f, 0.2f).setLoopPingPong(1);
            yield return new WaitForSeconds(0.4f);
            canvasGroup.gameObject.SetActive(false);
            yield return null;
        }
        #endregion
    }
}