public sealed class GameplayTimeManager : TimeManager
{
    #region Private Fields
    private GameplayUserInterfaceManager gameplayUserInterfaceManager;
    private FileManager fileManager;
    private Beatmap beatmap;
    #endregion

    #region Public Methods
    public void SetTimingDetailsFromFile()
    {
        beatsPerMinute = beatmap.BeatsPerMinute;
        offsetMilliseconds = beatmap.OffsetMilliseconds;
    }
    #endregion

    #region Protected Methods
    protected override void Awake()
    {
        base.Awake();

        gameplayUserInterfaceManager = FindObjectOfType<GameplayUserInterfaceManager>();
        fileManager = FindObjectOfType<FileManager>();
        beatmap = fileManager.Beatmap;
    }

    protected override void OnTimerUpdate()
    {
        if (timerStarted == true)
        {
            UpdateSongTime();
            UpdateMetronomeValues();
            gameplayUserInterfaceManager.UpdateSongTimeText(songTime);
        }
    }
    #endregion
}
