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

        #region Public Methods
        public void SetBackgroundImage(Texture _imageTexture)
        {
            backgroundImage.material.mainTexture = _imageTexture;
        }

        public void ActivateSongSlider()
        {
            songSlider.UpdateSongSliderProgress();
        }
        #endregion
    }
}
