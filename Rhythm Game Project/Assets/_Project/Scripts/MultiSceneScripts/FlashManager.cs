namespace VisualEffects
{
    using UnityEngine;

    public sealed class FlashManager : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] private Material flashMaterial = default;
        #endregion

        #region Public Methods
        public void PlayFlashTween(float _valueTo)
        {
            flashMaterial.SetFloat("_Vibrancy", 0f);
            LeanTween.value(gameObject, 0f, _valueTo, 0.2f).setLoopPingPong(1).setOnUpdate((float val) =>
            {
                flashMaterial.SetFloat("_Vibrancy", val);
            });
        }
        #endregion
    }
}
