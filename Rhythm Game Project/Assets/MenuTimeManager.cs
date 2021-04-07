namespace Menu
{
    using Timing;
    using Background;
    using VisualEffects;

    public sealed class MenuTimeManager : TimeManager
    {
        #region Private Fields
        private MenuManager menuManager;
        private BackgroundManager backgroundManager;
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

            menuManager = FindObjectOfType<MenuManager>();
            backgroundManager = FindObjectOfType<BackgroundManager>();
            flashManager = FindObjectOfType<FlashManager>();
        }

        protected override void OnTick()
        {
            base.OnTick();

            menuManager.PlayCurrentMenuOnTick();
        }

        protected override void OnMeasure()
        {
            base.OnMeasure();
            backgroundManager.PlayRhythmScaleTween();
            flashManager.PlayFlashTween(1f);
            menuManager.PlayCurrentMenuOnMeasure();
        }
        #endregion
    }
}

