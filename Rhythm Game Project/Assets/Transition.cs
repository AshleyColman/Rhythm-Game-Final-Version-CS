namespace SceneLoading
{
    using System.Collections;
    using UnityEngine;

    public sealed class Transition : MonoBehaviour
    {
        #region Constants
        public const float TransitionDuration = 1f;
        #endregion

        #region Private Fields
        [SerializeField] private GameObject transition = default;
        [SerializeField] private CanvasGroup transitionCanvasGroup = default;
        #endregion

        #region Public Methods
        public void PlayFadeInTween()
        {
            StopCoroutine("PlayFadeInTweenCoroutine");
            StartCoroutine(PlayFadeInTweenCoroutine());
        }

        public void PlayFadeOutTween()
        {
            StopCoroutine("PlayFadeOutTweenCoroutine");
            StartCoroutine(PlayFadeOutTweenCoroutine());
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            transition.gameObject.SetActive(true);
        }

        private IEnumerator PlayFadeInTweenCoroutine()
        {
            transitionCanvasGroup.alpha = 1f;
            transition.gameObject.SetActive(true);
            LeanTween.cancel(transitionCanvasGroup.gameObject);
            LeanTween.alphaCanvas(transitionCanvasGroup, 0f, 1f);
            yield return new WaitForSeconds(TransitionDuration);
            transition.gameObject.SetActive(false);
            yield return null;
        }

        private IEnumerator PlayFadeOutTweenCoroutine()
        {
            transitionCanvasGroup.alpha = 0f;
            transition.gameObject.SetActive(true);
            LeanTween.cancel(transitionCanvasGroup.gameObject);
            LeanTween.alphaCanvas(transitionCanvasGroup, 1f, 1f);
            yield return new WaitForSeconds(TransitionDuration);
            transition.gameObject.SetActive(false);
            yield return null;
        }
        #endregion
    }
}
