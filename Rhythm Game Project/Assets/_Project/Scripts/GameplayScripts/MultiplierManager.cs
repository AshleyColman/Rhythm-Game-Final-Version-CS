namespace Gameplay
{
    using UnityEngine;
    using TMPro;
    using System.Text;
    using System.Collections;

    public sealed class MultiplierManager : MonoBehaviour
    {
        #region Constants
        private const byte DefaultMultiplier = 1;

        private readonly Vector3 MultiplierScaleTo = new Vector3(1.25f, 1.25f, 1f);
        private readonly Vector3 MultiplierEffectScaleTo = new Vector3(1.75f, 1.75f, 1f);
        #endregion

        #region Private Fields
        [SerializeField] private GameObject multiplierContainer = default;

        [SerializeField] private TextMeshProUGUI multiplierText = default;
        [SerializeField] private TextMeshProUGUI multiplierEffectText = default;

        [SerializeField] private Transform multiplierTextCachedTransform = default;
        [SerializeField] private Transform multiplierEffectTextCachedTransform = default;

        [SerializeField] private TMP_ColorGradient multiplierGradientX1 = default;
        [SerializeField] private TMP_ColorGradient multiplierGradientX2 = default;
        [SerializeField] private TMP_ColorGradient multiplierGradientX3 = default;
        [SerializeField] private TMP_ColorGradient multiplierGradientX4 = default;

        private byte multiplier = 1;
        private byte highestMultiplier = 0;

        private IEnumerator playDeactivateTweenCoroutine;
        #endregion

        #region Properties
        public byte Multiplier => multiplier;
        public TMP_ColorGradient MultiplierTextColorGradient => multiplierText.colorGradientPreset;
        #endregion

        #region Public Methods
        public void ResetMultiplier()
        {
            multiplier = 1;
        }

        public void IncrementMultiplier()
        {
            multiplier++;
        }

        public void ActivateFeverMultiplier()
        {
            DoubleMultiplier();
            multiplierText.SetText($"x{multiplier}");
            multiplierEffectText.SetText(multiplier.ToString());
            SetMultiplierTextColorGradient();
            PlayActivatedTween();
            multiplierContainer.gameObject.SetActive(true);
            CheckIfHighest();
        }

        public void DeactivateFeverMultiplier()
        {
            multiplier = DefaultMultiplier;
            PlayDeactivatedTween();
        }
        #endregion

        #region Private Methods
        private void CheckIfHighest()
        {
            if (multiplier > highestMultiplier)
            {
                highestMultiplier = multiplier;
            }
        }

        private void PlayActivatedTween()
        {
            LeanTween.cancel(multiplierText.gameObject);
            LeanTween.cancel(multiplierEffectText.gameObject);
            multiplierTextCachedTransform.localScale = Vector3.one;
            multiplierEffectTextCachedTransform.localScale = Vector3.one;

            LeanTween.scale(multiplierText.gameObject, MultiplierScaleTo, 1f).setEasePunch();
            LeanTween.scale(multiplierEffectText.gameObject, MultiplierEffectScaleTo, 1f).setEasePunch();
        }

        private void PlayDeactivatedTween()
        {
            if (playDeactivateTweenCoroutine != null)
            {
                StopCoroutine(playDeactivateTweenCoroutine);
            }

            playDeactivateTweenCoroutine = PlayDeactivateTweenCoroutine();
            StartCoroutine(playDeactivateTweenCoroutine);
        }

        private void SetMultiplierTextColorGradient()
        {
            switch(multiplier)
            {
                case 4:
                    multiplierText.colorGradientPreset = multiplierGradientX1;
                    break;
                case 6:
                    multiplierText.colorGradientPreset = multiplierGradientX2;
                    break;
                case 8:
                    multiplierText.colorGradientPreset = multiplierGradientX3;
                    break;
                case 10:
                    multiplierText.colorGradientPreset = multiplierGradientX4;
                    break;
                default:
                    multiplierText.colorGradientPreset = multiplierGradientX1;
                    break;
            }

            multiplierEffectText.colorGradientPreset = multiplierText.colorGradientPreset;
        }

        private void DoubleMultiplier()
        {
            multiplier = (byte)(multiplier * 2);
        }

        private IEnumerator PlayDeactivateTweenCoroutine()
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(1f);

            LeanTween.scale(multiplierText.gameObject, Vector3.zero, 0.25f);
            LeanTween.scale(multiplierEffectText.gameObject, Vector3.zero, 0.25f);
            yield return waitForSeconds;
            multiplierContainer.gameObject.SetActive(false);
            yield return null;
        }
        #endregion
    }
}
