namespace Menu
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using Enums;

    public sealed class DescriptionPanel : MonoBehaviour
    {
        #region Constants
        private const byte DisplayTweenDuration = 2;

        private readonly string[] defaultDescriptionArr = new string[]
        {
            "welcome to project dia!",
            "did you know? this game is still being developed?"
        };
        #endregion

        #region Private Fields
        [SerializeField] private GameObject descriptionPanel = default;

        [SerializeField] private TextMeshProUGUI descriptionText = default;

        [SerializeField] private Image colorImage = default;

        [SerializeField] private CanvasGroup canvasGroup = default;

        [SerializeField] private FlashCanvasGroup flashCanvasGroup = default;

        private Vector3 animationEndPosition;
        private Vector3 animationStartPosition;

        private Transform descriptionTextCachedTransform;

        private IEnumerator playDescriptionArrayLoopCoroutine;
        private IEnumerator playNewTextThenDefaultTextArrayCoroutine;

        private string[] descriptionArr;

        private ColorCollection colorCollection;
        #endregion

        #region Public Methods
        public void TransitionIn()
        {
            descriptionPanel.gameObject.SetActive(true);
        }

        public void PlayDefaultTextArray()
        {
            CheckToClearDescriptionArr();
            PlayDescriptionArrayLoop();
        }

        public void PlayNewTextArray(string[] _arr, Color32 _color)
        {
            ClearDescriptionArr();
            SetNewDescriptionArr(_arr);
            SetPanelColor(_color);
            PlayDescriptionArrayLoop();
        }

        public void SetPanelColor(Color32 _color)
        {
            if (colorImage.color != _color)
            {
                colorImage.color = _color;
            }

            PlayFlashAnimation();
        }

        public void SetPanelColor(ColorName colorName)
        {
            switch (colorName)
            {
                case ColorName.RED:
                    colorImage.color = colorCollection.RedColor080;
                    break;
                case ColorName.ORANGE:
                    colorImage.color = colorCollection.OrangeColor080;
                    break;
                case ColorName.LIGHT_BLUE:
                    colorImage.color = colorCollection.LightBlueColor080;
                    break;
                case ColorName.DARK_BLUE:
                    colorImage.color = colorCollection.DarkBlueColor080;
                    break;
                case ColorName.PURPLE:
                    colorImage.color = colorCollection.PurpleColor080;
                    break;
                case ColorName.PINK:
                    colorImage.color = colorCollection.PinkColor080;
                    break;
                case ColorName.YELLOW:
                    colorImage.color = colorCollection.YellowColor080;
                    break;
                case ColorName.LIGHT_GREEN:
                    colorImage.color = colorCollection.LightGreenColor080;
                    break;
                case ColorName.GREY:
                    colorImage.color = colorCollection.GreyColor05;
                    break;
                default:
                    colorImage.color = colorCollection.WhiteColor080;
                    break;
            }

            PlayFlashAnimation();
        }

        public void PlayNewTextThenDefaultTextArray(string _text)
        {
            if (playNewTextThenDefaultTextArrayCoroutine != null)
            {
                StopCoroutine(playNewTextThenDefaultTextArrayCoroutine);
            }

            playNewTextThenDefaultTextArrayCoroutine = PlayNewTextThenDefaultTextArrayCoroutine(_text);
            StartCoroutine(playNewTextThenDefaultTextArrayCoroutine);
        }

        public void PlayNewText(string _text)
        {
            StopAllCoroutines();
            descriptionText.SetText(_text);
            PlayDisplayTween();
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            colorCollection = FindObjectOfType<ColorCollection>();

            animationEndPosition = descriptionText.transform.localPosition;
            animationStartPosition = new Vector3(animationEndPosition.x, animationEndPosition.y - 50f, animationEndPosition.z);
            descriptionTextCachedTransform = descriptionText.transform;

            descriptionArr = defaultDescriptionArr;
        }

        public IEnumerator PlayNewTextThenDefaultTextArrayCoroutine(string _text)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(DisplayTweenDuration);

            descriptionText.SetText(_text);
            PlayDisplayTween();
            yield return waitForSeconds;
            PlayDefaultTextArray();
            yield return null;
        }

        private void PlayFlashAnimation()
        {
            flashCanvasGroup.PlayFlashAnimation();
        }

        private void CheckToClearDescriptionArr()
        {
            if (descriptionArr != null)
            {
                if (descriptionArr.Length > 0)
                {
                    if (descriptionArr[0] != defaultDescriptionArr[0])
                    {
                        ClearDescriptionArr();
                        SetNewDescriptionArr(defaultDescriptionArr);
                    }
                }
            }
        }

        private void ClearDescriptionArr()
        {
            if (descriptionArr is null == false)
            {
                Array.Clear(descriptionArr, 0, descriptionArr.Length);
            }
        }

        private void SetNewDescriptionArr(string[] _arr)
        {
            descriptionArr = new string[_arr.Length];

            for (int i = 0; i < descriptionArr.Length; i++)
            {
                descriptionArr[i] = _arr[i];
            }
        }

        private void PlayDescriptionArrayLoop()
        {
            if (playDescriptionArrayLoopCoroutine != null)
            {
                StopCoroutine(playDescriptionArrayLoopCoroutine);
            }

            playDescriptionArrayLoopCoroutine = PlayDescriptionArrayLoopCoroutine();
            StartCoroutine(playDescriptionArrayLoopCoroutine);
        }

        private IEnumerator PlayDescriptionArrayLoopCoroutine()
        {
            WaitForSeconds textDisplayDuration = new WaitForSeconds(6f);
            WaitForSeconds textHideDuration = new WaitForSeconds(2f);

            for (int i = 0; i < descriptionArr.Length; i++)
            {
                descriptionText.SetText(descriptionArr[i]);
                PlayDisplayTween();
                yield return textDisplayDuration;
                PlayHideTween();
                yield return textHideDuration;
            }

            // Loop.
            PlayDescriptionArrayLoop();

            yield return null;
        }

        private void PlayDisplayTween()
        {
            descriptionTextCachedTransform.localPosition = animationStartPosition;
            LeanTween.cancel(descriptionText.gameObject);
            canvasGroup.alpha = 0f;
            LeanTween.cancel(canvasGroup.gameObject);

            LeanTween.alphaCanvas(canvasGroup, 1f, 2f).setEaseOutExpo();
            LeanTween.moveLocalY(descriptionText.gameObject, animationEndPosition.y, 1f).setEaseOutExpo();
        }

        private void PlayHideTween()
        {
            LeanTween.alphaCanvas(canvasGroup, 0f, 2f).setEaseOutExpo();
        }
        #endregion
    }
}