namespace Gameplay
{
    using System.Collections;
    using TMPro;
    using UnityEngine;

    public sealed class Countdown : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] private TextMeshProUGUI countDownText = default;
        [SerializeField] private GameObject startPanel = default;

        private float countdownDuration = 4f;

        private IEnumerator playCountdownCoroutine;

        private GameManager gameManager;
        #endregion

        #region Propertes
        public float CountdownDuration => countdownDuration;
        #endregion

        #region Public Methods
        public void PlayCountdown()
        {
            if (playCountdownCoroutine != null)
            {
                StopCoroutine(playCountdownCoroutine);
            }

            playCountdownCoroutine = PlayCountdownCoroutine();
            StartCoroutine(playCountdownCoroutine);
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            gameManager = MonoBehaviour.FindObjectOfType<GameManager>();
        }

        private IEnumerator PlayCountdownCoroutine()
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
            startPanel.gameObject.SetActive(false);


            countDownText.gameObject.SetActive(true);
            countDownText.SetText("3");
            yield return waitForSeconds;
            countDownText.SetText("2");
            yield return waitForSeconds;
            countDownText.SetText("1");
            yield return waitForSeconds;
            countDownText.SetText("go");
            yield return waitForSeconds;
            countDownText.gameObject.SetActive(false);

            gameManager.StartGameplay();

            yield return null;
        }
        #endregion
    }
}
