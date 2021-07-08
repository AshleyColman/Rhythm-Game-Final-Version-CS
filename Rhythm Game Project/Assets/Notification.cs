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
        private readonly Vector3 DefaultScale = new Vector3(1f, 0f, 1f); 
        #endregion

        #region Private Fields
        [SerializeField] private GameObject notification = default;
        [SerializeField] private GameObject titleNotificationPanel = default;
        [SerializeField] private GameObject descriptionNotificationPanel = default;

        [SerializeField] private Image colorImage = default;

        [SerializeField] private TextMeshProUGUI titleNotificationText = default;
        [SerializeField] private TextMeshProUGUI descriptionTitleNotificationText = default;
        [SerializeField] private TextMeshProUGUI descriptionNotificationText = default;

        [SerializeField] private FlashCanvasGroup flashCanvasGroup = default;

        private Transform notificationTransform;

        private IEnumerator displayTitleNotificationCoroutine;
        private IEnumerator transitionOutCoroutine;
        private IEnumerator playFlashCanvasGroupAnimation;

        private ColorCollection colorCollection;
        #endregion

        #region Public Methods
        public void DisplayTitleNotification(ColorName _colorName, string _text, float _duration)
        {
            if (displayTitleNotificationCoroutine != null)
            {
                StopCoroutine(displayTitleNotificationCoroutine);
            }

            displayTitleNotificationCoroutine = DisplayTitleNotificationCoroutine(_colorName, _text, _duration);
            StartCoroutine(displayTitleNotificationCoroutine);
        }

        public void DisplayDescriptionNotification(ColorName _colorName, string _titleText, string _descriptionText,
            float _duration)
        {
            if (displayTitleNotificationCoroutine != null)
            {
                StopCoroutine(displayTitleNotificationCoroutine);
            }

            displayTitleNotificationCoroutine = DisplayDescriptionNotificationCoroutine(_colorName, _titleText, _descriptionText,
                _duration);

            StartCoroutine(displayTitleNotificationCoroutine);
        }

        public void DisplayDescriptionNotification(ColorName _colorName, string _titleText, string _descriptionText,
            float _duration, Vector3 _position)
        {
            if (displayTitleNotificationCoroutine != null)
            {
                StopCoroutine(displayTitleNotificationCoroutine);
            }

            displayTitleNotificationCoroutine = DisplayDescriptionNotificationCoroutine(_colorName, _titleText, _descriptionText,
                _duration, _position);

            StartCoroutine(displayTitleNotificationCoroutine);
        }

        public void DisplayDescriptionNotification(Color32 _color, string _titleText, string _descriptionText,
            float _duration)
        {
            if (displayTitleNotificationCoroutine != null)
            {
                StopCoroutine(displayTitleNotificationCoroutine);
            }

            displayTitleNotificationCoroutine = DisplayDescriptionNotificationCoroutine(_color, _titleText, _descriptionText,
                _duration);

            StartCoroutine(displayTitleNotificationCoroutine);
        }

        public void StopTransitionOutCoroutine()
        {
            if (transitionOutCoroutine != null)
            {
                StopCoroutine(transitionOutCoroutine);
            }
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
                notificationTransform = notification.gameObject.transform;
            }
        }

        private IEnumerator DisplayTitleNotificationCoroutine(ColorName _colorName, string _text, float _duration)
        { 
            titleNotificationText.SetText(_text);
            colorImage.color = GetColor(_colorName);
            SetTitleNotificationActivity(true);
            SetDescriptionNotificationActivity(false);
            TransitionIn();
            yield return new WaitForSeconds(_duration);
            TransitionOut();
            yield return null;
        }

        private IEnumerator DisplayDescriptionNotificationCoroutine(ColorName _colorName, string _titleText, 
            string _descriptionText, float _duration)
        {
            descriptionTitleNotificationText.SetText(_titleText);
            descriptionNotificationText.SetText(_descriptionText);
            colorImage.color = GetColor(_colorName);
            SetTitleNotificationActivity(false);
            SetDescriptionNotificationActivity(true);
            TransitionIn();
            yield return new WaitForSeconds(_duration);
            TransitionOut();
            yield return null;
        }

        private IEnumerator DisplayDescriptionNotificationCoroutine(ColorName _colorName, string _titleText,
            string _descriptionText, float _duration, Vector3 _position)
        {
            notificationTransform.localPosition = _position;
            descriptionTitleNotificationText.SetText(_titleText);
            descriptionNotificationText.SetText(_descriptionText);
            colorImage.color = GetColor(_colorName);
            SetTitleNotificationActivity(false);
            SetDescriptionNotificationActivity(true);
            TransitionIn();
            yield return new WaitForSeconds(_duration);
            TransitionOut();
            yield return null;
        }

        private IEnumerator DisplayDescriptionNotificationCoroutine(Color _color, string _titleText,
            string _descriptionText, float _duration)
        {
            descriptionTitleNotificationText.SetText(_titleText);
            descriptionNotificationText.SetText(_descriptionText);
            colorImage.color = _color;
            SetTitleNotificationActivity(false);
            SetDescriptionNotificationActivity(true);
            TransitionIn();
            yield return new WaitForSeconds(_duration);
            TransitionOut();
            yield return null;
        }

        private Color32 GetColor(ColorName _colorName)
        {
            switch (_colorName)
            {
                case ColorName.RED:
                    return colorCollection.RedColor080;
                case ColorName.ORANGE:
                    return colorCollection.OrangeColor080;
                case ColorName.LIGHT_BLUE:
                    return colorCollection.LightBlueColor080;
                case ColorName.DARK_BLUE:
                    return colorCollection.DarkBlueColor080;
                case ColorName.PURPLE:
                    return colorCollection.PurpleColor080;
                case ColorName.PINK:
                    return colorCollection.PinkColor080;
                case ColorName.YELLOW:
                    return colorCollection.YellowColor080;
                case ColorName.LIGHT_GREEN:
                    return colorCollection.LightGreenColor080;
                case ColorName.GREY:
                    return colorCollection.GreyColor05;
                default:
                    return colorCollection.WhiteColor080;
            }
        }

        private void SetTitleNotificationActivity(bool _active)
        {
            if (titleNotificationPanel.activeSelf != _active)
            {
                titleNotificationPanel.SetActive(_active);
            }
        }

        private void SetDescriptionNotificationActivity(bool _active)
        {
            if (descriptionNotificationPanel.activeSelf != _active)
            {
                descriptionNotificationPanel.SetActive(_active);
            }
        }

        private void TransitionIn()
        {
            notificationTransform.localScale = DefaultScale;
            LeanTween.cancel(notification);
            notification.gameObject.SetActive(true);
            LeanTween.scaleY(notification, 1f, 0.1f);
            PlayFlashCanvasGroupAnimation();
        }

        private void TransitionOut()
        {
            StopTransitionOutCoroutine();

            transitionOutCoroutine = TransitionOutCoroutine();
            StartCoroutine(transitionOutCoroutine);
        }

        private IEnumerator TransitionOutCoroutine()
        {
            LeanTween.cancel(notification);
            LeanTween.scaleY(notification, 0f, 0.1f);
            yield return new WaitForSeconds(0.1f);
            notification.gameObject.SetActive(false);
            notificationTransform.localPosition = Vector3.zero;
            yield return null;
        }

        private void PlayFlashCanvasGroupAnimation()
        {
            flashCanvasGroup.PlayFlashAnimation();
        }
        #endregion
    }
}