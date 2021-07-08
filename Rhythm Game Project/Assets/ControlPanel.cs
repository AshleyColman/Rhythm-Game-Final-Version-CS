namespace Menu
{
    using TMPro;
    using UnityEngine;

    public sealed class ControlPanel : MonoBehaviour
    {
        #region Constants
        private byte TextSizeMultiplier = 16;
        #endregion

        #region Private Fields
        [SerializeField] private GameObject controlPanel = default;

        [SerializeField] private Transform textPanelTransform = default;

        [SerializeField] private GameObject[] spacingArr = default;

        [SerializeField] private TextMeshProUGUI[] controlTextArr = default;
        [SerializeField] private TextMeshProUGUI[] keyTextArr = default;

        [SerializeField] private CanvasGroup canvasGroup = default;

        private Vector3 animationEndPosition;
        private Vector3 animationStartPosition;

        private RectTransform[] controlTextRectTransformArr;
        private RectTransform[] keyTextRectTransformArr;
        #endregion

        #region Public Methods
        public void TransitionIn()
        {
            controlPanel.gameObject.SetActive(true);
        }

        public void SetControlText(string[] _controlTextArr, string[] _keyTextArr)
        {
            controlPanel.gameObject.SetActive(false);

            for (byte i = 0; i < controlTextArr.Length; i++)
            {
                if (i >= _controlTextArr.Length)
                {
                    DeactivateObjectIfActive(controlTextArr[i].gameObject);

                    if (i < spacingArr.Length)
                    {
                        DeactivateObjectIfActive(spacingArr[i]);
                    }
                }
                else
                {
                    controlTextArr[i].SetText(_controlTextArr[i]);
                    ActivateObjectIfActive(controlTextArr[i].gameObject);

                    if (i < spacingArr.Length)
                    {
                        ActivateObjectIfActive(spacingArr[i]);
                    }
                }
            }

            for (byte i = 0; i < keyTextArr.Length; i++)
            {
                if (i >= _keyTextArr.Length)
                {
                    DeactivateObjectIfActive(keyTextArr[i].gameObject);
                }
                else
                {
                    keyTextArr[i].SetText(_keyTextArr[i]);
                    ActivateObjectIfActive(keyTextArr[i].gameObject);
                }
            }

            UpdateTextSizes();
            controlPanel.gameObject.SetActive(true);
            PlayDisplayTween();
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            SetRectTransforms();

            animationEndPosition = textPanelTransform.transform.localPosition;
            animationStartPosition = new Vector3(animationEndPosition.x, animationEndPosition.y - 50f, animationEndPosition.z);
        }

        private void SetRectTransforms()
        {
            controlTextRectTransformArr = new RectTransform[controlTextArr.Length];
            keyTextRectTransformArr = new RectTransform[keyTextArr.Length];

            for (byte i = 0; i < controlTextArr.Length; i++)
            {
                controlTextRectTransformArr[i] = controlTextArr[i].rectTransform;
                keyTextRectTransformArr[i] = keyTextArr[i].rectTransform;
            }
        }

        private void UpdateTextSizes()
        {
            for (byte i = 0; i < controlTextArr.Length; i++)
            {
                controlTextRectTransformArr[i].sizeDelta = new Vector2(controlTextArr[i].text.Length * TextSizeMultiplier, 
                    controlTextRectTransformArr[i].sizeDelta.y);

                keyTextRectTransformArr[i].sizeDelta = new Vector2(keyTextArr[i].text.Length * TextSizeMultiplier,
                    keyTextRectTransformArr[i].sizeDelta.y);
            }
        }

        private void DeactivateObjectIfActive(GameObject _object)
        {
            if (_object.gameObject.activeSelf == true)
            {
                _object.gameObject.SetActive(false);
            }
        }

        private void ActivateObjectIfActive(GameObject _object)
        {
            if (_object.gameObject.activeSelf == false)
            {
                _object.gameObject.SetActive(true);
            }
        }

        private void PlayDisplayTween()
        {
            LeanTween.cancel(textPanelTransform.gameObject);
            textPanelTransform.localPosition = animationStartPosition;

            LeanTween.cancel(canvasGroup.gameObject);
            canvasGroup.alpha = 0f;

            LeanTween.alphaCanvas(canvasGroup, 1f, 2f).setEaseOutExpo();
            LeanTween.moveLocalY(textPanelTransform.gameObject, animationEndPosition.y, 1f).setEaseOutExpo();
        }
        #endregion
    }
}