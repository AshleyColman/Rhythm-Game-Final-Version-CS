namespace Menu
{
    using UnityEngine;
    using UnityEngine.UI;

    public sealed class BeatmapChallengeButton : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] private Image achievedColorImage = default;
        [SerializeField] private Image lockedColorImage = default;
        #endregion

        #region Public Methods
        public void SetNotAchieved()
        {
            if (achievedColorImage.gameObject.activeSelf == true)
            {
                achievedColorImage.gameObject.SetActive(false);
            }

            if (lockedColorImage.gameObject.activeSelf == false)
            {
                lockedColorImage.gameObject.SetActive(true);
            }
        }

        public void SetAchieved()
        {
            if (achievedColorImage.gameObject.activeSelf == false)
            {
                achievedColorImage.gameObject.SetActive(true);
            }

            if (lockedColorImage.gameObject.activeSelf == true)
            {
                lockedColorImage.gameObject.SetActive(false);
            }
        }
        #endregion

        #region Private Methods

        #endregion
    }

}