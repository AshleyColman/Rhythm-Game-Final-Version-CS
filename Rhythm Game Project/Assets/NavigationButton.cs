namespace Menu
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public sealed class NavigationButton : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] private byte buttonMenuIndex = default;

        [SerializeField] private Button button = default;

        [SerializeField] private EventTrigger eventTrigger = default;

        [SerializeField] private TextMeshProUGUI unselectedText = default;
        [SerializeField] private TextMeshProUGUI selectedText = default;

        [SerializeField] private FlashCanvasGroup flashCanvasGroup = default;

        private ModePanel modePanel;
        private NavigationPanel navigationPanel;
        private MenuManager menuManager;
        #endregion

        #region Properties
        public byte ButtonMenuIndex => buttonMenuIndex;
        #endregion

        #region Public Methods
        public void OnClick()
        {
            navigationPanel.UnselectCurrentButton();
            navigationPanel.SetCurrentNavigationButton(this);
            ShowSelectedText();
            flashCanvasGroup.PlayFlashAnimation();
            DisableButtonIntractability();
            modePanel.NavigationButton_OnPointerEnter(buttonMenuIndex);
        }

        public void OnClick_TransitionToMenu()
        {
            OnClick();
            menuManager.TransitionToMenu(buttonMenuIndex);
        }

        public void OnPointerEnter()
        {
            flashCanvasGroup.PlayFlashAnimation();
            modePanel.NavigationButton_OnPointerEnter(buttonMenuIndex);
            navigationPanel.SetCurrentOnPointerEnterNavigationButton(this);
        }

        public void OnPointerExit()
        {
            modePanel.NavigationButton_OnPointerExit();
        }

        public void EnableButtonIntractability()
        {
            button.interactable = true;
        }

        public void DisableButtonIntractability()
        {
            button.interactable = false;
        }

        public void EnableEventTrigger()
        {
            eventTrigger.enabled = true;
        }

        public void DisableEventTrigger()
        {
            eventTrigger.enabled = false;
        }

        public void Deselect()
        {
            ShowUnselectedText();
            EnableButtonIntractability();
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            modePanel = FindObjectOfType<ModePanel>();
            navigationPanel = FindObjectOfType<NavigationPanel>();
            menuManager = FindObjectOfType<MenuManager>();
        }

        private void ShowSelectedText()
        {
            unselectedText.gameObject.SetActive(false);
            selectedText.gameObject.SetActive(true);
        }

        private void ShowUnselectedText()
        {
            unselectedText.gameObject.SetActive(true);
            selectedText.gameObject.SetActive(false);
        }
        #endregion
    }
}