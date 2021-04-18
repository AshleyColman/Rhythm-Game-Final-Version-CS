namespace Menu
{
    using UnityEngine;
    using File;

    public sealed class ChallengeButtonPanel : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] private BeatmapChallengeButton standardClearButton = default;
        [SerializeField] private BeatmapChallengeButton hiddenClearButton = default;
        [SerializeField] private BeatmapChallengeButton minesClearButton = default;
        [SerializeField] private BeatmapChallengeButton fullComboButton = default;
        [SerializeField] private BeatmapChallengeButton maxPercentageButton = default;
        
        private FileManager fileManager;
        #endregion

        #region Public Methods
        public void SetButtonVisuals()
        {
            SetButtonVisualBasedOnFileBool(fileManager.Beatmap.StandardClear, standardClearButton);
            SetButtonVisualBasedOnFileBool(fileManager.Beatmap.HiddenClear, hiddenClearButton);
            SetButtonVisualBasedOnFileBool(fileManager.Beatmap.MinesClear, minesClearButton);
            SetButtonVisualBasedOnFileBool(fileManager.Beatmap.FullCombo, fullComboButton);
            SetButtonVisualBasedOnFileBool(fileManager.Beatmap.MaxPercentage, maxPercentageButton);
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            fileManager = FindObjectOfType<FileManager>();
        }
        
        private void SetButtonVisualBasedOnFileBool(bool _fileBool , BeatmapChallengeButton _beatmapChallengeButton)
        {
            if (_fileBool == true)
            {
                if (_beatmapChallengeButton.HasAchieved == false)
                {
                    _beatmapChallengeButton.SetAchieved();
                }
            }
            else
            {
                if (_beatmapChallengeButton.HasAchieved == true)
                {
                    _beatmapChallengeButton.SetNotAchieved();
                }
            }
        }
        #endregion
    }
}