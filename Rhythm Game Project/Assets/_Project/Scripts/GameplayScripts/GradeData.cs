namespace Grade
{
    using Enums;
    using UnityEngine;
    using TMPro;

    public sealed class GradeData : MonoBehaviour
    {
        #region Constants
        public const string GradeStringSPlus = "S+";
        public const string GradeStringS = "S";
        public const string GradeStringAPlus = "A+";
        public const string GradeStringA = "A";
        public const string GradeStringBPlus = "B+";
        public const string GradeStringB = "B";
        public const string GradeStringCPlus = "C+";
        public const string GradeStringC = "C";
        public const string GradeStringDPlus= "D+";
        public const string GradeStringD = "D";
        public const string GradeStringEPlus = "E+";
        public const string GradeStringE = "E";
        public const string GradeStringFPlus = "F+";
        public const string GradeStringF = "F";

        public const byte GradeRequiredSPlus = 100;
        public const byte GradeRequiredS = 95;
        public const byte GradeRequiredAPlus = 90;
        public const byte GradeRequiredA = 86;
        public const byte GradeRequiredBPlus = 80;
        public const byte GradeRequiredB = 75;
        public const byte GradeRequiredCPlus = 70;
        public const byte GradeRequiredC = 65;
        public const byte GradeRequiredDPlus = 60;
        public const byte GradeRequiredD = 55;
        public const byte GradeRequiredEPlus = 50;
        public const byte GradeRequiredE = 45;
        public const byte GradeRequiredFPlus = 40;
        public const byte GradeRequiredF = 0;

        [SerializeField] private TMP_ColorGradient gradientS = default;
        [SerializeField] private TMP_ColorGradient gradientA = default;
        [SerializeField] private TMP_ColorGradient gradientB = default;
        [SerializeField] private TMP_ColorGradient gradientC = default;
        [SerializeField] private TMP_ColorGradient gradientD = default;
        [SerializeField] private TMP_ColorGradient gradientE = default;
        [SerializeField] private TMP_ColorGradient gradientF = default;

        [SerializeField] private Color32 colorS = default;
        [SerializeField] private Color32 colorA = default;
        [SerializeField] private Color32 colorB = default;
        [SerializeField] private Color32 colorC = default;
        [SerializeField] private Color32 colorD = default;
        [SerializeField] private Color32 colorE = default;
        [SerializeField] private Color32 colorF = default;
        #endregion

        #region Public Methods
        public static Grade GetCurrentGrade(float _accuracy)
        {
            if (_accuracy == GradeRequiredSPlus)
            {
                return Grade.S_PLUS;
            }
            else if (_accuracy < GradeRequiredSPlus && _accuracy >= GradeRequiredS)
            {
                return Grade.S;
            }
            else if (_accuracy < GradeRequiredS && _accuracy >= GradeRequiredAPlus)
            {
                return Grade.A_PLUS;
            }
            else if (_accuracy < GradeRequiredAPlus && _accuracy >= GradeRequiredA)
            {
                return Grade.A;
            }
            else if (_accuracy < GradeRequiredA && _accuracy >= GradeRequiredBPlus)
            {
                return Grade.B_PLUS;
            }
            else if (_accuracy < GradeRequiredBPlus && _accuracy >= GradeRequiredB)
            {
                return Grade.B;
            }
            else if (_accuracy < GradeRequiredB && _accuracy >= GradeRequiredCPlus)
            {
                return Grade.C_PLUS;
            }
            else if (_accuracy < GradeRequiredCPlus && _accuracy >= GradeRequiredC)
            {
                return Grade.C;
            }
            else if (_accuracy < GradeRequiredC && _accuracy >= GradeRequiredDPlus)
            {
                return Grade.D_PLUS;
            }
            else if (_accuracy < GradeRequiredDPlus && _accuracy >= GradeRequiredD)
            {
                return Grade.D;
            }
            else if (_accuracy < GradeRequiredD && _accuracy >= GradeRequiredEPlus)
            {
                return Grade.E_PLUS;
            }
            else if (_accuracy < GradeRequiredEPlus && _accuracy >= GradeRequiredE)
            {
                return Grade.E;
            }
            else if (_accuracy < GradeRequiredE && _accuracy >= GradeRequiredFPlus)
            {
                return Grade.F_PLUS;
            }
            else
            {
                return Grade.F;
            }
        }

        public static string GetCurrentGradeString(Grade _grade)
        {
            switch (_grade)
            {
                case Grade.S_PLUS:
                    return GradeStringSPlus;
                case Grade.S:
                    return GradeStringS;
                case Grade.A_PLUS:
                    return GradeStringAPlus;
                case Grade.A:
                    return GradeStringA;
                case Grade.B_PLUS:
                    return GradeStringBPlus;
                case Grade.B:
                    return GradeStringB;
                case Grade.C_PLUS:
                    return GradeStringCPlus;
                case Grade.C:
                    return GradeStringC;
                case Grade.D_PLUS:
                    return GradeStringDPlus;
                case Grade.D:
                    return GradeStringD;
                case Grade.E_PLUS:
                    return GradeStringEPlus;
                case Grade.E:
                    return GradeStringE;
                case Grade.F_PLUS:
                    return GradeStringFPlus;
                case Grade.F:
                    return GradeStringF;
                default: return GradeStringF;
            }
        }

        public Color32 GetCurrentGradeColor(Grade _grade)
        {
            switch (_grade)
            {
                case Grade.S_PLUS:
                    return colorS;
                case Grade.S:
                    return colorS;
                case Grade.A_PLUS:
                    return colorA;
                case Grade.A:
                    return colorA;
                case Grade.B_PLUS:
                    return colorB;
                case Grade.B:
                    return colorB;
                case Grade.C_PLUS:
                    return colorC;
                case Grade.C:
                    return colorC;
                case Grade.D_PLUS:
                    return colorD;
                case Grade.D:
                    return colorD;
                case Grade.E_PLUS:
                    return colorE;
                case Grade.E:
                    return colorE;
                case Grade.F_PLUS:
                    return colorF;
                default:
                    return colorF;
            }
        }

        public TMP_ColorGradient GetCurrentGradeGradient(Grade _grade)
        {
            switch (_grade)
            {
                case Grade.S_PLUS:
                    return gradientS;
                case Grade.S:
                    return gradientS;
                case Grade.A_PLUS:
                    return gradientA;
                case Grade.A:
                    return gradientA;
                case Grade.B_PLUS:
                    return gradientB;
                case Grade.B:
                    return gradientB;
                case Grade.C_PLUS:
                    return gradientC;
                case Grade.C:
                    return gradientC;
                case Grade.D_PLUS:
                    return gradientD;
                case Grade.D:
                    return gradientD;
                case Grade.E_PLUS:
                    return gradientE;
                case Grade.E:
                    return gradientE;
                case Grade.F_PLUS:
                    return gradientF;
                default:
                    return gradientF;
            }
        }
        #endregion
    }
}
