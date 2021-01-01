namespace Menu
{
    using UnityEngine;

    public sealed class QuickplayMenuManager : MonoBehaviour, IMenu
    {
        #region Private Fields
        [SerializeField] private GameObject quickplayScreen = default;

        private TopCanvasManager topCanvasManager;
        private MenuManager menuManager;
        #endregion

        #region Public Methods
        public void TransitionIn()
        {
            quickplayScreen.gameObject.SetActive(true);
            topCanvasManager.Button_Click(MenuManager.QuickplayMenuIndex);
        }

        public void TransitionOut()
        {
            quickplayScreen.gameObject.SetActive(false);
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            topCanvasManager = FindObjectOfType<TopCanvasManager>();
            menuManager = FindObjectOfType<MenuManager>();
        }
        #endregion
    }
}