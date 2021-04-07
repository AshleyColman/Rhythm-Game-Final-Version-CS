namespace Menu
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using Enums;

    public sealed class Notification : MonoBehaviour
    {
        #region Constants
        private readonly Vector3 defaultScale = new Vector3(1f, 0f, 1f); 
        #endregion

        #region Private Fields
        [SerializeField] private GameObject notification = default;

        private Transform notificationCachedTransform;

        [SerializeField] private Image colorImage = default;

        [SerializeField] private TextMeshProUGUI text = default;

        private IEnumerator displayNotificationCoroutine;
        private IEnumerator transitionOutCoroutine;

        private ColorCollection colorCollection;
        #endregion

        #region Public Methods
        public void DisplayNotification(NotificationType _notificationType, string _text, float _duration)
        {
            if (displayNotificationCoroutine != null)
            {
                StopCoroutine(displayNotificationCoroutine);
            }

            displayNotificationCoroutine = DisplayNotificationCoroutine(_notificationType, _text, _duration);
            StartCoroutine(displayNotificationCoroutine);
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            TryReference();
        }

        private void TryReference()
        {
            if (colorCollection is null == true)
            {
                colorCollection = FindObjectOfType<ColorCollection>();
                notificationCachedTransform = notification.gameObject.transform;
            }
        }

        private IEnumerator DisplayNotificationCoroutine(NotificationType _notificationType, string _text, float _duration)
        {
            text.SetText(_text);
            SetNotificationColor(_notificationType);
            TransitionIn();
            yield return new WaitForSeconds(_duration);
            TransitionOut();
            yield return null;
        }

        private void SetNotificationColor(NotificationType _notificationType)
        {
            TryReference();

            switch (_notificationType)
            {
                case NotificationType.Error:
                    colorImage.color = colorCollection.RedColor080;
                    break;
                case NotificationType.General:
                    colorImage.color = colorCollection.LightBlueColor080;
                    break;
                case NotificationType.TwoKey:
                    colorImage.color = colorCollection.DarkBlueColor080;
                    break;
                case NotificationType.FourKey:
                    colorImage.color = colorCollection.PurpleColor080;
                    break;
                case NotificationType.SixKey:
                    colorImage.color = colorCollection.RedColor080;
                    break;
                default:
                    colorImage.color = colorCollection.WhiteColor080;
                    break;
            }
        }

        private void TransitionIn()
        {
            notificationCachedTransform.localScale = defaultScale;
            notification.gameObject.SetActive(true);
            LeanTween.cancel(notification);
            LeanTween.scaleY(notification, 1f, 0.1f);
        }

        private void TransitionOut()
        {
            if (transitionOutCoroutine != null)
            {
                StopCoroutine(transitionOutCoroutine);
            }

            transitionOutCoroutine = TransitionOutCoroutine();
            StartCoroutine(transitionOutCoroutine);
        }

        private IEnumerator TransitionOutCoroutine()
        {
            LeanTween.cancel(notification);
            LeanTween.scaleY(notification, 0f, 0.1f);
            yield return new WaitForSeconds(0.1f);
            notification.gameObject.SetActive(false);
            yield return null;
        }
        #endregion
    }
}