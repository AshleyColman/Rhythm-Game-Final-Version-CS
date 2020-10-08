using TMPro;
using UnityEngine;

public sealed class FramerateManager : MonoBehaviour 
{
    #region Constants
    private const int TargetFramerate = 1000;
    #endregion

    #region Private Fields
    private float deltaTime = 0.0f;
    private float milliseconds = 0f;
    private float framePerSecond = 0f;
    private float updateTimer = 0f;

    [SerializeField] private TextMeshProUGUI framePerSecondText;
    [SerializeField] private TextMeshProUGUI millisecondText;
    #endregion

    #region Private Methods
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = TargetFramerate;
    }

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        updateTimer += Time.deltaTime;
        UpdateText();
    }

    private void UpdateText()
    {
        if (updateTimer >= 1f)
        {
            updateTimer = 0f;
            milliseconds = deltaTime * 1000.0f;
            milliseconds = (int)milliseconds;
            framePerSecond = 1.0f / deltaTime;
            framePerSecond = (int)framePerSecond;
            framePerSecondText.SetText("fps {0}", framePerSecond);
            millisecondText.SetText("ms {0}", milliseconds);
        }
    }
    #endregion
}
