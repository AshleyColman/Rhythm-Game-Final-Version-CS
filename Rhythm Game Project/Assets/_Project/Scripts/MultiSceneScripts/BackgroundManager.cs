namespace Background
{
    using UnityEngine;
    using UnityEngine.UI;

    public sealed class BackgroundManager : MonoBehaviour
    {
        #region Constants
        private const int PositionLeftX = -10;
        private const int PositionRightX = 10;

        private readonly Vector3 scaleTo = new Vector3(1.01f, 1.01f, 1f);
        #endregion

        #region Private Fields
        [SerializeField] private Image backgroundImage = default;

        private Transform backgroundImageCachedTransform = default;
        #endregion

        #region Public Methods
        public void PlayRhythmScaleTween()
        {
            LeanTween.cancel(backgroundImage.gameObject);
            backgroundImageCachedTransform.localScale = Vector3.one;
            LeanTween.scale(backgroundImage.gameObject, scaleTo, 1f).setEasePunch();
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            backgroundImageCachedTransform = backgroundImage.transform;
        }
        #endregion
    }
}
