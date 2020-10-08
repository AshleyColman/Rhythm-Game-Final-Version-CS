using UnityEngine;
using System.Collections;
using TMPro;

public sealed class Countdown : MonoBehaviour 
{
    #region Private Fields
    [SerializeField] private TextMeshProUGUI countDownText = default;
    [SerializeField] private TextMeshProUGUI pressAnyKeyToPlayText = default;

    private float countdownDuration = 4f;

    private IEnumerator playCountdown;

    private GameManager gameManager;
    #endregion

    #region Propertes
    public float CountdownDuration => countdownDuration;
    #endregion

    #region Public Methods
    public void PlayCountdown()
    {
        StopCoroutine(playCountdown);
        StartCoroutine(playCountdown);
    }
    #endregion

    #region Private Methods
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        playCountdown = PlayCountdownCoroutine();
    }

    private IEnumerator PlayCountdownCoroutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1f);

        pressAnyKeyToPlayText.gameObject.SetActive(true);
        pressAnyKeyToPlayText.SetText("3");
        yield return waitForSeconds;
        pressAnyKeyToPlayText.SetText("2");
        yield return waitForSeconds;
        pressAnyKeyToPlayText.SetText("1");
        yield return waitForSeconds;
        pressAnyKeyToPlayText.SetText("LETS PLAY");
        yield return waitForSeconds;
        pressAnyKeyToPlayText.gameObject.SetActive(false);

        gameManager.StartGameplay();

        yield return null;
    }
    #endregion
}
