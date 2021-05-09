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
        private byte buttonArraySize = 13;

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
            SetupComputerGradeButtonProperties();
            //SetButtonProperties();
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

        }

        private void SetupComputerGradeButtonProperties()
        {
            Grade[] gradeArr = new Grade[] { Grade.S_PLUS, Grade.S, Grade.A_PLUS, Grade.A, Grade.B_PLUS, Grade.B,
                Grade.C_PLUS, Grade.C, Grade.D_PLUS, Grade.D, Grade.E_PLUS, Grade.E, Grade.F_PLUS, Grade.F };

            byte[] gradeRequirementArr = new byte[] { GradeData.GradeRequiredSPlus, GradeData.GradeRequiredS,
            GradeData.GradeRequiredAPlus, GradeData.GradeRequiredA, GradeData.GradeRequiredBPlus, GradeData.GradeRequiredB,
            GradeData.GradeRequiredCPlus, GradeData.GradeRequiredC, GradeData.GradeRequiredDPlus, GradeData.GradeRequiredD,
            GradeData.GradeRequiredEPlus, GradeData.GradeRequiredE, GradeData.GradeRequiredFPlus, GradeData.GradeRequiredF};

            for (byte i = 0; i < buttonArray.Length; i++)
            {
                byte index = (byte)(i + 1);
                buttonArray[i].SetPosition(index);
                buttonArray[i].SetName($"CPU {index}");

                if (i <= gradeArr.Length)
                {
                    // CPU score is based on the max base score possible, by the percentage given, multiplied by the highest base multiplier x5.
                    double percentageScore = ((scoreManager.MaxBaseScorePossible / 100) * gradeRequirementArr[i]) * 5;
                    buttonArray[i].SetScore((uint)percentageScore);

                    buttonArray[i].SetGrade(gradeData.GetCurrentGradeGradient(gradeArr[i]),
                        GradeData.GetCurrentGradeString(gradeArr[i]));
                }
            }
        }

        private void SetPersonalButtonProperties()
        {
            personalButton.transform.SetAsLastSibling();
            personalButton.SetPosition((byte)(buttonArray.Length + 1), "you");
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
                    personalButton.SetPositionValueOnly(tempPosition);
                }
            }
        }
        #endregion
    }
}
