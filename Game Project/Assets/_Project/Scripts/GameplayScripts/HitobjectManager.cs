using System.Collections;
using UnityEngine;

public sealed class HitobjectManager : MonoBehaviour 
{
    #region Private Methods
    [SerializeField] private double windowMilliseconds = 0.050;
    [SerializeField] private double missTime = 0;
    [SerializeField] private double okayLateStartTime = 0;
    [SerializeField] private double greatLateStartTime = 0;
    [SerializeField] private double perfectStartTime = 0;
    [SerializeField] private double greatEarlyStartTime = 0;
    [SerializeField] private double okayEarlyStartTime = 0;
    public double songTime = 0; // DELETE

    [SerializeField] private ushort currentHittableObjectIndex = 0;

    private IEnumerator trackHitobjects;

    [SerializeField] private Hitobject currentHittableObject;
    private GameplayTimeManager gameplayTimeManager;
    private HitobjectSpawnManager hitobjectSpawnManager;
    private PlayerSettings playerSettings;
    private GameManager gameManager;
    #endregion

    #region Public Methods
    public void CheckKeysPressed(KeyCode _keyCodePressed)
    {
        CheckIfMissKeysArePressed(_keyCodePressed);
        CheckIfHitKeysArePressed(_keyCodePressed);
    }

    public void SetHitobjectApproachTime()
    {
        Hitobject.ApproachTime = (float)playerSettings.ApproachTime;
    }

    public void TryToSetCurrentHittableObject()
    {
        if (currentHittableObject is null == true)
        {
            if (hitobjectSpawnManager.HitobjectIndex >= currentHittableObjectIndex)
            {
                currentHittableObject = hitobjectSpawnManager.MainHitobjectPoolArray[currentHittableObjectIndex];
                UpdateHitobjectJudgements();
            }
        }
    }

    public void SetFirstCurrentHittableObject()
    {
        currentHittableObject = hitobjectSpawnManager.MainHitobjectPoolArray[currentHittableObjectIndex];
        UpdateHitobjectJudgements();
    }

    public void TrackHitobjects()
    {
        StopCoroutine(trackHitobjects);
        StartCoroutine(trackHitobjects);
    }
    #endregion

    #region Private Methods
    private void Awake()
    {
        gameplayTimeManager = FindObjectOfType<GameplayTimeManager>();
        playerSettings = FindObjectOfType<PlayerSettings>();
        gameManager = FindObjectOfType<GameManager>();
        hitobjectSpawnManager = FindObjectOfType<HitobjectSpawnManager>();
        trackHitobjects = TrackHitobjectsCoroutine();
    }

    private IEnumerator TrackHitobjectsCoroutine()
    {
        while (gameManager.GameplayStarted == true)
        {
            songTime = gameplayTimeManager.SongTime;

            if (currentHittableObject is null == false)
            {
                CheckIfMissed();
            }

            yield return null;
        }

        yield return null;
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
        greatEarlyStartTime = hitTime - windowMilliseconds * 3;
        okayEarlyStartTime = hitTime - (windowMilliseconds * 5);
    }

    private void HasHit()
    {
        CheckJudgements();
        DeactivateCurrentHitobject();
        SetCurrentHittableObjectToNull();
        IncrementCurrentHittableObjectIndex();
        TryToSetCurrentHittableObject();
    }

    private void CheckIfHitKeysArePressed(KeyCode _keyCodePressed)
    {
        for (byte i = 0; i < currentHittableObject.HitKeyCodeArray.Length; i++)
        {
            if (_keyCodePressed == currentHittableObject.HitKeyCodeArray[i])
            {
                CheckIfHit();
                break;
            }
        }
    }

    private void CheckIfMissKeysArePressed(KeyCode _keyCodePressed)
    {
        for (byte i = 0; i < currentHittableObject.MissKeyCodeArray.Length; i++)
        {
            if (_keyCodePressed == currentHittableObject.MissKeyCodeArray[i])
            {
                HasMissed();
                break;
            }
        }
    }

    private void CheckIfHit()
    {
        if (gameplayTimeManager.SongTime >= okayEarlyStartTime && gameplayTimeManager.SongTime < missTime)
        {
            HasHit();
        }
    }

    private void HasMissed()
    {
        DeactivateCurrentHitobject();
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
        CheckOkayEarlyJudgement();
        // Break?
        CheckGreatEarlyJudgement();
        // Break?
        CheckPerfectJudgement();
        // Break?
        CheckGreatLateJudgement();
        // Break?
        CheckOkayLateJudgement();
        // Break?
    }

    private void CheckIfMissed()
    {
        if (gameplayTimeManager.SongTime >= missTime)
        {
            HasMissed();
        }
    }

    private void CheckOkayLateJudgement()
    {
        if (gameplayTimeManager.SongTime >= okayLateStartTime && gameplayTimeManager.SongTime < missTime)
        {

        }
    }

    private void CheckGreatLateJudgement()
    {
        if (gameplayTimeManager.SongTime >= greatLateStartTime && gameplayTimeManager.SongTime < okayLateStartTime)
        {

        }
    }

    private void CheckPerfectJudgement()
    {
        if (gameplayTimeManager.SongTime >= perfectStartTime && gameplayTimeManager.SongTime < greatLateStartTime)
        {

        }
    }

    private void CheckGreatEarlyJudgement()
    {
        if (gameplayTimeManager.SongTime >= greatEarlyStartTime && gameplayTimeManager.SongTime < perfectStartTime)
        {
        }
    }

    private void CheckOkayEarlyJudgement()
    {
        if (gameplayTimeManager.SongTime >= okayEarlyStartTime && gameplayTimeManager.SongTime < greatEarlyStartTime)
        {

        }
    }
    #endregion
}
