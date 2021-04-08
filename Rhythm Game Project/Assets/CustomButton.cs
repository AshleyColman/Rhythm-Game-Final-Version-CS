namespace Menu
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public sealed class CustomButton : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] private Button button = default;

        [SerializeField] private Image colorImage = default;

        [SerializeField] private TextMeshProUGUI text = default;
        #endregion
        #region Public Methods
        public void SetColorImageColor(Color32 _color)
        {
            colorImage.color = _color;
        }

        public void SetText(string _text)
        {
            text.SetText(_text);
        }

        public void DisableInteractableButton()
        {
            button.interactable = false;
        }

        public void EnableInteractableButton()
        {
            button.interactable = true;
        }
        #endregion
    }
}
