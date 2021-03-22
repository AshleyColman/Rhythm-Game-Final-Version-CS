namespace Background
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;
    using Menu;
    using Loading;

    public sealed class BackgroundManager : MonoBehaviour
    {
        #region Constants
        private readonly Vector3 scaleTo = new Vector3(1.01f, 1.01f, 1f);

        private const float FadeDuration = 0.5f;
        #endregion

        #region Private Fields
        [SerializeField] private LoadingIcon loadingIcon = default;

        [SerializeField] private CanvasGroup[] backgroundImageCanvasGroupArray = default;

        [SerializeField] private Image[] backgroundImageArray = default;

        private Transform[] backgroundImageCachedTransformArray = default;

        private byte activeBackgroundImageIndex = 0;
        private byte previousbackgroundImageIndex = 1;

        private ImageLoader imageLoader;

        private IEnumerator transitionAndLoadNewImageCoroutine;
        #endregion

        #region Public Methods
        public void PlayRhythmScaleTween()
        {
            LeanTween.cancel(backgroundImageArray[activeBackgroundImageIndex].gameObject);
            backgroundImageCachedTransformArray[activeBackgroundImageIndex].localScale = Vector3.one;
            LeanTween.scale(backgroundImageArray[activeBackgroundImageIndex].gameObject, scaleTo, 1f).setEasePunch();
        }

        public void TransitionAndLoadNewImage(Texture _imageTexture)
        {
            if (transitionAndLoadNewImageCoroutine != null)
            {
                StopCoroutine(transitionAndLoadNewImageCoroutine);
            }

            transitionAndLoadNewImageCoroutine = TransitionAndLoadNewImageCoroutine(_imageTexture);
            StartCoroutine(transitionAndLoadNewImageCoroutine);
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            imageLoader = FindObjectOfType<ImageLoader>();
            SetCachedTransforms();
        }

        private void SetCachedTransforms()
        {
            backgroundImageCachedTransformArray = new Transform[backgroundImageArray.Length];

            for (byte i = 0; i < backgroundImageArray.Length; i++)
            {
                backgroundImageCachedTransformArray[i] = backgroundImageArray[i].transform;
            }
        }

        private IEnumerator TransitionAndLoadNewImageCoroutine(Texture _imageTexture)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(FadeDuration);

            // Cancel current tweens.
            for (byte i = 0; i < backgroundImageCanvasGroupArray.Length; i++)
            {
                LeanTween.cancel(backgroundImageCanvasGroupArray[i].gameObject);
            }

            // Activate previous active Image.
            backgroundImageArray[previousbackgroundImageIndex].gameObject.SetActive(true);

            // Fade images.
            LeanTween.alphaCanvas(backgroundImageCanvasGroupArray[activeBackgroundImageIndex], 0f, FadeDuration);
            UpdateCurrentActiveImage();
            LeanTween.alphaCanvas(backgroundImageCanvasGroupArray[activeBackgroundImageIndex], 1f, FadeDuration);

            // Set new image texture.
            backgroundImageArray[activeBackgroundImageIndex].material.mainTexture = _imageTexture;

            yield return waitForSeconds;

            // Deactivate non active image.
            backgroundImageArray[previousbackgroundImageIndex].gameObject.SetActive(false);
            yield return null;
        }

        private void UpdateCurrentActiveImage()
        {
            if (activeBackgroundImageIndex == 0)
            {
                previousbackgroundImageIndex = 0;
                activeBackgroundImageIndex = 1;
            }
            else if (activeBackgroundImageIndex == 1)
            {
                previousbackgroundImageIndex = 1;
                activeBackgroundImageIndex = 0;
            }
        }
        #endregion
    }
}
