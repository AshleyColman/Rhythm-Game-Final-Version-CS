namespace Audio
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    public class SongSlider : MonoBehaviour
    {
        #region Protected Fields
        [SerializeField] protected AudioManager audioManager;

        protected float currentTimePercentage = 0f;
        #endregion

        #region Private Fields
        [SerializeField] private Slider songTimeSlider = default;

        [SerializeField] private Transform songTimeSliderCachedTransform;

        private IEnumerator lerpSliderToValueCoroutine;
        private IEnumerator updateSongSliderProgressCoroutine;
        #endregion

        #region Properties
        public Transform SongTimeSliderCachedTransform => songTimeSliderCachedTransform;
        #endregion

        #region Public Methods
        public void LerpSliderToValue(float _endingValue, float _duration)
        {
            StopLerpSliderToValueCoroutine();
            lerpSliderToValueCoroutine = LerpSliderToValueCoroutine(_endingValue, _duration);
            StartCoroutine(lerpSliderToValueCoroutine);
        }

        public void StopLerpSliderToValueCoroutine()
        {
            if (lerpSliderToValueCoroutine != null)
            {
                StopCoroutine(lerpSliderToValueCoroutine);
            }
        }

        public virtual void UpdateSongSliderProgress()
        {
            StopSongSliderProgressCoroutine();
            updateSongSliderProgressCoroutine = UpdateSongSliderProgressCoroutine();
            StartCoroutine(updateSongSliderProgressCoroutine);
        }
        #endregion

        #region Private Methods
        private IEnumerator LerpSliderToValueCoroutine(float _endingValue, float _duration)
        {
            LeanTween.cancel(gameObject);
            LeanTween.value(gameObject, songTimeSlider.value, _endingValue, _duration).setOnUpdate((float _val) =>
            {
                songTimeSlider.value = _val;
            });

            yield return null;
        }

        private IEnumerator UpdateSongSliderProgressCoroutine()
        {
            while (audioManager.SongAudioSource.isPlaying == true)
            {
                if (audioManager.SongAudioSource.clip is null == true)
                {
                    currentTimePercentage = 0f;
                }
                else
                {
                    currentTimePercentage = (audioManager.SongAudioSource.time / audioManager.SongAudioSource.clip.length) * 100;
                }

                songTimeSlider.value = currentTimePercentage;
                yield return null;
            }
            yield return null;
        }

        private void StopSongSliderProgressCoroutine()
        {
            if (updateSongSliderProgressCoroutine != null)
            {
                StopCoroutine(updateSongSliderProgressCoroutine);
            }
        }
        #endregion
    }
}