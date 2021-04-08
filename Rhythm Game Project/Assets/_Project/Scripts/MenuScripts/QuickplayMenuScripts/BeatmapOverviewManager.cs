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
        [SerializeField] private TextMeshProUGUI approachRateText = default;
        [SerializeField] private TextMeshProUGUI objectSizeText = default;
        [SerializeField] private TextMeshProUGUI healthDrainText = default;
        [SerializeField] private TextMeshProUGUI timingWindowText = default;
        [SerializeField] private TextMeshProUGUI difficultyMasteryText = default;

        [SerializeField] private DifficultyButton twoKeyDifficultyButton = default;
        [SerializeField] private DifficultyButton fourKeyDifficultyButton = default;
        [SerializeField] private DifficultyButton sixKeyDifficultyButton = default;

        [SerializeField] private CustomButton keyModeButton = default;

        [SerializeField] private HorizontalTextScroller songTextScroller;

        private IEnumerator lerpDifficultyStatisticTextCoroutine;
        private IEnumerator checkMenuInputCoroutine;

        private int selectedButtonIndex = -1;

        private Difficulty selectedDifficulty;

        private Notification notification;
        private Transition transition;
        private FileManager fileManager;
        private BeatmapSelectManager beatmapSelectManager;
        private BeatmapPreview beatmapPreview;
        private TopCanvasManager topCanvasManager;
        private DescriptionPanel descriptionPanel;
        private ColorCollection colorCollection;
        #endregion

        #region Properties
        public int SelectedButtonIndex => selectedButtonIndex;
        #endregion

        #region Public Methods
        public void TransitionIn()
        {
            transition.PlayFadeInTween();
            beatmapOverviewScreen.gameObject.SetActive(true);
            CheckMenuInput();
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
                keyModeButton.SetText($"{fileManager.Beatmap.TotalKeys}k");

                PlayDifficultyStatisticsAnimation();

                UnselectCurrentDifficultyButton();
                SelectBeatmapDifficulty();
                
                // Load Leaderboard. 
                // Update visuals.


                songTextScroller.Scroll();
            }
            else
            {
                notification.DisplayNotification(NotificationType.Error, "beatmap file not found", 4f);
            }
        }

        // Loads the beatmap with an image texture passed for preview.
        public void LoadBeatmap(int _index, Difficulty _difficulty, Texture _imageTexture)
        {
            LoadBeatmap(_index, _difficulty);
            beatmapPreview.SetBackgroundImage(_imageTexture);
            beatmapPreview.ActivateSongSlider();
        }

        public void Button_OnClick_SelectDifficultyTwoKey()
        {
            if (selectedDifficulty != Difficulty.TwoKey)
            {
                LoadBeatmap(selectedButtonIndex, Difficulty.TwoKey);
            }
        }

        public void Button_OnClick_SelectDifficultyFourKey()
        {
            if (selectedDifficulty != Difficulty.FourKey)
            {
                LoadBeatmap(selectedButtonIndex, Difficulty.FourKey);
            }
        }

        public void Button_OnClick_SelectDifficultySixKey()
        {
            if (selectedDifficulty != Difficulty.SixKey)
            {
                LoadBeatmap(selectedButtonIndex, Difficulty.SixKey);
            }
        }

        public void SelectNextDifficulty()
        {
            switch (selectedDifficulty)
            {
                case Difficulty.TwoKey:
                    LoadBeatmap(selectedButtonIndex, Difficulty.FourKey);
                    break;
                case Difficulty.FourKey:
                    LoadBeatmap(selectedButtonIndex, Difficulty.SixKey);
                    break;
                case Difficulty.SixKey:
                    LoadBeatmap(selectedButtonIndex, Difficulty.TwoKey);
                    break;
            }
        }

        public void SelectPreviousDifficulty()
        {
            switch (selectedDifficulty)
            {
                case Difficulty.TwoKey:
                    LoadBeatmap(selectedButtonIndex, Difficulty.SixKey);
                    break;
                case Difficulty.FourKey:
                    LoadBeatmap(selectedButtonIndex, Difficulty.TwoKey);
                    break;
                case Difficulty.SixKey:
                    LoadBeatmap(selectedButtonIndex, Difficulty.FourKey);
                    break;
            }
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
            descriptionPanel = FindObjectOfType<DescriptionPanel>();
            colorCollection = FindObjectOfType<ColorCollection>();
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
                    descriptionPanel.SetPanelColor(colorCollection.YellowColor080);
                    keyModeButton.SetColorImageColor(colorCollection.YellowColor);
                    break;
                case Difficulty.FourKey:
                    fourKeyDifficultyButton.SelectButton();
                    selectedDifficulty = Difficulty.FourKey;
                    notification.DisplayNotification(NotificationType.FourKey, "4K", 1f);
                    descriptionPanel.SetPanelColor(colorCollection.PurpleColor080);
                    keyModeButton.SetColorImageColor(colorCollection.PurpleColor);
                    break;
                case Difficulty.SixKey:
                    sixKeyDifficultyButton.SelectButton();
                    selectedDifficulty = Difficulty.SixKey;
                    notification.DisplayNotification(NotificationType.SixKey, "6K", 1f);
                    descriptionPanel.SetPanelColor(colorCollection.RedColor080);
                    keyModeButton.SetColorImageColor(colorCollection.RedColor);
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

        private void SetDifficultyButtonGrades(int _index)
        {
            TextMeshProUGUI gradeText = beatmapSelectManager.BeatmapButtonList[_index].GetDifficultyGradeText(Difficulty.TwoKey);
            twoKeyDifficultyButton.SetGradeText(gradeText);

            gradeText = beatmapSelectManager.BeatmapButtonList[_index].GetDifficultyGradeText(Difficulty.FourKey);
            fourKeyDifficultyButton.SetGradeText(gradeText);

            gradeText = beatmapSelectManager.BeatmapButtonList[_index].GetDifficultyGradeText(Difficulty.SixKey);
            sixKeyDifficultyButton.SetGradeText(gradeText);
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
            float lerpDuration = 1f;
            float previousFrameApproachRate = 0f;
            float approachRate = 0f;
            float objectSize = 0f;
            float previousFrameObjectSize = 0f;
            float healthDrain = 0f;
            float previousFrameHealthDrain = 0f;
            float timingWindow = 0f;
            float previousFrameTimingWindow = 0f;
            float difficultyMasterAccuracy = 0f;
            float previousFrameDifficultyMasterAccuracy = 0f;

            while (lerpTimer < lerpDuration)
            {
                lerpTimer += (Time.deltaTime / lerpDuration);

                UpdateStatisticText(ref approachRate, ref previousFrameApproachRate, fileManager.Beatmap.ApproachRate,
                    lerpTimer, "ar:", true, "F0", approachRateText);

                UpdateStatisticText(ref objectSize, ref previousFrameObjectSize, fileManager.Beatmap.ObjectSize,
                    lerpTimer, "os:", true, "F0", objectSizeText);

                UpdateStatisticText(ref healthDrain, ref previousFrameHealthDrain, fileManager.Beatmap.HealthDrain, 
                    lerpTimer, "hd:", true, "F0", healthDrainText);

                UpdateStatisticText(ref timingWindow, ref previousFrameTimingWindow, fileManager.Beatmap.TimingWindow,
                    lerpTimer, "tw:", true, "F0", timingWindowText);

                UpdateStatisticText(ref difficultyMasterAccuracy, ref previousFrameDifficultyMasterAccuracy,
                    beatmapSelectManager.BeatmapButtonList[selectedButtonIndex].DifficultyMasterAccuracy,
                    lerpTimer, "%", false, "F2", difficultyMasteryText);
                yield return null;
            }

            yield return null;
        }

        private void CheckMenuInput()
        {
            if (checkMenuInputCoroutine != null)
            {
                StopCoroutine(checkMenuInputCoroutine);
            }

            checkMenuInputCoroutine = CheckMenuInputCoroutine();
            StartCoroutine(checkMenuInputCoroutine);
        }

        private IEnumerator CheckMenuInputCoroutine()
        {
            while (beatmapOverviewScreen.gameObject.activeSelf == true)
            {
                if (Input.anyKey)
                {
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        SelectNextDifficulty();
                    }

                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        SelectPreviousDifficulty();
                    }

                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        LoadPreviousBeatmap();
                        Debug.Log(selectedButtonIndex);
                    }

                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        LoadNextBeatmap();
                        Debug.Log(selectedButtonIndex);
                    }
                }

                yield return null;
            }
            yield return null;
        }

        // Load the next beatmap when the right arrow key has been pressed.
        private void LoadNextBeatmap()
        {
            if (beatmapSelectManager.BeatmapButtonList.Count != 0)
            {
                if ((selectedButtonIndex + 1) < beatmapSelectManager.BeatmapButtonList.Count)
                {
                    beatmapSelectManager.BeatmapButtonList[selectedButtonIndex + 1].Button_OnHover();
                    beatmapSelectManager.BeatmapButtonList[selectedButtonIndex + 1].LoadBeatmap();
                }
                else
                {
                    beatmapSelectManager.BeatmapButtonList[0].Button_OnHover();
                    beatmapSelectManager.BeatmapButtonList[0].LoadBeatmap();
                }
            }
        }

        // Load the previous beatmap when the left arrow key has been pressed.
        private void LoadPreviousBeatmap()
        {
            if (beatmapSelectManager.BeatmapButtonList.Count != 0)
            {
                if ((selectedButtonIndex - 1) < 0)
                {
                    beatmapSelectManager.BeatmapButtonList[beatmapSelectManager.BeatmapButtonList.Count - 1].Button_OnHover();
                    beatmapSelectManager.BeatmapButtonList[beatmapSelectManager.BeatmapButtonList.Count - 1].LoadBeatmap();
                }
                else
                {
                    beatmapSelectManager.BeatmapButtonList[selectedButtonIndex - 1].Button_OnHover();
                    beatmapSelectManager.BeatmapButtonList[selectedButtonIndex - 1].LoadBeatmap();
                }
            }
        }

        // Lerps and updates the text. Ref passed to modify original value passed from LerpDifficultyStatisticTextCoroutine().
        private void UpdateStatisticText(ref float _value, ref float _previousFrameValue, float _valueToLerpTo,
            float _lerpTimer, string _prefix, bool prefixAtStart, string stringFormat, TextMeshProUGUI _text)
        {
            _value = Mathf.Lerp(0f, _valueToLerpTo, _lerpTimer);

            if (_value != _previousFrameValue)
            {
                if (prefixAtStart == true)
                {
                    _text.SetText($"{_prefix}{_value.ToString(stringFormat)}");
                }
                else
                {
                    _text.SetText($"{_value.ToString(stringFormat)}{_prefix}");
                }
            }

            _previousFrameValue = _value;
        }
        #endregion
    }
}