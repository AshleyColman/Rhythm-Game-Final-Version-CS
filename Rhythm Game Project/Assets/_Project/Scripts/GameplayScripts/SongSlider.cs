namespace Gameplay
{
    using System.Collections;
    using System.Text;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Audio;

    public sealed class SongSlider : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] private TextMeshProUGUI currentTimePercentageText = default;
        [SerializeField] private TextMeshProUGUI currentTimeText = default;
        [SerializeField] private TextMeshProUGUI endTimeText = default;

        [SerializeField] private Slider songTimeSlider = default;

        private float currentTimePercentage = 0f;

        private StringBuilder percentageTextStringBuilder = new StringBuilder();

        private GameplayAudioManager gameplayAudioManager;
        #endregion

        #region Public Methods
        public void UpdateProgress()
        {
            StopCoroutine("UpdateProgressCoroutine");
            StartCoroutine(UpdateProgressCoroutine());
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            gameplayAudioManager = MonoBehaviour.FindObjectOfType<GameplayAudioManager>();
            endTimeText.SetText(UtilityMethods.FromSecondsToMinutesAndSeconds(gameplayAudioManager.SongAudioSource.clip.length));
        }

        private void CalculateCurrentTimePercentage()
        {
            currentTimePercentage = (gameplayAudioManager.SongAudioSource.time /
                gameplayAudioManager.SongAudioSource.clip.length) * 100;
        }

        private void UpdateText()
        {
            percentageTextStringBuilder.Append(currentTimePercentage.ToString("F0"));
            percentageTextStringBuilder.Append("%");
            currentTimePercentageText.SetText(percentageTextStringBuilder.ToString());
            percentageTextStringBuilder.Clear();

            currentTimeText.SetText(UtilityMethods.FromSecondsToMinutesAndSeconds(gameplayAudioManager.SongAudioSource.time));
        }

        private IEnumerator UpdateProgressCoroutine()
        {
            while (currentTimePercentage < 100)
            {
                CalculateCurrentTimePercentage();
                UpdateText();
                songTimeSlider.value = currentTimePercentage;
                yield return null;
            }
            yield return null;
        }
        #endregion
    }
}