namespace Menu
{
    using UnityEngine;
    using UnityEngine.UI;
    using Enums;
    using System.Collections;

    public sealed class BeatmapChallengeButton : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] private Image achievedColorImage = default;
        [SerializeField] private Image lockedColorImage = default;

        [SerializeField] private CanvasGroup flashCanvasGroup = default;

        [SerializeField] private string notAchievedNotification = default;
        [SerializeField] private string hasAchievedNotification = default;

        private IEnumerator playFlashCanvasGroupAnimation;

        private Color32 achievedNotificationColor;

        private bool hasAchieved = false;

        private Notification notification;
        #endregion

        #region Properties
        public bool HasAchieved => hasAchieved;
        #endregion

        #region Public Methods
        public void SetNotAchieved()
        {
            hasAchieved = false;

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
            hasAchieved = true;

            if (achievedColorImage.gameObject.activeSelf == false)
            {
                achievedColorImage.gameObject.SetActive(true);
            }

            if (lockedColorImage.gameObject.activeSelf == true)
            {
                lockedColorImage.gameObject.SetActive(false);
            }
        }

        public void PlayFlashCanvasGroupAnimation()
        {
            if (playFlashCanvasGroupAnimation != null)
            {
                StopCoroutine(playFlashCanvasGroupAnimation);
            }

            playFlashCanvasGroupAnimation = PlayFlashCanvasGroupAnimationCoroutine();
            StartCoroutine(playFlashCanvasGroupAnimation);
        }


        public void Button_OnClick()
        {
            if (hasAchieved == true)
            {
                notification.DisplayNotification(achievedNotificationColor, hasAchievedNotification, 4f);
            }
            else
            {
                notification.DisplayNotification(lockedColorImage.color, notAchievedNotification, 4f);
            }

        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            notification = FindObjectOfType<Notification>();
            SetNotificationColor();
        }

        private void SetNotificationColor()
        {
            achievedNotificationColor = new Color(achievedColorImage.color.r, achievedColorImage.color.g,
                achievedColorImage.color.b, 0.8f);
        }

        private IEnumerator PlayFlashCanvasGroupAnimationCoroutine()
        {
            flashCanvasGroup.alpha = 0f;
            LeanTween.cancel(flashCanvasGroup.gameObject);
            flashCanvasGroup.gameObject.SetActive(true);

            LeanTween.alphaCanvas(flashCanvasGroup, 1f, 1f).setEasePunch();

            yield return new WaitForSeconds(1f);

            flashCanvasGroup.gameObject.SetActive(false);

            yield return null;
        }
        #endregion
    }
}