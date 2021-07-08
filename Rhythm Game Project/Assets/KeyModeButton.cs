namespace Menu
{
    using UnityEngine;
    using TMPro;
    using UnityEngine.UI;
    using System.Collections;

    public sealed class KeyModeButton : MonoBehaviour
    {
        #region Constants
        private readonly Vector3 textScaleTo = new Vector3(1.25f, 1.25f, 1f);
        private readonly Vector3 effectTextScaleTo = new Vector3(1.75f, 1.75f, 1f);
        #endregion

        #region Private Fields
        [SerializeField] private TextMeshProUGUI keyText = default;
        [SerializeField] private TextMeshProUGUI keyEffectText = default;

        [SerializeField] private Image colorImage = default;

        [SerializeField] private FlashCanvasGroup flashCanvasGroup = default;

        private Transform keyTextTransform;
        private Transform keyEffectTextTransform;
        #endregion

        #region Public Methods
        private void Awake()
        {
            keyTextTransform = keyText.transform;
            keyEffectTextTransform = keyEffectText.transform;
        }

        public void SetColorImageColor(Color32 _color)
        {
            colorImage.color = _color;
        }

        public void SetText(string _text)
        {
            keyText.SetText(_text);
            keyEffectText.SetText(_text);
        }

        public void PlaySelectedBeatAnimation()
        {
            keyTextTransform.localScale = Vector3.one;
            keyEffectTextTransform.localScale = Vector3.one;
            LeanTween.cancel(keyText.gameObject);
            LeanTween.cancel(keyEffectText.gameObject);

            LeanTween.scale(keyText.gameObject, textScaleTo, 1f).setEasePunch();
            LeanTween.scale(keyEffectText.gameObject, effectTextScaleTo, 1f).setEasePunch();
        }

        public void PlayButtonSelectedAnimation()
        {
            flashCanvasGroup.PlayFlashAnimation();
        }
        #endregion
    }
}