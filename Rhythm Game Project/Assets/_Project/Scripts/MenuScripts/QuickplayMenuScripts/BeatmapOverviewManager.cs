namespace Menu
{
    using UnityEngine;
    using SceneLoading;
    using File;
    using System.IO;
    using Enums;
    using TMPro;

    public sealed class BeatmapOverviewManager : MonoBehaviour, IMenu
    {
        #region Private Fields
        [SerializeField] private GameObject beatmapOverviewScreen = default;

        [SerializeField] private TextMeshProUGUI songText = default;
        [SerializeField] private TextMeshProUGUI artistText = default;
        [SerializeField] private TextMeshProUGUI creatorText = default;
        [SerializeField] private TextMeshProUGUI keyText = default;

        [SerializeField] private DifficultyButton easyDifficultyButton;
        [SerializeField] private DifficultyButton normalDifficultyButton;
        [SerializeField] private DifficultyButton hardDifficultyButton;

        [SerializeField] private HorizontalTextScroller songTextScroller;

        private int selectedButtonIndex = -1;

        private Difficulty selectedDifficulty;

        private Notification notification;
        private Transition transition;
        private FileManager fileManager;
        private BeatmapSelectManager beatmapSelectManager;
        private BeatmapPreview beatmapPreview;
        #endregion

        #region Properties
        public int SelectedButtonIndex => selectedButtonIndex;
        #endregion

        #region Public Methods
        public void TransitionIn()
        {
            transition.PlayFadeInTween();
            beatmapOverviewScreen.gameObject.SetActive(true);
        }

        public void TransitionOut()
        {
            beatmapOverviewScreen.gameObject.SetActive(false);
        }

        // Loads beatmap without an image texture.
        public void LoadBeatmap(int _index, Difficulty _difficulty)
        {
            string beatmapPath = string.Empty;

            switch (_difficulty)
            {
                case Difficulty.Easy:
                    beatmapPath = $"{fileManager.BeatmapDirectories[_index]}/{FileTypes.EasyFileType}";
                    break;
                case Difficulty.Normal:
                    beatmapPath = $"{fileManager.BeatmapDirectories[_index]}/{FileTypes.NormalFileType}";
                    break;
                case Difficulty.Hard:
                    beatmapPath = $"{fileManager.BeatmapDirectories[_index]}/{FileTypes.HardFileType}";
                    break;
            }

            if (File.Exists(beatmapPath) == true)
            {
                fileManager.Load(beatmapPath);

                if (_index != selectedButtonIndex)
                {
                    SetDifficultyButtonLevels(_index);
                    SetDifficultyButtonGrades(_index);
                }

                SetSelectedButtonIndex(_index);

                songText.SetText(fileManager.Beatmap.SongName);
                artistText.SetText(fileManager.Beatmap.ArtistName);
                creatorText.SetText($"created by {fileManager.Beatmap.CreatorName}");
                keyText.SetText($"{fileManager.Beatmap.TotalKeys}k");

                UnselectCurrentDifficultyButton();
                SelectBeatmapDifficulty();
                // Load leaderboard
                // Update beatmap difficulty (AR, OD, CS)


                songTextScroller.Scroll();
            }
        }

        // Loads the beatmap with an image texture passed for preview.
        public void LoadBeatmap(int _index, Difficulty _difficulty, Texture _imageTexture)
        {
            LoadBeatmap(_index, _difficulty);
            beatmapPreview.SetBackgroundImage(_imageTexture);
            beatmapPreview.ActivateSongSlider();
            //songTextScroller.Scroll();
            //artistTextScroller.Scroll();
        }
	    #endregion

	    #region Private Methods
        private void Awake()
        {
            notification = FindObjectOfType<Notification>();
            transition = FindObjectOfType<Transition>();
            fileManager = FindObjectOfType<FileManager>();
            beatmapSelectManager = FindObjectOfType<BeatmapSelectManager>();
            beatmapPreview = FindObjectOfType<BeatmapPreview>();
        }

        private void SetSelectedButtonIndex(int _index)
        {
            selectedButtonIndex = _index;
        }

        private void SelectBeatmapDifficulty()
        {
            switch (fileManager.Beatmap.Difficulty)
            {
                case Difficulty.Easy:
                    easyDifficultyButton.SelectButton();
                    selectedDifficulty = Difficulty.Easy;
                    notification.DisplayNotification(NotificationType.EasyDifficulty, "easy", 1f);
                    break;
                case Difficulty.Normal:
                    normalDifficultyButton.SelectButton();
                    selectedDifficulty = Difficulty.Normal;
                    notification.DisplayNotification(NotificationType.NormalDifficulty, "normal", 1f);
                    break;
                case Difficulty.Hard:
                    hardDifficultyButton.SelectButton();
                    selectedDifficulty = Difficulty.Hard;
                    notification.DisplayNotification(NotificationType.HardDifficulty, "hard", 1f);
                    break;
            }
        }

        private void UnselectCurrentDifficultyButton()
        {
            switch (selectedDifficulty)
            {
                case Difficulty.Easy:
                    easyDifficultyButton.UnselectButton();
                    break;
                case Difficulty.Normal:
                    normalDifficultyButton.UnselectButton();
                    break;
                case Difficulty.Hard:
                    hardDifficultyButton.UnselectButton();
                    break;
            }
        }

        private void SetDifficultyButtonLevels(int _index)
        {
            string level = beatmapSelectManager.BeatmapButtonList[_index].GetDifficultyLevelString(Difficulty.Easy);
            easyDifficultyButton.SetLevelText(level);
            SetActivityForButton(level, easyDifficultyButton);

            level = beatmapSelectManager.BeatmapButtonList[_index].GetDifficultyLevelString(Difficulty.Normal);
            normalDifficultyButton.SetLevelText(level);
            SetActivityForButton(level, normalDifficultyButton);

            level = beatmapSelectManager.BeatmapButtonList[_index].GetDifficultyLevelString(Difficulty.Hard);
            hardDifficultyButton.SetLevelText(level);
            SetActivityForButton(level, hardDifficultyButton);
        }

        private void SetDifficultyButtonGrades(int _index)
        {
            TextMeshProUGUI gradeText = beatmapSelectManager.BeatmapButtonList[_index].GetDifficultyGradeText(Difficulty.Easy);
            easyDifficultyButton.SetGradeText(gradeText);

            gradeText = beatmapSelectManager.BeatmapButtonList[_index].GetDifficultyGradeText(Difficulty.Normal);
            normalDifficultyButton.SetGradeText(gradeText);

            gradeText = beatmapSelectManager.BeatmapButtonList[_index].GetDifficultyGradeText(Difficulty.Hard);
            hardDifficultyButton.SetGradeText(gradeText);
        }

        private void SetActivityForButton(string _level, DifficultyButton _difficultyButton)
        {
            if (string.Equals(_level, "x"))
            {
                _difficultyButton.DisableButton();
            }
            else
            {
                _difficultyButton.ActivateButton();
            }
        }
	    #endregion
    }
}