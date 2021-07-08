namespace Gameplay
{
    using File;
    using Timing;
    using Background;

    public sealed class GameplayTimeManager : TimeManager
    {
        #region Private Fields
        private FileManager fileManager;
        private Beatmap beatmap;
        private HitobjectFollower hitobjectFollower;
        private BackgroundManager backgroundManager;
        #endregion

        #region Public Methods
        public void SetTimingDetailsFromFile()
        {
            BeatsPerMinute = beatmap.BeatsPerMinute;
            OffsetMilliseconds = beatmap.OffsetMilliseconds;
        }
        #endregion

        #region Protected Methods
        protected override void Awake()
        {
            base.Awake();

            fileManager = FindObjectOfType<FileManager>();
            beatmap = fileManager.Beatmap;
            hitobjectFollower = FindObjectOfType<HitobjectFollower>();
            backgroundManager = FindObjectOfType<BackgroundManager>();
        }

        protected override void OnTick()
        {
            base.OnTick();

            hitobjectFollower.PlayRhythmTween();
        }

        protected override void OnMeasure()
        {
            base.OnMeasure();

            backgroundManager.PlayImageScaleTween();
        }
        #endregion
    }
}
