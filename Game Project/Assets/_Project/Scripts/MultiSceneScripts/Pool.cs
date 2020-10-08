using System;
using UnityEngine;

[Serializable]
public abstract class Pool
{
    #region Private Fields
    [SerializeField] private byte type;
    [SerializeField] private ushort size;
    #endregion

    #region Properties
    public byte Type => type;
    public ushort Size => size;
    #endregion
}
