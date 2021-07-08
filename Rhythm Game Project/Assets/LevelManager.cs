namespace Level
{
    using UnityEngine;
    using UnityEngine.UI;

    public sealed class LevelManager : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] private Gradient gradient = default;
        #endregion

        #region Public Methods
        public void SetLevelSliderGradientColor(Slider _slider)
        {
            _slider.image.color = gradient.Evaluate(_slider.normalizedValue);
        }
        #endregion

        #region Private Methods

        #endregion
    }

}