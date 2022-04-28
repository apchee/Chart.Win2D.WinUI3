using System;

namespace ChartBase.Chart;
public static class DataProcessUtil
{


    public static (string formatedValue, float newDecimal, int division, string unit) FormatDecimal(float value)
    {
        float absNum = Math.Abs(value);

        string formatedValue = value.ToString();
        float newDecimal = 0;
        int division = 1;
        string unit = "";

        if (absNum >= 1000)
        {

            if (absNum >= 1000_000_000)
            {
                formatedValue = (value / 1000_000_000D).ToString("0.##");
                newDecimal = Convert.ToSingle(formatedValue);
                division = 1000_000_000;
                unit = "B";
            }
            else
            if (absNum >= 1000_000)
            {
                formatedValue = (value / 1000_000).ToString("0.##");
                newDecimal = Convert.ToSingle(formatedValue);
                division = 1000_000;
                unit = "M";
            }
            else
            if (absNum >= 1000)
            {
                formatedValue = (value / 1000).ToString("0.##");
                newDecimal = Convert.ToSingle(formatedValue);
                division = 1000;
                unit = "K";
            }

        }
        else if (absNum < 1 && absNum > 0)
        {
            if (absNum < 0.0001)
            {
                //formatedValue = ToLongString(value);
                formatedValue = ((decimal)(value)).ToString().TrimEnd('0');
                newDecimal = value;
            }
            else if (absNum < 0.001)
            {
                formatedValue = value.ToString("0.#####").TrimEnd('0');
                newDecimal = Convert.ToSingle(formatedValue);
            }
            else if (absNum < 0.01)
            {
                formatedValue = value.ToString("0.####").TrimEnd('0');
                newDecimal = Convert.ToSingle(formatedValue);
            }
            else if (absNum < 0.1)
            {
                formatedValue = value.ToString("0.###").TrimEnd('0');
                newDecimal = Convert.ToSingle(formatedValue);
            }
            else
            {
                formatedValue = value.ToString("0.##").TrimEnd('0');
                newDecimal = Convert.ToSingle(formatedValue);
            }

        }
        else
        {
            var strValue = value.ToString("0.##");
            if (strValue.LastIndexOf(".") > 0)
                formatedValue = strValue.TrimEnd('0');
            else
                formatedValue = strValue;

            if (value != 0)
            {
                newDecimal = Convert.ToSingle(formatedValue);
            }
            else
            {
                newDecimal = 0;
            }
        }

        return (formatedValue, newDecimal, division, unit);

    }



    //public static (int Div, string Unit) devision(float minWorldValue)
    //{
    //    int divisorOrFraction = 1;
    //    string unit = "";
    //    float value = Math.Abs(minWorldValue);
    //    if (value >= 1000)
    //    {
    //        if (value >= 1000 && value < 1000000)
    //        {
    //            divisorOrFraction = 1000;
    //            unit = "K";
    //        }
    //        else if (value >= 1000000 && value < 1000000000)
    //        {
    //            divisorOrFraction = 1000000;
    //            unit = "M";
    //        }
    //        else if (value >= 1000000000)
    //        {
    //            divisorOrFraction = 1000000000;
    //            unit = "B";
    //        }
    //    }
    //    else if (minWorldValue > -1)
    //    {
    //        if (minWorldValue >= 0.1)
    //        {
    //            divisorOrFraction = 1;
    //        }
    //        else if (minWorldValue >= 0.01)
    //        {
    //            divisorOrFraction = 2;
    //        }
    //        else if (minWorldValue >= 0.001)
    //        {
    //            divisorOrFraction = 3;
    //        }
    //        else if (minWorldValue >= 0.0001)
    //        {
    //            divisorOrFraction = 4;
    //        }
    //        else
    //        {
    //            divisorOrFraction = -1;
    //        }
    //    }
    //    return (divisorOrFraction, unit);
    //}
}
