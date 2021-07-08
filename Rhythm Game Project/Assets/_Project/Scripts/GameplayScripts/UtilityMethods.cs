using System;
using System.Text;
using UnityEngine.UI;

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

    // Return string of days, hours or minutes from the time passed and the current time
    public static string GetTimeSinceDateInput(string _dateInput)
    {
        DateTime parsedDate = DateTime.Parse(_dateInput);
        TimeSpan span = (DateTime.Now - parsedDate);
        string time = "";

        if (span.Days < 1)
        {
            time = ($"{span.Hours} hours {span.Minutes} minutes ago");

            if (span.Hours < 1)
            {
                time = ($"{span.Minutes} minutes ago");
            }
        }
        else
        {
            time = ($"{span.Days} days ago");
        }

        return time;
    }

    public static string GetTimeFromFloat(float _value)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(_value);
        return timeSpan.ToString();
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

    public static float GetSliderValuePercentageFromTime(float _audioStartTime, float _audioClipLength)
    {
        float percentage = (_audioStartTime / _audioClipLength) * 100;
        return percentage;
    }

    public static bool GetBoolFromString(string _string)
    {
        if (_string == "1")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void SelectNextSelectable(Selectable _currentSelectable)
    {
        Selectable selectable = _currentSelectable.navigation.selectOnRight;
        selectable.Select();
    }
}