namespace Loading
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    public sealed class LoadingIcon : MonoBehaviour
    {
        #region Constants
        private const float rotateSpeed = 150f;
        #endregion

        #region Private Fields
        [SerializeField] private GameObject loadingIcon = default;

        [SerializeField] private Transform loadingIconCachedTransform = default;

        [SerializeField] private CanvasGroup canvasGroup = default;

        private IEnumerator hideLoadingIconCoroutine;
        #endregion

        #region Public Methods
        public void DisplayLoadingIcon()
        {
            loadingIconCachedTransform.localRotation = Quaternion.identity;
            loadingIconCachedTransform.localScale = Vector3.zero;
            canvasGroup.alpha = 0f;
            LeanTween.cancel(loadingIcon);

            loadingIcon.gameObject.SetActive(true);
            LeanTween.alphaCanvas(canvasGroup, 1f, 0.25f).setEaseOutExpo();
            LeanTween.scale(loadingIcon, Vector3.one, 0.25f).setEaseOutExpo();
        }

        public void HideLoadingIcon()
        {
            if (hideLoadingIconCoroutine != null)
            {
                StopCoroutine(hideLoadingIconCoroutine);
            }

            hideLoadingIconCoroutine = HideLoadingIconCoroutine();
            StartCoroutine(hideLoadingIconCoroutine);
        }
        #endregion

        #region Private Methods
        private void Update()
        {
            transform.Rotate(0f, 0f, (rotateSpeed * Time.deltaTime));
        }

        private IEnumerator HideLoadingIconCoroutine()
        {
            LeanTween.cancel(loadingIcon);
            LeanTween.alphaCanvas(canvasGroup, 0f, 0.25f).setEaseOutExpo();
            LeanTween.scale(loadingIcon, Vector3.zero, 0.25f).setEaseOutExpo();
            yield return new WaitForSeconds(0.25f);
            loadingIcon.gameObject.SetActive(false);
            yield return null;
        }
        #endregion
    }
}