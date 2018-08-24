using Helper.Utility;
using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

public static class Extensions
{
    public static int ToInt(this string value)
    {
        int.TryParse(value, out int ret);
        return ret;
    }

    public static int ToInt(this double dblNumero)
    {
        int.TryParse(Math.Round(dblNumero).ToString(), out int intNumero);
        return intNumero;
    }

    public static int ToInt(this bool blnValor)
    {
        int intNumero = 0;
        if (blnValor)
        {
            intNumero = 1;
        }
        return intNumero;
    }

    public static double ToDouble(this string strNumero)
    {
        double.TryParse(strNumero, out double dblNumero);
        return dblNumero;
    }

    public static double ToDouble(this int intNumero)
    {
        double.TryParse(intNumero.ToString(), out double dblNumero);
        return dblNumero;
    }

    public static decimal ToDecimal(this string value)
    {
        return value.ToDecimal(new CultureInfo("en"));
    }

    public static decimal ToDecimal(this string value, CultureInfo cultureInfo)
    {
        decimal.TryParse(value, NumberStyles.Number, cultureInfo, out decimal ret);
        return ret;
    }

    /// <summary>
    /// Converts text to a date
    /// </summary>
    /// <param name="value">Datetime string</param>
    /// <param name="formatOrigin">format which is the date</param>
    /// <returns>converted string to datetime</returns>
    /// <example>"130909184731".ToDate("yyMMddHHmmSS")</example>
    public static DateTime ToDate(this string value, string formatOrigin)
    {
        try
        {
            DateTime ret = DateTime.Now;

            value = string.Format(
                        "{0}/{1}/{2} {3}:{4}:{5}",
                        (formatOrigin.Contains("d") ? value.SubstringStartEnd(formatOrigin.IndexOf("d"), formatOrigin.LastIndexOf("d") + 1).ToInt() : 0),
                        (formatOrigin.Contains("M") ? value.SubstringStartEnd(formatOrigin.IndexOf("M"), formatOrigin.LastIndexOf("M") + 1).ToInt() : 0),
                        (formatOrigin.Contains("y") ? value.SubstringStartEnd(formatOrigin.IndexOf("y"), formatOrigin.LastIndexOf("y") + 1).ToInt() : 0),
                        (formatOrigin.Contains("H") ? value.SubstringStartEnd(formatOrigin.IndexOf("H"), formatOrigin.LastIndexOf("H") + 1).ToInt() : 0),
                        (formatOrigin.Contains("m") ? value.SubstringStartEnd(formatOrigin.IndexOf("m"), formatOrigin.LastIndexOf("m") + 1).ToInt() : 0),
                        (formatOrigin.Contains("S") ? value.SubstringStartEnd(formatOrigin.IndexOf("S"), formatOrigin.LastIndexOf("S") + 1).ToInt() : 0)
                    );

            DateTime.TryParse(value, new CultureInfo("pt-br"), DateTimeStyles.None, out ret);

            return ret;
        }
        catch (Exception)
        {
            return DateTime.Now;
        }

    }

    public static string SubstringStartEnd(this string value, int start, int end)
    {
        return value.Substring(start, end - start);
    }

    public static string AppSettings(this string value)
    {
        return Ready.AppSettings[value];
    }

    public static string UTF8Encode(this string text)
    {
        byte[] bytes = Encoding.Default.GetBytes(text);
        return Encoding.UTF8.GetString(bytes);
    }

    public static string RemoveDiacritics(this string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    public static string CleanInvalidCharacters(this string text)
    {
        text = Regex.Replace(text, "[*ªº°§¹²³£¢¬']", "");
        text = Regex.Replace(text, "[&]", "e");
        return text;
    }
}
