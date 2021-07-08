namespace Settings
{
    using UnityEngine;
    using TMPro;
    using UnityEngine.UI;
    using Menu;
    using System.Collections;
    using UnityEngine.Networking;
    using Enums;
    using Loading;

    public sealed class SignupManager : MonoBehaviour
    {
        #region Constants
        private const byte MinimumUsernameInputFieldLength = 4;
        private const byte MinimumPasswordInputFieldLength = 8;

        private const string FieldUsername = "username";
        private const string FieldPassword = "password";
        private const string SignupUrl = "http://localhost/RhythmGameOnlineScripts/SignupScript.php";
        private const string ConnectError = "connectError";
        private const string UsernameExistError = "userExistError";
        private const string UsernameCheckQueryError = "usernameCheckQueryError";
        private const string UsernameInsertQueryError = "usernameInsertQueryError";
        private const string Success = "success";
        private readonly string[] controlTextArr = new string[] { "MOVE CURSOR:", "NAVIGATE:", "SELECT:",
            "type", "next field"};
        private readonly string[] keyTextArr = new string[] { "MOUSE", "ARROWS", "ENTER", "CHARARACTER", "ENTER" };
        #endregion

        #region Private Fields
        [SerializeField] private GameObject signupPanel = default;

        [SerializeField] private TMP_InputField usernameInputField = default;
        [SerializeField] private TMP_InputField passwordInputField = default;
        [SerializeField] private TMP_InputField passwordConfirmationInputField = default;

        [SerializeField] private Image usernameValidationColorImage = default;
        [SerializeField] private Image passwordValidationColorImage = default;
        [SerializeField] private Image passwordConfirmationValidationColorImage = default;

        [SerializeField] private Button submitButton = default;

        [SerializeField] private LoadingIcon loadingIcon = default;

        private Transform signupPanelTransform;

        private IEnumerator checkInputCoroutine;
        private IEnumerator signupCoroutine;

        private AccountPanel accountPanel;
        private LoginManager loginManager;
        private StartMenuManager startMenuManager;
        private Notification notification;
        private ControlPanel controlPanel;
        private TextPanel textPanel;
        #endregion

        #region Public Methods
        public void TransitionIn()
        {
            accountPanel.TransitionInPanel(signupPanelTransform);
            CheckInput();
            textPanel.TransitionInPanel(new Vector3(0f, 580f, 0f), 467.5f);
            startMenuManager.SetRhythmAndFadeTextWithTypingAnimation("signup", "create a new account");
            usernameInputField.Select();
            controlPanel.SetControlText(controlTextArr, keyTextArr);
        }

        public void TransitionOutToAccountPanel()
        {
            TransitionOut();
            accountPanel.TransitionIn();
        }

        public void TransitionOutToLoginPanel()
        {
            TransitionOut();
            loginManager.TransitionIn();
        }

        public void ValidateUsername()
        {
            if (usernameInputField.text.Length >= MinimumUsernameInputFieldLength)
            {
                accountPanel.SetValidationImageColorPass(usernameValidationColorImage);
            }
            else
            {
                accountPanel.SetValidationImageColorFail(usernameValidationColorImage);
            }
        }

        public void ValidatePassword()
        {
            if (passwordInputField.text.Length >= MinimumPasswordInputFieldLength)
            {
                accountPanel.SetValidationImageColorPass(passwordValidationColorImage);
            }
            else
            {
                accountPanel.SetValidationImageColorFail(passwordValidationColorImage);
            }
        }

        public void ValidateConfirmationPassword()
        {
            if (passwordInputField.text != string.Empty && passwordConfirmationInputField.text != string.Empty && 
                passwordInputField.text == passwordConfirmationInputField.text)
            {
                accountPanel.SetValidationImageColorPass(passwordConfirmationValidationColorImage);
            }
            else
            {
                accountPanel.SetValidationImageColorFail(passwordConfirmationValidationColorImage);
            }
        }

        public void ValidateSubmitButton()
        {
            if (usernameInputField.text.Length >= MinimumUsernameInputFieldLength &&
                passwordInputField.text.Length >= MinimumUsernameInputFieldLength &&
                passwordConfirmationInputField.text == passwordInputField.text)
            {
                submitButton.interactable = true;
            }
            else
            {
                submitButton.interactable = false;
            }
        }

        public void OnSelectUsernameInputField()
        {
            ValidateUsername();
            textPanel.TypeText("username must have at least 4 characters");
        }

        public void OnSelectPasswordInputField()
        {
            ValidatePassword();
            textPanel.TypeText("password must have at least 8 characters");
        }

        public void OnSelectPasswordConfirmationInputField()
        {
            ValidateConfirmationPassword();
            textPanel.TypeText("both passwords must match");
        }

        public void OnEndEditUsernameInputField()
        {
            UtilityMethods.SelectNextSelectable(usernameInputField);
        }

        public void OnEndEditPasswordInputField()
        {
            UtilityMethods.SelectNextSelectable(passwordInputField);
        }

        public void OnEndEditPasswordConfirmationInputField()
        {
            UtilityMethods.SelectNextSelectable(passwordConfirmationInputField);
        }

        public void SubmitButton_OnClick()
        {
            Signup();
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            accountPanel = FindObjectOfType<AccountPanel>();
            loginManager = FindObjectOfType<LoginManager>();
            startMenuManager = FindObjectOfType<StartMenuManager>();
            notification = FindObjectOfType<Notification>();
            controlPanel = FindObjectOfType<ControlPanel>();
            textPanel = FindObjectOfType<TextPanel>();

            signupPanelTransform = signupPanel.transform;
        }

        private void TransitionOut()
        {
            signupPanel.gameObject.SetActive(false);
            StopAllCoroutines();
        }

        private void CheckInput()
        {
            if (checkInputCoroutine != null)
            {
                StopCoroutine(checkInputCoroutine);
            }

            checkInputCoroutine = CheckInputCoroutine();
            StartCoroutine(checkInputCoroutine);
        }

        private IEnumerator CheckInputCoroutine()
        {
            while (signupPanel.gameObject.activeSelf == true)
            {
                if (Input.anyKey)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        TransitionOutToAccountPanel();
                    }
                }
                yield return null;
            }
            yield return null;
        }

        private void Signup()
        {
            if (signupCoroutine != null)
            {
                StopCoroutine(signupCoroutine);
            }

            signupCoroutine = SignupCoroutine();
            StartCoroutine(signupCoroutine);
        }

        private IEnumerator SignupCoroutine()
        {
            loadingIcon.DisplayLoadingIcon();
            submitButton.interactable = false;

            WWWForm form = new WWWForm();

            form.AddField(FieldUsername, usernameInputField.text);
            form.AddField(FieldPassword, passwordInputField.text);

            UnityWebRequest www = UnityWebRequest.Post(SignupUrl, form);

            yield return www.SendWebRequest();

            loadingIcon.HideLoadingIcon();

            if (www.downloadHandler.text == Success)
            {
                submitButton.interactable = false;
                accountPanel.DisableSignupButton();
                notification.DisplayDescriptionNotification(ColorName.LIGHT_GREEN, "Account created", "you can now log in", 4f);
                yield return new WaitForSeconds(4f);
                TransitionOutToLoginPanel();
            }
            else if (www.isNetworkError)
            {
                notification.DisplayDescriptionNotification(ColorName.RED, "network error", "unable to complete signup, " +
                    "please contact the developer", 8f);
                submitButton.interactable = true;
            }
            else if (www.isHttpError)
            {
                notification.DisplayDescriptionNotification(ColorName.RED, "HTTP error", "unable to complete signup, " +
                    "please contact the developer", 8f);
                submitButton.interactable = true;
            }
            else if (www.downloadHandler.text == ConnectError)
            {
                notification.DisplayDescriptionNotification(ColorName.RED, "connection error", "unable to complete signup, " +
                    "please check your internet or contact the developer", 8f);
                submitButton.interactable = true;
            }
            else if (www.downloadHandler.text == UsernameCheckQueryError)
            {
                notification.DisplayDescriptionNotification(ColorName.RED, "username check error", "unable to complete signup, " +
                    "please contact the developer", 8f);
                submitButton.interactable = true;
            }
            else if (www.downloadHandler.text == UsernameExistError)
            {
                notification.DisplayDescriptionNotification(ColorName.RED, "user already exists error", "unable to complete signup " +
                    "the username you entered already exists", 8f);
                submitButton.interactable = true;
            }
            else if (www.downloadHandler.text == UsernameInsertQueryError)
            {
                notification.DisplayDescriptionNotification(ColorName.RED, "user insert error", "unable to complete signup, " +
                    "please contact the developer", 8f);
                submitButton.interactable = true;
            }
        }
        #endregion
    }
}
