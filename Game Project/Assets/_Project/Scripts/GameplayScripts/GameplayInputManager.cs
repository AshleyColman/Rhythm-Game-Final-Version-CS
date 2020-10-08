using UnityEngine;
using System.Collections;

public sealed class GameplayInputManager : InputManager
{
    #region Private Fields
    private bool hasCheckedInputForStartingGameplay = false;

    private IEnumerator checkInputToStartGameplay;
    private IEnumerator checkInputForHitobjects;

    private GameManager gameManager;
    private HitobjectManager hitObjectManager;
    private Countdown countdown;
    #endregion

    #region Public Methods
    public void CheckInputForHitobjects()
    {
        StopCoroutine(checkInputForHitobjects);
        StartCoroutine(checkInputForHitobjects);
    }
    #endregion

    #region Private Methods
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        hitObjectManager = FindObjectOfType<HitobjectManager>();
        countdown = FindObjectOfType<Countdown>();
        checkInputToStartGameplay = CheckInputToStartGameplayCoroutine();
        checkInputForHitobjects = CheckInputForHitobjectsCoroutine();
    }

    private void Start()
    {
        CheckInputToStartGameplay();
    }

    private void CheckInputToStartGameplay()
    {
        StopCoroutine(checkInputToStartGameplay);
        StartCoroutine(checkInputToStartGameplay);
    }

    private IEnumerator CheckInputToStartGameplayCoroutine()
    {
        while (hasCheckedInputForStartingGameplay == false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("START GAMEPLAY");
                gameManager.PrepareToStartGameplay();
            }

            yield return null;
        }

        StopCoroutine(checkInputToStartGameplay);

        yield return null;
    }

    private IEnumerator CheckInputForHitobjectsCoroutine()
    {
        while (gameManager.GameplayStarted == true)
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(KeyCode.D))
                {
                    hitObjectManager.CheckKeysPressed(KeyCode.D);
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    hitObjectManager.CheckKeysPressed(KeyCode.D);
                }

                if (Input.GetKeyDown(KeyCode.J))
                {
                    hitObjectManager.CheckKeysPressed(KeyCode.D);
                }

                if (Input.GetKeyDown(KeyCode.K))
                {
                    hitObjectManager.CheckKeysPressed(KeyCode.D);
                }
            }
        }

        yield return null;
    }
    #endregion
}
