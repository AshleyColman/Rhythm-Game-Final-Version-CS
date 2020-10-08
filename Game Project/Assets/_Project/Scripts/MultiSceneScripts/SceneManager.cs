using UnityEngine;

public sealed class SceneManager : MonoBehaviour
{
    #region Constants
    private const byte gameplaySceneIndex = 0;
    #endregion

    #region Private Fields
    private byte sceneIndex = 0;
    #endregion

    #region Properties
    public byte SceneIndex => sceneIndex;
    public byte GameplaySceneIndex => gameplaySceneIndex;
    #endregion
}
