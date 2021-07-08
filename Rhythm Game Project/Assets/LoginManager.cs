namespace Settings
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using Menu;
    using System.Collections;
    using Loading;
    using UnityEngine.Networking;
    using Enums;
    using Player;

    public sealed class LoginManager : MonoBehaviour
    {
        #region Constants
        private const string FieldUsername = "username";
        private const string FieldPassword = "password";
        private const string LoginUrl = "http://localhost/RhythmGameOnlineScripts/LoginScript.php";

        private const string ConnectError = "connectError";
        private const string UsernameCheckQueryError = "usernameCheckQueryError";
        private const string UsernameDoesNotExistError = "usernameDoesNotExistError";
        private const string WrongPasswordError = "wrongPasswordError";
        private const string Success = "success";
        #endregion

        #region Private Fields
        [SerializeField] private GameObject loginPanel = default;

        [SerializeField] private TMP_InputField usernameInputField = default;
        [SerializeField] private TMP_InputField passwordInputField = default;

        [SerializeField] private Image usernameValidationColorImage = default;
        [SerializeField] private Image passwordValidationColorImage = default;

        [SerializeField] private Button submitButton = default;

        [SerializeField] private LoadingIcon loadingIcon = default;

        private Transform loginPanelTransform;

        private IEnumerator escapeInputCheck;
        private IEnumerator login;

        private AccountPanel accountPanel;
        private StartMenuManager startMenuManager;
        private Notification notification;
        private Player player;
        private ModePanel modePanel;
        private TextPanel textPanel;
        #endregion

        #region Public Methods
        public void TransitionIn()
        {
            accountPanel.TransitionInPanel(loginPanelTransform);
            textPanel.TransitionInPanel(new Vector3(0f, 580f, 0f), 467.5f);
            startMenuManager.SetRhythmAndFadeTextWithTypingAnimation("login", "log into an existing account");
            usernameInputField.Select();
            EscapeInputCheck();
        }

        public void TransitionOutToAccountPanel()
        {
            StopAllCoroutines();
            loginPanel.gameObject.SetActive(false);
            accountPanel.TransitionIn();
        }

        public void TransitionOutToModePanel()
        {
            StopAllCoroutines();
            startMenuManager.TransitionOutStartPanel();
            modePanel.TransitionIn();
        }

        public void SubmitButton_OnClick()
        {
            Login();
        }

        public void ValidateUsername()
        {
            if (usernameInputField.text.Length > 0)
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
            if (passwordInputField.text.Length > 0)
            {
                accountPanel.SetValidationImageColorPass(passwordValidationColorImage);
            }
            else
            {
                accountPanel.SetValidationImageColorFail(passwordValidationColorImage);
            }
        }

        public void ValidateSubmitButton()
        {
            if (usernameInputField.text.Length > 0 && passwordInputField.text.Length > 0)
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
            textPanel.TypeText("enter your username");
        }

        public void OnSelectPasswordInputField()
        {
            ValidatePassword();
            textPanel.TypeText("enter your password");
        }

        public void OnEndEditUsernameInputField()
        {
            UtilityMethods.SelectNextSelectable(usernameInputField);
        }

        public void OnEndEditPasswordInputField()
        {
            UtilityMethods.SelectNextSelectable(passwordInputField);
        }

        public void LoginAsGuest()
        {
            player.LoginWithAccount("GUEST");
            TransitionOutToModePanel();
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            accountPanel = FindObjectOfType<AccountPanel>();
            startMenuManager = FindObjectOfType<StartMenuManager>();
            notification = FindObjectOfType<Notification>();
            player = FindObjectOfType<Player>();
            modePanel = FindObjectOfType<ModePanel>();
            textPanel = FindObjectOfType<TextPanel>();

            loginPanelTransform = loginPanel.transform;
        }

        private void EscapeInputCheck()
        {
            if (escapeInputCheck != null)
            {
                StopCoroutine(escapeInputCheck);
            }

            escapeInputCheck = EscapeInputCheckCoroutine();
            StartCoroutine(escapeInputCheck);
        }

        private IEnumerator EscapeInputCheckCoroutine()
        {
            while (loginPanel.gameObject.activeSelf == true)
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

        private void Login()
        {
            if (login != null)
            {
                StopCoroutine(login);
            }

            login = LoginCoroutine();
            StartCoroutine(login);
        }

        private IEnumerator LoginCoroutine()
        {
            loadingIcon.DisplayLoadingIcon();
            submitButton.interactable = false;

            WWWForm form = new WWWForm();

            form.AddField(FieldUsername, usernameInputField.text);
            form.AddField(FieldPassword, passwordInputField.text);

            UnityWebRequest www = UnityWebRequest.Post(LoginUrl, form);

            yield return www.SendWebRequest();

            loadingIcon.HideLoadingIcon();

            if (www.downloadHandler.text == Success)
            {
                notification.DisplayDescriptionNotification(ColorName.LIGHT_GREEN, "logged in", $"playing as " +
                    $"{usernameInputField.text}", 4f, VectorConstants.VectorPositionY356);

                player.LoginWithAccount(usernameInputField.text);

                yield return new WaitForSeconds(4f);

                TransitionOutToModePanel();
            }
            else if (www.isNetworkError)
            {
                notification.DisplayDescriptionNotification(ColorName.RED, "network error", "unable to login, " +
                    "please contact the developer", 8f, VectorConstants.VectorPositionY356);
            }
            else if (www.isHttpError)
            {
                notification.DisplayDescriptionNotification(ColorName.RED, "HTTP error", "unable to login, " +
                    "please contact the developer", 8f, VectorConstants.VectorPositionY356);
            }
            else if (www.downloadHandler.text == ConnectError)
            {
                notification.DisplayDescriptionNotification(ColorName.RED, "connection error", "unable to login, " +
                    "please contact the developer", 4f, VectorConstants.VectorPositionY356);
            }
            else if (www.downloadHandler.text == UsernameCheckQueryError)
            {
                notification.DisplayDescriptionNotification(ColorName.RED, "username check error", "unable to login, " +
                    "please contact the developer", 8f, VectorConstants.VectorPositionY356);
            }
            else if (www.downloadHandler.text == UsernameDoesNotExistError)
            {
                notification.DisplayDescriptionNotification(ColorName.RED, "user does not exist error", "unable to login, " +
                    "the username you entered does not exist", 8f, VectorConstants.VectorPositionY356);
            }
            else if (www.downloadHandler.text == WrongPasswordError)
            {
                notification.DisplayDescriptionNotification(ColorName.RED, "wrong password error", "unable to login, " +
                    "the password entered is wrong", 8f, VectorConstants.VectorPositionY356);
            }

            yield return null;
        }
        #endregion
    }
}
