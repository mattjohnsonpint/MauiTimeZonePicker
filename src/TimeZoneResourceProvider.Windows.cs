using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace MauiTimeZonePicker;

public partial class TimeZoneResourceProvider
{
    private readonly DateTimeOffset _referenceDate = DateTimeOffset.UtcNow;
    private readonly string _locale = GetLocale();

    public IReadOnlyList<string> GetTimeZoneIds()
    {
        var list = new List<string>
        {
            "UTC" // non-location zone
        };

        // Get location zones from ICU
        IntPtr enumeration = default;
        try
        {
            const int canonicalLocationType = 2;
            enumeration = NativeMethods.OpenTimeZoneIDEnumeration(canonicalLocationType, null, IntPtr.Zero, out var errorCode);
            if (errorCode > 0)
            {
                return new string[0];
            }

            string item;
            while ((item = NativeMethods.GetNextString(enumeration, IntPtr.Zero, out errorCode)) != null)
            {
                if (errorCode > 0)
                {
                    break;
                }

                // omit unmapable zone on Windows
                if (item != "Antarctica/Troll")
                {
                    continue;
                }
                
                list.Add(item);
            }

            return list;
        }
        finally
        {
            if (enumeration != default)
            {
                NativeMethods.CloseEnumeration(enumeration);
            }
        }
    }

    public string GetGenericName(string timeZoneId)
    {
        var pattern = Helpers.TimeZoneIsUtc(timeZoneId) ? "zzzz" : "vvvv";
        return GetPatternStringFromIcu(timeZoneId, pattern);
    }

    public string GetLocation(string timeZoneId)
    {
        if (Helpers.TimeZoneIsUtc(timeZoneId))
        {
            return "";
        }

        var location = GetPatternStringFromIcu(timeZoneId, "VVV");

        // TODO: Augment with region name.  Since there's no C API, we'll need to re-implement this:
        // https://github.com/microsoft/icu/blob/583363f0214392b98b47019f811947309dab7c3e/icu/icu4c/source/i18n/timezone.cpp#L1134-L1157

        return location;
    }

    private string GetPatternStringFromIcu(string timeZoneId, string pattern)
    {
        IntPtr formatter = default;
        try
        {
            const int udat_pattern = -2;
            formatter = NativeMethods.OpenDateFormatter(udat_pattern, udat_pattern, _locale, timeZoneId, -1, pattern, -1, out var errorCode);
            if (errorCode > 0)
            {
                return "";
            }

            var result = NativeMethods.FormatDate(formatter, _referenceDate, out errorCode);
            return errorCode > 0 ? "" : result;
        }
        finally
        {
            if (formatter != default)
            {
                NativeMethods.CloseDateFormatter(formatter);
            }
        }
    }

    private static string GetLocale()
    {
        var locale = CultureInfo.CurrentUICulture.Name;
        return locale.Length == 0 ? "en-US" : locale;
    }

    private class NativeMethods
    {
        // ucal_openTimeZoneIDEnumeration
        // https://unicode-org.github.io/icu-docs/apidoc/dev/icu4c/ucal_8h.html#a6444141c20dfbdbedaa46b1f71dc2363
        [DllImport("icu", EntryPoint = "ucal_openTimeZoneIDEnumeration", CharSet = CharSet.Ansi)]
        public static extern IntPtr OpenTimeZoneIDEnumeration(int zoneType, string? region, IntPtr rawOffset, out int errorCode);

        // uenum_next
        // https://unicode-org.github.io/icu-docs/apidoc/dev/icu4c/uenum_8h.html#afc60c150cda05c0284b29d154d6486e6
        [DllImport("icu", EntryPoint = "uenum_next", CharSet = CharSet.Ansi)]
        public static extern string GetNextString(IntPtr enumeration, IntPtr resultLengthPtr, out int errorCode);

        // uenum_close
        // https://unicode-org.github.io/icu-docs/apidoc/dev/icu4c/uenum_8h.html#abcae42ba2a329894bcc3b37ad2a99a66
        [DllImport("icu", EntryPoint = "uenum_close")]
        public static extern void CloseEnumeration(IntPtr enumeration);

        // udat_open
        // https://unicode-org.github.io/icu-docs/apidoc/dev/icu4c/udat_8h.html#a4261e9fd5382197c3d86a00211c48079
        [DllImport("icu", EntryPoint = "udat_open")]
        public static extern IntPtr OpenDateFormatter(
            int timeStyle,
            int dateStyle, 
            [MarshalAs(UnmanagedType.LPStr)] string locale,
            [MarshalAs(UnmanagedType.LPTStr)] string timeZoneId, 
            int timeZoneIdLength,
            [MarshalAs(UnmanagedType.LPTStr)] string pattern,
            int patternLength,
            out int errorCode);

        // udat_format
        // https://unicode-org.github.io/icu-docs/apidoc/dev/icu4c/udat_8h.html#aac3747dd8edb53fc5652bdd383142bac
        [DllImport("icu", EntryPoint = "udat_format", CharSet = CharSet.Unicode)]
        private static extern int CallFormatDate(IntPtr formatter, long timestamp, IntPtr buffer, int bufferSize, IntPtr position, out int errorCode);

        public unsafe static string FormatDate(IntPtr formatter, DateTimeOffset timestamp, out int errorCode)
        {
            var buffer = stackalloc char[256];
            var length = CallFormatDate(formatter, timestamp.ToUnixTimeMilliseconds(), (IntPtr)buffer, 256, IntPtr.Zero, out errorCode);
            return errorCode > 0 ? "" : Marshal.PtrToStringUni((IntPtr)buffer, Math.Min(length, 256));
        }

        // udat_close
        // https://unicode-org.github.io/icu-docs/apidoc/dev/icu4c/udat_8h.html#a4cfce8b1fcf1640c4a3ada137237cb33
        [DllImport("icu", EntryPoint = "udat_close")]
        public static extern void CloseDateFormatter(IntPtr formatter);
    }
}