namespace Menu
{
    using Settings;
    using UnityEngine;

    public sealed class AccountPanel : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] private GameObject accountPanel = default;
        [SerializeField] private GameObject accountOptionsPanel = default;

        private Transform accountPanelCachedTransform;
        private Transform accountOptionsPanelCachedTransform;

        private MenuManager menuManager;
        #endregion

        #region Public Methods
        public void TransitionIn()
        {
            accountPanel.gameObject.SetActive(true);
            TransitionInAccountOptionsPanel();
        }

        public void TransitionOut()
        {
            accountPanel.gameObject.SetActive(false);
        }

        public void Click_SignupButton()
        {
             menuManager.TransitionAccountPanelToSignupPanel();
        }

        public void Click_LoginButton()
        {
            menuManager.TransitionAccountPanelToLoginPanel();
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            menuManager = FindObjectOfType<MenuManager>();
            accountPanelCachedTransform = accountPanel.transform;
            accountOptionsPanelCachedTransform = accountOptionsPanel.transform;
        }

        public void TransitionInAccountOptionsPanel()
        {
            accountOptionsPanel.gameObject.SetActive(true);
            accountOptionsPanelCachedTransform.localScale = Vector3.zero;
            LeanTween.cancel(accountOptionsPanel);
            LeanTween.scale(accountOptionsPanel, Vector3.one, 1f).setEaseOutExpo();
        }

        public void TransitionOutAccountOptionsPanel()
        {
            accountOptionsPanel.gameObject.SetActive(false);
        }
        #endregion
    }
}