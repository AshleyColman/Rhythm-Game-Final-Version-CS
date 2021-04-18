namespace Menu
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Enums;
    using Audio;

    public sealed class BeatmapButton : MonoBehaviour
    {
        #region Private Fields
        private DateTime date;

        private int buttonIndex = 0;

        private float twoKeyDifficultyAccuracy = 0f;
        private float fourKeyDifficultyAccuracy = 0f;
        private float sixKeyDifficultyAccuracy = 0f;
        private float difficultyMasterAccuracy = 0f;
        private float audioStartTime = 0f;

        [SerializeField] private Image beatmapImage = default;

        [SerializeField] private TextMeshProUGUI songNameText = default;
        [SerializeField] private TextMeshProUGUI artistNameText = default;
        [SerializeField] private TextMeshProUGUI creatorNameText = default;
        [SerializeField] private TextMeshProUGUI dateText = default;
        [SerializeField] private TextMeshProUGUI numberText = default;
        [SerializeField] private TextMeshProUGUI difficultyMasteryText = default;
        [SerializeField] private TextMeshProUGUI twoKeyDifficultyGradeText = default;
        [SerializeField] private TextMeshProUGUI fourKeyDifficultyGradeText = default;
        [SerializeField] private TextMeshProUGUI sixKeyDifficultyGradeText = default;
        [SerializeField] private TextMeshProUGUI twoKeyDifficultyAccuracyText = default;
        [SerializeField] private TextMeshProUGUI fourKeyDifficultyAccuracyText = default;
        [SerializeField] private TextMeshProUGUI sixKeyDifficultyAccuracyText = default;

        [SerializeField] private Button twoKeyDifficultyButton = default;
        [SerializeField] private Button fourKeyDifficultyButton = default;
        [SerializeField] private Button sixKeyDifficultyButton = default;

        private BeatmapSelectManager beatmapSelectManager = default;
        private BeatmapOverviewManager beatmapOverviewManager = default;
        private QuickplayMenuManager quickplayMenuManager = default;
        #endregion

        #region Properties
        public Image BeatmapImage => beatmapImage;
        public float DifficultyMasterAccuracy => difficultyMasterAccuracy;
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
                case Difficulty.TwoKey:
                    twoKeyDifficultyButton.interactable = true;
                    twoKeyDifficultyAccuracyText.SetText($"{_accuracy}%");
                    twoKeyDifficultyAccuracy = _accuracy;
                    twoKeyDifficultyGradeText.SetText(_grade);
                    twoKeyDifficultyGradeText.colorGradientPreset = _gradeColor;
                    break;
                case Difficulty.FourKey:
                    fourKeyDifficultyButton.interactable = true;
                    fourKeyDifficultyAccuracyText.SetText($"{_accuracy}%");
                    fourKeyDifficultyAccuracy = _accuracy;
                    fourKeyDifficultyGradeText.SetText(_grade);
                    fourKeyDifficultyGradeText.colorGradientPreset = _gradeColor;
                    break;
                case Difficulty.SixKey:
                    sixKeyDifficultyButton.interactable = true;
                    sixKeyDifficultyAccuracyText.SetText($"{_accuracy}%");
                    sixKeyDifficultyAccuracy = _accuracy;
                    sixKeyDifficultyGradeText.SetText(_grade);
                    sixKeyDifficultyGradeText.colorGradientPreset = _gradeColor;
                    break;
            }
        }

        public void SetDifficultyGradeFalse(Difficulty _difficulty)
        {
            switch (_difficulty)
            {
                case Difficulty.TwoKey:
                    twoKeyDifficultyButton.interactable = false;
                    twoKeyDifficultyAccuracyText.SetText("0%");
                    twoKeyDifficultyAccuracy = 0f;
                    twoKeyDifficultyGradeText.SetText("x");
                    twoKeyDifficultyGradeText.color = Color.white;
                    break;
                case Difficulty.FourKey:
                    fourKeyDifficultyButton.interactable = false;
                    fourKeyDifficultyAccuracyText.SetText("0%");
                    fourKeyDifficultyAccuracy = 0f;
                    fourKeyDifficultyGradeText.SetText("x");
                    fourKeyDifficultyGradeText.color = Color.white;
                    break;
                case Difficulty.SixKey:
                    sixKeyDifficultyButton.interactable = false;
                    sixKeyDifficultyAccuracyText.SetText("0%");
                    sixKeyDifficultyAccuracy = 0f;
                    sixKeyDifficultyGradeText.SetText("x");
                    sixKeyDifficultyGradeText.color = Color.white;
                    break;
            }
        }

        public void CalculateDifficultyMasteryAccuracy()
        {
            float[] difficultyAccuracyArray = new float[] { twoKeyDifficultyAccuracy, fourKeyDifficultyAccuracy,
                sixKeyDifficultyAccuracy };

            difficultyMasterAccuracy = UtilityMethods.GetAverageFromNumberArr(difficultyAccuracyArray);

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
            LoadBeatmap();
        }

        public void LoadBeatmap()
        {
            beatmapOverviewManager.LoadBeatmap(buttonIndex, Difficulty.TwoKey, beatmapImage.mainTexture);
        }

        public void LoadBeatmapWithAudioAndImage(Difficulty _difficulty)
        {
            beatmapOverviewManager.LoadBeatmapWithAudioAndImage(buttonIndex, _difficulty, 
                beatmapImage.mainTexture, UnityEngine.Random.Range(0f, 95f));
        }

        public TextMeshProUGUI GetDifficultyGradeText(Difficulty _difficulty)
        {
            switch (_difficulty)
            {
                case Difficulty.TwoKey:
                    return twoKeyDifficultyGradeText;
                case Difficulty.FourKey:
                    return fourKeyDifficultyGradeText;
                case Difficulty.SixKey:
                    return sixKeyDifficultyGradeText;
                default:
                    return twoKeyDifficultyGradeText;
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