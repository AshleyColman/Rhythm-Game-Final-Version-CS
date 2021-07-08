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
        private readonly Vector3 gradeTextScaleTo = new Vector3(1.25f, 1.25f, 1f);
        private readonly Vector3 gradeEffectTextScaleTo = new Vector3(1.75f, 1.75f, 1f);
        #endregion

        #region Private Fields
        [SerializeField] private TextMeshProUGUI difficultyText = default;
        [SerializeField] private TextMeshProUGUI gradeText = default;
        [SerializeField] private TextMeshProUGUI gradeEffectText = default;
        [SerializeField] private TextMeshProUGUI accuracyText = default;

        [SerializeField] private CanvasGroup beatFlashCanvasGroup = default;
        [SerializeField] private CanvasGroup accuracyPanelFlashCanvasGroup = default;
        [SerializeField] private CanvasGroup difficultyPanelFlashCanvasGroup = default;
        [SerializeField] private CanvasGroup gradePanelFlashCanvasGroup = default;

        [SerializeField] private Button button = default;

        [SerializeField] private Difficulty difficulty = default;

        private IEnumerator playButtonSelectedAnimation;

        private Transform gradeTextCachedTransform = default;
        private Transform gradeEffectTextCachedTransform = default;

        private BeatmapOverviewManager beatmapOverviewManager;
        #endregion

        #region Public Methods
        public void SetGradeText(TextMeshProUGUI _text)
        {
            gradeText.text = _text.text;
            gradeEffectText.text = _text.text;
            gradeText.colorGradientPreset = _text.colorGradientPreset;
            gradeEffectText.colorGradientPreset = _text.colorGradientPreset;
        }

        public void Button_OnClick()
        {
            beatmapOverviewManager.LoadBeatmap(beatmapOverviewManager.SelectedButtonIndex, difficulty);
        }

        public void SelectButton()
        {
            button.interactable = false;
            PlayButtonSelectedAnimation();
        }

        public void UnselectButton()
        {
            button.interactable = true;
        }

        public void PlaySelectedBeatAnimation()
        {
            PlayGradeTextAnimation();
            PlayFlashAnimation();
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            beatmapOverviewManager = FindObjectOfType<BeatmapOverviewManager>();

            gradeTextCachedTransform = gradeText.transform;
            gradeEffectTextCachedTransform = gradeEffectText.transform;
        }

        private void PlayButtonSelectedAnimation()
        {
            if (playButtonSelectedAnimation != null)
            {
                StopCoroutine(playButtonSelectedAnimation);
            }

            playButtonSelectedAnimation = PlayButtonSelectedAnimationCoroutine();
            StartCoroutine(playButtonSelectedAnimation);
        }

        private IEnumerator PlayButtonSelectedAnimationCoroutine()
        {
            accuracyPanelFlashCanvasGroup.alpha = 0f;
            difficultyPanelFlashCanvasGroup.alpha = 0f;
            gradePanelFlashCanvasGroup.alpha = 0f;
            LeanTween.cancel(accuracyPanelFlashCanvasGroup.gameObject);
            LeanTween.cancel(difficultyPanelFlashCanvasGroup.gameObject);
            LeanTween.cancel(gradePanelFlashCanvasGroup.gameObject);
            accuracyPanelFlashCanvasGroup.gameObject.SetActive(true);
            difficultyPanelFlashCanvasGroup.gameObject.SetActive(true);
            gradePanelFlashCanvasGroup.gameObject.SetActive(true);

            LeanTween.alphaCanvas(accuracyPanelFlashCanvasGroup, 1f, 1f).setEasePunch();
            LeanTween.alphaCanvas(difficultyPanelFlashCanvasGroup, 1f, 1f).setEasePunch();
            LeanTween.alphaCanvas(gradePanelFlashCanvasGroup, 1f, 1f).setEasePunch();

            yield return new WaitForSeconds(1f);

            accuracyPanelFlashCanvasGroup.gameObject.SetActive(false);
            difficultyPanelFlashCanvasGroup.gameObject.SetActive(false);
            gradePanelFlashCanvasGroup.gameObject.SetActive(false);

            yield return null;

        }

        private void PlayFlashAnimation()
        {
            beatFlashCanvasGroup.alpha = 0f;
            LeanTween.cancel(beatFlashCanvasGroup.gameObject);

            LeanTween.alphaCanvas(beatFlashCanvasGroup, 1f, 0.2f).setLoopPingPong(1);
        }

        private void PlayGradeTextAnimation()
        {
            gradeTextCachedTransform.localScale = Vector3.one;
            gradeEffectTextCachedTransform.localScale = Vector3.one;
            LeanTween.cancel(gradeText.gameObject);
            LeanTween.cancel(gradeEffectText.gameObject);

            LeanTween.scale(gradeText.gameObject, gradeTextScaleTo, 1f).setEasePunch();
            LeanTween.scale(gradeEffectText.gameObject, gradeEffectTextScaleTo, 1f).setEasePunch();
        }
        #endregion
    }
}