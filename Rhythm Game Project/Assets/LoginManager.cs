namespace Settings
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using Menu;
    using System.Collections;

    public sealed class LoginManager : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] private GameObject loginPanel = default;

        private Transform loginPanelCachedTransform;

        [SerializeField] private TMP_InputField usernameInputField = default;
        [SerializeField] private TMP_InputField passwordInputField = default;

        [SerializeField] private TextMeshProUGUI usernameErrorText = default;
        [SerializeField] private TextMeshProUGUI passwordErrorText = default;

        [SerializeField] private Button submitButton = default;

        private MenuManager menuManager;
        #endregion

        #region Public Methods
        public void TransitionIn()
        {
            loginPanel.gameObject.SetActive(true);
            loginPanelCachedTransform.localScale = Vector3.zero;
            LeanTween.cancel(loginPanel);
            LeanTween.scale(loginPanel, Vector3.one, 1f).setEaseOutExpo();
        }

        public void TransitionOut()
        {
            loginPanel.gameObject.SetActive(false);
        }

        public void Click_SubmitButton()
        {
            Login();
        }

        public void Click_BackButton()
        {
            loginPanel.gameObject.SetActive(false);
            menuManager.TransitionLoginPanelToAccountPanel();
        }

        public void Validate_InputFieldLengths()
        {
            if (usernameInputField.text.Length > 0 &&
                passwordInputField.text.Length > 0)
            {
                submitButton.interactable = true;
            }
            else
            {
                submitButton.interactable = false;
            }
        }

        public void Validate_UsernameInputFieldLength()
        {
            if (usernameInputField.text.Length > 0)
            {
                if (usernameErrorText.gameObject.activeSelf == true)
                {
                    usernameErrorText.gameObject.SetActive(false);
                }
            }
            else
            {
                if (usernameErrorText.gameObject.activeSelf == false)
                {
                    usernameErrorText.gameObject.SetActive(true);
                }
            }
        }

        public void Validate_PasswordInputFieldLength()
        {
            if (passwordInputField.text.Length > 0)
            {
                if (passwordErrorText.gameObject.activeSelf == true)
                {
                    passwordErrorText.gameObject.SetActive(false);
                }
            }
            else
            {
                if (passwordErrorText.gameObject.activeSelf == false)
                {
                    passwordErrorText.gameObject.SetActive(true);
                }
            }
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            menuManager = FindObjectOfType<MenuManager>();
            loginPanelCachedTransform = loginPanel.transform;
        }

        private void Login()
        {
            loginPanel.gameObject.SetActive(false);
            menuManager.TransitionLoginPanelToModePanel();
        }
        #endregion
    }
}
