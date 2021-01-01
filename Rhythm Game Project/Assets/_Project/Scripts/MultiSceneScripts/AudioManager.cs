namespace Audio
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Networking;
    using Timing;

    public class AudioManager : MonoBehaviour
    {
        #region Constants
        public const byte Select1AudioClipIndex = 0;
        #endregion

        #region Private Fields
        private double songAudioStartTime = 0;

        private float songAudioSourceVolume = 1f;
        private float userInterfaceAudioSourceVolume = 1f;

        [SerializeField] private AudioSource songAudioSource = default;
        [SerializeField] private AudioSource userInterfaceAudioSource = default;

        [SerializeField] private AudioClip[] userInterfaceAudioClipArray = default;

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

        public void PlayOneShotUserInterfaceAudioSource(byte _clipIndex)
        {
            userInterfaceAudioSource.PlayOneShot(userInterfaceAudioClipArray[_clipIndex], userInterfaceAudioSourceVolume);
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            timeManager = MonoBehaviour.FindObjectOfType<TimeManager>();
            songAudioSource.volume = songAudioSourceVolume;
            userInterfaceAudioSource.volume = userInterfaceAudioSourceVolume;
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
}
