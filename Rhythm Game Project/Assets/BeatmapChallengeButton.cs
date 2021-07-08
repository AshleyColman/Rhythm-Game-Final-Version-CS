namespace Menu
{
    using UnityEngine;
    using UnityEngine.UI;
    using Enums;
    using System.Collections;
    using TMPro;

    public sealed class BeatmapChallengeButton : MonoBehaviour
    {
        #region Constants
        private byte pointTextArrIndex = 1;
        #endregion

        #region Private Fields
        [SerializeField] private Image achievedColorImage = default;
        [SerializeField] private Image lockedColorImage = default;

        [SerializeField] protected TextMeshProUGUI[] textArr = default;

        [SerializeField] private string titleNotification = default;
        [SerializeField] private string notAchievedDescriptionNotification = default;
        [SerializeField] private string hasAchievedDescriptionNotification = default;

        [SerializeField] private FlashCanvasGroup flashCanvasGroup = default;

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
            flashCanvasGroup.PlayFlashAnimation();
        }

        public void Button_OnClick()
        {
            if (hasAchieved == true)
            {
                notification.DisplayDescriptionNotification(achievedColorImage.color, titleNotification, 
                    $"{hasAchievedDescriptionNotification} {textArr[pointTextArrIndex].text}", 4f);
            }
            else
            {
                notification.DisplayDescriptionNotification(ColorName.GREY, titleNotification,
                    $"{notAchievedDescriptionNotification} {textArr[pointTextArrIndex].text}", 4f);
            }

        }

        public void ShowText()
        {
            for (byte i = 0; i < textArr.Length; i++)
            {
                textArr[i].gameObject.SetActive(true);
            }
        }

        public void HideText()
        {
            for (byte i = 0; i < textArr.Length; i++)
            {
                textArr[i].gameObject.SetActive(false);
            }
        }

        public void SetPointText(string _pointValue, string _requirement)
        {
            textArr[pointTextArrIndex].SetText($"{_pointValue}/{_requirement}");
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            notification = FindObjectOfType<Notification>();
        }
        #endregion
    }
}