
namespace Gameplay
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using System.Text;

    public sealed class HealthManager : MonoBehaviour
    {
        #region Constants
        private const byte DrainRate = 15;
        private const byte GrowRate = 35;
        #endregion

        #region Private Fields
        [SerializeField] private Slider healthSlider = default;

        [SerializeField] private Gradient gradient = default;

        [SerializeField] private Image healthSliderColorImage = default;

        [SerializeField] private TextMeshProUGUI healthText = default;

        private StringBuilder healthTextStringBuilder = new StringBuilder();

        private bool hasFailed = false;

        private IEnumerator drainHealthCoroutine;
        private IEnumerator growHealthCoroutine;
        #endregion

        #region Public Methods
        public void IncreaseHealth(int _healthValue)
        {
            if (hasFailed == false)
            {
                healthSlider.value += _healthValue;
                CheckIfAboveMax();
            }
        }

        public void GrowHealth()
        {
            if (growHealthCoroutine != null)
            {
                StopCoroutine(growHealthCoroutine);
            }

            growHealthCoroutine = GrowHealthCoroutine();
            StartCoroutine(growHealthCoroutine);
        }
        #endregion

        #region Private Methods
        #endregion
        private void Awake()
        {
            healthSlider.value = healthSlider.minValue;
        }

        private IEnumerator DrainHealthCoroutine()
        {
            while (hasFailed == false)
            {
                healthSlider.value = Mathf.MoveTowards(healthSlider.value, healthSlider.minValue, Time.deltaTime * DrainRate);
                CheckIfFailed();
                UpdateHealthSliderColor();
                UpdateHealthText();
                yield return null;
            }
            yield return null;
        }

        private IEnumerator GrowHealthCoroutine()
        {
            while (healthSlider.value != healthSlider.maxValue)
            {
                healthSlider.value = Mathf.MoveTowards(healthSlider.value, healthSlider.maxValue, Time.deltaTime * GrowRate);
                CheckIfFailed();
                UpdateHealthSliderColor();
                UpdateHealthText();
                yield return null;
            }

            if (drainHealthCoroutine != null)
            {
                StopCoroutine(drainHealthCoroutine);
            }

            drainHealthCoroutine = DrainHealthCoroutine();
            StartCoroutine(drainHealthCoroutine);

            yield return null;
        }

        private void CheckIfAboveMax()
        {
            if (healthSlider.value > healthSlider.maxValue)
            {
                healthSlider.value = healthSlider.maxValue;
            }
        }

        private void CheckIfFailed()
        {
            if (healthSlider.value <= healthSlider.minValue)
            {
                healthSlider.value = healthSlider.minValue;
                hasFailed = true;
            }
        }

        private void UpdateHealthSliderColor()
        {
            healthSliderColorImage.color = gradient.Evaluate(healthSlider.normalizedValue);
        }

        private void UpdateHealthText()
        {
            healthTextStringBuilder.Append(healthSlider.value.ToString("F0"));
            healthTextStringBuilder.Append("%");
            healthText.SetText(healthTextStringBuilder.ToString());
            healthTextStringBuilder.Clear();
        }

    }
}