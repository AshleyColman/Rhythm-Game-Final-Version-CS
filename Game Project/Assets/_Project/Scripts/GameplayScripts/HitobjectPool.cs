using System;
using UnityEngine;

[Serializable]
public sealed class HitobjectPool : Pool
{
    #region Private Fields
    [SerializeField] private Hitobject prefab;
    #endregion

    #region Properties
    public Hitobject Prefab => prefab;
    #endregion
}