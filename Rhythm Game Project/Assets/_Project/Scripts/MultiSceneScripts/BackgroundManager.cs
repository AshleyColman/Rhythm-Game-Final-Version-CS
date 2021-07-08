namespace Background
{
    using ImageLoad;
    using Loading;
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    public sealed class BackgroundManager : MonoBehaviour
    {
        #region Constants
        private const float FadeDuration = 0.5f;
        #endregion

        #region Private Fields
        [SerializeField] private readonly LoadingIcon loadingIcon = default;

        [SerializeField] private CanvasGroup[] imageCanvasGroupArr = default;

        [SerializeField] private Image[] imageArr = default;

        private Transform[] imageTransformArr = default;

        private int activeImageIndex = 0;

        private ImageLoader imageLoader;

        private IEnumerator transitionAndLoadNewImageCoroutine;
        #endregion

        #region Public Methods
        public void SetNewImageReferences(Image[] _imageArr)
        {
            if (imageArr.Length != 0)
            {
                Array.Clear(imageArr, 0, imageArr.Length);
                Array.Clear(imageTransformArr, 0, imageTransformArr.Length);
                Array.Clear(imageCanvasGroupArr, 0, imageCanvasGroupArr.Length);
            }

            imageArr = new Image[_imageArr.Length];
            imageTransformArr = new Transform[_imageArr.Length];
            imageCanvasGroupArr = new CanvasGroup[_imageArr.Length];

            for (int i = 0; i < _imageArr.Length; i++)
            {
                imageArr[i] = _imageArr[i];
                imageTransformArr[i] = _imageArr[i].transform;
                imageCanvasGroupArr[i] = _imageArr[i].GetComponent<CanvasGroup>();
            }

            ResetActiveIndex();
        }

        public void PlayImageScaleTween(Vector3 _startScale, Vector3 _scaleTo, float _duration)
        {
            if (imageArr.Length != 0)
            {
                LeanTween.cancel(imageArr[activeImageIndex].gameObject);
                imageTransformArr[activeImageIndex].localScale = _startScale;
                LeanTween.scale(imageArr[activeImageIndex].gameObject, _scaleTo, _duration).setEaseOutExpo();
            }
        }

        public void PlayImageScaleTween()
        {
            //if (imageArr.Length != 0)
            //{
            //    LeanTween.cancel(imageArr[activeImageIndex].gameObject);
            //    imageTransformArr[activeImageIndex].localScale = Vector3.one;
            //    LeanTween.scale(imageArr[activeImageIndex].gameObject, VectorConstants.Vector101, 1f).setEasePunch();
            //}
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

        public void TransitionToImageFromIndex(int _index)
        {
            imageArr[activeImageIndex].gameObject.SetActive(false);
            SetActiveIndex(_index);
            imageArr[activeImageIndex].gameObject.SetActive(true);
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            imageLoader = FindObjectOfType<ImageLoader>();
        }

        private IEnumerator TransitionAndLoadNewImageCoroutine(Texture _imageTexture)
        {
            //WaitForSeconds waitForSeconds = new WaitForSeconds(FadeDuration);

            //// Cancel current tweens.
            //for (int i = 0; i < imageCanvasGroupArr.Length; i++)
            //{
            //    LeanTween.cancel(imageCanvasGroupArr[i].gameObject);
            //}

            //// Activate previous active Image.
            //imageArr[previousbackgroundImageIndex].gameObject.SetActive(true);

            //// Fade images.
            //LeanTween.alphaCanvas(backgroundImageCanvasGroupArray[activeImageIndex], 0f, FadeDuration);
            //UpdateCurrentActiveImage();
            //LeanTween.alphaCanvas(backgroundImageCanvasGroupArray[activeImageIndex], 1f, FadeDuration);

            //// Set new image texture.
            //imageArr[activeImageIndex].material.mainTexture = _imageTexture;

            //yield return waitForSeconds;

            //// Deactivate non active image.
            //imageArr[previousbackgroundImageIndex].gameObject.SetActive(false);
            yield return null;
        }

        private void LoadNewImageFromTexture(Texture2D _texture)
        {
            imageArr[activeImageIndex].material.mainTexture = _texture;
        }

        private void ResetActiveIndex()
        {
            activeImageIndex = 0;
        }

        private void SetActiveIndex(int _index)
        {
            activeImageIndex = _index;  
        }
        #endregion
    }
}
