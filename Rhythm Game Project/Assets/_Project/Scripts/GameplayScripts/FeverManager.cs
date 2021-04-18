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

        [SerializeField] private Gradient gradient = default;

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

        private IEnumerator cancelActivatedTweenCoroutine;
        private IEnumerator trackActivatedTimeCoroutine;

        private FlashManager flashManager;
        private MultiplierManager multiplierManager;
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
            flashManager = MonoBehaviour.FindObjectOfType<FlashManager>();
            multiplierManager = MonoBehaviour.FindObjectOfType<MultiplierManager>();
            ReferenceTransforms();
            CalculateMeasureDuration();
            CalculateMaxPhraseDuration();
        }

        private void ReferenceTransforms()
        {
            durationTextTransform = durationText.transform;
            durationEffectTextTransform = durationEffectText.transform;
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
            HideMaxText();
            ShowRequirementText();
            CancelActivatedTween();
            UpdateDurationText();
            durationEffectText.gameObject.SetActive(true);
            PlayDurationTextAnimation();
            multiplierManager.DeactivateFeverMultiplier();
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
            LeanTween.scale(phraseRequirementTextArray[currentPhrase].gameObject, ScaleTo, 1f).setEasePunch();
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
                    PlayPhraseAchievedTween();
                }
            }
        }

        private void CheckIfMax()
        {
            if (currentPhrase == (PhraseRequirementValueArray.Length - 1))
            {
                ShowMaxText();
                HideRequirementText();
            }
        }

        private void ShowMaxText()
        {
            phraseRequirementTextArray[phraseRequirementTextArray.Length - 1].gameObject.SetActive(true);
        }

        private void HideMaxText()
        {
            phraseRequirementTextArray[phraseRequirementTextArray.Length - 1].gameObject.SetActive(false);
        }

        private void HideRequirementText()
        {
            for (byte i = 0; i < (phraseRequirementTextArray.Length - 1); i++)
            {
                phraseRequirementTextArray[i].gameObject.SetActive(false);
            }
        }

        private void ShowRequirementText()
        {
            for (byte i = 0; i < (phraseRequirementTextArray.Length - 1); i++)
            {
                phraseRequirementTextArray[i].gameObject.SetActive(true);
            }
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
            durationTextTransform.localScale = Vector3.one;

            LeanTween.cancel(durationEffectTextTransform.gameObject);
            durationEffectTextTransform.localScale = Vector3.one;

            LeanTween.scale(durationTextTransform.gameObject, ScaleTo, 0.5f).setEasePunch();
            LeanTween.scale(durationEffectTextTransform.gameObject, EffectScaleTo, 0.5f).setEasePunch();
        }
    }
    #endregion
}
