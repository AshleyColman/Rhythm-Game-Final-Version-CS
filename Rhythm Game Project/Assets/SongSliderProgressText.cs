namespace Audio
{
    using System.Text;
    using System.Collections;
    using TMPro;
    using UnityEngine;

    public sealed class SongSliderProgressText : SongSlider
    {
        #region Private Fields
        [SerializeField] private TextMeshProUGUI currentTimePercentageText = default;
        [SerializeField] private TextMeshProUGUI currentTimeText = default;
        [SerializeField] private TextMeshProUGUI endTimeText = default;

        private StringBuilder percentageTextStringBuilder = new StringBuilder();

        private IEnumerator updateTextCoroutine;
        #endregion

        #region Public Methods
        public override void UpdateSongSliderProgress()
        {
            base.UpdateSongSliderProgress();
            UpdateText();
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            SetEndTimeText();
        }

        private void SetEndTimeText()
        {
            if (audioManager.SongAudioSource.clip is null == false)
            {
                endTimeText.SetText(UtilityMethods.FromSecondsToMinutesAndSeconds(audioManager.SongAudioSource.clip.length));
            }
            else
            {
                endTimeText.SetText("00:00");
            }
        }

        private void UpdateText()
        {
            if (updateTextCoroutine != null)
            {
                StopCoroutine(updateTextCoroutine);
            }

            updateTextCoroutine = UpdateTextCoroutine();
            StartCoroutine(updateTextCoroutine);
        }

        private IEnumerator UpdateTextCoroutine()
        {
            while (audioManager.SongAudioSource.isPlaying == true)
            {
                percentageTextStringBuilder.Append(currentTimePercentage.ToString("F0"));
                percentageTextStringBuilder.Append("%");
                currentTimePercentageText.SetText(percentageTextStringBuilder.ToString());
                percentageTextStringBuilder.Clear();
                currentTimeText.SetText(UtilityMethods.FromSecondsToMinutesAndSeconds(audioManager.SongAudioSource.time));

                yield return null;
            }

            yield return null;
        }
        #endregion
    }
}
