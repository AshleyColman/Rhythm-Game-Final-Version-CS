namespace ResultScreen
{
    using UnityEngine;
    using TMPro;
    using System.Collections;
    using Gameplay;
    using System.Text;
    using VisualEffects;
    using Grade;

    public sealed class ResultScreenManager : MonoBehaviour
    {
        #region Constants
        // Index's for accessing the statistic text arrays.
        private const byte TextNameIndex = 0;
        private const byte TextValueIndex = 1;
        private const byte TextNameEffectIndex = 2;
        private const byte TextValueEffectIndex = 3;

        private readonly Vector3 ScaleTo = new Vector3(1.25f, 1.25f, 1f);
        private readonly Vector3 EffectScaleTo = new Vector3(1.75f, 1.75f, 1f);
        #endregion

        #region Private Fields
        [SerializeField] private GameObject resultScreen = default;
        [SerializeField] private GameObject gradePanel = default;

        [SerializeField] private TextMeshProUGUI[] scoreTextArr = default;
        [SerializeField] private TextMeshProUGUI[] comboTextArr = default;
        [SerializeField] private TextMeshProUGUI[] perfectTextArr = default;
        [SerializeField] private TextMeshProUGUI[] greatTextArr = default;
        [SerializeField] private TextMeshProUGUI[] okayTextArr = default;
        [SerializeField] private TextMeshProUGUI[] missTextArr = default;
        [SerializeField] private TextMeshProUGUI[] feverTextArr = default;
        [SerializeField] private TextMeshProUGUI[] bonusTextArr = default;
        [SerializeField] private TextMeshProUGUI songNameText = default;
        [SerializeField] private TextMeshProUGUI artistNameText = default;
        [SerializeField] private TextMeshProUGUI creatorNameText = default;
        [SerializeField] private TextMeshProUGUI gradeText = default;
        [SerializeField] private TextMeshProUGUI gradeEffectText = default;
        [SerializeField] private TextMeshProUGUI accuracyText = default;
        [SerializeField] private TextMeshProUGUI accuracyEffectText = default;

        private Transform[] comboTransformArr = default;
        private Transform[] perfectTransformArr = default;
        private Transform[] greatTransformArr = default;
        private Transform[] okayTransformArr = default;
        private Transform[] missTransformArr = default;
        private Transform[] feverTransformArr = default;
        private Transform[] bonusTransformArr = default;
        private Transform gradeTextTransform = default;
        private Transform gradeEffectTextTransform = default;
        private Transform accuracyTextTransform = default;
        private Transform accuracyEffectTextTransform = default;

        private IEnumerator playStatisticTextAnimationCoroutine;
        private IEnumerator playResultScreenAnimationCoroutine;
        private IEnumerator lerpValueUpdateTextCoroutine;
        private IEnumerator lerpValueUpdateScoreTextCoroutine;

        private ScoreManager scoreManager;
        private ComboManager comboManager;
        private JudgementManager judgementManager;
        private FeverManager feverManager;
        private BonusManager bonusManager;
        private AccuracyManager accuracyManager;
        private GradeSlider gradeSlider;
        private HitobjectSpawnManager hitobjectSpawnManager;
        private FlashManager flashManager;
        #endregion

        #region Public Methods
        public void DisplayResultsScreen()
        {
            resultScreen.gameObject.SetActive(true);
            // Play display animation
            SetGradeText();
            SetAccuracyText();
            // Set leaderboard position text
            // Set song name text
            // Set artist name text
            // Set creator name text
            // Set creator image
            PlayResultScreenAnimation();
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            scoreManager = FindObjectOfType<ScoreManager>();
            comboManager = FindObjectOfType<ComboManager>();
            judgementManager = FindObjectOfType<JudgementManager>();
            feverManager = FindObjectOfType<FeverManager>();
            bonusManager = FindObjectOfType<BonusManager>();
            hitobjectSpawnManager = FindObjectOfType<HitobjectSpawnManager>();
            flashManager = FindObjectOfType<FlashManager>();
            accuracyManager = FindObjectOfType<AccuracyManager>();
            gradeSlider = FindObjectOfType<GradeSlider>();

            ReferenceTransforms();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                DisplayResultsScreen();
            }
        }

        private void PlayResultScreenAnimation()
        {
            if (playResultScreenAnimationCoroutine != null)
            {
                StopCoroutine(playResultScreenAnimationCoroutine);
            }

            playResultScreenAnimationCoroutine = PlayResultScreenAnimationCoroutine();
            StartCoroutine(playResultScreenAnimationCoroutine);
        }

        private IEnumerator PlayResultScreenAnimationCoroutine()
        {
            PlayStatisticTextAnimation();
            yield return null;
        }

        private void ReferenceTransforms()
        {
            InitializeTransformArray(comboTextArr.Length, ref comboTransformArr);
            InitializeTransformArray(perfectTextArr.Length, ref perfectTransformArr);
            InitializeTransformArray(greatTextArr.Length, ref greatTransformArr);
            InitializeTransformArray(okayTextArr.Length, ref okayTransformArr);
            InitializeTransformArray(missTextArr.Length, ref missTransformArr);
            InitializeTransformArray(feverTextArr.Length, ref feverTransformArr);
            InitializeTransformArray(bonusTextArr.Length, ref bonusTransformArr);

            ReferenceTransformArray(comboTransformArr, comboTextArr);
            ReferenceTransformArray(perfectTransformArr, perfectTextArr);
            ReferenceTransformArray(greatTransformArr, greatTextArr);
            ReferenceTransformArray(okayTransformArr, okayTextArr);
            ReferenceTransformArray(missTransformArr, missTextArr);
            ReferenceTransformArray(feverTransformArr, feverTextArr);
            ReferenceTransformArray(bonusTransformArr, bonusTextArr);

            gradeTextTransform = gradeText.transform;
            gradeEffectTextTransform = gradeEffectText.transform;
            accuracyTextTransform = accuracyText.transform;
            accuracyEffectTextTransform = accuracyEffectText.transform;
        }

        private void InitializeTransformArray(int _arrayLength, ref Transform[] _transformArr)
        {
            _transformArr = new Transform[_arrayLength];
        }

        private void ReferenceTransformArray(Transform[] _transformArr, TextMeshProUGUI[]  _referenceArr)
        {
            for (int i = 0; i < _transformArr.Length; i++)
            {
                _transformArr[i] = _referenceArr[i].transform;
            }
        }

        private void SetGradeText()
        {
            gradeText.SetText(gradeSlider.GradeText.text);
            gradeEffectText.SetText(gradeText.text);
            gradeText.colorGradientPreset = gradeSlider.GradeText.colorGradientPreset;
            gradeEffectText.colorGradientPreset = gradeText.colorGradientPreset;
        }

        private void SetAccuracyText()
        {
            accuracyText.SetText(accuracyManager.AccuracyText.text);
            accuracyEffectText.SetText(accuracyText.text);
        }

        private void PlayStatisticTextAnimation()
        {
            if (playStatisticTextAnimationCoroutine != null)
            {
                StopCoroutine(playStatisticTextAnimationCoroutine);
            }

            playStatisticTextAnimationCoroutine = PlayStatisticTextAnimationCoroutine();
            StartCoroutine(playStatisticTextAnimationCoroutine);
        }

        private IEnumerator PlayStatisticTextAnimationCoroutine()
        {
            float lerpDuration = 0.05f;
            float scoreLerpDuration = (lerpDuration * 8);
            string stringFormat = "F0";

            WaitForSeconds waitForSeconds = new WaitForSeconds(lerpDuration);

            CancelLeanTweenInArray(comboTextArr);
            CancelLeanTweenInArray(perfectTextArr);
            CancelLeanTweenInArray(greatTextArr);
            CancelLeanTweenInArray(okayTextArr);
            CancelLeanTweenInArray(missTextArr);
            CancelLeanTweenInArray(feverTextArr);
            CancelLeanTweenInArray(bonusTextArr);

            ResetTextSizeInArray(comboTransformArr);
            ResetTextSizeInArray(perfectTransformArr);
            ResetTextSizeInArray(greatTransformArr);
            ResetTextSizeInArray(okayTransformArr);
            ResetTextSizeInArray(missTransformArr);
            ResetTextSizeInArray(feverTransformArr);
            ResetTextSizeInArray(bonusTransformArr);

            LerpValueUpdateScoreText(scoreLerpDuration, scoreManager.TotalScore, stringFormat, scoreTextArr[0], scoreTextArr[1]);

            LerpValueUpdateText(lerpDuration, comboManager.HighestCombo, stringFormat, $"/{hitobjectSpawnManager.TotalHitobjects}",
                comboTextArr[TextValueIndex], comboTextArr[TextValueEffectIndex]);
            yield return waitForSeconds;
            PlayTextPunchAnimation(comboTextArr);

            LerpValueUpdateText(lerpDuration, judgementManager.Perfect, stringFormat, perfectTextArr[TextValueIndex],
                perfectTextArr[TextValueEffectIndex]);
            yield return waitForSeconds;
            PlayTextPunchAnimation(perfectTextArr);

            LerpValueUpdateText(lerpDuration, judgementManager.Perfect, stringFormat, greatTextArr[TextValueIndex],
                greatTextArr[TextValueEffectIndex]);
            yield return waitForSeconds;
            PlayTextPunchAnimation(greatTextArr);

            LerpValueUpdateText(lerpDuration, judgementManager.Perfect, stringFormat, okayTextArr[TextValueIndex],
                okayTextArr[TextValueEffectIndex]);
            yield return waitForSeconds;
            PlayTextPunchAnimation(okayTextArr);

            LerpValueUpdateText(lerpDuration, judgementManager.Perfect, stringFormat, missTextArr[TextValueIndex],
                missTextArr[TextValueEffectIndex]);
            yield return waitForSeconds;
            PlayTextPunchAnimation(missTextArr);

            LerpValueUpdateText(lerpDuration, judgementManager.Perfect, stringFormat, feverTextArr[TextValueIndex],
                feverTextArr[TextValueEffectIndex]);
            yield return waitForSeconds;
            PlayTextPunchAnimation(feverTextArr);

            LerpValueUpdateText(lerpDuration, judgementManager.Perfect, stringFormat, bonusTextArr[TextValueIndex],
                bonusTextArr[TextValueEffectIndex]);
            yield return waitForSeconds;
            PlayTextPunchAnimation(bonusTextArr);

            yield return waitForSeconds;

            PlayTextPunchAnimation(scoreTextArr[0], scoreTextArr[1]);
            flashManager.PlayFlashTween(5f);
            PlayGradeAnimation();

            yield return null;
        }

        private void PlayGradeAnimation()
        {
            gradePanel.gameObject.SetActive(true);

            gradeTextTransform.localScale = Vector3.zero;
            gradeEffectTextTransform.localScale = Vector3.zero;
            accuracyTextTransform.localScale = Vector3.zero;
            accuracyEffectTextTransform.localScale = Vector3.zero;

            LeanTween.cancel(gradeTextTransform.gameObject);
            LeanTween.cancel(gradeEffectTextTransform.gameObject);
            LeanTween.cancel(accuracyTextTransform.gameObject);
            LeanTween.cancel(accuracyEffectTextTransform.gameObject);

            LeanTween.scale(gradeTextTransform.gameObject, Vector3.one, 1f).setEaseOutExpo();
            LeanTween.scale(gradeEffectTextTransform.gameObject, Vector3.one, 1f).setEaseOutExpo();
            LeanTween.scale(accuracyTextTransform.gameObject, Vector3.one, 1f).setEaseOutExpo();
            LeanTween.scale(accuracyEffectTextTransform.gameObject, Vector3.one, 1f).setEaseOutExpo();
        }

        private void PlayTextPunchAnimation(TextMeshProUGUI[] _array)
        {
            LeanTween.scale(_array[TextNameIndex].gameObject, ScaleTo, 1f).setEasePunch();
            LeanTween.scale(_array[TextValueIndex].gameObject, ScaleTo, 1f).setEasePunch();
            LeanTween.scale(_array[TextNameEffectIndex].gameObject, EffectScaleTo, 1f).setEasePunch();
            LeanTween.scale(_array[TextValueEffectIndex].gameObject, EffectScaleTo, 1f).setEasePunch();
        }

        private void PlayTextPunchAnimation(TextMeshProUGUI _text, TextMeshProUGUI _effectText)
        {
            LeanTween.scale(_text.gameObject, ScaleTo, 1f).setEasePunch();
            LeanTween.scale(_effectText.gameObject, EffectScaleTo, 1f).setEasePunch();
        }

        private void CancelLeanTweenInArray(TextMeshProUGUI[] _array)
        {
            for (int i = 0; i < _array.Length; i++)
            {
                LeanTween.cancel(_array[i].gameObject);
            }
        }

        private void ResetTextSizeInArray(Transform[] _array)
        {
            for (int i = 0; i < _array.Length; i++)
            {
                _array[i].localScale = Vector3.one;
            }
        }

        private void PlayEffectAnimation()
        {

        }

        private void LerpValueUpdateText(float _lerpDuration, float _valueToLerpTo, string _stringFormat, 
            TextMeshProUGUI _text)
        {
            if (lerpValueUpdateTextCoroutine != null)
            {
                StopCoroutine(lerpValueUpdateTextCoroutine);
            }

            lerpValueUpdateTextCoroutine = LerpValueUpdateTextCoroutine(_lerpDuration, _valueToLerpTo, _stringFormat,
                _text);
            StartCoroutine(lerpValueUpdateTextCoroutine);
        }

        private void LerpValueUpdateText(float _lerpDuration, float _valueToLerpTo, string _stringFormat,
            TextMeshProUGUI _text1, TextMeshProUGUI _text2)
        {
            if (lerpValueUpdateTextCoroutine != null)
            {
                StopCoroutine(lerpValueUpdateTextCoroutine);
            }

            lerpValueUpdateTextCoroutine = LerpValueUpdateTextCoroutine(_lerpDuration, _valueToLerpTo, _stringFormat,
                _text1, _text2);
            StartCoroutine(lerpValueUpdateTextCoroutine);
        }

        private void LerpValueUpdateText(float _lerpDuration, float _valueToLerpTo, string _stringFormat, string _endString,
            TextMeshProUGUI _text1, TextMeshProUGUI _text2)
        {
            if (lerpValueUpdateTextCoroutine != null)
            {
                StopCoroutine(lerpValueUpdateTextCoroutine);
            }

            lerpValueUpdateTextCoroutine = LerpValueUpdateTextCoroutine(_lerpDuration, _valueToLerpTo, _stringFormat, _endString,
                _text1, _text2);

            StartCoroutine(lerpValueUpdateTextCoroutine);
        }

        private IEnumerator LerpValueUpdateTextCoroutine(float _lerpDuration,
            float _valueToLerpTo, string _stringFormat, TextMeshProUGUI _text)
        {
            float lerpValueFrom = 0f;
            float lerpTimer = 0f;
            float newValue = 0f;
            float previousFrameValue = 0f;

            while (lerpTimer < _lerpDuration)
            {
                lerpTimer += (Time.deltaTime / _lerpDuration);

                newValue = Mathf.Lerp(lerpValueFrom, _valueToLerpTo, lerpTimer);

                if (newValue != previousFrameValue)
                {
                    _text.SetText($"{newValue.ToString(_stringFormat)}");
                }

                previousFrameValue = newValue;

                yield return null;
            }

            yield return null;
        }

        private IEnumerator LerpValueUpdateTextCoroutine(float _lerpDuration,
            float _valueToLerpTo, string _stringFormat, TextMeshProUGUI _text1, TextMeshProUGUI _text2)
        {
            float lerpValueFrom = 0f;
            float lerpTimer = 0f;
            float newValue = 0f;
            float previousFrameValue = 0f;

            while (lerpTimer < _lerpDuration)
            {
                lerpTimer += (Time.deltaTime / _lerpDuration);

                newValue = Mathf.Lerp(lerpValueFrom, _valueToLerpTo, lerpTimer);

                if (newValue != previousFrameValue)
                {
                    _text1.SetText($"{newValue.ToString(_stringFormat)}");
                    _text2.SetText(_text1.text);
                }

                previousFrameValue = newValue;

                yield return null;
            }

            yield return null;
        }

        private IEnumerator LerpValueUpdateTextCoroutine(float _lerpDuration, float _valueToLerpTo, string _stringFormat, 
            string _endString, TextMeshProUGUI _text1, TextMeshProUGUI _text2)
        {
            float lerpValueFrom = 0f;
            float lerpTimer = 0f;
            float newValue = 0f;
            float previousFrameValue = 0f;

            while (lerpTimer < _lerpDuration)
            {
                lerpTimer += (Time.deltaTime / _lerpDuration);

                newValue = Mathf.Lerp(lerpValueFrom, _valueToLerpTo, lerpTimer);

                if (newValue != previousFrameValue)
                {
                    _text1.SetText($"{newValue.ToString(_stringFormat)}{_endString}");
                    _text2.SetText(_text1.text);
                }

                previousFrameValue = newValue;

                yield return null;
            }

            yield return null;
        }

        private void LerpValueUpdateScoreText(float _lerpDuration, float _valueToLerpTo, string _stringFormat,
            TextMeshProUGUI _text1, TextMeshProUGUI _text2)
        {
            if (lerpValueUpdateScoreTextCoroutine != null)
            {
                StopCoroutine(lerpValueUpdateScoreTextCoroutine);
            }

            lerpValueUpdateScoreTextCoroutine = LerpValueUpdateScoreTextCoroutine(_lerpDuration, _valueToLerpTo, _stringFormat,
                _text1, _text2);
            StartCoroutine(lerpValueUpdateScoreTextCoroutine);
        }

        private IEnumerator LerpValueUpdateScoreTextCoroutine(float _lerpDuration,
            float _valueToLerpTo, string _stringFormat, TextMeshProUGUI _text1, TextMeshProUGUI _text2)
        {
            float lerpValueFrom = 0f;
            float lerpTimer = 0f;
            float newValue = 0f;
            float previousFrameValue = 0f;
            StringBuilder stringBuilder = new StringBuilder();

            while (lerpTimer < _lerpDuration)
            {
                lerpTimer += (Time.deltaTime / _lerpDuration);

                newValue = Mathf.Lerp(lerpValueFrom, _valueToLerpTo, lerpTimer);

                if (newValue != previousFrameValue)
                {
                    stringBuilder.Append(newValue);
                    stringBuilder = UtilityMethods.AddZerosToScoreString(stringBuilder);
                    _text1.SetText(stringBuilder.ToString());
                    _text2.SetText(_text1.text);
                }

                stringBuilder.Clear();

                previousFrameValue = newValue;

                yield return null;
            }

            yield return null;
        }
        #endregion
    }
}