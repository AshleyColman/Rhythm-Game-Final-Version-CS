namespace Menu
{
    using Settings;
    using System.Collections;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public sealed class AccountPanel : MonoBehaviour
    {
        #region Constants
        private readonly string[] controlTextArr = new string[] { "MOVE CURSOR:", "NAVIGATE:", "SELECT:" };
        private readonly string[] keyTextArr = new string[] { "MOUSE", "ARROWS", "ENTER" };
        #endregion

        #region Private Fields
        [SerializeField] private GameObject accountPanel = default;
        [SerializeField] private GameObject optionPanel = default;

        [SerializeField] private Button signupButton = default;
        [SerializeField] private Button loginButton = default;

        [SerializeField] private Color validationFailColor = default;
        [SerializeField] private Color validationPassColor = default;

        private IEnumerator checkInputCoroutine;

        private Transform optionPanelTransform;

        private LoginManager loginManager;
        private SignupManager signupManager;
        private StartMenuManager startMenuManager;
        private ControlPanel controlPanel;
        private DescriptionPanel descriptionPanel;
        private TextPanel textPanel;
        #endregion

        #region Public Methods
        public void TransitionOut()
        {
            accountPanel.gameObject.SetActive(false);
            StopAllCoroutines();
        }

        public void TransitionIn()
        {
            accountPanel.gameObject.SetActive(true);
            CheckInput();
            TransitionInPanel(optionPanelTransform);
            TransitionInTextPanel();
            SelectFirstButton();
            startMenuManager.SetRhythmAndFadeTextWithTypingAnimation("Account", "signup, login or play as a guest");
            controlPanel.SetControlText(controlTextArr, keyTextArr);
            descriptionPanel.PlayDefaultTextArray();
        }

        public void SignupButton_OnClick()
        {
            TransitionOutOptionPanel();
            signupManager.TransitionIn();
        }

        public void LoginButton_OnClick()
        {
            TransitionOutOptionPanel();
            loginManager.TransitionIn();
        }

        public void SignupButton_OnSelect()
        {
            textPanel.TypeText("create a new account");
        }

        public void LoginButton_OnSelect()
        {
            textPanel.TypeText("log into an existing account");
        }

        public void GuestButton_OnSelect()
        {
            textPanel.TypeText("play as a guest without having to login");
        }

        public void SetValidationImageColorPass(Image _image)
        {
            if (_image.color != validationPassColor)
            {
                _image.color = validationPassColor;
            }
        }

        public void SetValidationImageColorFail(Image _image)
        {
            if (_image.color != validationFailColor)
            {
                _image.color = validationFailColor;
            }
        }

        public void SettextPanelText(string _error)
        {
            textPanel.TypeText(_error);
        }

        public void DisableSignupButton()
        {
            signupButton.interactable = false;
        }

        public void TransitionInTextPanel()
        {
            ReferenceTextPanel();
            textPanel.TransitionInPanel(new Vector3(0f, 580f, 0f), 467.5f);
        }

        public void TransitionInPanel(Transform _panelTransform)
        {
            Vector3 startPosition = new Vector3(0f, 580f, 0f);
            float endPositionY = 502.5f;

            LeanTween.cancel(_panelTransform.gameObject);
            _panelTransform.localPosition = startPosition;
            _panelTransform.gameObject.SetActive(true);
            LeanTween.moveLocalY(_panelTransform.gameObject, endPositionY, 1f).setEaseOutExpo();
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            loginManager = FindObjectOfType<LoginManager>();
            signupManager = FindObjectOfType<SignupManager>();
            startMenuManager = FindObjectOfType<StartMenuManager>();
            controlPanel = FindObjectOfType<ControlPanel>();
            descriptionPanel = FindObjectOfType<DescriptionPanel>();
            ReferenceTextPanel();

            optionPanelTransform = optionPanel.transform;
        }

        private void ReferenceTextPanel()
        {
            if (textPanel == null)
            {
                textPanel = FindObjectOfType<TextPanel>();
            }
        }

        private void TransitionOutOptionPanel()
        {
            optionPanel.gameObject.SetActive(false);
        }

        private void SelectFirstButton()
        {
            if (signupButton.interactable == false)
            {
                loginButton.Select();
            }
            else
            {
                signupButton.Select();
            }
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
            while (accountPanel.gameObject.activeSelf == true)
            {
                if (Input.anyKey)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        Application.Quit();
                    }
                }
                yield return null;
            }
            yield return null;
        }
        #endregion
    }
}