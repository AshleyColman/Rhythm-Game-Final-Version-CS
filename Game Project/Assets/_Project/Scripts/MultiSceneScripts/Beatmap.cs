using System;

[Serializable]
public sealed class Beatmap
{
    #region Private Fields
    private double[] objectSpawnTimeArray;
    private double offsetMilliseconds;

    private float[] objectPositionArrayX;
    private float[] objectPositionArrayY;
    private float beatsPerMinute;

    private byte[] objectTypeArray;
    private byte totalKeys;

    private ushort totalObjects;
    private ushort totalPhrases;

    private int rankedBeatmapID;
    private int localBeatmapID;

    private bool[] hasTypeArray;

    private string songName;
    private string artistName;
    private string creatorName;
    private string difficultyName;
    private string creatorMessage;
    private string databaseTable;
    private string songLength;
    private string level;

    private DateTime createdDate;
    #endregion

    #region Properties
    public double[] ObjectSpawnTimeArray { get => objectSpawnTimeArray; set => objectSpawnTimeArray = value; }
    public double OffsetMilliseconds { get => offsetMilliseconds; set => offsetMilliseconds = value; }

    public float[] ObjectPositionArrayX { get => objectPositionArrayX; set => objectPositionArrayX = value; }
    public float[] ObjectPositionArrayY { get => objectPositionArrayY; set => objectPositionArrayY = value; }
    public float BeatsPerMinute { get => beatsPerMinute; set => beatsPerMinute = value; }

    public byte[] ObjectTypeArray { get => objectTypeArray; set => objectTypeArray = value; }
    public byte TotalKeys { get => totalKeys; set => totalKeys = value; }

    public ushort TotalObjects { get => totalObjects; set => totalObjects = value; }
    public ushort TotalFeverPhrases { get => totalPhrases; set => totalPhrases = value; }

    public int RankedBeatmapID { get => rankedBeatmapID; set => rankedBeatmapID = value; }
    public int LocalBeatmapID { get => localBeatmapID; set => localBeatmapID = value; }

    public bool[] HasTypeArray { get => hasTypeArray; set => hasTypeArray = value; }

    public string Level { get => level; set => level = value; }
    public string SongName { get => songName; set => songName = value; }
    public string ArtistName { get => artistName; set => artistName = value; }
    public string DifficultyName { get => difficultyName; set => difficultyName = value; }
    public string CreatorName { get => creatorName; set => creatorName = value; }
    public string CreatorMessage { get => creatorMessage; set => creatorMessage = value; }
    public string DatabaseTable { get => databaseTable; set => databaseTable = value; }
    public string SongLength { get => songLength; set => songLength = value; }

    public DateTime CreatedDate { get => createdDate; set => createdDate = value; }
    #endregion
}