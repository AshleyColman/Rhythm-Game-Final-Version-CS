namespace Grade
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using Gameplay;
    using Enums;
    using VisualEffects;

    public sealed class GradeSlider : MonoBehaviour
    {
        #region Constants
        private readonly Vector3 scaleTo = new Vector3(1.25f, 1.25f, 1f);
        private readonly Vector3 effectScaleTo = new Vector3(1.75f, 1.75f, 1f);
        #endregion

        #region Private Fields
        [SerializeField] private Slider gradeSlider = default;

        [SerializeField] private Image colorImage = default;

        [SerializeField] private CanvasGroup flashImageCanvasGroup = default;

        [SerializeField] private TextMeshProUGUI gradeText = default;
        [SerializeField] private TextMeshProUGUI gradeEffectText = default;

        private Transform gradeTextTransform;
        private Transform gradeEffectTextTransform;

        private AccuracyManager accuracyManager;
        private GradeData gradeData;
        private Leaderboard leaderboard;
        #endregion

        #region Properties
        public TextMeshProUGUI GradeText => gradeText;
        #endregion

        #region Public Methods
        public void CalculateValueForCurrentGrade()
        {
            switch (accuracyManager.CurrentGrade)
            {
                case Grade.F:
                    gradeSlider.value = (accuracyManager.CurrentAccuracy / GradeData.GradeRequiredFPlus) * 100;
                    break;
                case Grade.F_PLUS:
                    gradeSlider.value = ((accuracyManager.CurrentAccuracy - GradeData.GradeRequiredFPlus) * 100) /
                        (GradeData.GradeRequiredE - GradeData.GradeRequiredFPlus);
                    break;
                case Grade.E:
                    gradeSlider.value = ((accuracyManager.CurrentAccuracy - GradeData.GradeRequiredE) * 100) /
                        (GradeData.GradeRequiredEPlus - GradeData.GradeRequiredE);
                    break;
                case Grade.E_PLUS:
                    gradeSlider.value = ((accuracyManager.CurrentAccuracy - GradeData.GradeRequiredEPlus) * 100) /
                        (GradeData.GradeRequiredD - GradeData.GradeRequiredEPlus);
                    break;
                case Grade.D:
                    gradeSlider.value = ((accuracyManager.CurrentAccuracy - GradeData.GradeRequiredD) * 100) /
                        (GradeData.GradeRequiredDPlus - GradeData.GradeRequiredD);
                    break;
                case Grade.D_PLUS:
                    gradeSlider.value = ((accuracyManager.CurrentAccuracy - GradeData.GradeRequiredDPlus) * 100) /
                        (GradeData.GradeRequiredC - GradeData.GradeRequiredDPlus);
                    break;
                case Grade.C:
                    gradeSlider.value = ((accuracyManager.CurrentAccuracy - GradeData.GradeRequiredC) * 100) /
                        (GradeData.GradeRequiredCPlus - GradeData.GradeRequiredC);
                    break;
                case Grade.C_PLUS:
                    gradeSlider.value = ((accuracyManager.CurrentAccuracy - GradeData.GradeRequiredCPlus) * 100) /
                        (GradeData.GradeRequiredB - GradeData.GradeRequiredCPlus);
                    break;
                case Grade.B:
                    gradeSlider.value = ((accuracyManager.CurrentAccuracy - GradeData.GradeRequiredB) * 100) /
                        (GradeData.GradeRequiredBPlus - GradeData.GradeRequiredB);
                    break;
                case Grade.B_PLUS:
                    gradeSlider.value = ((accuracyManager.CurrentAccuracy - GradeData.GradeRequiredBPlus) * 100) /
                        (GradeData.GradeRequiredA - GradeData.GradeRequiredBPlus);
                    break;
                case Grade.A:
                    gradeSlider.value = ((accuracyManager.CurrentAccuracy - GradeData.GradeRequiredA) * 100) /
                        (GradeData.GradeRequiredAPlus - GradeData.GradeRequiredA);
                    break;
                case Grade.A_PLUS:
                    gradeSlider.value = ((accuracyManager.CurrentAccuracy - GradeData.GradeRequiredAPlus) * 100) /
                        (GradeData.GradeRequiredS - GradeData.GradeRequiredAPlus);
                    break;
                case Grade.S:
                    gradeSlider.value = ((accuracyManager.CurrentAccuracy - GradeData.GradeRequiredS) * 100) /
                        (GradeData.GradeRequiredSPlus - GradeData.GradeRequiredS);
                    break;
                case Grade.S_PLUS:
                    gradeSlider.value = gradeSlider.maxValue;
                    break;
            }

            PlayGradeEffectTextTween();
        }

        public void UpdateGradeVisuals()
        {
            switch (accuracyManager.CurrentGrade)
            {
                case Grade.F:
                    colorImage.color = gradeData.GetCurrentGradeColor(Grade.F);
                    gradeText.colorGradientPreset = gradeData.GetCurrentGradeGradient(Grade.F);
                    gradeText.SetText(GradeData.GradeStringF);
                    break;
                case Grade.F_PLUS:
                    gradeText.SetText(GradeData.GradeStringFPlus);
                    break;
                case Grade.E:
                    colorImage.color = gradeData.GetCurrentGradeColor(Grade.E);
                    gradeText.colorGradientPreset = gradeData.GetCurrentGradeGradient(Grade.E);
                    gradeText.SetText(GradeData.GradeStringE);
                    break;
                case Grade.E_PLUS:
                    gradeText.SetText(GradeData.GradeStringEPlus);
                    break;
                case Grade.D:
                    colorImage.color = gradeData.GetCurrentGradeColor(Grade.D);
                    gradeText.colorGradientPreset = gradeData.GetCurrentGradeGradient(Grade.D);
                    gradeText.SetText(GradeData.GradeStringD);
                    break;
                case Grade.D_PLUS:
                    gradeText.SetText(GradeData.GradeStringDPlus);
                    break;
                case Grade.C:
                    colorImage.color = gradeData.GetCurrentGradeColor(Grade.C);
                    gradeText.colorGradientPreset = gradeData.GetCurrentGradeGradient(Grade.C);
                    gradeText.SetText(GradeData.GradeStringC);
                    break;
                case Grade.C_PLUS:
                    gradeText.SetText(GradeData.GradeStringCPlus);
                    break;
                case Grade.B:
                    colorImage.color = gradeData.GetCurrentGradeColor(Grade.B);
                    gradeText.colorGradientPreset = gradeData.GetCurrentGradeGradient(Grade.B);
                    gradeText.SetText(GradeData.GradeStringB);
                    break;
                case Grade.B_PLUS:
                    gradeText.SetText(GradeData.GradeStringBPlus);
                    break;
                case Grade.A:
                    colorImage.color = gradeData.GetCurrentGradeColor(Grade.A);
                    gradeText.colorGradientPreset = gradeData.GetCurrentGradeGradient(Grade.A);
                    gradeText.SetText(GradeData.GradeStringA);
                    break;
                case Grade.A_PLUS:
                    gradeText.SetText(GradeData.GradeStringAPlus);
                    break;
                case Grade.S:
                    colorImage.color = gradeData.GetCurrentGradeColor(Grade.S);
                    gradeText.colorGradientPreset = gradeData.GetCurrentGradeGradient(Grade.S);
                    gradeText.SetText(GradeData.GradeStringS);
                    break;
                case Grade.S_PLUS:
                    gradeText.SetText(GradeData.GradeStringSPlus);
                    break;
            }

            gradeEffectText.SetText(gradeText.text);
            gradeEffectText.colorGradientPreset = gradeText.colorGradientPreset;
            leaderboard.UpdatePersonalGrade();
            PlayFullFlashFillTween();
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            accuracyManager = FindObjectOfType<AccuracyManager>();
            gradeData = FindObjectOfType<GradeData>();
            leaderboard = FindObjectOfType<Leaderboard>();

            ReferenceTransforms();
        }

        private void ReferenceTransforms()
        {
            gradeTextTransform = gradeText.transform;
            gradeEffectTextTransform = gradeEffectText.transform;
        }

        private void PlayGradeEffectTextTween()
        {
            LeanTween.cancel(gradeText.gameObject);
            LeanTween.cancel(gradeEffectText.gameObject);

            gradeTextTransform.localScale = Vector3.one;
            gradeEffectTextTransform.localScale = Vector3.one;

            LeanTween.scale(gradeText.gameObject, scaleTo, 1f).setEasePunch();
            LeanTween.scale(gradeEffectText.gameObject, effectScaleTo, 1f).setEasePunch();
        }

        private void PlayFullFlashFillTween()
        {
            LeanTween.cancel(flashImageCanvasGroup.gameObject);
            flashImageCanvasGroup.alpha = 0f;
            LeanTween.alphaCanvas(flashImageCanvasGroup, 1f, 0.25f).setLoopPingPong(1);
        }
        #endregion
    }
}
