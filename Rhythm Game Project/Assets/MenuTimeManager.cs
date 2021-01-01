namespace Menu
{
    using Timing;
    using Background;
    using VisualEffects;

    public sealed class MenuTimeManager : TimeManager
    {
        #region Private Fields
        private BackgroundManager backgroundManager;
        private StartMenuManager startMenuManager;
        private FlashManager flashManager;
        #endregion

        #region Public Methods
        public void SetTimingDetails(float _beatsPerMinute, double _offsetMilliseconds)
        {
            BeatsPerMinute = _beatsPerMinute;
            OffsetMilliseconds = _offsetMilliseconds;
        }
        #endregion

        #region Protected Methods
        protected override void Awake()
        {
            base.Awake();

            backgroundManager = FindObjectOfType<BackgroundManager>();
            startMenuManager = FindObjectOfType<StartMenuManager>();
            flashManager = FindObjectOfType<FlashManager>();
        }

        protected override void OnTick()
        {
            base.OnTick();
        }

        protected override void OnMeasure()
        {
            base.OnMeasure();
            backgroundManager.PlayRhythmScaleTween();
            startMenuManager.PlayTitleRhythmTween();
            flashManager.PlayFlashTween(1f);
        }
        #endregion
    }
}

