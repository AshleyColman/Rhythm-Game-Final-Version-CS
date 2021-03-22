namespace Menu
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Enums;

    public sealed class BeatmapButton : MonoBehaviour
    {
        #region Private Fields
        private DateTime date;

        private int buttonIndex = 0;

        private float easyDifficultyAccuracy = 0f;
        private float normalDifficultyAccuracy = 0f;
        private float hardDifficultyAccuracy = 0f;
        private float difficultyMasterAccuracy = 0f;

        private float audioStartTime = 0f;

        [SerializeField] private Image beatmapImage = default;

        [SerializeField] private TextMeshProUGUI songNameText = default;
        [SerializeField] private TextMeshProUGUI artistNameText = default;
        [SerializeField] private TextMeshProUGUI creatorNameText = default;
        [SerializeField] private TextMeshProUGUI dateText = default;
        [SerializeField] private TextMeshProUGUI numberText = default;
        [SerializeField] private TextMeshProUGUI difficultyMasteryText = default;
        [SerializeField] private TextMeshProUGUI easyDifficultyLevelText = default;
        [SerializeField] private TextMeshProUGUI normalDifficultyLevelText = default;
        [SerializeField] private TextMeshProUGUI hardDifficultyLevelText = default;
        [SerializeField] private TextMeshProUGUI easyDifficultyGradeText = default;
        [SerializeField] private TextMeshProUGUI normalDifficultyGradeText = default;
        [SerializeField] private TextMeshProUGUI hardDifficultyGradeText = default;
        [SerializeField] private TextMeshProUGUI easyDifficultyAccuracyText = default;
        [SerializeField] private TextMeshProUGUI normalDifficultyAccuracyText = default;
        [SerializeField] private TextMeshProUGUI hardDifficultyAccuracyText = default;

        [SerializeField] private Button easyDifficultyButton = default;
        [SerializeField] private Button normalDifficultyButton = default;
        [SerializeField] private Button hardDifficultyButton = default;

        private BeatmapSelectManager beatmapSelectManager = default;
        private BeatmapOverviewManager beatmapOverviewManager = default;
        private QuickplayMenuManager quickplayMenuManager = default;
        #endregion

        #region Properties
        public Image BeatmapImage => beatmapImage;
        #endregion

        #region Public Methods
        public void SetBeatmapInformation(int _index, string _songName, string _artistName, string _creatorName, DateTime _date, 
            float _audioStartTime)
        {
            numberText.SetText((_index + 1).ToString());
            songNameText.SetText(_songName);
            artistNameText.SetText(_artistName);
            creatorNameText.SetText(_creatorName);
            dateText.SetText(_date.ToString());
            date = _date;
            audioStartTime = _audioStartTime;
            buttonIndex = _index;
        }

        public void SetDifficultyGradeTrue(Difficulty _difficulty, string _grade, float _accuracy, 
            TMP_ColorGradient _gradeColor)
        {
            switch (_difficulty)
            {
                case Difficulty.Easy:
                    easyDifficultyAccuracyText.SetText($"{_accuracy}%");
                    easyDifficultyAccuracy = _accuracy;
                    easyDifficultyGradeText.SetText(_grade);
                    easyDifficultyGradeText.colorGradientPreset = _gradeColor;
                    break;
                case Difficulty.Normal:
                    normalDifficultyAccuracyText.SetText($"{_accuracy}%");
                    normalDifficultyAccuracy = _accuracy;
                    normalDifficultyGradeText.SetText(_grade);
                    normalDifficultyGradeText.colorGradientPreset = _gradeColor;
                    break;
                case Difficulty.Hard:
                    hardDifficultyAccuracyText.SetText($"{_accuracy}%");
                    hardDifficultyAccuracy = _accuracy;
                    hardDifficultyGradeText.SetText(_grade);
                    hardDifficultyGradeText.colorGradientPreset = _gradeColor;
                    break;
            }
        }

        public void SetDifficultyLevelButton(Difficulty _difficulty, bool _fileExists, string _level)
        {
            switch (_difficulty)
            {
                case Difficulty.Easy:
                    easyDifficultyButton.interactable = _fileExists;
                    easyDifficultyLevelText.SetText(_level);
                    break;
                case Difficulty.Normal:
                    normalDifficultyButton.interactable = _fileExists;
                    normalDifficultyLevelText.SetText(_level);
                    break;
                case Difficulty.Hard:
                    hardDifficultyButton.interactable = _fileExists;
                    hardDifficultyLevelText.SetText(_level);
                    break;
            }
        }

        public void SetDifficultyGradeFalse(Difficulty _difficulty)
        {
            switch (_difficulty)
            {
                case Difficulty.Easy:
                    easyDifficultyButton.interactable = false;
                    easyDifficultyAccuracyText.SetText("0%");
                    easyDifficultyAccuracy = 0f;
                    easyDifficultyGradeText.SetText("x");
                    easyDifficultyGradeText.color = Color.white;
                    break;
                case Difficulty.Normal:
                    normalDifficultyButton.interactable = false;
                    normalDifficultyAccuracyText.SetText("0%");
                    normalDifficultyAccuracy = 0f;
                    normalDifficultyGradeText.SetText("x");
                    normalDifficultyGradeText.color = Color.white;
                    break;
                case Difficulty.Hard:
                    hardDifficultyButton.interactable = false;
                    hardDifficultyAccuracyText.SetText("0%");
                    hardDifficultyAccuracy = 0f;
                    hardDifficultyGradeText.SetText("x");
                    hardDifficultyGradeText.color = Color.white;
                    break;
            }
        }

        public void CalculateDifficultyMasteryAccuracy()
        {
            float[] difficultyAccuracyArray = new float[] { easyDifficultyAccuracy, normalDifficultyAccuracy, hardDifficultyAccuracy };
            int totalIncrements = 0;

            for (byte i = 0; i < difficultyAccuracyArray.Length; i++)
            {
                if (difficultyAccuracyArray[i] != 0)
                {
                    difficultyMasterAccuracy += difficultyAccuracyArray[i];
                    totalIncrements++;
                }
            }

            if (totalIncrements == 0)
            {
                difficultyMasterAccuracy = 0f;
            }
            else
            {
                difficultyMasterAccuracy = ((difficultyMasterAccuracy / (totalIncrements * 100) * 100));
            }

            difficultyMasteryText.SetText($"{difficultyMasterAccuracy.ToString("F2")}%");
        }

        public void Button_OnHover()
        {
            if (beatmapSelectManager.CurrentBeatmapPreviewIndex != buttonIndex)
            {
                beatmapSelectManager.LoadBeatmapPreview(buttonIndex, UnityEngine.Random.Range(0f, 95f), beatmapImage.mainTexture);
            }
        }

        public void Button_OnClick()
        {
            quickplayMenuManager.TransitionToMenu(QuickplayMenuManager.BeatmapOverviewMenuIndex);
            beatmapOverviewManager.LoadBeatmap(buttonIndex, Difficulty.Easy, beatmapImage.mainTexture); // Difficulty passed for testing.
        }

        public string GetDifficultyLevelString(Difficulty _difficulty)
        {
            switch (_difficulty)
            {
                case Difficulty.Easy:
                    return easyDifficultyLevelText.text;
                case Difficulty.Normal:
                    return normalDifficultyLevelText.text;
                case Difficulty.Hard:
                    return hardDifficultyLevelText.text;
                default:
                    return String.Empty;
            }
        }

        public TextMeshProUGUI GetDifficultyGradeText(Difficulty _difficulty)
        {
            switch (_difficulty)
            {
                case Difficulty.Easy:
                    return easyDifficultyGradeText;
                case Difficulty.Normal:
                    return normalDifficultyGradeText;
                case Difficulty.Hard:
                    return hardDifficultyGradeText;
                default:
                    return easyDifficultyGradeText;
            }
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            beatmapSelectManager = FindObjectOfType<BeatmapSelectManager>();
            beatmapOverviewManager = FindObjectOfType<BeatmapOverviewManager>();
            quickplayMenuManager = FindObjectOfType<QuickplayMenuManager>();
        }
        #endregion
    }
}