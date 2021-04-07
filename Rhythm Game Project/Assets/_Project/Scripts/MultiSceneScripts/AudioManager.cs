namespace Audio
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Networking;
    using Timing;
    using Menu;
    using Enums;
    using File;

    public class AudioManager : MonoBehaviour
    {
        #region Constants
        public const byte Select1AudioClipIndex = 0;

        public const float AudioClipLoadDelayDuration = 0.1f;
        #endregion

        #region Private Fields
        private double songAudioStartTime = 0;

        private float songAudioSourceVolume = 1f;
        private float userInterfaceAudioSourceVolume = 1f;

        [SerializeField] private AudioSource songAudioSource = default;
        [SerializeField] private AudioSource userInterfaceAudioSource = default;

        [SerializeField] private AudioClip[] userInterfaceAudioClipArray = default;

        private IEnumerator loadSongAudioClipFromFileCoroutine;

        private TimeManager timeManager;
        private Notification notification;
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

        public void SetAudioStartTime(float _audioStartTime)
        {
            songAudioSource.time = _audioStartTime;
        }

        public void PlayOneShotUserInterfaceAudioSource(byte _clipIndex)
        {
            userInterfaceAudioSource.PlayOneShot(userInterfaceAudioClipArray[_clipIndex], userInterfaceAudioSourceVolume);
        }

        public void LoadSongAudioClipFromFile(string _beatmapFolderPath, float _audioStartTime, TimeManager timeManager)
        {
            if (loadSongAudioClipFromFileCoroutine != null)
            {
                StopCoroutine(loadSongAudioClipFromFileCoroutine);
            }

            loadSongAudioClipFromFileCoroutine = LoadSongAudioClipFromFileCoroutine(_beatmapFolderPath, _audioStartTime,
                timeManager);
            StartCoroutine(loadSongAudioClipFromFileCoroutine);
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            timeManager = FindObjectOfType<TimeManager>();
            notification = FindObjectOfType<Notification>();
            songAudioSource.volume = songAudioSourceVolume;
            userInterfaceAudioSource.volume = userInterfaceAudioSourceVolume;
        }

        private IEnumerator LoadSongAudioClipFromFileCoroutine(string _beatmapFolderPath, float _audioStartTime, 
            TimeManager _timeManager)
        {
            DeactivateSongAudioSource();
            UnloadSongAudioClip();
            _timeManager.StopTimer();

            if (string.IsNullOrEmpty(_beatmapFolderPath) == false)
            {
                string audioFilePath = string.Empty;
                bool hasLoadedAudioFile = false;

                for (byte i = 0; i < FileTypes.AudioFileTypesArray.Length; i++)
                {
                    audioFilePath = $"{_beatmapFolderPath}/{FileTypes.AudioFileTypesArray[i]}";

                    using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(audioFilePath, AudioType.OGGVORBIS))
                    {
                        ((DownloadHandlerAudioClip)www.downloadHandler).streamAudio = true;
                        yield return www.SendWebRequest();

                        if (www.isNetworkError || www.isHttpError)
                        {
                            hasLoadedAudioFile = false;
                        }
                        else
                        {
                            songAudioSource.clip = DownloadHandlerAudioClip.GetContent(www);
                            yield return new WaitForSeconds(AudioClipLoadDelayDuration);
                            ActivateSongAudioSource();
                            PlayScheduledSongAudio(AudioClipLoadDelayDuration);
                            SetAudioStartTime(_audioStartTime);
                            yield return new WaitForSeconds(AudioClipLoadDelayDuration);
                            _timeManager.RecalculateAndPlayFromNewPosition();
                            hasLoadedAudioFile = true;
                        }
                    }

                    if (hasLoadedAudioFile == true)
                    {
                        break;
                    }
                    else
                    {
                        if (i == FileTypes.AudioFileTypesArray.Length)
                        {
                            DeactivateSongAudioSource();
                            DisplayErrorNotification();
                        }
                        continue;
                    }
                }
            }
            else
            {
                DisplayErrorNotification();
            }

            yield return null;
        }

        private void DeactivateSongAudioSource()
        {
            songAudioSource.gameObject.SetActive(false);
        }

        private void ActivateSongAudioSource()
        {
            songAudioSource.gameObject.SetActive(true);
        }

        private void UnloadSongAudioClip()
        {
            if (songAudioSource.clip != null)
            {
                songAudioSource.clip.UnloadAudioData();
                //AudioClip.DestroyImmediate(songAudioSource.clip, true);
            }
        }

        private void DisplayErrorNotification()
        {
            notification.DisplayNotification(NotificationType.Error, "error loading audio", 4f);
        }
        #endregion
    }
}
