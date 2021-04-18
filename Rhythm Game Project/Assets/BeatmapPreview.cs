namespace Menu
{
    using UnityEngine;
    using UnityEngine.UI;
    using Audio;

    public sealed class BeatmapPreview : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] private Image backgroundImage = default;

        [SerializeField] private SongSliderProgressText songSlider = default;
        #endregion

        #region Properties
        public SongSliderProgressText SongSlider => songSlider;
        #endregion

        #region Public Methods
        public void SetBackgroundImage(Texture _imageTexture)
        {
            backgroundImage.gameObject.SetActive(false);
            backgroundImage.material.mainTexture = _imageTexture;
            backgroundImage.gameObject.SetActive(true);
        }

        public void ActivateSongSlider()
        {
            songSlider.UpdateSongSliderProgress();
        }
        #endregion
    }
}
