namespace Menu
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    public sealed class TopCanvasManager : MonoBehaviour
    {
        #region Private Fields
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
