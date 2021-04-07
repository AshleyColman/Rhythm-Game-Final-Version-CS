﻿namespace Gameplay
{
    using UnityEngine;
    using TMPro;
    using System.Text;
    using System.Collections;

    public sealed class MultiplierManager : MonoBehaviour
    {
        #region Constants
        private const byte DefaultMultiplier = 1;
        private const byte FeverMultiplier = 2;

        private const string FeverMultiplierString = "X2";

        private readonly Vector3 MultiplierScaleTo = new Vector3(1.25f, 1.25f, 1f);
        private readonly Vector3 MultiplierEffectScaleTo = new Vector3(1.75f, 1.75f, 1f);
        #endregion

        #region Private Fields
        [SerializeField] private GameObject multiplierContainer = default;

        [SerializeField] private TextMeshProUGUI multiplierText = default;
        [SerializeField] private TextMeshProUGUI multiplierEffectText = default;

        [SerializeField] private Transform multiplierTextCachedTransform = default;
        [SerializeField] private Transform multiplierEffectTextCachedTransform = default;

        private byte multiplier = 1;
        private byte highestMultiplier = 0;

        private IEnumerator playDeactivateTweenCoroutine;
        #endregion

        #region Properties
        public byte Multiplier => multiplier;
        #endregion

        #region Public Methods
        public void ActivateFeverMultiplier()
        {
            multiplierText.SetText(FeverMultiplierString);
            multiplierEffectText.SetText(FeverMultiplierString);
            multiplier = FeverMultiplier;
            PlayActivatedTween();
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
            multiplierContainer.gameObject.SetActive(true);
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