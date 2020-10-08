using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

// For Osu! files. Needs changing to work for .bm files.
public sealed class HitobjectSpawnManager : MonoBehaviour 
{
    #region Constants
    private const byte HitobjectTypePoolSize = 50;
    private const byte HitobjectTypes = 1;
    #endregion

    #region Private Fields
    [SerializeField] private DefaultAsset osuMapFile = default;

    [SerializeField] private Transform spawnTransform = default;

    [SerializeField] private Vector2[] positionArray;

    [SerializeField] private double[] spawnTimeArray;

    private byte[] typeArray;

    [SerializeField] private ushort hitobjectIndex = 0;
    [SerializeField] private ushort totalHitobjects = 0;

    [SerializeField] private bool allObjectSpawned = false;

    private IEnumerator trackHitobjects;

    private Hitobject[] mainHitobjectPoolArray;
    private Hitobject[] hitobjectType0Array;
    [SerializeField] private Hitobject[] hitobjectTypePrefabArray = default;

    private HitobjectManager hitobjectManager;
    private GameManager gameManager;
    private GameplayTimeManager gameplayTimeManager;
    private PlayerSettings playerSettings;
    #endregion

    #region Properties
    public double[] SpawnTimeArray => spawnTimeArray;
    public Hitobject[] MainHitobjectPoolArray => mainHitobjectPoolArray;
    public ushort HitobjectIndex => hitobjectIndex;
    #endregion

    #region Public Methods
    public void ReadOsuFile()
    {
        // List for osu file reading per line. Is applied to the array after.
        List<Vector2> positionList = new List<Vector2>();
        List<double> spawnTimeList = new List<double>();

        string osuMapFilePath = AssetDatabase.GetAssetPath(osuMapFile);

        StreamReader reader = new StreamReader(osuMapFilePath);

        string line = string.Empty;
        string hitObjectLine = "[HitObjects]";

        // Skip to [HitObjects] part.
        while (true)
        {
            if (reader.ReadLine() == hitObjectLine)
            {
                break;
            }
        }

        int totalLines = 0;

        // Count all lines.
        while (!reader.EndOfStream)
        {
            reader.ReadLine();
            totalLines++;
        }

        reader.Close();
        reader = new StreamReader(osuMapFilePath);

        // Skip to [HitObjects] part again.
        while (true)
        {
            if (reader.ReadLine() == hitObjectLine)
            {
                break;
            }
        }

        while (!reader.EndOfStream)
        {
            // Uncomment to skip sliders
            /*while (true)
            {
                line = reader.ReadLine();
                if (line != null)
                {
                    if (!line.Contains("|"))
                        break;
                }
                else
                    break;
            }*/

            line = reader.ReadLine();
            if (line == null)
            {
                break;
            }

            string[] lineParamsArray;

            lineParamsArray = line.Split(','); // Line parameters (X&Y axis, time position)

            // Sorting configuration
            int FlipY = 384 - int.Parse(lineParamsArray[1]); // Flip Y axis

            int AdjustedX = Mathf.RoundToInt(Screen.height * 1.333333f); // Aspect Ratio

            // Padding
            float Slices = 8f;
            float PaddingX = AdjustedX / Slices;
            float PaddingY = Screen.height / Slices;

            // Resolution set
            float NewRangeX = ((AdjustedX - PaddingX) - PaddingX);
            float NewValueX = ((int.Parse(lineParamsArray[0]) * NewRangeX) / 512f) + PaddingX + ((Screen.width - AdjustedX) / 2f);
            float NewRangeY = Screen.height;
            float NewValueY = ((FlipY * NewRangeY) / 512f) + PaddingY;

            positionList.Add(new Vector2(NewValueX, NewValueY));

            // Convert to double milliseconds
            double hitTime = (double.Parse(lineParamsArray[2]) / 1000);
            double spawnTime = hitTime - playerSettings.ApproachTime;
            spawnTimeList.Add(spawnTime);
        }

        // Convert list to array.
        positionArray = positionList.ToArray();
        spawnTimeArray = spawnTimeList.ToArray();

        // Set total hitobject count
        totalHitobjects = (ushort)positionArray.Length;

        InitializeTypeArray();
    }

