namespace Menu
{
    using System.Collections;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public sealed class PersonalBestLeaderboardButton : MonoBehaviour
    {
        #region Constants
        private byte scoreFieldButtonIndex = 0;
        private byte comboFieldButtonIndex = 1;
        private byte perfectFieldButtonIndex = 2;
        private byte greatFieldButtonIndex = 3;
        private byte okayFieldButtonIndex = 4;
        private byte missFieldButtonIndex = 5;
        private byte feverScoreFieldButtonIndex = 6;
        private byte bonusScoreFieldButtonIndex = 7;
        #endregion

        #region Private Fields
        [SerializeField] private FieldButton[] fieldButtonArr = default;

        [SerializeField] private TextMeshProUGUI positionText = default;

        [SerializeField] private ChallengeButtonPanel challengeButtonPanel = default;

        private IEnumerator activateFieldButtonArray;
        #endregion

        #region Public Methods
        public void DeactivatePersonalBest()
        {
            for (byte i = 0; i < fieldButtonArr.Length; i++)
            {
                fieldButtonArr[i].DeactivateText();
            }

            challengeButtonPanel.HideAllButtonText();
        }

        public void DisplayPersonalBest()
        {
            ActivateFieldButtonArray();
            challengeButtonPanel.ShowAllButtonText();
        }

        public void SetButtonText(string _scoreValueText, string _comboValueText, string _perfectValueText,
            string _greatValueText, string _okayValueText, string _missValueText, string _feverScoreValueText,
            string _bonusScoreValueText, string _positionText)
        {
            fieldButtonArr[scoreFieldButtonIndex].SetValueText(_scoreValueText);
            fieldButtonArr[comboFieldButtonIndex].SetValueText(_comboValueText);
            fieldButtonArr[perfectFieldButtonIndex].SetValueText(_perfectValueText);
            fieldButtonArr[greatFieldButtonIndex].SetValueText(_greatValueText);
            fieldButtonArr[okayFieldButtonIndex].SetValueText(_okayValueText);
            fieldButtonArr[missFieldButtonIndex].SetValueText(_missValueText);
            fieldButtonArr[feverScoreFieldButtonIndex].SetValueText(_feverScoreValueText);
            fieldButtonArr[bonusScoreFieldButtonIndex].SetValueText(_bonusScoreValueText);
            positionText.SetText(_positionText);
        }

        public void SetButtonTextNoRecord()
        {
            string noRecordText = "-";

            for (byte i = 0; i < fieldButtonArr.Length; i++)
            {
                fieldButtonArr[i].SetValueText(noRecordText);

            }

            positionText.SetText(string.Empty);
        }

        public void SetChallengeButtonPanelVisual(string _clearPoints, string _hiddenPoints, string _minePoints,
            string _lowApproachRatePoints, string _highApproachRatePoints, string _fullComboPoints, string _maxPercentagePoints)
        {
            challengeButtonPanel.SetAllButtonVisual(_clearPoints, _hiddenPoints, _minePoints, _lowApproachRatePoints,
                _highApproachRatePoints, _fullComboPoints, _maxPercentagePoints);
        }
        #endregion

        #region Private Methods
        private void ActivateFieldButtonArray()
        {
            if (activateFieldButtonArray != null)
            {
                StopCoroutine(activateFieldButtonArray);
            }

            activateFieldButtonArray = ActivateFieldButtonArrayCoroutine();
            StartCoroutine(activateFieldButtonArray);
        }

        private IEnumerator ActivateFieldButtonArrayCoroutine()
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
            
            for (byte i = 0; i < fieldButtonArr.Length; i++)
            {
                fieldButtonArr[i].ActivateText();

                yield return waitForSeconds;
            }

            yield return null;
        }
        #endregion
    }
}