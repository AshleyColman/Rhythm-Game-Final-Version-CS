namespace Gameplay
{
    using UnityEngine;
    using Audio;

    public sealed class GameManager : MonoBehaviour
    {
        #region Private Fields
        private bool gameplayStarted = false;

        private GameplayAudioManager gameplayAudioManager;
        private GameplayTimeManager gameplayTimeManager;
        private GameplayInputManager gameplayInputManager;
        private HitobjectSpawnManager hitobjectSpawnManager;
        private HitobjectManager hitobjectManager;
        private Countdown countdown;
        private HealthManager healthManager;
        private ScoreManager scoreManager;
        private Leaderboard leaderboard;
        private AccuracyManager accuracyManager;
        private SongSlider songSlider;
        #endregion

        #region Properties
        public bool GameplayStarted => gameplayStarted;
        #endregion

        #region Public Methods
        public void PrepareToStartGameplay()
        {
            gameplayAudioManager.PlayScheduledSongAudio(countdown.CountdownDuration);
            countdown.PlayCountdown();
            healthManager.GrowHealth();
        }

        public void StartGameplay()
        {
            gameplayStarted = true;
            songSlider.UpdateSongSliderProgress();
            gameplayTimeManager.StartTimer();
            hitobjectSpawnManager.TrackHitobjects();
            scoreManager.TrackIncreasingScore();
            accuracyManager.TrackIncreasingAccuracy();
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            gameplayAudioManager = FindObjectOfType<GameplayAudioManager>();
            gameplayTimeManager = FindObjectOfType<GameplayTimeManager>();
            hitobjectSpawnManager = FindObjectOfType<HitobjectSpawnManager>();
            hitobjectManager = FindObjectOfType<HitobjectManager>();
            countdown = FindObjectOfType<Countdown>();
            healthManager = FindObjectOfType<HealthManager>();
            scoreManager = FindObjectOfType<ScoreManager>();
            leaderboard = FindObjectOfType<Leaderboard>();
            accuracyManager = FindObjectOfType<AccuracyManager>();
            songSlider = FindObjectOfType<SongSlider>();
        }

        private void Start()
        {
            hitobjectSpawnManager.ReadOsuFile();
            hitobjectSpawnManager.InstantiateHitobjectTypePools();
            hitobjectSpawnManager.SetMainHitobjectPool();
            hitobjectSpawnManager.ClearHitobjectTypePools();
            scoreManager.CalculateMaxBaseScorePossible(hitobjectSpawnManager.TotalHitobjects);
            gameplayTimeManager.SetTimingDetailsFromFile();
            gameplayTimeManager.UpdateTimingPosition();
            hitobjectManager.SetHitobjectApproachTime();
            leaderboard.Initialize();
        }
        #endregion
    }
}