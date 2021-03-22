namespace Gameplay
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using Grade;
    using Enums;

    public sealed class Leaderboard : MonoBehaviour
    {
        #region Private Fields
        private byte buttonArraySize = 10;

        [SerializeField] ScrollRect leaderboardScrollRect = default;

        [SerializeField] private Transform contentTransform = default;

        [SerializeField] private Scrollbar scrollbar = default;

        private LeaderboardButton[] buttonArray = default;

        [SerializeField] private LeaderboardButton buttonPrefab = default;
        [SerializeField] private LeaderboardButton personalButton = default;

        private IEnumerator initializeCoroutine;

        private ScoreManager scoreManager;
        private ComboManager comboManager;
        private GradeSlider gradeSlider;
        private GradeData gradeData;
        #endregion

        #region Public Methods
        public void Initialize()
        {
            if (initializeCoroutine != null)
            {
                StopCoroutine(initializeCoroutine);
            }

            initializeCoroutine = InitializeCoroutine();
            StartCoroutine(initializeCoroutine);
        }

        public void UpdatePersonalBestScore()
        {
            personalButton.SetScore(scoreManager.ScoreText);
            CheckPersonalPosition();
        }

        public void UpdatePersonalCombo()
        {
            personalButton.SetCombo(comboManager.ComboText);
        }

        public void UpdatePersonalGrade()
        {
            personalButton.SetGrade(gradeSlider.GradeText);
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            scoreManager = FindObjectOfType<ScoreManager>();
            comboManager = FindObjectOfType<ComboManager>();
            gradeData = FindObjectOfType<GradeData>();
            gradeSlider = FindObjectOfType<GradeSlider>();
        }

        private IEnumerator InitializeCoroutine()
        {
            DeactivateLeaderboard();
            InstantiateButtons();
            SetButtonProperties();
            SetPersonalButtonProperties();
            ActivateLeaderboard();
            yield return new WaitForEndOfFrame();
            SetScrollbarToStart();
            yield return null;
        }

        private void InstantiateButtons()
        {
            buttonArray = new LeaderboardButton[buttonArraySize];

            for (byte i = 0; i < buttonArraySize; i++)
            {
                LeaderboardButton button = Instantiate(buttonPrefab, contentTransform);
                buttonArray[i] = button;
            }
        }

        private void DeactivateLeaderboard()
        {
            leaderboardScrollRect.gameObject.SetActive(false);
        }

        private void ActivateLeaderboard()
        {
            leaderboardScrollRect.gameObject.SetActive(true);
        }

        private void SetButtonProperties()
        {
            for (byte i = 0; i < buttonArray.Length; i++)
            {
                byte index = (byte)(i + 1);
                buttonArray[i].SetPosition(index);
                buttonArray[i].SetName("name" + index);
                buttonArray[i].SetScore(scoreManager.MaxBaseScorePossible / index);
                buttonArray[i].SetCombo("?");


                float accuracy = ((float)buttonArray[i].Score / (float)scoreManager.MaxBaseScorePossible) * 100;
                Grade grade = GradeData.GetCurrentGrade(accuracy);
                buttonArray[i].SetGrade(gradeData.GetCurrentGradeColor(grade), GradeData.GetCurrentGradeString(grade));
            }
        }

        private void SetPersonalButtonProperties()
        {
            personalButton.transform.SetAsLastSibling();
            personalButton.SetPosition((byte)(buttonArray.Length + 1));
            personalButton.SetScore(0);
        }

        private void SetScrollbarToStart()
        {
            scrollbar.value = 0f;
        }

        private void CheckPersonalPosition()
        {
            if (personalButton.Position != 1)
            {
                byte nextPosition = (byte)(personalButton.Position - 2);
                if (scoreManager.CurrentScore > buttonArray[nextPosition].Score)
                {
                    byte tempPosition = buttonArray[nextPosition].Position;
                    buttonArray[nextPosition].SetPosition(personalButton.Position);
                    buttonArray[nextPosition].Deactivate();
                    personalButton.SetPosition(tempPosition);
                }
            }
        }
        #endregion
    }
}
