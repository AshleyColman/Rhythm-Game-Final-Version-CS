namespace Gameplay
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using System.Text;
    using VisualEffects;

    public sealed class FeverManager : MonoBehaviour
    {
        #region Constants
        private const byte MinimumActivationValue = 25;
        private readonly byte[] PhraseRequirementValueArray = new byte[4] { 25, 50, 75, 100 };

        private const string FeverBackgroundAnimationString = "FeverBackground_Activated";
        private const string MaxDurationString = "MAX";

        private readonly Vector3 ScaleTo = new Vector3(1.25f, 1.25f, 1f);
        private readonly Vector3 EffectScaleTo = new Vector3(1.75f, 1.75f, 1f);
        #endregion

        #region Private Fields
        [SerializeField] private AudioReverbFilter audioReverbFilter = default;

        [SerializeField] private Animator feverBackgroundAnimator = default;

        [SerializeField] private Slider feverSlider = default;

        [SerializeField] private Image sliderFillImage = default;

        [SerializeField] private CanvasGroup flashFillImageCanvasGroup = default;
        [SerializeField] private CanvasGroup feverBackgroundCanvasGroup = default;

        [SerializeField] private TextMeshProUGUI durationText = default;
        [SerializeField] private TextMeshProUGUI durationEffectText = default;
        [SerializeField] private TextMeshProUGUI[] phraseRequirementTextArray = default;
        [SerializeField] private TextMeshProUGUI[] phraseEffectRequirementTextArray = default;

        [SerializeField] private Gradient gradient = default;

        private Transform[] phraseRequirementTextCachedTransformArray;
        private Transform[] phraseEffectRequirementTextCachedTransformArray;

        private uint hit = 0;
        private int currentPhrase = -1;
        private int nextPhrase = 0;

        private double activatedTimer = 0;
        private double tickDuration = 0.34;
        private double measureDuration = 0;
        private double phraseDuration = 0;
        private double maxPhraseDuration = 0;

        private float lerpToValue = 0f;
        private float lerpFromValue = 0f;
        private float sliderLerpValue = 0f;

        private StringBuilder phraseDurationTextStringBuilder = new StringBuilder();

        private bool activated = false;
        private bool canActivate = false;

        private Transform durationTextTransform;
        private Transform durationEffectTextTransform;

        private IEnumerator hideMultiplierTextCoroutine;
        private IEnumerator cancelActivatedTweenCoroutine;
        private IEnumerator trackActivatedTimeCoroutine;

        private FlashManager flashManager;
        private MultiplierManager multiplierManager;
        private ScoreManager scoreManager;
        #endregion

        #region Public Methods
        public void IncrementFeverSlider()
        {
            if (activated == false)
            {
                if (feverSlider.value < feverSlider.maxValue)
                {
                    hit++;
                    feverSlider.value++;
                    UpdatePhraseDuration();
                    PlayDurationTextAnimation();
                    CheckIfCanActivate();
                    CheckIfNextPhrase();
                    UpdateSliderColor();
                }
            }
        }

        public void TryToActivateFever()
        {
            if (activated == false && canActivate == true)
            {
                ActivateFever();
            }
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            flashManager = FindObjectOfType<FlashManager>();
            multiplierManager = FindObjectOfType<MultiplierManager>();
            scoreManager = FindObjectOfType<ScoreManager>();
            ReferenceTransforms();
            CalculateMeasureDuration();
            CalculateMaxPhraseDuration();
        }

        private void ReferenceTransforms()
        {
            ReferenceDurationTextTransforms();
            ReferencePhraseRequirementCachedTransforms();
            ReferencePhraseEffectRequirementCachedTransforms();
        }

        private void ReferenceDurationTextTransforms()
        {
            durationTextTransform = durationText.transform;
            durationEffectTextTransform = durationEffectText.transform;
        }

        private void ReferencePhraseRequirementCachedTransforms()
        {
            phraseRequirementTextCachedTransformArray = new Transform[phraseRequirementTextArray.Length];
            for (byte i = 0; i < phraseRequirementTextArray.Length; i++)
            {
                phraseRequirementTextCachedTransformArray[i] = phraseRequirementTextArray[i].transform;
            }
        }

        private void ReferencePhraseEffectRequirementCachedTransforms()
        {
            phraseEffectRequirementTextCachedTransformArray = new Transform[phraseEffectRequirementTextArray.Length];
            for (byte i = 0; i < phraseEffectRequirementTextArray.Length; i++)
            {
                phraseEffectRequirementTextCachedTransformArray[i] = phraseEffectRequirementTextArray[i].transform;
            }
        }

        private void UpdatePhraseDuration()
        {
            phraseDuration = (maxPhraseDuration * feverSlider.value) / 100;
            UpdateDurationText();
        }

        private void CalculateMaxPhraseDuration()
        {
            maxPhraseDuration = (measureDuration * 8);
        }

        private void TrackActivatedTime()
        {
            if (trackActivatedTimeCoroutine != null)
            {
                StopCoroutine(trackActivatedTimeCoroutine);
            }

            trackActivatedTimeCoroutine = TrackActivatedTimeCoroutine();
            StartCoroutine(trackActivatedTimeCoroutine);
        }

        private IEnumerator TrackActivatedTimeCoroutine()
        {
            activatedTimer = phraseDuration;

            while (activated == true)
            {
                activatedTimer -= Time.deltaTime;
                UpdateActivatedDurationText();

                if (activatedTimer <= 0)
                {
                    DeactivateFever();
                }

                LerpSliderDown();
                UpdateSliderColor();

                yield return null;
            }
            yield return null;
        }

        private void CheckIfCanActivate()
        {
            if (canActivate == false)
            {
                if (feverSlider.value >= MinimumActivationValue)
                {
                    canActivate = true;
                }
            }
        }

        private void LerpSliderDown()
        {
            sliderLerpValue += (float)(Time.deltaTime / phraseDuration);
            feverSlider.value = Mathf.Lerp(lerpFromValue, lerpToValue, sliderLerpValue);
        }

        private void ActivateFever()
        {
            activated = true;
            canActivate = false;
            audioReverbFilter.enabled = true;
            sliderLerpValue = 0f;
            lerpFromValue = feverSlider.value;
            TrackActivatedTime();
            PlayFeverBackgroundAnimation();
            flashManager.PlayFlashTween(5f);
            PlayActivatedTween();
            multiplierManager.ActivateFeverMultiplier();
            HideMultiplierText(phraseRequirementTextArray.Length);
            ShowDurationText();
            durationEffectText.gameObject.SetActive(false);
        }

        private void DeactivateFever()
        {
            activated = false;
            canActivate = false;
            audioReverbFilter.enabled = false;
            activatedTimer = 0.0;
            phraseDuration = 0f;
            feverSlider.value = 0f;
            hit = 0;
            currentPhrase = -1;
            nextPhrase = 0;
            StopFeverBackgroundAnimation();
            CancelActivatedTween();
            UpdateDurationText();
            ShowTextAfterDeactivationAnimation();
            PlayDurationTextAnimation();
            multiplierManager.ResetMultiplier();
            multiplierManager.DeactivateFeverMultiplier();
        }

        private void ShowTextAfterDeactivationAnimation()
        {
            for (byte i = 0; i < (phraseRequirementTextArray.Length - 1); i++)
            {
                LeanTween.cancel(phraseRequirementTextArray[i].gameObject);
                LeanTween.cancel(phraseEffectRequirementTextArray[i].gameObject);

                phraseRequirementTextCachedTransformArray[i].localScale = Vector3.one;
                phraseEffectRequirementTextCachedTransformArray[i].localScale = Vector3.one;

                phraseRequirementTextArray[i].gameObject.SetActive(true);
                phraseEffectRequirementTextArray[i].gameObject.SetActive(true);

                LeanTween.scale(phraseRequirementTextArray[i].gameObject, ScaleTo, 1f).setEasePunch();
                LeanTween.scale(phraseEffectRequirementTextArray[i].gameObject, EffectScaleTo, 1f).setEasePunch();
            }

            durationEffectText.gameObject.SetActive(true);
            PlayDurationTextAnimation();
        }

        private void HideMultiplierText(int _toIndex)
        {
            if (hideMultiplierTextCoroutine != null)
            {
                StopCoroutine(hideMultiplierTextCoroutine);
            }

            hideMultiplierTextCoroutine = HideMultiplierTextCoroutine(_toIndex);
            StartCoroutine(hideMultiplierTextCoroutine);
        }

        private IEnumerator HideMultiplierTextCoroutine(int _toIndex)
        {
            if (_toIndex <= phraseRequirementTextArray.Length)
            {
                for (int i = 0; i < _toIndex; i++)
                {
                    if (phraseRequirementTextArray[i].gameObject.activeSelf == true)
                    {
                        LeanTween.cancel(phraseRequirementTextArray[i].gameObject);
                        LeanTween.cancel(phraseEffectRequirementTextArray[i].gameObject);

                        phraseRequirementTextCachedTransformArray[i].localScale = Vector3.one;
                        phraseEffectRequirementTextCachedTransformArray[i].localScale = Vector3.one;

                        LeanTween.scale(phraseRequirementTextArray[i].gameObject, Vector3.zero, 1f).setEaseOutExpo();
                        LeanTween.scale(phraseEffectRequirementTextArray[i].gameObject, Vector3.zero, 1f).setEaseOutExpo();
                    }
                }

                yield return new WaitForSeconds(1f);

                for (int i = 0; i < (_toIndex); i++)
                {
                    if (phraseRequirementTextArray[i].gameObject.activeSelf == true)
                    {
                        phraseRequirementTextArray[i].gameObject.SetActive(false);
                        phraseEffectRequirementTextArray[i].gameObject.SetActive(false);
                    }
                }
            }

            yield return null;
        }

        private void CalculateMeasureDuration()
        {
            measureDuration = (tickDuration * 4);
        }

        private void PlayFeverBackgroundAnimation()
        {
            feverBackgroundAnimator.speed = 1;
            feverBackgroundAnimator.Play(FeverBackgroundAnimationString);
        }

        private void StopFeverBackgroundAnimation()
        {
            feverBackgroundAnimator.speed = 0;
        }

        private void UpdateActivatedDurationText()
        {
            phraseDurationTextStringBuilder.Append(activatedTimer.ToString("F2"));
            durationText.SetText(phraseDurationTextStringBuilder.ToString());
            phraseDurationTextStringBuilder.Clear();
        }

        private void UpdateDurationText()
        {
            phraseDurationTextStringBuilder.Append(phraseDuration.ToString("F2"));
            durationText.SetText(phraseDurationTextStringBuilder.ToString());
            durationEffectText.SetText(phraseDurationTextStringBuilder.ToString());
            phraseDurationTextStringBuilder.Clear();
        }

        private void UpdateSliderColor()
        {
            sliderFillImage.color = gradient.Evaluate(feverSlider.normalizedValue);
        }

        private void PlayFullFlashFillTween()
        {
            LeanTween.cancel(flashFillImageCanvasGroup.gameObject);
            flashFillImageCanvasGroup.alpha = 0f;
            LeanTween.alphaCanvas(flashFillImageCanvasGroup, 1f, 0.25f).setLoopPingPong(1);
        }

        private void PlayActivatedTween()
        {
            LeanTween.cancel(flashFillImageCanvasGroup.gameObject);
            flashFillImageCanvasGroup.alpha = 0f;
            LeanTween.alphaCanvas(flashFillImageCanvasGroup, 0.5f, 1f).setLoopPingPong(-1);

            FadeFeverBackgroundCanvasGroupIn();
        }

        private void PlayPhraseAchievedTween()
        {
            LeanTween.cancel(phraseRequirementTextArray[currentPhrase].gameObject);
            LeanTween.cancel(phraseEffectRequirementTextArray[currentPhrase].gameObject);

            phraseEffectRequirementTextCachedTransformArray[currentPhrase].localScale = Vector3.one;
            phraseRequirementTextCachedTransformArray[currentPhrase].localScale = Vector3.one;

            LeanTween.scale(phraseRequirementTextArray[currentPhrase].gameObject, ScaleTo, 1f).setEasePunch();
            LeanTween.scale(phraseEffectRequirementTextArray[currentPhrase].gameObject, EffectScaleTo, 1f).setEasePunch();
        }

        private void CancelActivatedTween()
        {
            if (cancelActivatedTweenCoroutine != null)
            {
                StopCoroutine(cancelActivatedTweenCoroutine);
            }

            cancelActivatedTweenCoroutine = CancelActivatedTweenCoroutine();
            StartCoroutine(cancelActivatedTweenCoroutine);
        }

        private IEnumerator CancelActivatedTweenCoroutine()
        {
            FadeFeverBackgroundCanvasGroupOut();

            yield return new WaitForSeconds(0.5f);

            LeanTween.cancel(flashFillImageCanvasGroup.gameObject);

            yield return null;
        }

        private void CheckIfNextPhrase()
        {
            if (nextPhrase < PhraseRequirementValueArray.Length)
            {
                if (feverSlider.value >= PhraseRequirementValueArray[nextPhrase])
                {
                    currentPhrase = nextPhrase;
                    nextPhrase++;
                    CheckIfMax();
                    PlayFullFlashFillTween();
                    flashManager.PlayFlashTween(5f);
                    PlayPhraseAchievedTween();
                    multiplierManager.IncrementMultiplier();
                }
            }
        }

        private void CheckIfMax()
        {
            if (currentPhrase == (PhraseRequirementValueArray.Length - 1))
            {
                HideMultiplierText(phraseRequirementTextArray.Length - 1);
                HideDurationTexts();
                UpdateMaxText();
                ShowMaxText();
            }
        }

        private void UpdateMaxText()
        {
            phraseRequirementTextArray[phraseRequirementTextArray.Length - 1].SetText($"x5 {durationText.text}");
            phraseEffectRequirementTextArray[phraseRequirementTextArray.Length - 1].SetText($"x5 {durationText.text}");
        }

        private void ShowMaxText()
        {
            phraseRequirementTextArray[phraseRequirementTextArray.Length - 1].gameObject.SetActive(true);
            phraseEffectRequirementTextArray[phraseEffectRequirementTextArray.Length - 1].gameObject.SetActive(true);
        }

        private void HideDurationTexts()
        {
            durationText.gameObject.SetActive(false);
            durationEffectText.gameObject.SetActive(false);
        }

        private void ShowDurationText()
        {
            durationText.gameObject.SetActive(true);
        }

        private void FadeFeverBackgroundCanvasGroupIn()
        {
            LeanTween.cancel(feverBackgroundCanvasGroup.gameObject);
            feverBackgroundCanvasGroup.alpha = 0f;

            LeanTween.alphaCanvas(feverBackgroundCanvasGroup, 0.1f, 0.5f).setEaseOutExpo();        }

        private void FadeFeverBackgroundCanvasGroupOut()
        {
            LeanTween.cancel(feverBackgroundCanvasGroup.gameObject);
            feverBackgroundCanvasGroup.alpha = 0.1f;

            LeanTween.alphaCanvas(feverBackgroundCanvasGroup, 0f, 0.5f).setEaseOutExpo();
        }

        private void PlayDurationTextAnimation()
        {
            LeanTween.cancel(durationTextTransform.gameObject);
            LeanTween.cancel(durationEffectTextTransform.gameObject);

            durationTextTransform.localScale = Vector3.one;
            durationEffectTextTransform.localScale = Vector3.one;

            LeanTween.scale(durationTextTransform.gameObject, ScaleTo, 0.5f).setEasePunch();
            LeanTween.scale(durationEffectTextTransform.gameObject, EffectScaleTo, 0.5f).setEasePunch();
        }
    }
    #endregion
}
