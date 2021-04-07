namespace Menu
{
    using UnityEngine;
    using SceneLoading;
    using File;
    using System.IO;
    using System.Collections;
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
        [SerializeField] private TextMeshProUGUI approachRateText = default;
        [SerializeField] private TextMeshProUGUI objectSizeText = default;
        [SerializeField] private TextMeshProUGUI healthDrainText = default;
        [SerializeField] private TextMeshProUGUI timingWindowText = default;

        [SerializeField] private DifficultyButton twoKeyDifficultyButton;
        [SerializeField] private DifficultyButton fourKeyDifficultyButton;
        [SerializeField] private DifficultyButton sixKeyDifficultyButton;

        [SerializeField] private HorizontalTextScroller songTextScroller;

        private IEnumerator lerpDifficultyStatisticTextCoroutine;

        private int selectedButtonIndex = -1;

        private Difficulty selectedDifficulty;

        private Notification notification;
        private Transition transition;
        private FileManager fileManager;
        private BeatmapSelectManager beatmapSelectManager;
        private BeatmapPreview beatmapPreview;
        private TopCanvasManager topCanvasManager;
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

        public void OnTick()
        {
            switch (selectedDifficulty)
            {
                case Difficulty.TwoKey:
                    twoKeyDifficultyButton.PlaySelectedBeatAnimation();
                    break;
                case Difficulty.FourKey:
                    fourKeyDifficultyButton.PlaySelectedBeatAnimation();
                    break;
                case Difficulty.SixKey:
                    sixKeyDifficultyButton.PlaySelectedBeatAnimation();
                    break;
            }
        }

        public void OnMeasure()
        {

        }

        // Loads beatmap without an image texture.
        public void LoadBeatmap(int _index, Difficulty _difficulty)
        {
            string beatmapPath = string.Empty;

            switch (_difficulty)
            {
                case Difficulty.TwoKey:
                    beatmapPath = $"{fileManager.BeatmapDirectories[_index]}/{FileTypes.TwoKeyFileType}";
                    break;
                case Difficulty.FourKey:
                    beatmapPath = $"{fileManager.BeatmapDirectories[_index]}/{FileTypes.FourKeyFileType}";
                    break;
                case Difficulty.SixKey:
                    beatmapPath = $"{fileManager.BeatmapDirectories[_index]}/{FileTypes.SixKeyFileType}";
                    break;
            }

            if (File.Exists(beatmapPath) == true)
            {
                fileManager.Load(beatmapPath);

                if (_index != selectedButtonIndex)
                {
                    SetDifficultyButtonGrades(_index);
                }

                SetSelectedButtonIndex(_index);

                songText.SetText(fileManager.Beatmap.SongName);
                artistText.SetText(fileManager.Beatmap.ArtistName);
                creatorText.SetText($"created by {fileManager.Beatmap.CreatorName}");
                keyText.SetText($"{fileManager.Beatmap.TotalKeys}k");
                PlayDifficultyStatisticsAnimation();

                UnselectCurrentDifficultyButton();
                SelectBeatmapDifficulty();
                
                // Load Leaderboard. 
                // Update visuals.


                songTextScroller.Scroll();
            }
        }

        // Loads the beatmap with an image texture passed for preview.
        public void LoadBeatmap(int _index, Difficulty _difficulty, Texture _imageTexture)
        {
            LoadBeatmap(_index, _difficulty);
            beatmapPreview.SetBackgroundImage(_imageTexture);
            beatmapPreview.ActivateSongSlider();
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
            topCanvasManager = FindObjectOfType<TopCanvasManager>();
        }

        private void SetSelectedButtonIndex(int _index)
        {
            selectedButtonIndex = _index;
        }

        private void SelectBeatmapDifficulty()
        {
            switch (fileManager.Beatmap.Difficulty)
            {
                case Difficulty.TwoKey:
                    twoKeyDifficultyButton.SelectButton();
                    selectedDifficulty = Difficulty.TwoKey;
                    notification.DisplayNotification(NotificationType.TwoKey, "2K", 1f);
                    break;
                case Difficulty.FourKey:
                    fourKeyDifficultyButton.SelectButton();
                    selectedDifficulty = Difficulty.FourKey;
                    notification.DisplayNotification(NotificationType.FourKey, "4K", 1f);
                    break;
                case Difficulty.SixKey:
                    sixKeyDifficultyButton.SelectButton();
                    selectedDifficulty = Difficulty.SixKey;
                    notification.DisplayNotification(NotificationType.SixKey, "6K", 1f);
                    break;
            }
        }

        private void UnselectCurrentDifficultyButton()
        {
            switch (selectedDifficulty)
            {
                case Difficulty.TwoKey:
                    twoKeyDifficultyButton.UnselectButton();
                    break;
                case Difficulty.FourKey:
                    fourKeyDifficultyButton.UnselectButton();
                    break;
                case Difficulty.SixKey:
                    sixKeyDifficultyButton.UnselectButton();
                    break;
            }
        }

        private void SetActivityForButtons(int _index)
        {
            SetActivityForButton(twoKeyDifficultyButton);

            SetActivityForButton(fourKeyDifficultyButton);

            SetActivityForButton(sixKeyDifficultyButton);
        }

        private void SetDifficultyButtonGrades(int _index)
        {
            TextMeshProUGUI gradeText = beatmapSelectManager.BeatmapButtonList[_index].GetDifficultyGradeText(Difficulty.TwoKey);
            twoKeyDifficultyButton.SetGradeText(gradeText);

            gradeText = beatmapSelectManager.BeatmapButtonList[_index].GetDifficultyGradeText(Difficulty.FourKey);
            fourKeyDifficultyButton.SetGradeText(gradeText);

            gradeText = beatmapSelectManager.BeatmapButtonList[_index].GetDifficultyGradeText(Difficulty.SixKey);
            sixKeyDifficultyButton.SetGradeText(gradeText);
        }

        private void SetActivityForButton(DifficultyButton _difficultyButton)
        {
            /*
            if (string.Equals(_level, "x"))
            {
                _difficultyButton.DisableButton();
            }
            else
            {
                _difficultyButton.ActivateButton();
            }
            */
            _difficultyButton.ActivateButton();
        }

        private void PlayDifficultyStatisticsAnimation()
        {
            if (lerpDifficultyStatisticTextCoroutine != null)
            {
                StopCoroutine(lerpDifficultyStatisticTextCoroutine);
            }

            lerpDifficultyStatisticTextCoroutine = LerpDifficultyStatisticTextCoroutine();
            StartCoroutine(lerpDifficultyStatisticTextCoroutine);
        }

        private IEnumerator LerpDifficultyStatisticTextCoroutine()
        {
            float lerpTimer = 0f;
            float lerpDuration = 0.5f;
            float previousFrameApproachRate = 0f;
            float approachRate = 0f;
            float objectSize = 0f;
            float previousFrameObjectSize = 0f;
            float healthDrain = 0f;
            float previousFrameHealthDrain = 0f;
            float timingWindow = 0f;
            float previousFrameTimingWindow = 0f;

            while (lerpTimer < lerpDuration)
            {
                lerpTimer += (Time.deltaTime / lerpDuration);

                UpdateStatisticText(ref approachRate, ref previousFrameApproachRate, fileManager.Beatmap.ApproachRate,
                    lerpTimer, "ar:", approachRateText);

                UpdateStatisticText(ref objectSize, ref previousFrameObjectSize, fileManager.Beatmap.ObjectSize,
                    lerpTimer, "os:", objectSizeText);

                UpdateStatisticText(ref healthDrain, ref previousFrameHealthDrain, fileManager.Beatmap.HealthDrain, 
                    lerpTimer, "hd:", healthDrainText);

                UpdateStatisticText(ref timingWindow, ref previousFrameTimingWindow, fileManager.Beatmap.TimingWindow,
                    lerpTimer, "tw:", timingWindowText);

                yield return null;
            }

            yield return null;
        }

        // Lerps and updates the text. Ref passed to modify original value passed from LerpDifficultyStatisticTextCoroutine().
        private void UpdateStatisticText(ref float _value, ref float _previousFrameValue, float _valueToLerpTo,
            float _lerpTimer, string _prefix, TextMeshProUGUI _text)
        {
            _value = Mathf.Lerp(0f, _valueToLerpTo, _lerpTimer);

            if (_value != _previousFrameValue)
            {
                _text.SetText($"{_prefix}{_value.ToString("F0")}");
            }

            _previousFrameValue = _value;
        }
        #endregion
    }
}