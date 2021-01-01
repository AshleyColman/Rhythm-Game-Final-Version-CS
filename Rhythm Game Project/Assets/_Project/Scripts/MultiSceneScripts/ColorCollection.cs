using UnityEngine;

public sealed class ColorCollection : MonoBehaviour
{
    #region Private Fields
    [SerializeField] Color32 redColor = default, redColor080 = default, redColor025 = default;
    [SerializeField] Color32 whiteColor = default, whiteColor080 = default, whiteColor025 = default;
    [SerializeField] Color32 orangeColor = default, orangeColor080 = default;
    [SerializeField] Color32 lightBlueColor = default, lightBlueColor080 = default;
    [SerializeField] Color32 darkBlueColor = default, darkBlueColor080 = default;
    [SerializeField] Color32 purpleColor = default, purpleColor080 = default;
    [SerializeField] Color32 pinkColor = default, pinkColor080 = default;
    [SerializeField] Color32 yellowColor = default, yellowColor080 = default;
    [SerializeField] Color32 lightGreenColor = default, lightGreenColor080 = default;

    [SerializeField] Color32[] testHitObjectColorArray = default; // TEST
    #endregion

    #region Properties
    public Color32 OrangeColor => orangeColor;
    public Color32 OrangeColor080 => orangeColor080;
    public Color32 LightBlueColor => lightBlueColor;
    public Color32 LightBlueColor080 => lightBlueColor080;
    public Color32 DarkBlueColor => darkBlueColor;
    public Color32 DarkBlueColor080 => darkBlueColor080;
    public Color32 PurpleColor => purpleColor;
    public Color32 PurpleColor080 => purpleColor080;
    public Color32 PinkColor => pinkColor;
    public Color32 PinkColor080 => pinkColor080;
    public Color32 YellowColor => yellowColor;
    public Color32 YellowColor080 => yellowColor080;
    public Color32 LightGreenColor => lightGreenColor;
    public Color32 LightGreenColor080 => lightGreenColor080;
    public Color32 RedColor => redColor;
    public Color32 RedColor080 => redColor080;
    public Color32 RedColor025 => redColor025;
    public Color32 WhiteColor => whiteColor;
    public Color32 WhiteColor080 => whiteColor080;
    public Color32 WhiteColor025 => whiteColor025;
    #endregion

    #region Public Methods
    public Color32 GetRandomHitobjectColor()
    {
        return testHitObjectColorArray[UnityEngine.Random.Range(0, testHitObjectColorArray.Length)];
    }
    #endregion
}
