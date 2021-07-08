namespace Menu
{
    using Level;
    using System.Collections;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public sealed class LeaderboardButton : MonoBehaviour 
    {
        #region Private Fields
        [SerializeField] private GameObject contentPanel = default;

        [SerializeField] private Image profileImage = default;

        [SerializeField] private TextMeshProUGUI positionText = default;
        [SerializeField] private TextMeshProUGUI nameText = default;
        [SerializeField] private TextMeshProUGUI scoreText = default;
        [SerializeField] private TextMeshProUGUI comboAndAccuracyText = default;
        [SerializeField] private TextMeshProUGUI gradeText = default;
        [SerializeField] private TextMeshProUGUI dateText = default;
        [SerializeField] private TextMeshProUGUI noRecordSetText = default;
        [SerializeField] private TextMeshProUGUI levelText = default;

        [SerializeField] private Slider levelSlider = default;

        [SerializeField] private ChallengeButtonPanel challengeButtonPanel = default;

        private IEnumerator playLerpAnimation;

        private LevelManager levelManager;
        #endregion

        #region Properties
        public Image ProfileImage => profileImage;
        #endregion

        #region Public Methods
        public void DeactivateContentPanel()
        {
            if (contentPanel.gameObject.activeSelf == true)
            {
                contentPanel.gameObject.SetActive(false);
            }
        }

        public void ActivateContentPanel()
        {
            if (contentPanel.gameObject.activeSelf == false)
            {
                contentPanel.gameObject.SetActive(true);
            }
        }

        public void DeactivateNoRecordSetText()
        {
            if (noRecordSetText.gameObject.activeSelf == true)
            {
                noRecordSetText.gameObject.SetActive(false);
            }
        }

        public void ActivateNoRecordText()
        {
            if (noRecordSetText.gameObject.activeSelf == false)
            {
                noRecordSetText.gameObject.SetActive(true);
            }
        }

        public void SetProfileImage(Texture _texture)
        {
        }

        public void SetNewProfileImageMaterial(Material _material)
        {
            profileImage.material = _material;
        }

        public void SetPositionText(string _text)
        {
            positionText.SetText(_text);
        }

        public void SetScoreText(string _text)
        {
            scoreText.SetText(_text);
        }

        public void SetComboAndAccuracyText(string _text)
        {
            comboAndAccuracyText.SetText(_text);
        }

        public void SetNameText(string _text)
        {
            nameText.SetText(_text);
        }

        public void SetGradeText(string _text, TMP_ColorGradient _color)
        {
            gradeText.SetText(_text);
            gradeText.colorGradientPreset = _color;
        }

        public void SetDateText(string _text)
        {
            dateText.SetText(_text);
        }

        public void ResetLevelSliderValue()
        {
            levelSlider.value = levelSlider.minValue;
        }

        public void SetLevel(string _level)
        {
            ReferenceLevelManager();
            levelSlider.value = float.Parse(_level);
            levelManager.SetLevelSliderGradientColor(levelSlider);
            ResetLevelSliderValue();
            levelText.SetText(_level);
        }

        public void PlayLerpAnimation(float _scoreToLerpTo, float _accuracyToLerpTo, float _comboToLerpTo, float _levelToLerpTo)
        {
            if (playLerpAnimation != null)
            {
                StopCoroutine(playLerpAnimation);
            }

            playLerpAnimation = PlayLerpAnimationCoroutine(_scoreToLerpTo, _accuracyToLerpTo, _comboToLerpTo, _levelToLerpTo);
            StartCoroutine(playLerpAnimation);
        }

        public void SetChallengeButtonPanelVisual(string _clearPoints, string _hiddenPoints, string _minePoints,
            string _lowApproachRatePoints, string _highApproachRatePoints, string _fullComboPoints, string _maxPercentagePoints)
        {
            challengeButtonPanel.SetAllButtonVisual(_clearPoints, _hiddenPoints, _minePoints, _lowApproachRatePoints,
                _highApproachRatePoints, _fullComboPoints, _maxPercentagePoints);
        }

        public void PlayFlashAnimationForAchievedButtons()
        {
            challengeButtonPanel.PlayFlashAnimationForAchievedButtons();
        }
        #endregion

        #region Private Methods
        private void ReferenceLevelManager()
        {
            if (levelManager is null == true)
            {
                levelManager = FindObjectOfType<LevelManager>();
            }
        }

        private IEnumerator PlayLerpAnimationCoroutine(float _scoreToLerpTo, float _accuracyToLerpTo, float _comboToLerpTo,
            float _levelToLerpTo)
        {
            float lerpTimer = 0f;
            float lerpDuration = 1f;
            float scoreValue = 0f;
            float previousFrameScoreValue = 0f;
            float accuracyValue = 0f;
            float previousFrameAccuracyValue = 0f;
            float comboValue = 0f;
            float previousFrameComboValue = 0f;
            float levelValue = 0f;
            float previousFrameLevelValue = 0f;
            string stringFormatF0 = "F0";
            string stringFormatF2 = "F2";

            while (lerpTimer < lerpDuration)
            {
                lerpTimer += (Time.deltaTime / lerpDuration);

                UpdateLerpText(ref scoreValue, ref previousFrameScoreValue, _scoreToLerpTo, lerpTimer, stringFormatF0, scoreText);

                UpdateComboAndAccuracyLerpText(ref comboValue, ref previousFrameComboValue, _comboToLerpTo,
                    ref accuracyValue, ref previousFrameAccuracyValue, _accuracyToLerpTo,
                    lerpTimer, stringFormatF0, stringFormatF2, comboAndAccuracyText);

                UpdateLerpText(ref levelValue, ref previousFrameLevelValue, _levelToLerpTo, lerpTimer, stringFormatF0, levelText);

                UpdateLerpSlider(ref levelValue, ref previousFrameLevelValue, _levelToLerpTo, lerpTimer, levelSlider);

                yield return null;
            }

            yield return null;
        }

        private static void UpdateLerpSlider(ref float _value, ref float _previousFrameValue, float _valueToLerpTo, 
            float _lerpTimer, Slider _slider)
        {
            _value = Mathf.Lerp(0f, _valueToLerpTo, _lerpTimer);
            _slider.value = _value;
        }

        private static void UpdateLerpText(ref float _value, ref float _previousFrameValue, float _valueToLerpTo, float _lerpTimer,
            string _stringFormat, TextMeshProUGUI _text)
        {
            _value = Mathf.Lerp(0f, _valueToLerpTo, _lerpTimer);

            if (_value != _previousFrameValue)
            {
                _text.SetText(_value.ToString(_stringFormat));
            }

            _previousFrameValue = _value;
        }

        private static void UpdateComboAndAccuracyLerpText(ref float _comboValue, ref float _previousFrameComboValue, 
            float _comboValueToLerpTo, ref float _accuracyValue, ref float _previousFrameAccuracyValue,
            float _accuracyValueToLerpTo, float _lerpTimer, string _comboStringFormat, string accuracyStringFormat, 
            TextMeshProUGUI _text)
        {
            _comboValue = Mathf.Lerp(0f, _comboValueToLerpTo, _lerpTimer);
            _accuracyValue = Mathf.Lerp(0f, _accuracyValueToLerpTo, _lerpTimer);

            if (_comboValue != _previousFrameComboValue || _accuracyValue != _previousFrameAccuracyValue)
            {
                _text.SetText($"{_comboValue.ToString(_comboStringFormat)}x {_accuracyValue.ToString(accuracyStringFormat)}%");
            }

            _previousFrameComboValue = _comboValue;
            _previousFrameAccuracyValue = _accuracyValue;
        }
        #endregion
    }
}