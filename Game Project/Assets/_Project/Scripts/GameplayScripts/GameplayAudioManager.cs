using UnityEngine;

public sealed class GameplayAudioManager : AudioManager 
{
    #region Private Fields
    [SerializeField] private AudioSource[] hitSoundAudioSourceArray = default;
    [SerializeField] private AudioSource[] missSoundAudioSourceArray = default;

    [SerializeField] private AudioClip hitSoundAudioClip = default;
    [SerializeField] private AudioClip missSoundAudioClip = default;

    private byte hitSoundAudioSourceIndex = 0;
    private byte missSoundAudioSourceIndex = 0;
    #endregion

    #region Public Methods
    public void PlayHitSound()
    {
        ErrorCheckHitSoundAudioSourceIndex();
        hitSoundAudioSourceArray[hitSoundAudioSourceIndex].Play();
    }

    public void PlayMissSound()
    {
        ErrorCheckMissSoundAudioSourceIndex();
        missSoundAudioSourceArray[missSoundAudioSourceIndex].Play();
    }
    #endregion

    #region Private Methods
    //private void Awake()
   // {
        //InitializeAudioSourceClips();
   // }

    private void InitializeAudioSourceClips()
    {
        for (byte i = 0; i < hitSoundAudioSourceArray.Length; i++)
        {
            hitSoundAudioSourceArray[i].clip = hitSoundAudioClip;
        }

        for (byte i = 0; i < missSoundAudioSourceArray.Length; i++)
        {
            missSoundAudioSourceArray[i].clip = missSoundAudioClip;
        }
    }

    private void ErrorCheckHitSoundAudioSourceIndex()
    {
        if (hitSoundAudioSourceIndex == hitSoundAudioSourceArray.Length)
        {
            hitSoundAudioSourceIndex = 0;
        }
    }

    private void ErrorCheckMissSoundAudioSourceIndex()
    {
        if (missSoundAudioSourceIndex == missSoundAudioSourceArray.Length)
        {
            missSoundAudioSourceIndex = 0;
        }
    }
    #endregion
}
