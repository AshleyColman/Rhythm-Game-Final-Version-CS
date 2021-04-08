namespace Menu
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public sealed class DescriptionPanel : MonoBehaviour
    {
        #region Constants
        private readonly string[] defaultDescriptionArr = new string[]
        {
            "gameplay tip 1",
            "gameplay tip 2",
            "gameplay tip 3",
            "gameplay tip 4",
            "gameplay tip 5",
            "update 1",
            "update 2",
            "update 3",
            "update 4",
            "update 5"
        };
        #endregion

        #region Private Fields
        [SerializeField] private GameObject descriptionPanel = default;

        [SerializeField] private TextMeshProUGUI descriptionText = default;

        [SerializeField] private Image colorImage = default;

        [SerializeField] private CanvasGroup canvasGroup = default;

        private Vector3 animationEndPosition = default;
        private Vector3 animationStartPosition = default;

        private Transform descriptionTextCachedTransform;

        private IEnumerator playDescriptionArrayLoopCoroutine;

        private string[] descriptionArr;
        #endregion

        #region Public Methods
        public void PlayDefaultDescriptionArr()
        {
            ClearDescriptionArr();
            SetNewDescriptionArr(defaultDescriptionArr);
            PlayDescriptionArrayLoop();
        }

        public void SetDescriptionArr(string[] _arr, Color32 _color)
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
        }

        public void SetSingleDescription(string _text, Color32 _color)
        {
            descriptionText.SetText(_text);
            SetPanelColor(_color);
            PlayDisplayTween();
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            animationEndPosition = descriptionText.transform.localPosition;
            animationStartPosition = new Vector3(animationEndPosition.x, animationEndPosition.y - 50f, animationEndPosition.z);
            descriptionTextCachedTransform = descriptionText.transform;
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