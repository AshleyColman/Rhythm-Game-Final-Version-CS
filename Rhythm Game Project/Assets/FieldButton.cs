namespace Menu
{
    using UnityEngine;
    using TMPro;
    using System.Collections;

    public sealed class FieldButton : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] private TextMeshProUGUI fieldText = default;
        [SerializeField] private TextMeshProUGUI valueText = default;
        [SerializeField] private TextMeshProUGUI fieldEffectText = default;
        [SerializeField] private TextMeshProUGUI valueEffectText = default;

        [SerializeField] private FlashCanvasGroup flashCanvasGroup = default;
        #endregion

        #region Public Methods
        public void SetValueText(string _text)
        {
            valueText.SetText(_text);
            valueEffectText.SetText(_text);
        }

        public void ActivateText()
        {
            fieldText.gameObject.SetActive(true);
            valueText.gameObject.SetActive(true);
            fieldEffectText.gameObject.SetActive(true);
            valueEffectText.gameObject.SetActive(true);
            flashCanvasGroup.PlayFlashAnimation();
        }

        public void DeactivateText()
        {
            fieldText.gameObject.SetActive(false);
            valueText.gameObject.SetActive(false);
            fieldEffectText.gameObject.SetActive(false);
            valueEffectText.gameObject.SetActive(false);
        }
        #endregion
    }
}