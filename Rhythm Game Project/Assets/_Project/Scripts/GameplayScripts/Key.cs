namespace Gameplay
{
    using UnityEngine;

    public sealed class Key : MonoBehaviour
    {
        #region Constants
        private const string OnKeyDownAnimation = "Key_OnKeyDown";
        private const string OnKeyReleaseAnimation = "Key_OnKeyRelease";
        private readonly Vector3 ScaleTo = new Vector3(1.25f, 1.25f, 1f);
        private readonly Vector3 EffectScaleTo = new Vector3(1.75f, 1.75f, 1f);
        #endregion

        #region Private Fields
        [SerializeField] private Transform keyTransform = default;
        [SerializeField] private Transform keyTextTransform = default;
        [SerializeField] private Transform keyEffectTextTransform = default;

        [SerializeField] private KeyCode keyCode = default;

        [SerializeField] private Animator keyAnimator = default;
        #endregion

        #region Properties
        public KeyCode KeyCode => keyCode;
        #endregion

        #region Public Methods
        public void PlayOnKeyAnimation()
        {
            keyAnimator.Play(OnKeyDownAnimation);
        }

        public void PlayOnKeyReleaseAnimation()
        {
            keyAnimator.Play(OnKeyReleaseAnimation);
        }

        public void PlayOnKeyDownAnimation()
        {
            LeanTween.cancel(keyTransform.gameObject);
            keyTransform.localScale = Vector3.one;

            LeanTween.cancel(keyTextTransform.gameObject);
            keyTextTransform.localScale = Vector3.one;

            LeanTween.cancel(keyEffectTextTransform.gameObject);
            keyEffectTextTransform.localScale = Vector3.one;

            LeanTween.scale(keyTransform.gameObject, ScaleTo, 0.5f).setEasePunch();
            LeanTween.scale(keyTextTransform.gameObject, ScaleTo, 0.5f).setEasePunch();
            LeanTween.scale(keyEffectTextTransform.gameObject, EffectScaleTo, 0.5f).setEasePunch();
        }
        #endregion
    }
}
