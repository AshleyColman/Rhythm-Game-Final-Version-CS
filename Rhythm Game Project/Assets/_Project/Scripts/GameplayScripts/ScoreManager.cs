namespace Gameplay
{
    using System.Collections;
    using System.Text;
    using TMPro;
    using UnityEngine;

    public sealed class ScoreManager : MonoBehaviour
    {
        #region Constants
        private const float scoreLerpDuration = 0.25f;
        private readonly Vector3 effectScaleTo = new Vector3(1.25f, 1.25f, 1f);
        #endregion

        #region Private Fields
        private uint totalScore = 0;
        private uint currentScore = 0;
        private uint totalBaseScore = 0;
        private uint currentBaseScore = 0;
        private uint maxBaseScorePossible = 0;

        private float scoreLerpTimer = 0f;

        private StringBuilder scoreStringBuilder = new StringBuilder();

        [SerializeField] private TextMeshProUGUI scoreText = default;

        private Transform scoreEffectTextTransform;

        private Leaderboard leaderboard;
        private GameManager gameManager;
        private ScoreEffect scoreEffect;
        private MultiplierManager multiplierManager;
        #endregion

        #region Properties
        public uint TotalScore => totalScore;
        public uint CurrentScore => currentScore;
        public uint CurrentBaseScore => currentBaseScore;
        public uint TotalBaseScore => totalBaseScore;
        public uint MaxBaseScorePossible => maxBaseScorePossible;
        public TextMeshProUGUI ScoreText => scoreText;
        #endregion

        #region Public Methods
        public void TrackIncreasingScore()
        {
            StopCoroutine("TrackIncreasingScoreCoroutine");
            StartCoroutine(TrackIncreasingScoreCoroutine());
        }

        public void IncreaseScore(uint _score)
        {
            ResetScoreLerpTimer();
            scoreEffect.PlayScoreEffect(_score * multiplierManager.Multiplier);
            totalScore += (_score * multiplierManager.Multiplier);
            totalBaseScore += _score;
        }

        public void CalculateMaxBaseScorePossible(ushort _totalObjects)
        {
            maxBaseScorePossible = (uint)_totalObjects * JudgementData.PerfectScore;
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            leaderboard = FindObjectOfType<Leaderboard>();
            gameManager = FindObjectOfType<GameManager>();
            scoreEffect = FindObjectOfType<ScoreEffect>();
            multiplierManager = FindObjectOfType<MultiplierManager>();
        }

        private void ResetScoreLerpTimer()
        {
            scoreLerpTimer = 0f;
        }

        private IEnumerator TrackIncreasingScoreCoroutine()
        {
            while (gameManager.GameplayStarted == true)
            {
                if (currentScore != totalScore)
                {
                    scoreLerpTimer += Time.deltaTime / scoreLerpDuration;
                    currentScore = (uint)Mathf.Lerp(currentScore, totalScore, scoreLerpTimer);
                    currentBaseScore = (uint)Mathf.Lerp(currentBaseScore, totalBaseScore, scoreLerpTimer);
                    UpdateScoreText();
                }
                yield return null;
            }
            yield return null;
        }

        private void UpdateScoreText()
        {
            scoreStringBuilder.Append(currentScore);
            scoreStringBuilder = UtilityMethods.AddZerosToScoreString(scoreStringBuilder);
            scoreText.SetText(scoreStringBuilder.ToString());
            scoreStringBuilder.Clear();
            leaderboard.UpdatePersonalBestScore();
        }
        #endregion
    }
}
