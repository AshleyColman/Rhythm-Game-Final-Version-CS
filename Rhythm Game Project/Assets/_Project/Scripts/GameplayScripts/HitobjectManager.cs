namespace Gameplay
{
    using System.Collections;
    using UnityEngine;
    using GameSettings;
    using Enums;
    using Audio;

    public sealed class HitobjectManager : MonoBehaviour
    {
        #region Private Fields
        private double windowMilliseconds = 0.050;
        private double missTime = 0;
        private double okayLateStartTime = 0;
        private double greatLateStartTime = 0;
        private double perfectStartTime = 0;
        private double greatEarlyStartTime = 0;
        private double okayEarlyStartTime = 0;

        private ushort currentHittableObjectIndex = 0;

        private KeyCode hitKeyCode;
        private KeyCode[] missKeyCodeArray;

        private IEnumerator trackHitobjects;

        private Hitobject currentHittableObject;

        private GameplayTimeManager gameplayTimeManager;
        private HitobjectSpawnManager hitobjectSpawnManager;
        private PlayerSettings playerSettings;
        private GameManager gameManager;
        private GameplayAudioManager gameplayAudioManager;
        private ColorCollection colorCollection;
        private HitobjectFollower hitobjectFollower;
        private HealthManager healthManager;
        private ComboManager comboManager;
        private AccuracyManager accuracyManager;
        private JudgementManager judgementManager;
        private ScoreManager scoreManager;
        private FeverManager feverManager;
        private GameplayInputManager gameplayInputManager;
        #endregion

        #region Properties
        public KeyCode HitKeyCode => hitKeyCode;
        public Hitobject CurrentHittableObject => currentHittableObject;
        public ushort CurrentHittableObjectIndex => currentHittableObjectIndex;
        #endregion

        #region Public Methods
        public void SetHitobjectApproachTime()
        {
            Hitobject.ApproachTime = (float)playerSettings.ApproachTime;
        }

        public void TryToSetCurrentHittableObject()
        {
            hitobjectSpawnManager.CheckIfAllPlayed();

            if (hitobjectSpawnManager.AllObjectsPlayed == false)
            {
                if (currentHittableObject is null == true)
                {
                    if (hitobjectSpawnManager.HitobjectIndex >= currentHittableObjectIndex)
                    {
                        currentHittableObject = hitobjectSpawnManager.MainHitobjectPoolArray[currentHittableObjectIndex];
                        UpdateHitobjectJudgements();
                        UpdateInputKeys();
                        hitobjectFollower.ResetPositionTimer();
                    }
                }
            }
        }

        public void SetFirstCurrentHittableObject()
        {
            currentHittableObject = hitobjectSpawnManager.MainHitobjectPoolArray[currentHittableObjectIndex];
            UpdateHitobjectJudgements();
            UpdateInputKeys();
            gameplayInputManager.CheckInputForGameplay();
            hitobjectFollower.ResetPositionTimer();
            hitobjectFollower.TrackMoveToPosition();
        }

        public void TrackHitobjects()
        {
            StopCoroutine(trackHitobjects);
            StartCoroutine(trackHitobjects);
        }

        public void CheckIfHit()
        {
            if (currentHittableObject != null)
            {
                if (gameplayTimeManager.SongTime >= okayEarlyStartTime && gameplayTimeManager.SongTime < missTime)
                {
                    HasHit();
                }
            }
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            gameplayTimeManager = MonoBehaviour.FindObjectOfType<GameplayTimeManager>();
            playerSettings = MonoBehaviour.FindObjectOfType<PlayerSettings>();
            gameManager = MonoBehaviour.FindObjectOfType<GameManager>();
            hitobjectSpawnManager = MonoBehaviour.FindObjectOfType<HitobjectSpawnManager>();
            gameplayAudioManager = MonoBehaviour.FindObjectOfType<GameplayAudioManager>();
            colorCollection = MonoBehaviour.FindObjectOfType<ColorCollection>();
            hitobjectFollower = MonoBehaviour.FindObjectOfType<HitobjectFollower>();
            healthManager = MonoBehaviour.FindObjectOfType<HealthManager>();
            comboManager = MonoBehaviour.FindObjectOfType<ComboManager>();
            accuracyManager = MonoBehaviour.FindObjectOfType<AccuracyManager>();
            judgementManager = MonoBehaviour.FindObjectOfType<JudgementManager>();
            scoreManager = MonoBehaviour.FindObjectOfType<ScoreManager>();
            feverManager = MonoBehaviour.FindObjectOfType<FeverManager>();
            gameplayInputManager = MonoBehaviour.FindObjectOfType<GameplayInputManager>();
            trackHitobjects = TrackHitobjectsCoroutine();
        }

        private IEnumerator TrackHitobjectsCoroutine()
        {
            while (gameManager.GameplayStarted == true)
            {
                if (currentHittableObject is null == false)
                {
                    if (Input.anyKey)
                    {
                        AutoPlayHit();
                    }
                    else
                    {
                        CheckIfMissed();
                    }

                }

                yield return null;
            }
            yield return null;
        }

        private void AutoPlayHit()
        {
            if (gameplayTimeManager.SongTime >= perfectStartTime + windowMilliseconds)
            {
                HasHit();
            }
        }

        private void IncrementCurrentHittableObjectIndex()
        {
            currentHittableObjectIndex++;
        }

        private void UpdateHitobjectJudgements()
        {
            /*
            double hitTime = hitobjectSpawnManager.SpawnTimeArray[currentHittableObjectIndex] + playerSettings.ApproachTime;
            missTime = hitTime + (windowMilliseconds * 3);
            okayLateStartTime = hitTime + (windowMilliseconds * 2);
            greatLateStartTime = hitTime + windowMilliseconds;
            perfectStartTime = hitTime - windowMilliseconds;
            greatEarlyStartTime = hitTime - windowMilliseconds * 2;
            okayEarlyStartTime = hitTime - (windowMilliseconds * 3);
            */

            double hitTime = hitobjectSpawnManager.SpawnTimeArray[currentHittableObjectIndex] + playerSettings.ApproachTime;
            missTime = hitTime + (windowMilliseconds * 5);
            okayLateStartTime = hitTime + (windowMilliseconds * 3);
            greatLateStartTime = hitTime + windowMilliseconds;
            perfectStartTime = hitTime - windowMilliseconds;
            greatEarlyStartTime = hitTime - (windowMilliseconds * 3);
            okayEarlyStartTime = hitTime - (windowMilliseconds * 5);
        }

        private void UpdateInputKeys()
        {
            switch (hitobjectSpawnManager.GetCurrentHitobjectType())
            {
                case (byte)HitobjectType.Hitobject0:
                    hitKeyCode = Hitobject0.HitKeyCode;
                    missKeyCodeArray = Hitobject0.MissKeyCodeArray;
                    break;
            }
        }

        private void HasHit()
        {
            comboManager.IncreaseCombo();
            accuracyManager.UpdateAccuracy();
            gameplayAudioManager.PlayHitSound();
            feverManager.IncrementFeverSlider();
            CheckJudgements();
            currentHittableObject.PlayHitTween();
            SetCurrentHittableObjectToNull();
            IncrementCurrentHittableObjectIndex();
            TryToSetCurrentHittableObject();
        }

        private void HasMissed()
        {
            judgementManager.IncreaseMissCount();
            comboManager.ResetCombo();
            currentHittableObject.UpdateHitParticleSystemColor(colorCollection.RedColor);
            currentHittableObject.UpdateJudgementImageColor(colorCollection.RedColor);
            currentHittableObject.PlayMissTween();
            SetCurrentHittableObjectToNull();
            IncrementCurrentHittableObjectIndex();
            TryToSetCurrentHittableObject();
        }

        private void DeactivateCurrentHitobject()
        {
            currentHittableObject.gameObject.SetActive(false);
        }

        private void SetCurrentHittableObjectToNull()
        {
            currentHittableObject = null;
        }

        private void CheckJudgements()
        {
            // OkayEarly.
            if (gameplayTimeManager.SongTime >= okayLateStartTime && gameplayTimeManager.SongTime < missTime)
            {
                judgementManager.IncreaseOkayCount();
                scoreManager.IncreaseScore(JudgementData.OkayScore);
                healthManager.IncreaseHealth(JudgementData.OkayHealth);
                currentHittableObject.UpdateHitParticleSystemColor(colorCollection.PinkColor);
                currentHittableObject.UpdateJudgementImageColor(colorCollection.PinkColor);
            }
            // GreatEarly.
            else if (gameplayTimeManager.SongTime >= greatLateStartTime && gameplayTimeManager.SongTime < okayLateStartTime)
            {
                judgementManager.IncreaseGreatCount();
                scoreManager.IncreaseScore(JudgementData.GreatScore);
                healthManager.IncreaseHealth(JudgementData.GreatHealth);
                currentHittableObject.UpdateHitParticleSystemColor(colorCollection.DarkBlueColor);
                currentHittableObject.UpdateJudgementImageColor(colorCollection.DarkBlueColor);
            }
            // Perfect.
            else if (gameplayTimeManager.SongTime >= perfectStartTime && gameplayTimeManager.SongTime < greatLateStartTime)
            {
                judgementManager.IncreasePerfectCount();
                scoreManager.IncreaseScore(JudgementData.PerfectScore);
                healthManager.IncreaseHealth(JudgementData.PerfectHealth);
                currentHittableObject.UpdateHitParticleSystemColor(colorCollection.YellowColor);
                currentHittableObject.UpdateJudgementImageColor(colorCollection.YellowColor);
            }
            // GreatLate.
            else if (gameplayTimeManager.SongTime >= greatEarlyStartTime && gameplayTimeManager.SongTime < perfectStartTime)
            {
                judgementManager.IncreaseGreatCount();
                scoreManager.IncreaseScore(JudgementData.GreatScore);
                healthManager.IncreaseHealth(JudgementData.GreatHealth);
                currentHittableObject.UpdateHitParticleSystemColor(colorCollection.DarkBlueColor);
                currentHittableObject.UpdateJudgementImageColor(colorCollection.DarkBlueColor);
            }
            // OkayLate.
            else if (gameplayTimeManager.SongTime >= okayEarlyStartTime && gameplayTimeManager.SongTime < greatEarlyStartTime)
            {
                judgementManager.IncreaseOkayCount();
                scoreManager.IncreaseScore(JudgementData.OkayScore);
                healthManager.IncreaseHealth(JudgementData.OkayHealth);
                currentHittableObject.UpdateHitParticleSystemColor(colorCollection.PinkColor);
                currentHittableObject.UpdateJudgementImageColor(colorCollection.PinkColor);
            }
        }

        private void CheckIfMissed()
        {
            if (gameplayTimeManager.SongTime >= missTime)
            {
                HasMissed();
            }
        }
        #endregion
    }
}
