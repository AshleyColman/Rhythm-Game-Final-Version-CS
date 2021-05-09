// For Osu! files. Needs changing to work for .bm files.
namespace Gameplay
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UI;
    using PlayerSettings = GameSettings.PlayerSettings;

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
        [SerializeField] private bool allObjectsPlayed = false;

        private IEnumerator trackHitobjects;

        private Hitobject[] mainHitobjectPoolArray;
        private Hitobject[] hitobjectType0Array;
        [SerializeField] private Hitobject[] hitobjectTypePrefabArray = default;

        [SerializeField] private Camera camera = default;

        private HitobjectManager hitobjectManager;
        private GameManager gameManager;
        private GameplayTimeManager gameplayTimeManager;
        private PlayerSettings playerSettings;
        private ColorCollection colorCollection;
        #endregion

        #region Properties
        public double[] SpawnTimeArray => spawnTimeArray;
        public Hitobject[] MainHitobjectPoolArray => mainHitobjectPoolArray;
        public ushort HitobjectIndex => hitobjectIndex;
        public ushort TotalHitobjects => totalHitobjects;
        public byte[] TypeArray => typeArray;
        public bool AllObjectsPlayed { get => allObjectsPlayed; set => allObjectsPlayed = value; }
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

                /*
                // Position
                int positionX = int.Parse(lineParamsArray[0]);
                int positionY = int.Parse(lineParamsArray[1]);
                positionList.Add(new Vector2(positionX, positionY));
                */


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

                Vector3 MainPos = camera.ScreenToWorldPoint(new Vector3(NewValueX, NewValueY, 0)); // Convert from screen position to world position

                positionList.Add(new Vector2(MainPos.x * 100, MainPos.y * 100));


                // Convert to double milliseconds
                double hitTime = double.Parse(lineParamsArray[2]) / 1000;
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
                        hitobject.CachedTransform.rotation = hitobject.CachedTransform.rotation;
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
            if (trackHitobjects != null)
            {
                StopCoroutine(trackHitobjects);
            }

            trackHitobjects = TrackHitobjectsCoroutine();
            StartCoroutine(trackHitobjects);
        }

        public byte GetCurrentHitobjectType(int _hitobjectIndex)
        {
            return typeArray[_hitobjectIndex];
        }

        public void CheckIfAllPlayed()
        {
            if (hitobjectManager.CurrentHittableObjectIndex >= totalHitobjects)
            {
                allObjectsPlayed = true;
            }
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            hitobjectManager = MonoBehaviour.FindObjectOfType<HitobjectManager>();
            gameManager = MonoBehaviour.FindObjectOfType<GameManager>();
            gameplayTimeManager = MonoBehaviour.FindObjectOfType<GameplayTimeManager>();
            playerSettings = MonoBehaviour.FindObjectOfType<PlayerSettings>();
            colorCollection = MonoBehaviour.FindObjectOfType<ColorCollection>();
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
            mainHitobjectPoolArray[hitobjectIndex].UpdateNumberText((hitobjectIndex + 1).ToString());
            mainHitobjectPoolArray[hitobjectIndex].CachedTransform.localPosition = positionArray[hitobjectIndex];
            mainHitobjectPoolArray[hitobjectIndex].CachedTransform.SetAsFirstSibling();
            mainHitobjectPoolArray[hitobjectIndex].UpdateColorImageColor(colorCollection.GetRandomHitobjectColor());
            mainHitobjectPoolArray[hitobjectIndex].gameObject.SetActive(true);
            SetHittableObject();
            IncrementHitobjectIndex();
            CheckIfAllObjectsHaveSpawned();
        }

        private void SetHittableObject()
        {
            if (hitobjectIndex == 0)
            {
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
}
