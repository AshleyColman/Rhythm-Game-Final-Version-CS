using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AudioManager : MonoBehaviour
{
    #region Private Fields
    private double songAudioStartTime = 0;

    [SerializeField] private AudioSource songAudioSource = default;

    private TimeManager timeManager;
    #endregion

    #region Properties
    public AudioSource SongAudioSource => songAudioSource;
    public double SongAudioStartTime => songAudioStartTime;
    #endregion
    
    #region Public Methods
    public void PlayScheduledSongAudio(double _timeToPlay)
    {
        songAudioStartTime = AudioSettings.dspTime + _timeToPlay;
        songAudioSource.PlayScheduled(songAudioStartTime);
    }   
    #endregion

    #region Private Methods
    private void Awake()
    {
        timeManager = FindObjectOfType<TimeManager>();
    }

    private IEnumerator LoadSongAudioClipFromFileCoroutine(string _filePath)
    {
        UnloadSongAudioClip();

        if (string.IsNullOrEmpty(_filePath) == false)
        {
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(_filePath, AudioType.OGGVORBIS))
            {
                ((DownloadHandlerAudioClip)www.downloadHandler).streamAudio = true;
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    DeactivateSongAudioSource();
                }
                else
                {
                    songAudioSource.clip = DownloadHandlerAudioClip.GetContent(www);
                }
            }
        }
    }

    private void DeactivateSongAudioSource()
    {
        songAudioSource.gameObject.SetActive(false);
    }

    private void UnloadSongAudioClip()
    {
        if (songAudioSource.clip != null)
        {
            songAudioSource.clip.UnloadAudioData();
            AudioClip.Destroy(songAudioSource.clip);
        }
    }
    #endregion
}
