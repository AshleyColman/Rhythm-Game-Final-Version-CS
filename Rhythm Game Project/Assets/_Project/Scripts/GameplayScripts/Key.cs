namespace Gameplay
{
    using UnityEngine;

    public sealed class Key : MonoBehaviour
    {
        #region Constants
        private const string OnKeyDownAnimation = "Key_OnKeyDown";
        private const string OnKeyReleaseAnimation = "Key_OnKeyRelease";
        #endregion

        #region Private Fields
        [SerializeField] private KeyCode keyCode = default;

        [SerializeField] private Animator keyAnimator = default;
        #endregion

        #region Properties
        public KeyCode KeyCode => keyCode;
        #endregion

        #region Public Methods
        public void PlayOnKeyDownAnimation()
        {
            keyAnimator.Play(OnKeyDownAnimation);
        }

        public void PlayOnKeyReleaseAnimation()
        {
            keyAnimator.Play(OnKeyReleaseAnimation);
        }
        #endregion
    }
}
