namespace File
{
    using Enums;
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public sealed class FileManager : MonoBehaviour
    {
        #region Private Fields
        private string beatmapDirectoryPath = string.Empty;
        private string[] beatmapDirectories;

        private Beatmap beatmap;
        #endregion

        #region Properties
        public Beatmap Beatmap => beatmap;
        public string BeatmapDirectoryPath => beatmapDirectoryPath;
        public string[] BeatmapDirectories => beatmapDirectories;
        #endregion

        #region Public Methods
        public void Load(string _filePath)
        {
            FileStream stream = new FileStream(_filePath, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            beatmap = (Beatmap)binaryFormatter.Deserialize(stream);
            stream.Close();
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            beatmapDirectoryPath = $"{Application.persistentDataPath}/Beatmaps";
            SetBeatmapDirectories();
            CreateNewBeatmapFileTest();
        }

        private void SetBeatmapDirectories()
        {
            beatmapDirectories = Directory.GetDirectories(beatmapDirectoryPath);
        }

        private void CreateNewBeatmapFileTest()
        {
            beatmap = new Beatmap();

            string folderName = "Magical";
            string beatmapFolder = "Beatmaps";
            string difficultyFile = "hard.bm";
            string filePath = $"{Application.persistentDataPath}/{beatmapFolder}/{folderName}/{difficultyFile}";

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            Stream stream = new FileStream(filePath, FileMode.Create);

            beatmap.SongName = "Really really really really really really really long name for testing";
            beatmap.ArtistName = "Unknown artist name";
            beatmap.CreatorName = "Ashley";
            beatmap.Genre = "Hardcore";
            beatmap.Difficulty = Difficulty.Hard;
            beatmap.PlayerDifficultyGrade = "S";
            beatmap.PlayerDifficultyGradeUsername = "Ashley";
            beatmap.CreatorMessage = "Hard difficulty???";
            beatmap.TotalObjects = 1005;
            beatmap.TotalFeverPhrases = 20;
            beatmap.AudioStartTime = 25f;
            beatmap.BeatsPerMinute = 125f;
            beatmap.OffsetMilliseconds = 0;
            beatmap.DifficultyAccuracy = 54;
            beatmap.TotalKeys = 4;
            beatmap.SongLength = "2 minutes 30 seconds";
            beatmap.CreatedDate = DateTime.Now;
            beatmap.DatabaseTable = "testTable";
            beatmap.Level = "9";
            beatmap.HasTypeArray = new bool[1];
            beatmap.HasTypeArray[0] = true;

            binaryFormatter.Serialize(stream, beatmap);
            stream.Close();
        }
        #endregion
    }
}