using System.Text;

public static class UtilityMethods
{
    public static string FromSecondsToMinutesAndSeconds(float _seconds)
    {
        int sec = (int)(_seconds % 60f);
        int min = (int)((_seconds / 60f) % 60f);
        return $"{min.ToString("D2")} : {sec.ToString("D2")}";
    }

    public static StringBuilder AddZerosToScoreString(StringBuilder _stringBuilder)
    {
        switch (_stringBuilder.Length)
        {
            case 0:
                _stringBuilder.Insert(0, "00000000");
                break;
            case 1:
                _stringBuilder.Insert(0, "0000000");
                break;
            case 2:
                _stringBuilder.Insert(0, "000000");
                break;
            case 3:
                _stringBuilder.Insert(0, "00000");
                break;
            case 4:
                _stringBuilder.Insert(0, "0000");
                break;
            case 5:
                _stringBuilder.Insert(0, "000");
                break;
            case 6:
                _stringBuilder.Insert(0, "00");
                break;
            case 7:
                _stringBuilder.Insert(0, "0");
                break;
        }
        return _stringBuilder;
    }

    public static string AddZerosToScoreString(string _string)
    {
        switch (_string.Length)
        {
            case 0:
                _string = _string.Insert(0, "00000000");
                break;
            case 1:
                _string = _string.Insert(0, "0000000");
                break;
            case 2:
                _string = _string.Insert(0, "000000");
                break;
            case 3:
                _string = _string.Insert(0, "00000");
                break;
            case 4:
                _string = _string.Insert(0, "0000");
                break;
            case 5:
                _string = _string.Insert(0, "000");
                break;
            case 6:
                _string = _string.Insert(0, "00");
                break;
            case 7:
                _string = _string.Insert(0, "0");
                break;
        }
        return _string;
    }

    public static float GetSliderValuePercentageFromTime(float _audioStartTime, float _audioClipLength)
    {
        float percentage = (_audioStartTime / _audioClipLength) * 100;
        return percentage;
    }

    public static float GetAverageFromNumberArr(float[] _numberArr)
    {
        int totalIncrements = 0;
        float average = 0f;

        for (int i = 0; i < _numberArr.Length; i++)
        {
            if (_numberArr[i] != 0)
            {
                average += _numberArr[i];
                totalIncrements++;
            }
        }

        if (totalIncrements == 0)
        {
            average = 0f;
        }
        else
        {
            average = ((average / (totalIncrements * 100) * 100));
        }

        return average;
    }
}