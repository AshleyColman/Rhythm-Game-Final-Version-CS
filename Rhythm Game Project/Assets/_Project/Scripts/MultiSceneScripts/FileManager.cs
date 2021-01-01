namespace File
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public sealed class FileManager : MonoBehaviour
    {
        #region Private Fields
        private Beatmap beatmap;
        #endregion

        #region Properties
        public Beatmap Beatmap => beatmap;
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
            CreateNewBeatmapFileTest();
        }

        private void CreateNewBeatmapFileTest()
        {
            beatmap = new Beatmap();

            string folderName = "TestFolder";
            string beatmapFolder = "Beatmaps";
            string easyFile = "easy.bm";
            string filePath = $"{Application.persistentDataPath}/{beatmapFolder}/{folderName}/{easyFile}";

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            Stream stream = new FileStream(filePath, FileMode.Create);

            beatmap.SongName = "Beyond";
            beatmap.ArtistName = "XI";
            beatmap.CreatorName = "Ashley";
            beatmap.DifficultyName = "Skystar";
            beatmap.CreatorMessage = "This is my creator message";
            beatmap.TotalObjects = 500;
            beatmap.TotalFeverPhrases = 6;
            beatmap.BeatsPerMinute = 125f;
            beatmap.OffsetMilliseconds = 0;
            beatmap.TotalKeys = 4;
            beatmap.SongLength = "2 minutes 30 seconds";
            beatmap.CreatedDate = DateTime.Now;
            beatmap.DatabaseTable = "testTable";
            beatmap.Level = Random.Range(0, 10).ToString();
            beatmap.HasTypeArray = new bool[1];
            beatmap.HasTypeArray[0] = true;

            binaryFormatter.Serialize(stream, beatmap);
            stream.Close();
        }
        #endregion
    }
}