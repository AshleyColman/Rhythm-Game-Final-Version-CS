using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public sealed class ToMainThread : MonoBehaviour
{
    #region Fields
    readonly static Queue<Action> queue = new Queue<Action>();

    static ToMainThread _instance = null;
    #endregion

    #region Methods
    // Check if exist another instance of this script.
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            //DontDestroyOnLoad (gameObject);
        }
    }

    // Start Actions when are ready.
    public void Update()
    {
        lock (queue)
        {
            while (queue.Count > 0)
            {
                queue.Dequeue().Invoke();
            }
        }

    }

    // Get IEnumerator's and prepare to execute on MainThread.
    public void ExecuteOnMainThread(IEnumerator action)
    {
        lock (queue)
        {
            queue.Enqueue(() => StartCoroutine(action));
        }
    }

    // Get Actions and prepare to execute on MainThread.
    public void ExecuteOnMainThread(Action action)
    {
        ExecuteOnMainThread(ExecuteAction(action));
    }

    // Execute the action.
    IEnumerator ExecuteAction(Action action)
    {
        action();
        yield return null;
    }

    // Check if a instance of this object exists.
    public static bool Exists()
    {
        return _instance != null;
    }

    // Prepare to assign new actions.
    public static ToMainThread AssignNewAction()
    {
        if (!Exists())
        {
            throw new Exception("Please add ToMainThreadPrefab to your scene.");
        }
        return _instance;
    }

    // (Fix) Set instance value to null to avoid errors at Assign New Actions if this Object has destroyed.
    void OnDestroy()
    {
        _instance = null;
    }
    #endregion
}