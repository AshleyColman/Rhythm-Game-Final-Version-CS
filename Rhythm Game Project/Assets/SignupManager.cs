namespace Settings
{
    using UnityEngine;
    using TMPro;
    using UnityEngine.UI;
    using Menu;
    using System.Collections;

    public sealed class SignupManager : MonoBehaviour
    {
        #region Constants
        private byte RequiredUsernameInputFieldLength = 5;
        private byte RequiredPasswordInputFieldLength = 8;
        #endregion

        #region Private Fields
        [SerializeField] private GameObject signupPanel = default;

        private Transform signupPanelCachedTransform = default;

        [SerializeField] private TMP_InputField usernameInputField = default;
        [SerializeField] private TMP_InputField passwordInputField = default;

        [SerializeField] private TMP_Dropdown genderDropdown = default;

        [SerializeField] private TextMeshProUGUI usernameErrorText = default;
        [SerializeField] private TextMeshProUGUI passwordErrorText = default;
        [SerializeField] private TextMeshProUGUI genderErrorText = default;

        [SerializeField] private Button submitButton = default;

        private MenuManager menuManager;
        #endregion

        #region Public Methods
        public void TransitionIn()
        {
            signupPanel.gameObject.SetActive(true);
            signupPanelCachedTransform.localScale = Vector3.zero;
            LeanTween.cancel(signupPanel);
            LeanTween.scale(signupPanel, Vector3.one, 1f).setEaseOutExpo();
        }

        public void TransitionOut()
        {
            signupPanel.gameObject.SetActive(false);
        }

        public void Validate_InputFieldLengths()
        {
            if (usernameInputField.text.Length >= RequiredUsernameInputFieldLength &&
                passwordInputField.text.Length >= RequiredPasswordInputFieldLength)
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
            if (usernameInputField.text.Length >= RequiredUsernameInputFieldLength)
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
            if (passwordInputField.text.Length >= RequiredPasswordInputFieldLength)
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

        public void Validate_GenderDropdown()
        {
            if (genderDropdown.value != 0)
            {
                if (genderErrorText.gameObject.activeSelf == true)
                {
                    genderErrorText.gameObject.SetActive(false);
                }
            }
            else
            {
                if (genderErrorText.gameObject.activeSelf == false)
                {
                    genderErrorText.gameObject.SetActive(true);
                }
            }
        }

        public void Validate_CheckSubmitButton()
        {
            if (usernameInputField.text.Length >= RequiredUsernameInputFieldLength &&
                passwordInputField.text.Length >= RequiredPasswordInputFieldLength &&
                genderDropdown.value != 0)
            {
                submitButton.interactable = true;
            }
            else
            {
                submitButton.interactable = false;
            }
        }

        public void Click_SubmitButton()
        {
            signupPanel.gameObject.SetActive(false);
            menuManager.TransitionSignupPanelToLoginPanel();
        }

        public void Click_BackButton()
        {
            signupPanel.gameObject.SetActive(false);
            menuManager.TransitionSignupPanelToAccountPanel();
        }
        #endregion

        #region Private Fields
        private void Awake()
        {
            menuManager = FindObjectOfType<MenuManager>();
            signupPanelCachedTransform = signupPanel.transform;
        }
        #endregion
    }
}
