using UnityEngine;

public class Hitobject0 : Hitobject 
{
    #region Protected Methods
    protected override void SetMissKeyCodeArray()
    {
        base.SetMissKeyCodeArray();
        missKeyCodeArray = new KeyCode[] { KeyCode.D, KeyCode.F };
    }

    protected override void SetHitKeyCodeArray()
    {
        base.SetHitKeyCodeArray();
        missKeyCodeArray = new KeyCode[] { KeyCode.J, KeyCode.K };
    }
    #endregion
}
