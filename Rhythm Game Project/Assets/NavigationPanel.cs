namespace Menu
{
    using UnityEngine;
    using Player;

    public sealed class NavigationPanel : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] private GameObject navigationPanel = default;

        [SerializeField] private NavigationButton[] buttonArray = default;

        private NavigationButton currentSelectedNavigationButton;
        private NavigationButton currentOnPointerEnterNavigationButton;

        private TextPanel textPanel = default;
        #endregion

        #region Properties
        public NavigationButton CurrentSelectedNavigationButton => currentSelectedNavigationButton;
        public NavigationButton CurrentOnPointerEnterNavigationButton => currentOnPointerEnterNavigationButton;
        #endregion

        #region Public Methods
        public void TransitionIn()
        {
            navigationPanel.gameObject.SetActive(true);
            textPanel.TransitionInPanel(new Vector3(0f, 580f, 0f), 492f);
            textPanel.TypeText($"playing as {Player.Username}");
        }

        public void EnableButtonEventTriggers()
        {
            for (byte i = 0; i < buttonArray.Length; i++)
            {
                buttonArray[i].EnableEventTrigger();
            }
        }

        public void DisableButtonEventTriggers()
        {
            for (byte i = 0; i < buttonArray.Length; i++)
            {
                buttonArray[i].DisableEventTrigger();
            }
        }

        public void OnClickButton(int _index)
        {
            buttonArray[_index].OnClick();
        }

        public void SetCurrentNavigationButton(NavigationButton _navigationButton)
        {
            currentSelectedNavigationButton = _navigationButton;
        }

        public void UnselectCurrentButton()
        {
            if (currentSelectedNavigationButton is null == false)
            {
                currentSelectedNavigationButton.Deselect();
            }
        }

        public void SetCurrentOnPointerEnterNavigationButton(NavigationButton _navigationButton)
        {
            currentOnPointerEnterNavigationButton = _navigationButton;
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            textPanel = FindObjectOfType<TextPanel>();
        }
        #endregion
    }
}