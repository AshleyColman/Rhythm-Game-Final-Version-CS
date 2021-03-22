namespace Menu
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    using TMPro;

    public sealed class TopCanvasManager : MonoBehaviour
    {
        #region Constants
        public readonly string[] ControlDescriptionArray = new string[]
        {
            // Mode Screen
            "mouse: aim    escape: to title screen    arrows: navigate    1: quickplay    2: editor    3: rankings    4: settings    5: exit",
            // Quickplay Screen
            "mouse: aim    escape: to beatmap select screen    left/right: difficulty    up/down: beatmap    enter: play"
        };
        #endregion

        #region Private Fields
        [SerializeField] private TextMeshProUGUI controlDescriptionText = default;

        [SerializeField] private GameObject menuOverlay = default;

        [SerializeField] private Button[] modeButtonArray = default;
        private Button lastButtonClicked;

        private EventTrigger[] modeButtonTriggerArray = default;

        private MenuManager menuManager;
        #endregion

        #region Public Methods
        public void TransitionInMenuOverlay()
        {
            menuOverlay.gameObject.SetActive(true);
        }

        public void EnableModeButtonTriggers()
        {
            for (byte i = 0; i < modeButtonTriggerArray.Length; i++)
            {
                modeButtonTriggerArray[i].enabled = true;
            }
        }

        public void DisableModeButtonTriggers()
        {
            for (byte i = 0; i < modeButtonTriggerArray.Length; i++)
            {
                modeButtonTriggerArray[i].enabled = false;
            }
        }

        public void Button_Click_Transition(int _buttonIndex)
        {
            Button_Click(_buttonIndex);
            menuManager.TransitionToMenu(_buttonIndex);
        }

        public void Button_Click(int _buttonIndex)
        {
            if (lastButtonClicked is null == false)
            {
                lastButtonClicked.interactable = true;
            }

            modeButtonArray[_buttonIndex].interactable = false;
            lastButtonClicked = modeButtonArray[_buttonIndex];
            menuManager.TransitionToMenu(_buttonIndex);
        }

        public void UpdateControlDescriptionText(int _menuIndex)
        {
            controlDescriptionText.SetText(ControlDescriptionArray[_menuIndex]);
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            menuManager = FindObjectOfType<MenuManager>();
            SetModeTriggerArray();
        }

        private void SetModeTriggerArray()
        {
            modeButtonTriggerArray = new EventTrigger[modeButtonArray.Length];

            for (byte i = 0; i < modeButtonArray.Length; i++)
            {
                modeButtonTriggerArray[i] = modeButtonArray[i].GetComponent<EventTrigger>();
            }
        }
        #endregion
    }
}
