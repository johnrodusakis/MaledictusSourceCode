using System;
using System.Text.RegularExpressions;

namespace Maledictus
{
    public static class AbbreviateNumbers
    {
        public static string AbbreviateNumber(int number)
        {
            const int billion = 1_000_000_000;
            const int million = 1_000_000;
            const int thousand = 1_000;

            if (number >= billion)
                return (number / (float)billion).ToString("F1") + "B"; // Billions
            else if (number >= million)
                return (number / (float)million).ToString("F1") + "M"; // Millions
            else if (number >= thousand)
                return (number / (float)thousand).ToString("F1") + "K"; // Thousands
            else
                return number.ToString(); // No abbreviation needed
        }
    }

    public static class EnumExtentions
    {
        public static string ToSpacedString(this Enum value) => Regex.Replace(value.ToString(), "(\\B[A-Z])", " $1");

        public static T GetNext<T>(this T value) where T : Enum 
        {
            T[] values = (T[])Enum.GetValues(value.GetType());
            int index = Array.IndexOf(values, value) + 1;
            value = (index >= values.Length) ? values[0] : values[index];
            return value;
        }
    }
}