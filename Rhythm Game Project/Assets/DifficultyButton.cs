namespace Menu
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using Enums;
    using System.Collections;

    public sealed class DifficultyButton : MonoBehaviour
    {
        #region Constants
        private readonly Vector3 textScaleTo = new Vector3(1.25f, 1.25f, 1f);

        private const byte defaultTextSize = 75;
        private const byte selectedTextSize = 100;
        #endregion

        #region Private Fields
        [SerializeField] private TextMeshProUGUI levelText = default;
        [SerializeField] private TextMeshProUGUI gradeText = default;

        [SerializeField] private Button button = default;

        [SerializeField] private Image glowImage = default;

        [SerializeField] private CanvasGroup glowImageCanvasGroup = default;
        [SerializeField] private CanvasGroup gradeCanvasGroup = default;
        [SerializeField] private CanvasGroup levelCanvasGroup = default;

        [SerializeField] private Difficulty difficulty = default;

        private IEnumerator playLevelAndGradeAnimationCoroutine;

        private Transform levelTextCachedTransform;
        private Transform gradeTextCachedTransform;

        private BeatmapOverviewManager beatmapOverviewManager;
        #endregion

        #region Public Methods
        public void SetLevelText(string _text)
        {
            levelText.SetText(_text);
        }

        public void SetGradeText(TextMeshProUGUI _text)
        {
            gradeText.text = _text.text;
            gradeText.colorGradientPreset = _text.colorGradientPreset;
        }

        public void Button_OnClick()
        {
            beatmapOverviewManager.LoadBeatmap(beatmapOverviewManager.SelectedButtonIndex, difficulty);
        }

        public void SelectButton()
        {
            PlaySelectedAnimation();
        }

        public void DisableButton()
        {
            button.interactable = false;
        }

        public void ActivateButton()
        {
            button.interactable = true;
            PlayLevelAndGradeAnimation();
        }

        public void UnselectButton()
        {
            StopSelectAnimation();
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            beatmapOverviewManager = FindObjectOfType<BeatmapOverviewManager>();

            levelTextCachedTransform = levelText.transform;
            gradeTextCachedTransform = gradeText.transform;
        }

        private void PlaySelectedAnimation()
        {
            levelText.fontSize = selectedTextSize;
            gradeText.fontSize = selectedTextSize;

            glowImageCanvasGroup.alpha = 0f;
            glowImageCanvasGroup.gameObject.SetActive(true);

            LeanTween.alphaCanvas(glowImageCanvasGroup, 1f, 0.10f).setLoopPingPong(-1);
            LeanTween.scale(levelText.gameObject, textScaleTo, 0.10f).setLoopPingPong(-1);
            LeanTween.scale(gradeText.gameObject, textScaleTo, 0.10f).setLoopPingPong(-1);
        }

        private void StopSelectAnimation()
        {
            levelText.fontSize = defaultTextSize;
            gradeText.fontSize = defaultTextSize;

            glowImageCanvasGroup.alpha = 0f;
            levelTextCachedTransform.localScale = Vector3.one;
            gradeTextCachedTransform.localScale = Vector3.one;
            LeanTween.cancel(glowImageCanvasGroup.gameObject);
            LeanTween.cancel(levelText.gameObject);
            LeanTween.cancel(gradeText.gameObject);

            glowImageCanvasGroup.gameObject.SetActive(false);
        }

        private void PlayLevelAndGradeAnimation()
        {
            if (playLevelAndGradeAnimationCoroutine != null)
            {
                StopCoroutine(playLevelAndGradeAnimationCoroutine);
            }

            playLevelAndGradeAnimationCoroutine = PlayLevelAndGradeAnimationCoroutine();
            StartCoroutine(playLevelAndGradeAnimationCoroutine);
        }

        private IEnumerator PlayLevelAndGradeAnimationCoroutine()
        {
            WaitForSeconds fadeDelay = new WaitForSeconds(1f);
            WaitForSeconds visibleDelay = new WaitForSeconds(3f);

            while (this.gameObject.activeSelf == true)
            {
                gradeCanvasGroup.alpha = 0f;
                levelCanvasGroup.alpha = 1f;
                LeanTween.cancel(gradeCanvasGroup.gameObject);
                LeanTween.cancel(levelCanvasGroup.gameObject);

                LeanTween.alphaCanvas(levelCanvasGroup, 0f, 1f);

                yield return fadeDelay;

                LeanTween.alphaCanvas(gradeCanvasGroup, 1f, 1f);

                yield return visibleDelay;

                LeanTween.alphaCanvas(gradeCanvasGroup, 0f, 1f);

                yield return fadeDelay;

                LeanTween.alphaCanvas(levelCanvasGroup, 1f, 1f);

                yield return visibleDelay;

                yield return null;
            }

            yield return null;
        }
        #endregion
    }

}