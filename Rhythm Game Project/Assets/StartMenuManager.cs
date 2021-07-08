namespace Menu
{
    using System.Collections;
    using UnityEngine;
    using Audio;
    using TMPro;
    using SceneLoading;
    using Background;
    using UnityEngine.UI;

    public sealed class StartMenuManager : MonoBehaviour, IMenu
    {
        #region Constants
        private const float StartSongBeatsPerMinute = 191f;
        private const float TimeToPlayAudio = 1f;

        private const double StartSongOffsetMilliseconds = 1000;
        #endregion

        #region Private Fields
        [SerializeField] private GameObject startMenuScreen = default;

        [SerializeField] private Canvas startPanel = default;

        [SerializeField] private TextMeshProUGUI rhythmText = default;
        [SerializeField] private TextMeshProUGUI rhythmEffectText = default;
        [SerializeField] private TextMeshProUGUI fadeText = default;
        [SerializeField] private TextMeshProUGUI fadeEffectText = default;

        [SerializeField] private CanvasGroup fadeTextCanvasGroup = default;

        [SerializeField] private Image backgroundImage = default;

        private Transform rhythmTextTransform;
        private Transform rhythmEffectTextTransform;
        private Transform fadeTextTransform;
        private Transform fadeEffectTextTransform;
        private Transform backgroundImageTransform;

        private bool anyKeyHasBeenPressed = false;

        private IEnumerator transitionOutCoroutine;
        private IEnumerator prepareToStartAudioAndTimerCoroutine;
        private IEnumerator checkToStartGameCoroutine;
        private IEnumerator startGameCoroutine;
        private IEnumerator transitionInCoroutine;
        private IEnumerator setRhythmAndFadeTextWithTypingAnimationCoroutine;

        private MenuAudioManager menuAudioManager;
        private MenuTimeManager menuTimeManager;
        private Transition transition;
        private TextTyper textTyper;
        private AccountPanel accountPanel;
        private DescriptionPanel descriptionPanel;
        private ControlPanel controlPanel;
        private BackgroundManager backgroundManager;
        #endregion

        #region Public Methods
        public void TransitionIn()
        {
            if (transitionInCoroutine != null)
            {
                StopCoroutine(transitionInCoroutine);
            }

            transitionInCoroutine = TransitionInCoroutine();
            StartCoroutine(transitionInCoroutine);
        }

        public void TransitionOut()
        {
            if (transitionOutCoroutine != null)
            {
                StopCoroutine(transitionOutCoroutine);
            }

            transitionOutCoroutine = TransitionOutCoroutine();
            StartCoroutine(transitionOutCoroutine);
        }

        public void TransitionOutStartPanel()
        {
            startPanel.gameObject.SetActive(false);
        }

        public void OnTick()
        {

        }

        public void OnMeasure()
        {
            PlayRhythmTextTween();
        }

        public void SetRhythmAndFadeTextWithTypingAnimation(string _rhythmTextString, string _fadeTextString)
        {
            if (setRhythmAndFadeTextWithTypingAnimationCoroutine != null)
            {
                StopCoroutine(setRhythmAndFadeTextWithTypingAnimationCoroutine);
            }

            setRhythmAndFadeTextWithTypingAnimationCoroutine = SetRhythmAndFadeTextWithTypingAnimationCoroutine(_rhythmTextString,
                _fadeTextString);
            StartCoroutine(setRhythmAndFadeTextWithTypingAnimationCoroutine);
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            menuAudioManager = FindObjectOfType<MenuAudioManager>();
            menuTimeManager = FindObjectOfType<MenuTimeManager>();
            transition = FindObjectOfType<Transition>();
            textTyper = FindObjectOfType<TextTyper>();
            accountPanel = FindObjectOfType<AccountPanel>();
            descriptionPanel = FindObjectOfType<DescriptionPanel>();
            controlPanel = FindObjectOfType<ControlPanel>();
            backgroundManager = FindObjectOfType<BackgroundManager>();

            SetTransforms();
            menuTimeManager.SetTimingDetails(StartSongBeatsPerMinute, StartSongOffsetMilliseconds);
            menuTimeManager.UpdateTimingPosition();
            backgroundManager.SetNewImageReferences(new Image[] { backgroundImage });
        }

        private void PlayRhythmTextTween()
        {
            LeanTween.cancel(rhythmText.gameObject);
            LeanTween.cancel(rhythmEffectText.gameObject);
            rhythmTextTransform.localScale = Vector3.one;
            rhythmEffectTextTransform.localScale = Vector3.one;

            LeanTween.scale(rhythmText.gameObject, VectorConstants.Vector125, 1f).setEasePunch();
            LeanTween.scale(rhythmEffectText.gameObject, VectorConstants.Vector175, 1f).setEasePunch();
        }

        private IEnumerator TransitionInCoroutine()
        {
            rhythmText.SetText(string.Empty);
            rhythmEffectText.SetText(string.Empty);
            fadeText.SetText(string.Empty);
            fadeEffectText.SetText(string.Empty);

            startMenuScreen.gameObject.SetActive(true);
            startPanel.gameObject.SetActive(true);
            PrepareToStartAudioAndTimer();
            transition.PlayFadeInTween();
            PlayFadeTextCanvasGroupAnimation();

            yield return new WaitForSeconds(Transition.TransitionDuration);

            string rhythmTextString = "project dia";
            textTyper.TypeTextNoCancel(rhythmTextString, rhythmText, rhythmEffectText);
            yield return new WaitForSeconds(textTyper.GetTextTypeDuration(rhythmTextString));
            string fadeTextString = "press any key to start";
            textTyper.TypeTextNoCancel(fadeTextString, fadeText, fadeEffectText);
            yield return new WaitForSeconds(textTyper.GetTextTypeDuration(fadeTextString));

            CheckToStartGame();

            yield return null;
        }

        private IEnumerator TransitionOutCoroutine()
        {
            transition.PlayFadeOutTween();
            yield return new WaitForSeconds(Transition.TransitionDuration);
            startMenuScreen.gameObject.SetActive(false);
            yield return null;
        }

        private void SetTransforms()
        {
            rhythmTextTransform = rhythmText.transform;
            rhythmEffectTextTransform = rhythmText.transform;
            fadeTextTransform = fadeText.transform;
            fadeEffectTextTransform = fadeEffectText.transform;
            backgroundImageTransform = backgroundImage.transform;
        }

        private void PrepareToStartAudioAndTimer()
        {
            if (prepareToStartAudioAndTimerCoroutine != null)
            {
                StopCoroutine(prepareToStartAudioAndTimerCoroutine);
            }

            prepareToStartAudioAndTimerCoroutine = PrepareToStartAudioAndTimerCoroutine();
            StartCoroutine(prepareToStartAudioAndTimerCoroutine);
        }

        private IEnumerator PrepareToStartAudioAndTimerCoroutine()
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(TimeToPlayAudio);
            menuAudioManager.PlayScheduledSongAudio(TimeToPlayAudio);
            yield return waitForSeconds;
            menuTimeManager.StartTimer();
            yield return null;
        }

        private void PlayFadeTextCanvasGroupAnimation()
        {
            LeanTween.alphaCanvas(fadeTextCanvasGroup, 0f, 1f).setLoopPingPong(-1);
        }

        private void CancelFadeTextCanvasGroupAnimation()
        {
            LeanTween.cancel(fadeTextCanvasGroup.gameObject);
        }

        private void PlayGameStartedAnimation()
        {
            fadeTextCanvasGroup.alpha = 1f;
            LeanTween.alphaCanvas(fadeTextCanvasGroup, 0f, 0.25f).setLoopPingPong(5);

            LeanTween.scale(fadeText.gameObject, VectorConstants.Vector125, 1f).setEasePunch();
            LeanTween.scale(fadeEffectText.gameObject, VectorConstants.Vector175, 1f).setEasePunch();
        }

        private void CheckToStartGame()
        {
            if (checkToStartGameCoroutine != null)
            {
                StopCoroutine(checkToStartGameCoroutine);
            }

            checkToStartGameCoroutine = CheckToStartGameCoroutine();
            StartCoroutine(checkToStartGameCoroutine);
        }

        private IEnumerator CheckToStartGameCoroutine()
        {
            while (anyKeyHasBeenPressed == false)
            {
                if (Input.anyKeyDown)
                {
                    StartGame();
                    break;
                }
                yield return null;
            }

            yield return null;
        }

        private void StartGame()
        {
            if (startGameCoroutine != null)
            {
                StopCoroutine(startGameCoroutine);
            }

            startGameCoroutine = StartGameCoroutine();
            StartCoroutine(startGameCoroutine);
        }

        private IEnumerator StartGameCoroutine()
        {
            anyKeyHasBeenPressed = true;
            CancelFadeTextCanvasGroupAnimation();
            PlayGameStartedAnimation();
       
            textTyper.TypeTextNoCancel("game started", rhythmText, rhythmEffectText);
            textTyper.TypeTextNoCancel("welcome", fadeText, fadeEffectText);
            yield return new WaitForSeconds(2f);

            accountPanel.TransitionIn();
            descriptionPanel.TransitionIn();
            controlPanel.TransitionIn();
        }

        private void SetFadeText(string _text)
        {
            fadeText.SetText(_text);
            fadeEffectText.SetText(_text);
        }

        private IEnumerator SetRhythmAndFadeTextWithTypingAnimationCoroutine(string _rhythmTextString, string _fadeTextString)
        {
            textTyper.TypeTextNoCancel(_rhythmTextString, rhythmText, rhythmEffectText);
            yield return new WaitForSeconds(textTyper.GetTextTypeDuration(_rhythmTextString));
            textTyper.TypeTextNoCancel(_fadeTextString, fadeText, fadeEffectText);
            yield return null;
        }
        #endregion
    }
}
