namespace Gameplay
{
    using UnityEngine;
    using Grade;
    using TMPro;
    using System.Text;
    using Enums;
    using System.Collections;

    public sealed class AccuracyManager : MonoBehaviour
    {

        #region Constants
        private const float accuracyLerpDuration = 0.25f;
        #endregion

        #region Private Fields
        private float currentAccuracy = 0f;
        private float totalAccuracy = 0;
        private float accuracyLerpTimer = 0f;

        private Grade currentGrade = Grade.X;

        private StringBuilder accuracyTextStringBuilder = new StringBuilder();

        [SerializeField] private TextMeshProUGUI accuracyText = default;

        private IEnumerator trackIncreasingAccuracyCoroutine;

        private ScoreManager scoreManager;
        private GradeSlider gradeSlider;
        private GameManager gameManager;
        #endregion

        #region Properties
        public float CurrentAccuracy => currentAccuracy;
        public Grade CurrentGrade => currentGrade;
        #endregion

        #region Public Methods
        public void UpdateAccuracy()
        {
            totalAccuracy = ((float)scoreManager.CurrentBaseScore / (float)scoreManager.MaxBaseScorePossible) * 100;
            ResetAccuracyLerpTimer();
            CheckIfNewGradeAchieved();
            gradeSlider.CalculateValueForCurrentGrade();
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            scoreManager = MonoBehaviour.FindObjectOfType<ScoreManager>();
            gradeSlider = MonoBehaviour.FindObjectOfType<GradeSlider>();
            gameManager = MonoBehaviour.FindObjectOfType<GameManager>();
        }

        private void UpdateAccuracyText()
        {
            accuracyTextStringBuilder.Append(currentAccuracy.ToString("F2"));
            accuracyTextStringBuilder.Append("%");
            accuracyText.SetText(accuracyTextStringBuilder.ToString());
            accuracyTextStringBuilder.Clear();
        }

        private void CheckIfNewGradeAchieved()
        {
            Grade resultGrade = GradeData.GetCurrentGrade(currentAccuracy);

            if (currentGrade != resultGrade)
            {
                currentGrade = resultGrade;
                gradeSlider.UpdateGradeVisuals();
            }
        }

        public void TrackIncreasingAccuracy()
        {
            if (trackIncreasingAccuracyCoroutine != null)
            {
                StopCoroutine(trackIncreasingAccuracyCoroutine);
            }

            trackIncreasingAccuracyCoroutine = TrackIncreasingAccuracyCoroutine();
            StartCoroutine(trackIncreasingAccuracyCoroutine);
        }

        private void ResetAccuracyLerpTimer()
        {
            accuracyLerpTimer = 0f;
        }

        private IEnumerator TrackIncreasingAccuracyCoroutine()
        {
            while (gameManager.GameplayStarted == true)
            {
                if (currentAccuracy != totalAccuracy)
                {
                    accuracyLerpTimer += Time.deltaTime / accuracyLerpDuration;
                    currentAccuracy = Mathf.Lerp(currentAccuracy, totalAccuracy, accuracyLerpTimer);
                    UpdateAccuracyText();
                }
                yield return null;
            }
            yield return null;
        }
        #endregion
    }
}
