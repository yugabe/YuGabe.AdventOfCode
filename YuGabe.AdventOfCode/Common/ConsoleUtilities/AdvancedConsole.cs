using System.Drawing;

namespace YuGabe.AdventOfCode.ConsoleUtilities;

public static class AdvancedConsole
{
    private const char Esc = '\x1B';   //''

    public static string Format(object value, string rawFormat) => $"{Esc}[{rawFormat}m{value}{Esc}[0m";
    public static string Format(object value, FormatCode code) => $"{Esc}[{(int)code}m{value}{Esc}[0m";
    public static string Foreground(object value, ForegroundCode code) => Format(value, (FormatCode)code);
    public static string Foreground(object value, Color color) => Format(value, $"{(int)FormatCode.ForegroundExtended};2;{color.R};{color.G};{color.B}");
    public static string Foreground(object value, int sgrColorCode) => Format(value, $"{(int)FormatCode.ForegroundExtended};5;{sgrColorCode}");
    public static string Background(object value, BackgroundCode code) => Format(value, (FormatCode)code);
    public static string Background(object value, Color color) => Format(value, $"{(int)FormatCode.BackgroundExtended};2;{color.R};{color.G};{color.B}");
    public static string Background(object value, int sgrColorCode) => Format(value, $"{(int)FormatCode.BackgroundExtended};5;{sgrColorCode}");

    public static string Default(object value) => Format(value, FormatCode.Default);
    public static string Bold(object value) => Format(value, FormatCode.Bold);
    public static string NoBold(object value) => Format(value, FormatCode.NoBold);
    public static string Underline(object value) => Format(value, FormatCode.Underline);
    public static string NoUnderline(object value) => Format(value, FormatCode.NoUnderline);
    public static string Negative(object value) => Format(value, FormatCode.Negative);
    public static string Positive(object value) => Format(value, FormatCode.Positive);
    public static string ForegroundBlack(object value) => Format(value, FormatCode.ForegroundBlack);
    public static string ForegroundRed(object value) => Format(value, FormatCode.ForegroundRed);
    public static string ForegroundGreen(object value) => Format(value, FormatCode.ForegroundGreen);
    public static string ForegroundYellow(object value) => Format(value, FormatCode.ForegroundYellow);
    public static string ForegroundBlue(object value) => Format(value, FormatCode.ForegroundBlue);
    public static string ForegroundMagenta(object value) => Format(value, FormatCode.ForegroundMagenta);
    public static string ForegroundCyan(object value) => Format(value, FormatCode.ForegroundCyan);
    public static string ForegroundWhite(object value) => Format(value, FormatCode.ForegroundWhite);
    public static string ForegroundExtended(object value) => Format(value, FormatCode.ForegroundExtended);
    public static string ForegroundDefault(object value) => Format(value, FormatCode.ForegroundDefault);
    public static string BackgroundBlack(object value) => Format(value, FormatCode.BackgroundBlack);
    public static string BackgroundRed(object value) => Format(value, FormatCode.BackgroundRed);
    public static string BackgroundGreen(object value) => Format(value, FormatCode.BackgroundGreen);
    public static string BackgroundYellow(object value) => Format(value, FormatCode.BackgroundYellow);
    public static string BackgroundBlue(object value) => Format(value, FormatCode.BackgroundBlue);
    public static string BackgroundMagenta(object value) => Format(value, FormatCode.BackgroundMagenta);
    public static string BackgroundCyan(object value) => Format(value, FormatCode.BackgroundCyan);
    public static string BackgroundWhite(object value) => Format(value, FormatCode.BackgroundWhite);
    public static string BackgroundExtended(object value) => Format(value, FormatCode.BackgroundExtended);
    public static string BackgroundDefault(object value) => Format(value, FormatCode.BackgroundDefault);
    public static string BrightForegroundBlack(object value) => Format(value, FormatCode.BrightForegroundBlack);
    public static string BrightForegroundRed(object value) => Format(value, FormatCode.BrightForegroundRed);
    public static string BrightForegroundGreen(object value) => Format(value, FormatCode.BrightForegroundGreen);
    public static string BrightForegroundYellow(object value) => Format(value, FormatCode.BrightForegroundYellow);
    public static string BrightForegroundBlue(object value) => Format(value, FormatCode.BrightForegroundBlue);
    public static string BrightForegroundMagenta(object value) => Format(value, FormatCode.BrightForegroundMagenta);
    public static string BrightForegroundCyan(object value) => Format(value, FormatCode.BrightForegroundCyan);
    public static string BrightForegroundWhite(object value) => Format(value, FormatCode.BrightForegroundWhite);
    public static string BrightBackgroundBlack(object value) => Format(value, FormatCode.BrightBackgroundBlack);
    public static string BrightBackgroundRed(object value) => Format(value, FormatCode.BrightBackgroundRed);
    public static string BrightBackgroundGreen(object value) => Format(value, FormatCode.BrightBackgroundGreen);
    public static string BrightBackgroundYellow(object value) => Format(value, FormatCode.BrightBackgroundYellow);
    public static string BrightBackgroundBlue(object value) => Format(value, FormatCode.BrightBackgroundBlue);
    public static string BrightBackgroundMagenta(object value) => Format(value, FormatCode.BrightBackgroundMagenta);
    public static string BrightBackgroundCyan(object value) => Format(value, FormatCode.BrightBackgroundCyan);
    public static string BrightBackgroundWhite(object value) => Format(value, FormatCode.BrightBackgroundWhite);
}
