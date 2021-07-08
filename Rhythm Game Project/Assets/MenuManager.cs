namespace Menu
{
    using UnityEngine;
    using System.Collections;
    using Settings;
    using SceneLoading;

    public sealed class MenuManager : MonoBehaviour
    {
        #region Constants
        public const byte StartMenuIndex = 0;
        public const byte QuickplayMenuIndex = 1;
        //public const byte EditorMenuIndex = 0;
        //public const byte DownloadMenuIndex = 0;
        //public const byte RankingsMenuIndex = 0;
        //public const byte SettingsMenuIndex = 0;
        //public const byte ProfileMenuIndex = 0;
        public const byte ExitMenuIndex = 7;
        #endregion

        #region Private Fields
        private int currentMenuIndex = 0;

        private IEnumerator transitionToMenuCoroutine;

        private IMenu currentMenuScript;

        private StartMenuManager startMenuManager;
        private OverlayCanvasManager overlayCanvasManager;
        private QuickplayMenuManager quickplayMenuManager;
        #endregion

        #region Properties
        public int CurrentMenuIndex => currentMenuIndex;
        #endregion

        #region Public Methods
        public void PlayCurrentMenuOnTick()
        {
            currentMenuScript.OnTick();
        }

        public void PlayCurrentMenuOnMeasure()
        {
            currentMenuScript.OnMeasure();
        }

        public void TransitionToMenu(int _menuIndex)
        {
            if (transitionToMenuCoroutine != null)
            {
                StopCoroutine(transitionToMenuCoroutine);
            }

            transitionToMenuCoroutine = TransitionToMenuCoroutine(_menuIndex);
            StartCoroutine(transitionToMenuCoroutine);
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            startMenuManager = FindObjectOfType<StartMenuManager>();
            overlayCanvasManager = FindObjectOfType<OverlayCanvasManager>();
            quickplayMenuManager = FindObjectOfType<QuickplayMenuManager>();
        }

        private void Start()
        {
            TransitionToMenu(StartMenuIndex);
        }

        private IEnumerator TransitionToMenuCoroutine(int _menuIndex)
        {
            if (currentMenuScript is null == false)
            {
                currentMenuScript.TransitionOut();
            }

            yield return new WaitForSeconds(Transition.TransitionDuration);

            switch (_menuIndex)
            {
                case 0:
                    startMenuManager.TransitionIn();
                    UpdateCurrentMenuScript(startMenuManager);
                    UpdateCurrentMenuIndex(StartMenuIndex);
                    break;
                case 1:
                    quickplayMenuManager.TransitionIn();
                    UpdateCurrentMenuScript(quickplayMenuManager);
                    UpdateCurrentMenuIndex(QuickplayMenuIndex);
                    break;
            }

            //topCanvasManager.UpdateControlDescriptionText(currentMenuIndex);

            yield return null;
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
