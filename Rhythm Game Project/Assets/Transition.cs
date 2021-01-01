namespace SceneLoading
{
    using UnityEngine;

    public sealed class Transition : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] private GameObject transition = default;
        [SerializeField] private CanvasGroup transitionCanvasGroup = default;
        #endregion

        #region Public Methods
        public void PlayFadeInTween()
        {
            transitionCanvasGroup.alpha = 1f;
            LeanTween.cancel(transitionCanvasGroup.gameObject);

            LeanTween.alphaCanvas(transitionCanvasGroup, 0f, 1f);
        }

        public void PlayFadeOutTween()
        {
            transitionCanvasGroup.alpha = 0f;
            LeanTween.cancel(transitionCanvasGroup.gameObject);

            LeanTween.alphaCanvas(transitionCanvasGroup, 1f, 1f);
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            transition.gameObject.SetActive(true);
        }
        #endregion
    }
}
