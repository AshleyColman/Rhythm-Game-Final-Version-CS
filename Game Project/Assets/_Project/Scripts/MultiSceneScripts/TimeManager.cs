using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    #region Constants
    private const byte Step = 4;
    private const byte Base = 4;
    #endregion

    #region Private Fields
    [SerializeField] private byte currentStep = 0;

    [SerializeField] private ushort currentMeasure = 0;
    [SerializeField] private ushort currentTick = 0;

    [SerializeField] private double[] tickTimeArray;
    [SerializeField] private double interval = 0;

    private AudioManager audioManager;
    #endregion

    #region Protected Fields
    [SerializeField] protected float beatsPerMinute = 0f;

    [SerializeField] protected double songTime = 0;
    protected double offsetMilliseconds = 0f;

    protected bool timerStarted = false;

    #endregion

    #region Properties
    public double SongTime => songTime;
    #endregion

    #region Public Methods
    public void StartTimer()
    {
        timerStarted = true;
    }

    public void UpdateTimingPosition()
    {
        CalculateIntervals();
        SetClosestTickAndMeasure();
        SetStepBasedOnCurrentAudioTime();
    }
    #endregion

    #region Protected Methods
    protected virtual void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    protected void UpdateSongTime()
    {
        songTime = AudioSettings.dspTime - audioManager.SongAudioStartTime;
    }

    protected virtual void OnTimerUpdate()
    {
        if (timerStarted == true)
        {
            UpdateSongTime();
            UpdateMetronomeValues();
        }
    }
    #endregion

    #region Private Methods
    private void Update()
    {
        OnTimerUpdate();
    }

    private void SetClosestTickAndMeasure()
    {
        for (ushort i = 0; i < tickTimeArray.Length; i++)
        {
            if (songTime <= tickTimeArray[i])
            {
                currentMeasure = (ushort)(i / 4);
                currentTick = i;
                break;
            }
        }
    }

    private void SetStepBasedOnCurrentAudioTime()
    {
        for (ushort i = 0; i < currentTick; i++)
        {
            if (currentStep >= Step)
            {
                currentStep = 1;
            }
            else
            {
                currentStep++;
            }
        }
    }

    private void CalculateIntervals()
    {
        if (audioManager.SongAudioSource.clip is null == false)
        {
            int i = 0;
            int multiplier = Base / Step;
            float tmpInterval = 60f / beatsPerMinute;
            interval = tmpInterval / multiplier;
            List<double> tickTimeList = new List<double>();

            while (interval * i <= audioManager.SongAudioSource.clip.length)
            {
                tickTimeList.Add((interval * i) + (offsetMilliseconds / 1000f));
                i++;
            }

            tickTimeArray = tickTimeList.ToArray();
        }
    }

    protected void UpdateMetronomeValues()
    {
        if (currentTick < tickTimeArray.Length)
        {
            if (songTime >= tickTimeArray[currentTick])
            {
                OnTick();
                CheckIfMeasure();
            }
        }
    }

    private void CheckIfMeasure()
    {
        if (currentStep >= Step)
        {
            currentStep = 1;
            currentMeasure++;
            OnMeasure();
        }
        else
        {
            currentStep++;
        }
    }

    protected void OnTick()
    {
        currentTick++;
    }

    protected void OnMeasure()
    {

    }
    #endregion
}
