namespace Gameplay
{
    using System.Collections;
    using UnityEngine;
    using InputManager;
    using Audio;

    public sealed class GameplayInputManager : InputManager
    {
        #region Private Fields
        private bool hasCheckedInputForStartingGameplay = false;

        [SerializeField] private Key[] keyArray = default;

        private IEnumerator checkInputForGameplayCoroutine;
        private IEnumerator checkInputToStartGameplayCoroutine;

        private GameManager gameManager;
        private HitobjectManager hitobjectManager;
        private Countdown countdown;
        private GameplayAudioManager gameplayAudioManager;
        private HitobjectSpawnManager hitobjectSpawnManager;
        private FeverManager feverManager;
        #endregion

        #region Public Methods
        public void CheckInputForGameplay()
        {
            if (checkInputForGameplayCoroutine != null)
            {
                StopCoroutine(checkInputForGameplayCoroutine);
            }

            checkInputForGameplayCoroutine = CheckInputForGameplayCoroutine();
            StartCoroutine(checkInputForGameplayCoroutine);
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            gameManager = MonoBehaviour.FindObjectOfType<GameManager>();
            hitobjectManager = MonoBehaviour.FindObjectOfType<HitobjectManager>();
            countdown = MonoBehaviour.FindObjectOfType<Countdown>();
            gameplayAudioManager = MonoBehaviour.FindObjectOfType<GameplayAudioManager>();
            hitobjectSpawnManager = MonoBehaviour.FindObjectOfType<HitobjectSpawnManager>();
            feverManager = MonoBehaviour.FindObjectOfType<FeverManager>();
        }

        private void Start()
        {
            CheckInputToStartGameplay();
        }

        private void CheckInputToStartGameplay()
        {
            if (checkInputToStartGameplayCoroutine != null)
            {
                StopCoroutine(checkInputToStartGameplayCoroutine);
            }

            checkInputToStartGameplayCoroutine = CheckInputToStartGameplayCoroutine();
            StartCoroutine(checkInputToStartGameplayCoroutine);
        }

        private IEnumerator CheckInputToStartGameplayCoroutine()
        {
            while (hasCheckedInputForStartingGameplay == false)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    gameManager.PrepareToStartGameplay();
                    StopCoroutine("CheckInputToStartGameplayCoroutine");
                    gameplayAudioManager.PlayOneShotUserInterfaceAudioSource(AudioManager.Select1AudioClipIndex);
                    hasCheckedInputForStartingGameplay = true;
                }

                yield return null;
            }

            yield return null;
        }

        private IEnumerator CheckInputForGameplayCoroutine()
        {
            while (gameManager.GameplayStarted == true)
            {
                if (Input.anyKeyDown)
                {
                    CheckHitKeyInput();
                    CheckFeverInput();
                }

                yield return null;
            }

            yield return null;
        }

        private void Update()
        {
            CheckKeyUI();
        }

        private void CheckHitKeyInput()
        {
            if (Input.GetKeyDown(hitobjectManager.HitKeyCode))
            {
                hitobjectManager.CheckIfHit();
            }
        }

        private void CheckFeverInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                feverManager.TryToActivateFever();
            }
        }

        private void CheckKeyUI()
        {
            CheckKeyDown();
            CheckKeyUp();
        }

        private void CheckKeyDown()
        {
            if (Input.anyKey)
            {
                for (byte i = 0; i < keyArray.Length; i++)
                {
                    if (Input.GetKey(keyArray[i].KeyCode))
                    {
                        keyArray[i].PlayOnKeyDownAnimation();
                    }
                }
            }
        }

        private void CheckKeyUp()
        {
            for (byte i = 0; i < keyArray.Length; i++)
            {
                if (Input.GetKeyUp(keyArray[i].KeyCode))
                {
                    keyArray[i].PlayOnKeyReleaseAnimation();
                }
            }
        }
        #endregion
    }
}
