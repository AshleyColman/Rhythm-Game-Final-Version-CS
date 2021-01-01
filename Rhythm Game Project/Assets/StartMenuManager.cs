namespace Menu
{
    using System.Collections;
    using UnityEngine;
    using Audio;
    using TMPro;
    using SceneLoading;

    public sealed class StartMenuManager : MonoBehaviour, IMenu
    {
        #region Constants
        private const float StartSongBeatsPerMinute = 191f;
        private const float TimeToPlayAudio = 1f;

        private const double StartSongOffsetMilliseconds = 1000; //1283.00;

        private readonly Vector3 titleScaleTo = new Vector3(1.25f, 1.25f, 1f);
        private readonly Vector3 titleEffectScaleTo = new Vector3(1.75f, 1.75f, 1f);
        #endregion

        #region Private Fields
        [SerializeField] private GameObject startupScreen = default;
        [SerializeField] private GameObject startAndAccountPanel = default;

        [SerializeField] private TextMeshProUGUI titleText = default;
        [SerializeField] private TextMeshProUGUI titleEffectText = default;
        [SerializeField] private TextMeshProUGUI startText = default;

        private Transform titleTextCachedTransform;
        private Transform titleEffectTextCachedTransform;

        [SerializeField] private CanvasGroup startTextCanvasGroup = default;

        private bool anyKeyHasBeenPressed = false;

        private MenuAudioManager menuAudioManager;
        private MenuTimeManager menuTimeManager;
        private Transition transition;
        private MenuManager menuManager;
        #endregion

        #region Public Methods
        public void TransitionIn()
        {
            PrepareToStartAudioAndTimer();
            PlayStartCanvasGroupTween();
            transition.PlayFadeInTween();
            CheckToStartGame();
        }

        public void TransitionOut()
        {

        }

        public void PlayTitleRhythmTween()
        {
            if (startupScreen.gameObject.activeSelf == true)
            {
                LeanTween.cancel(titleText.gameObject);
                LeanTween.cancel(titleEffectText.gameObject);
                titleTextCachedTransform.localScale = Vector3.one;
                titleEffectTextCachedTransform.localScale = Vector3.one;

                LeanTween.scale(titleText.gameObject, titleScaleTo, 1f).setEasePunch();
                LeanTween.scale(titleEffectText.gameObject, titleEffectScaleTo, 1f).setEasePunch();
            }
        }

        public void TransitionOutStartAndAccountPanel()
        {
            startAndAccountPanel.gameObject.SetActive(false);
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            menuAudioManager = FindObjectOfType<MenuAudioManager>();
            menuTimeManager = FindObjectOfType<MenuTimeManager>();
            transition = FindObjectOfType<Transition>();
            menuManager = FindObjectOfType<MenuManager>();
            menuTimeManager.SetTimingDetails(StartSongBeatsPerMinute, StartSongOffsetMilliseconds);
            menuTimeManager.UpdateTimingPosition();
            SetTitleTextTransforms();
        }

        private void SetTitleTextTransforms()
        {
            titleTextCachedTransform = titleText.transform;
            titleEffectTextCachedTransform = titleEffectText.transform;
        }

        private void PrepareToStartAudioAndTimer()
        {
            StopCoroutine("PrepareToStartAudioAndTimerCoroutine");
            StartCoroutine(PrepareToStartAudioAndTimerCoroutine());
        }

        private IEnumerator PrepareToStartAudioAndTimerCoroutine()
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(TimeToPlayAudio);
            menuAudioManager.PlayScheduledSongAudio(TimeToPlayAudio);
            yield return waitForSeconds;
            menuTimeManager.StartTimer();
            yield return null;
        }

        private void PlayStartCanvasGroupTween()
        {
            LeanTween.alphaCanvas(startTextCanvasGroup, 0f, 1f).setLoopPingPong(-1);
        }

        private void CancelStartCanvasGroupTween()
        {
            LeanTween.cancel(startTextCanvasGroup.gameObject);
        }

        private void PlayStartGameStartedCanvasGroupTween()
        {
            startTextCanvasGroup.alpha = 1f;
            LeanTween.alphaCanvas(startTextCanvasGroup, 0f, 0.25f).setLoopPingPong(4);
        }

        private void CheckToStartGame()
        {
            StopCoroutine("CheckToStartGameCoroutine");
            StartCoroutine(CheckToStartGameCoroutine());
        }

        private IEnumerator CheckToStartGameCoroutine()
        {
            while (anyKeyHasBeenPressed == false)
            {
                if (Input.anyKeyDown)
                {
                    StartGame();
                }
                yield return null;
            }

            yield return null;
        }

        private void StartGame()
        {
            StopCoroutine("StartGameCoroutine");
            StartCoroutine(StartGameCoroutine());
        }

        private IEnumerator StartGameCoroutine()
        {
            anyKeyHasBeenPressed = true;
            CancelStartCanvasGroupTween();
            PlayStartGameStartedCanvasGroupTween();
            startText.SetText("game started");
            yield return new WaitForSeconds(2f);
            menuManager.TransitionStartMenuToAccountPanel();
        }
        #endregion
    }
}
