using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    #region Private Fields
    private bool gameplayStarted = false;

    private GameplayAudioManager gameplayAudioManager;
    private GameplayUserInterfaceManager gameplayUserInterfaceManager;
    private GameplayTimeManager gameplayTimeManager;
    private GameplayInputManager gameplayInputManager;
    private HitobjectSpawnManager hitobjectSpawnManager;
    private HitobjectManager hitobjectManager;
    private Countdown countdown;
    #endregion

    #region Properties
    public bool GameplayStarted => gameplayStarted;
    #endregion

    #region Public Methods
    public void PrepareToStartGameplay()
    {
        gameplayAudioManager.PlayScheduledSongAudio(countdown.CountdownDuration);
        countdown.PlayCountdown();
    }

    public void StartGameplay()
    {
        gameplayStarted = true;
        gameplayTimeManager.StartTimer();
        hitobjectSpawnManager.TrackHitobjects();
        gameplayInputManager.CheckInputForHitobjects();
    }
    #endregion

    #region Private Methods
    private void Awake()
    {
        gameplayAudioManager = FindObjectOfType<GameplayAudioManager>();
        gameplayUserInterfaceManager = FindObjectOfType<GameplayUserInterfaceManager>();
        gameplayTimeManager = FindObjectOfType<GameplayTimeManager>();
        gameplayInputManager = FindObjectOfType<GameplayInputManager>();
        hitobjectSpawnManager = FindObjectOfType<HitobjectSpawnManager>();
        hitobjectManager = FindObjectOfType<HitobjectManager>();
        countdown = FindObjectOfType<Countdown>();
    }

    private void Start()
    {
        hitobjectSpawnManager.ReadOsuFile();
        hitobjectSpawnManager.InstantiateHitobjectTypePools();
        hitobjectSpawnManager.SetMainHitobjectPool();
        hitobjectSpawnManager.ClearHitobjectTypePools();
        gameplayTimeManager.SetTimingDetailsFromFile();
        gameplayTimeManager.UpdateTimingPosition();
        hitobjectManager.SetHitobjectApproachTime();
    }
    #endregion
}
