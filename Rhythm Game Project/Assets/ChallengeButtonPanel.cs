namespace Menu
{
    using UnityEngine;
    using File;

    public sealed class ChallengeButtonPanel : MonoBehaviour
    {
        #region Constants
        private const string Requirement100 = "100";
        private const string Requirement1 = "1";
        #endregion

        #region Private Fields
        [SerializeField] private BeatmapChallengeButton clearButton = default;
        [SerializeField] private BeatmapChallengeButton hiddenButton = default;
        [SerializeField] private BeatmapChallengeButton mineButton = default;
        [SerializeField] private BeatmapChallengeButton lowApproachRateButton = default;
        [SerializeField] private BeatmapChallengeButton highApproachRateButton = default;
        [SerializeField] private BeatmapChallengeButton fullComboButton = default;
        [SerializeField] private BeatmapChallengeButton maxPercentageButton = default;
        #endregion

        #region Public Methods
        public void SetAllButtonVisual(string _clearPoints, string _hiddenPoints, string _minePoints, string _lowApproachRatePoints,
            string _highApproachRatePoints, string _fullComboPoints, string _maxPercentagePoints)
        {
            SetButtonVisual(_clearPoints, Requirement100, clearButton);
            SetButtonVisual(_hiddenPoints, Requirement100, hiddenButton);
            SetButtonVisual(_minePoints, Requirement100, mineButton);
            SetButtonVisual(_lowApproachRatePoints, Requirement100, lowApproachRateButton);
            SetButtonVisual(_highApproachRatePoints, Requirement100, highApproachRateButton);
            SetButtonVisual(_fullComboPoints, Requirement1, fullComboButton);
            SetButtonVisual(_maxPercentagePoints, Requirement1, maxPercentageButton);
        }

        public void HideAllButtonText()
        {
            clearButton.HideText();
            hiddenButton.HideText();
            mineButton.HideText();
            lowApproachRateButton.HideText();
            highApproachRateButton.HideText();
            fullComboButton.HideText();
            maxPercentageButton.HideText();

            ResetAllButtons();
        }

        public void ShowAllButtonText()
        {
            clearButton.ShowText();
            hiddenButton.ShowText();
            mineButton.ShowText();
            lowApproachRateButton.ShowText();
            highApproachRateButton.ShowText();
            fullComboButton.ShowText();
            maxPercentageButton.ShowText();

            PlayFlashAnimationForAchievedButtons();
        }

        public void PlayFlashAnimationForAchievedButtons()
        {
            PlayFlashAnimationForButtonIfAchieved(clearButton);
            PlayFlashAnimationForButtonIfAchieved(hiddenButton);
            PlayFlashAnimationForButtonIfAchieved(mineButton);
            PlayFlashAnimationForButtonIfAchieved(lowApproachRateButton);
            PlayFlashAnimationForButtonIfAchieved(highApproachRateButton);
            PlayFlashAnimationForButtonIfAchieved(fullComboButton);
            PlayFlashAnimationForButtonIfAchieved(maxPercentageButton);
        }
        #endregion

        #region Private Methods
        private void PlayFlashAnimationForButtonIfAchieved(BeatmapChallengeButton _beatmapChallengeButton)
        {
            if (_beatmapChallengeButton.HasAchieved == true)
            {
                _beatmapChallengeButton.PlayFlashCanvasGroupAnimation();
            }
        }

        private void ResetAllButtons()
        {
            clearButton.SetNotAchieved();
            hiddenButton.SetNotAchieved();
            mineButton.SetNotAchieved();
            lowApproachRateButton.SetNotAchieved();
            highApproachRateButton.SetNotAchieved();
            fullComboButton.SetNotAchieved();
            maxPercentageButton.SetNotAchieved();
        }

        private void SetButtonVisual(string _pointValue, string _requirement, BeatmapChallengeButton _beatmapChallengeButton)
        {
            _beatmapChallengeButton.SetPointText(_pointValue, _requirement);

            if (_pointValue == _requirement)
            {
                if (_beatmapChallengeButton.HasAchieved == false)
                {
                    _beatmapChallengeButton.SetAchieved();
                }
            }
            else
            {
                if (_beatmapChallengeButton.HasAchieved == true)
                {
                    _beatmapChallengeButton.SetNotAchieved();
                }
            }
        }
        #endregion
    }
}