    public void InstantiateHitobjectTypePools()
    {
        hitobjectType0Array = new Hitobject[HitobjectTypePoolSize];
        Hitobject hitobject;

        for (byte i = 0; i < HitobjectTypePoolSize; i++)
        {
            switch (typeArray[i])
            {
                case 0:
                    hitobject = Instantiate(hitobjectTypePrefabArray[0], spawnTransform);
                    hitobject.gameObject.SetActive(false);
                    hitobject.CachedTransform.rotation = Quaternion.identity;
                    hitobject.CachedTransform.localPosition = Vector3.zero;

                    hitobjectType0Array[i] = hitobject;
                    break;
            }
        }
    }

    public void SetMainHitobjectPool()
    {
        // Initialize size with total amount of hit objects.
        mainHitobjectPoolArray = new Hitobject[totalHitobjects];
        // Keeps track of how many hitobjects of that type have spawned, resets to 0 if over the pool size.
        ushort[] typeIndexCountArray = new ushort[HitobjectTypes];

        for (ushort i = 0; i < mainHitobjectPoolArray.Length; i++)
        {
            SetHitobject(i, typeArray[i], typeIndexCountArray);
        }
    }

    public void ClearHitobjectTypePools()
    {
        Array.Clear(hitobjectType0Array, 0, hitobjectType0Array.Length);
    }

    public void TrackHitobjects()
    {
        StopCoroutine(trackHitobjects);
        StartCoroutine(trackHitobjects);
    }
    #endregion

    #region Private Methods
    private void Awake()
    {
        hitobjectManager = FindObjectOfType<HitobjectManager>();
        gameManager = FindObjectOfType<GameManager>();
        gameplayTimeManager = FindObjectOfType<GameplayTimeManager>();
        playerSettings = FindObjectOfType<PlayerSettings>();
        trackHitobjects = TrackHitobjectsCoroutine();
    }

    private IEnumerator TrackHitobjectsCoroutine()
    {
        while (gameManager.GameplayStarted == true)
        {
            if (allObjectSpawned == false)
            {
                CheckIfTimeToSpawnNextHitobject();
            }

            yield return null;
        }

        yield return null;
    }

    private void InitializeTypeArray()
    {
        typeArray = new byte[totalHitobjects];

        for (ushort i = 0; i < typeArray.Length; i++)
        {
            typeArray[i] = 0;
        }
    }

    // Sets the hitobject type in the hitobject pool array.
    private void SetHitobject(ushort _index, byte _type, ushort[] _typeIndexCountArray)
    {
        switch (_type)
        {
            case 0:
                mainHitobjectPoolArray[_index] = hitobjectType0Array[_typeIndexCountArray[_type]];
                break;
        }

        _typeIndexCountArray[_type]++;

        if (_typeIndexCountArray[_type] >= HitobjectTypePoolSize)
        {
            _typeIndexCountArray[_type] = 0;
        }
    }

    private void CheckIfTimeToSpawnNextHitobject()
    {
        if (gameplayTimeManager.SongTime >= spawnTimeArray[hitobjectIndex])
        {
            SpawnHitobjectFromPool();
        }
    }

    private void SpawnHitobjectFromPool()
    {
        mainHitobjectPoolArray[hitobjectIndex].CachedTransform.localPosition = positionArray[hitobjectIndex];
        mainHitobjectPoolArray[hitobjectIndex].CachedTransform.SetAsFirstSibling();
        mainHitobjectPoolArray[hitobjectIndex].gameObject.SetActive(true);
        SetHittableObject();
        IncrementHitobjectIndex();
        CheckIfAllObjectsHaveSpawned();
    }

    private void SetHittableObject()
    {
        if (hitobjectIndex == 0)
        {
            Debug.Log("FIRST OBJECT");
            hitobjectManager.SetFirstCurrentHittableObject();
            hitobjectManager.TrackHitobjects();
        }
        else
        {
            hitobjectManager.TryToSetCurrentHittableObject();
        }
    }

    private void IncrementHitobjectIndex()
    {
        hitobjectIndex++;
    }

    private void CheckIfAllObjectsHaveSpawned()
    {
        if (hitobjectIndex == totalHitobjects)
        {
            allObjectSpawned = true;
        }
    }
    #endregion
}
