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

        [SerializeField] private CanvasGroup flashCanvasGroup = default;

        [SerializeField] private Button button = default;

        [SerializeField] private Difficulty difficulty = default;

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
            
        }

        public void DisableButton()
        {
            button.interactable = false;
        }

        public void ActivateButton()
        {
            button.interactable = true;
        }

        public void UnselectButton()
        {

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

        private void PlayFlashAnimation()
        {
            flashCanvasGroup.alpha = 0f;
            LeanTween.cancel(flashCanvasGroup.gameObject);

            LeanTween.alphaCanvas(flashCanvasGroup, 1f, 1f).setEasePunch();
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