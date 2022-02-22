using System;
using UnityEngine;


public static class FormatNumsHelper
{
    private static string[] names = new[]
    {
            "",
            "K",
            "M",
            "B",
            "T",
            "Q"
    };

    public static string FormatNum(double num)
    {
        if (num == 0) return "0";

        num = Mathf.Round((float)num);

        int i = 0;
        while (i + 1 < names.Length && num >= 1000d)
        {
            num /= 1000d;
            i++;
        }

        return num.ToString("#.##") + names[i];
    }

    public static string FormatNum(float num)
    {
        if (num == 0) return "0";

        num = Mathf.Round(num);

        int i = 0;
        while (i + 1 < names.Length && num >= 1000f)
        {
            num /= 1000f;
            i++;
        }
        return num.ToString("#.##") + names[i];
    }

    public static string FormatNum(decimal num)
    {
        if (num == 0) return "0";

        num = Decimal.Round(num);

        int i = 0;
        while (i + 1 < names.Length && num >= 1000m)
        {
            num /= 1000m;
            i++;
        }

        return num.ToString("#.##") + names[i];
    }

}