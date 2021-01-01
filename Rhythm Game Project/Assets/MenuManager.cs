namespace Menu
{
    using UnityEngine;
    using System.Collections;
    using Settings;

    public sealed class MenuManager : MonoBehaviour
    {
        #region Constants
        public const byte StartMenuIndex = 0;
        public const byte ModeMenuIndex = 1;
        public const byte QuickplayMenuIndex = 2;
        //public const byte EditorMenuIndex = 0;
        //public const byte DownloadMenuIndex = 0;
        //public const byte RankingsMenuIndex = 0;
        //public const byte SettingsMenuIndex = 0;
        //public const byte ProfileMenuIndex = 0;
        public const byte ExitMenuIndex = 8;
        #endregion

        #region Private Fields
        private int currentMenuIndex = 0;

        private IMenu currentMenuScript;

        private StartMenuManager startMenuManager;
        private ModeMenuManager modeMenuManager;
        private AccountPanel accountPanel;
        private LoginManager loginManager;
        private SignupManager signupManager;
        private TopCanvasManager topCanvasManager;
        private QuickplayMenuManager quickplayMenuManager;
        #endregion

        #region Properties
        public int CurrentMenuIndex => currentMenuIndex;
        #endregion

        #region Public Methods
        // ACCOUNT (needs changing)
        public void TransitionStartMenuToAccountPanel()
        {
            accountPanel.TransitionIn();
        }

        public void TransitionAccountPanelToModeMenu()
        {
            accountPanel.TransitionOut();
            modeMenuManager.TransitionIn();
        }

        public void TransitionAccountPanelToSignupPanel()
        {
            accountPanel.TransitionOutAccountOptionsPanel();
            signupManager.TransitionIn();
        }

        public void TransitionSignupPanelToAccountPanel()
        {
            signupManager.TransitionOut();
            accountPanel.TransitionInAccountOptionsPanel();
        }

        public void TransitionAccountPanelToLoginPanel()
        {
            accountPanel.TransitionOutAccountOptionsPanel();
            loginManager.TransitionIn();
        }

        public void TransitionLoginPanelToAccountPanel()
        {
            loginManager.TransitionOut();
            accountPanel.TransitionInAccountOptionsPanel();
        }

        public void TransitionSignupPanelToLoginPanel()
        {
            signupManager.TransitionOut();
            loginManager.TransitionIn();
        }

        public void TransitionLoginPanelToModePanel()
        {
            loginManager.TransitionOut();
            accountPanel.TransitionOut();
            startMenuManager.TransitionOutStartAndAccountPanel();
            modeMenuManager.TransitionIn();
            topCanvasManager.TransitionInMenuOverlay();
        }

        public void TransitionSignupPanelToModePanel()
        {
            signupManager.TransitionOut();
            accountPanel.TransitionOut();
            startMenuManager.TransitionOutStartAndAccountPanel();
            modeMenuManager.TransitionIn();
            topCanvasManager.TransitionInMenuOverlay();
        }

        public void TransitionModePanelToQuickplayPanel()
        {
            modeMenuManager.TransitionOut();
            quickplayMenuManager.TransitionIn();
        }



        public void TransitionToMenu(int _menuIndex)
        {
            if (currentMenuScript is null == false)
            {
                currentMenuScript.TransitionOut();
            }

            switch (_menuIndex)
            {
                case 0:
                    startMenuManager.TransitionIn();
                    UpdateCurrentMenuScript(startMenuManager);
                    UpdateCurrentMenuIndex(StartMenuIndex);
                    break;
                case 1:
                    modeMenuManager.TransitionIn();
                    UpdateCurrentMenuScript(modeMenuManager);
                    UpdateCurrentMenuIndex(ModeMenuIndex);
                    break;
                case 2:
                    quickplayMenuManager.TransitionIn();
                    UpdateCurrentMenuScript(quickplayMenuManager);
                    UpdateCurrentMenuIndex(QuickplayMenuIndex);
                    break;
            }
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            startMenuManager = FindObjectOfType<StartMenuManager>();
            modeMenuManager = FindObjectOfType<ModeMenuManager>();
            accountPanel = FindObjectOfType<AccountPanel>();
            signupManager = FindObjectOfType<SignupManager>();
            loginManager = FindObjectOfType<LoginManager>();
            topCanvasManager = FindObjectOfType<TopCanvasManager>();
            quickplayMenuManager = FindObjectOfType<QuickplayMenuManager>();
        }

        private void Start()
        {
            TransitionToMenu(StartMenuIndex);
        }

        private void UpdateCurrentMenuScript(IMenu _menuScript)
        {
            currentMenuScript = _menuScript;
        }

        private void UpdateCurrentMenuIndex(int _index)
        {
            currentMenuIndex = _index;
        }
        #endregion
    }
}
