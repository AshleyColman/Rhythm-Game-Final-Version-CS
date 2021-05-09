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


            CreateTwoKeyBeatmap();
            CreateFourKeyBeatmap();
            CreateSixKeyBeatmap();
        }

        private void SetBeatmapDirectories()
        {
            beatmapDirectories = Directory.GetDirectories(beatmapDirectoryPath);
        }

        private void CreateTwoKeyBeatmap()
        {
            beatmap = new Beatmap();

            string folderName = "WhitePeak";
            string beatmapFolder = "Beatmaps";
            string difficultyFile = FileTypes.TwoKeyFileType;
            string filePath = $"{Application.persistentDataPath}/{beatmapFolder}/{folderName}/{difficultyFile}";

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            Stream stream = new FileStream(filePath, FileMode.Create);

            beatmap.SongName = "White Peak";
            beatmap.ArtistName = "XI";
            beatmap.CreatorName = "Ashley";
            beatmap.Genre = "Happy Hardcore";
            beatmap.Difficulty = Difficulty.TwoKey;
            beatmap.PlayerDifficultyGrade = "S";
            beatmap.PlayerDifficultyGradeUsername = "Ashley";
            beatmap.CreatorMessage = "Two key difficulty created by me!";
            beatmap.TotalObjects = 500;
            beatmap.TotalFeverPhrases = 5;
            beatmap.AudioStartTime = 25f;
            beatmap.BeatsPerMinute = 200f;
            beatmap.OffsetMilliseconds = 0;
            beatmap.DifficultyAccuracy = 98.5f;
            beatmap.TotalKeys = 2;
            beatmap.SongLength = "2 minutes 30 seconds";
            beatmap.CreatedDate = DateTime.Now;
            beatmap.DatabaseTable = "testtable";
            beatmap.HasTypeArray = new bool[1];
            beatmap.HasTypeArray[0] = true;

            beatmap.StandardClear = true;
            beatmap.HiddenClear = false;
            beatmap.MinesClear = true;
            beatmap.FullCombo = false;
            beatmap.MaxPercentage = false;

            beatmap.ApproachRate = 10;
            beatmap.ObjectSize = 10;
            beatmap.HealthDrain = 10;
            beatmap.TimingWindow = 10;

            binaryFormatter.Serialize(stream, beatmap);
            stream.Close();
        }

        private void CreateFourKeyBeatmap()
        {
            beatmap = new Beatmap();

            string folderName = "WhitePeak";
            string beatmapFolder = "Beatmaps";
            string difficultyFile = FileTypes.FourKeyFileType;
            string filePath = $"{Application.persistentDataPath}/{beatmapFolder}/{folderName}/{difficultyFile}";

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            Stream stream = new FileStream(filePath, FileMode.Create);

            beatmap.SongName = "White Peak";
            beatmap.ArtistName = "XI";
            beatmap.CreatorName = "Ashley";
            beatmap.Genre = "Happy Hardcore";
            beatmap.Difficulty = Difficulty.FourKey;
            beatmap.PlayerDifficultyGrade = "A";
            beatmap.PlayerDifficultyGradeUsername = "Ashley";
            beatmap.CreatorMessage = "Four key difficulty created by me!";
            beatmap.TotalObjects = 1000;
            beatmap.TotalFeverPhrases = 10;
            beatmap.AudioStartTime = 50f;
            beatmap.BeatsPerMinute = 200f;
            beatmap.OffsetMilliseconds = 0;
            beatmap.DifficultyAccuracy = 90.5f;
            beatmap.TotalKeys = 4;
            beatmap.SongLength = "2 minutes 30 seconds";
            beatmap.CreatedDate = DateTime.Now;
            beatmap.DatabaseTable = "testTable";
            beatmap.HasTypeArray = new bool[1];
            beatmap.HasTypeArray[0] = true;

            beatmap.StandardClear = false;
            beatmap.HiddenClear = true;
            beatmap.MinesClear = false;
            beatmap.FullCombo = true;
            beatmap.MaxPercentage = true;

            beatmap.ApproachRate = 5;
            beatmap.ObjectSize = 3;
            beatmap.HealthDrain = 5;
            beatmap.TimingWindow = 5;

            binaryFormatter.Serialize(stream, beatmap);
            stream.Close();
        }

        private void CreateSixKeyBeatmap()
        {
            beatmap = new Beatmap();

            string folderName = "WhitePeak";
            string beatmapFolder = "Beatmaps";
            string difficultyFile = FileTypes.SixKeyFileType;
            string filePath = $"{Application.persistentDataPath}/{beatmapFolder}/{folderName}/{difficultyFile}";

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            Stream stream = new FileStream(filePath, FileMode.Create);

            beatmap.SongName = "White Peak";
            beatmap.ArtistName = "XI";
            beatmap.CreatorName = "Ashley";
            beatmap.Genre = "Happy Hardcore";
            beatmap.Difficulty = Difficulty.SixKey;
            beatmap.PlayerDifficultyGrade = "E";
            beatmap.PlayerDifficultyGradeUsername = "Ashley";
            beatmap.CreatorMessage = "Four key difficulty created by me!";
            beatmap.TotalObjects = 2000;
            beatmap.TotalFeverPhrases = 20;
            beatmap.AudioStartTime =75f;
            beatmap.BeatsPerMinute = 200f;
            beatmap.OffsetMilliseconds = 0;
            beatmap.DifficultyAccuracy = 35.28f;
            beatmap.TotalKeys = 6;
            beatmap.SongLength = "2 minutes 30 seconds";
            beatmap.CreatedDate = DateTime.Now;
            beatmap.DatabaseTable = "testTable";
            beatmap.HasTypeArray = new bool[1];
            beatmap.HasTypeArray[0] = true;

            beatmap.StandardClear = true;
            beatmap.HiddenClear = true;
            beatmap.MinesClear = true;
            beatmap.FullCombo = true;
            beatmap.MaxPercentage = true;

            beatmap.ApproachRate = 8;
            beatmap.ObjectSize = 4;
            beatmap.HealthDrain = 9;
            beatmap.TimingWindow = 8;

            binaryFormatter.Serialize(stream, beatmap);
            stream.Close();
        }
        #endregion
    }
}