namespace Menu
{
    using UnityEngine;
    using SceneLoading;

    public sealed class QuickplayMenuManager : MonoBehaviour, IMenu
    {
        #region Constants
        public const byte BeatmapSelectMenuIndex = 0;
        public const byte BeatmapOverviewMenuIndex = 1;
        #endregion

        #region Private Fields
        [SerializeField] private GameObject quickplayScreen = default;

        private int currentMenuIndex = 0;

        private IMenu currentMenuScript;

        private BeatmapOverviewManager beatmapOverviewManager;
        private BeatmapSelectManager beatmapSelectManager;
        private DescriptionPanel descriptionPanel;
        #endregion

        #region Public Methods
        public void TransitionIn()
        {
            descriptionPanel.PlayDefaultDescriptionArr();
            quickplayScreen.gameObject.SetActive(true);
            beatmapSelectManager.TransitionIn();
            UpdateCurrentMenuScript(beatmapSelectManager);
            UpdateCurrentMenuIndex(BeatmapSelectMenuIndex);
        }

        public void TransitionOut()
        {
            // Transition out current active menu           
        }

        public void OnTick()
        {
            currentMenuScript.OnTick();
        }

        public void OnMeasure()
        {
            currentMenuScript.OnMeasure();
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
                    beatmapSelectManager.TransitionIn();
                    UpdateCurrentMenuScript(beatmapSelectManager);
                    UpdateCurrentMenuIndex(BeatmapSelectMenuIndex);
                    break;
                case 1:
                    beatmapOverviewManager.TransitionIn();
                    UpdateCurrentMenuScript(beatmapOverviewManager);
                    UpdateCurrentMenuIndex(BeatmapOverviewMenuIndex);
                    break;
            }
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            beatmapOverviewManager = FindObjectOfType<BeatmapOverviewManager>();
            beatmapSelectManager = FindObjectOfType<BeatmapSelectManager>();
            descriptionPanel = FindObjectOfType<DescriptionPanel>();
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