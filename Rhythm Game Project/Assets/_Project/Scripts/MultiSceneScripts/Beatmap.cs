using System;
using Enums;

[Serializable]
public sealed class Beatmap
{
    #region Private Fields
    private double[] objectSpawnTimeArray;
    private double offsetMilliseconds;

    private float[] objectPositionArrayX;
    private float[] objectPositionArrayY;
    private float beatsPerMinute;
    private float difficultyAccuracy;
    private float audioStartTime;

    private byte[] objectTypeArray;
    private byte totalKeys;
    private byte approachRate;
    private byte objectSize;
    private byte healthDrain;
    private byte timingWindow;

    private ushort totalObjects;
    private ushort totalPhrases;

    private int rankedBeatmapID;
    private int localBeatmapID;

    private bool[] hasTypeArray;

    private string songName;
    private string artistName;
    private string creatorName;
    private string genre;
    private string creatorMessage;
    private string databaseTable;
    private string songLength;
    private string playerDifficultyGrade;
    private string playerDifficultyGradeUsername;

    private Difficulty difficulty;

    private DateTime createdDate;
    #endregion

    #region Properties
    public double[] ObjectSpawnTimeArray { get => objectSpawnTimeArray; set => objectSpawnTimeArray = value; }
    public double OffsetMilliseconds { get => offsetMilliseconds; set => offsetMilliseconds = value; }

    public float[] ObjectPositionArrayX { get => objectPositionArrayX; set => objectPositionArrayX = value; }
    public float[] ObjectPositionArrayY { get => objectPositionArrayY; set => objectPositionArrayY = value; }
    public float BeatsPerMinute { get => beatsPerMinute; set => beatsPerMinute = value; }
    public float DifficultyAccuracy { get => difficultyAccuracy; set => difficultyAccuracy = value; }
    public float AudioStartTime { get => audioStartTime; set => audioStartTime = value; }

    public byte[] ObjectTypeArray { get => objectTypeArray; set => objectTypeArray = value; }
    public byte TotalKeys { get => totalKeys; set => totalKeys = value; }
    public byte ApproachRate { get => approachRate; set => approachRate = value; }
    public byte ObjectSize { get => objectSize; set => objectSize = value; }
    public byte HealthDrain { get => healthDrain; set => healthDrain = value; }
    public byte TimingWindow { get => timingWindow; set => timingWindow = value; }

    public ushort TotalObjects { get => totalObjects; set => totalObjects = value; }
    public ushort TotalFeverPhrases { get => totalPhrases; set => totalPhrases = value; }

    public int RankedBeatmapID { get => rankedBeatmapID; set => rankedBeatmapID = value; }
    public int LocalBeatmapID { get => localBeatmapID; set => localBeatmapID = value; }

    public bool[] HasTypeArray { get => hasTypeArray; set => hasTypeArray = value; }

    public string SongName { get => songName; set => songName = value; }
    public string ArtistName { get => artistName; set => artistName = value; }
    public string Genre { get => genre; set => genre = value; }
    public string CreatorName { get => creatorName; set => creatorName = value; }
    public string CreatorMessage { get => creatorMessage; set => creatorMessage = value; }
    public string DatabaseTable { get => databaseTable; set => databaseTable = value; }
    public string SongLength { get => songLength; set => songLength = value; }
    public string PlayerDifficultyGrade { get => playerDifficultyGrade; set => playerDifficultyGrade = value; }
    public string PlayerDifficultyGradeUsername { get => playerDifficultyGradeUsername; set => playerDifficultyGradeUsername = value; }

    public Difficulty Difficulty { get => difficulty; set => difficulty = value; }

    public DateTime CreatedDate { get => createdDate; set => createdDate = value; }
    #endregion
}