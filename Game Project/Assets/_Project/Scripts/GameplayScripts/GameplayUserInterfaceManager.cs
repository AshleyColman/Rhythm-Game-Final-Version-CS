using System.Collections;
using TMPro;
using UnityEngine;

public class GameplayUserInterfaceManager : MonoBehaviour
{
    #region Private Fields
    [SerializeField] private TextMeshProUGUI scoreText = default;
    [SerializeField] private TextMeshProUGUI comboText = default;
    [SerializeField] private TextMeshProUGUI levelText = default;
    [SerializeField] private TextMeshProUGUI songTimeText = default;

    private GameplayTimeManager gameplayTimeManager;
    #endregion

    #region Public Methods
    public void UpdateSongTimeText(double _time)
    {
        songTimeText.SetText(_time.ToString("F2"));
    }

    public void UpdateScoreText()
    {
    }

    public void UpdateComboText()
    {
    }

    public void UpdateLevelText()
    {
    }
    #endregion
}
