namespace Menu
{
    using UnityEngine;

    public sealed class OverlayCanvasManager : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] private Canvas overlayCanvas = default;

        private NavigationPanel navigationPanel;
        private ControlPanel controlPanel;
        private DescriptionPanel descriptionPanel;
        #endregion

        #region Public Methods
        public void TransitionIn()
        {
            overlayCanvas.gameObject.SetActive(true);
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            navigationPanel = FindObjectOfType<NavigationPanel>();
            controlPanel = FindObjectOfType<ControlPanel>();
            descriptionPanel = FindObjectOfType<DescriptionPanel>();
        }
        #endregion
    }
}