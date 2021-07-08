namespace Menu
{
    using TMPro;
    using UnityEngine;

    public sealed class TextPanel : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] private TextMeshProUGUI text = default;

        [SerializeField] private Transform textPanelTransform = default;

        private TextTyper textTyper;
        #endregion

        #region Public Methods
        public void TypeText(string _text)
        {
            textTyper.TypeTextCancel(_text, text);
        }

        public void TransitionInPanel(Vector3 _startPosition, float _endPositionY)
        {
            ActivatePanel();
            LeanTween.cancel(textPanelTransform.gameObject);
            textPanelTransform.localPosition = _startPosition;
            textPanelTransform.gameObject.SetActive(true);
            LeanTween.moveLocalY(textPanelTransform.gameObject, _endPositionY, 1f).setEaseOutExpo();
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            textTyper = FindObjectOfType<TextTyper>();
            text.SetText(string.Empty);
        }

        private void ActivatePanel()
        {
            if (textPanelTransform.gameObject.activeSelf == false)
            {
                textPanelTransform.gameObject.SetActive(true);
            }
        }

        private void DeactivatePanel()
        {
            if (textPanelTransform.gameObject.activeSelf == true)
            {
                textPanelTransform.gameObject.SetActive(false);
            }
        }
        #endregion
    }
}