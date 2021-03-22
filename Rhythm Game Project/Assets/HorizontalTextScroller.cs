namespace Menu
{
    using System.Collections;
    using UnityEngine;
    using TMPro;

    public sealed class HorizontalTextScroller : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] private TextMeshProUGUI[] textArr;

        [SerializeField] private float textSpaceMultiplier = 65f;
        [SerializeField] private float[] letterCountToStartScrollArr = default;

        [SerializeField] private CanvasGroup canvasGroup = default;

        private Transform[] textCachedTransformArr;

        private Vector3[] defaultTextPositionArr;

        private float[] scrollPositionToXArr;
        private float[] scrollDurationArr;

        private IEnumerator scrollCoroutine;
        #endregion

        #region Public Methods
        public void Scroll()
        {
            if (scrollCoroutine != null)
            {
                StopCoroutine(scrollCoroutine);
            }

            scrollCoroutine = ScrollCoroutine();
            StartCoroutine(scrollCoroutine);
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            textCachedTransformArr = new Transform[textArr.Length];
            defaultTextPositionArr = new Vector3[textArr.Length];
            scrollDurationArr = new float[textArr.Length];
            scrollPositionToXArr = new float[textArr.Length];

            for (int i = 0; i < textArr.Length; i++)
            {
                textCachedTransformArr[i] = textArr[i].transform;
                defaultTextPositionArr[i] = textCachedTransformArr[i].localPosition;
            }
        }

        private void SetScrollProperties()
        {
            for (int i = 0; i < textArr.Length; i++)
            {
                scrollPositionToXArr[i] = (defaultTextPositionArr[i].x - (textArr[i].text.Length * textSpaceMultiplier));
                scrollDurationArr[i] = textArr[i].text.Length;
            }
        }

        private IEnumerator ScrollCoroutine()
        {
            SetScrollProperties();
            ResetTweenProperties();

            PlayCanvasTween();

            yield return new WaitForSeconds(1f);

            for (int i = 0; i < textArr.Length; i++)
            {
                if (textArr[i].text.Length >= letterCountToStartScrollArr[i])
                {
                    LeanTween.moveLocalX(textArr[i].gameObject, scrollPositionToXArr[i], scrollDurationArr[i]);
                }
            }

            yield return null;
        }

        private void ResetTweenProperties()
        {
            for (int i = 0; i < textArr.Length; i++)
            {
                textCachedTransformArr[i].localPosition = defaultTextPositionArr[i];
                LeanTween.cancel(textArr[i].gameObject);
            }

            LeanTween.cancel(canvasGroup.gameObject);
            canvasGroup.alpha = 0f;
        }

        private void PlayCanvasTween()
        {
            LeanTween.alphaCanvas(canvasGroup, 1f, 1f);
        }
        #endregion
    }

}