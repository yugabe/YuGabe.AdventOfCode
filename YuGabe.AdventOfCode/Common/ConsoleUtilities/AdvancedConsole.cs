using System.Drawing;

namespace YuGabe.AdventOfCode.ConsoleUtilities;

public static class AdvancedConsole
{
    private const char Esc = '\x1B';   //''

    public static string Format(string text, string rawFormat) => $"{Esc}[{rawFormat}m{text}{Esc}[0m";
    public static string Format(string text, FormatCode code) => $"{Esc}[{(int)code}m{text}{Esc}[0m";
    public static string Foreground(string text, ForegroundCode code) => Format(text, (FormatCode)code);
    public static string Foreground(string text, Color color) => Format(text, $"{(int)FormatCode.ForegroundExtended};2;{color.R};{color.G};{color.B}");
    public static string Foreground(string text, int sgrColorCode) => Format(text, $"{(int)FormatCode.ForegroundExtended};5;{sgrColorCode}");
    public static string Background(string text, BackgroundCode code) => Format(text, (FormatCode)code);
    public static string Background(string text, Color color) => Format(text, $"{(int)FormatCode.BackgroundExtended};2;{color.R};{color.G};{color.B}");
    public static string Background(string text, int sgrColorCode) => Format(text, $"{(int)FormatCode.BackgroundExtended};5;{sgrColorCode}");

    public static string Default(string text) => Format(text, FormatCode.Default);
    public static string Bold(string text) => Format(text, FormatCode.Bold);
    public static string NoBold(string text) => Format(text, FormatCode.NoBold);
    public static string Underline(string text) => Format(text, FormatCode.Underline);
    public static string NoUnderline(string text) => Format(text, FormatCode.NoUnderline);
    public static string Negative(string text) => Format(text, FormatCode.Negative);
    public static string Positive(string text) => Format(text, FormatCode.Positive);
    public static string ForegroundBlack(string text) => Format(text, FormatCode.ForegroundBlack);
    public static string ForegroundRed(string text) => Format(text, FormatCode.ForegroundRed);
    public static string ForegroundGreen(string text) => Format(text, FormatCode.ForegroundGreen);
    public static string ForegroundYellow(string text) => Format(text, FormatCode.ForegroundYellow);
    public static string ForegroundBlue(string text) => Format(text, FormatCode.ForegroundBlue);
    public static string ForegroundMagenta(string text) => Format(text, FormatCode.ForegroundMagenta);
    public static string ForegroundCyan(string text) => Format(text, FormatCode.ForegroundCyan);
    public static string ForegroundWhite(string text) => Format(text, FormatCode.ForegroundWhite);
    public static string ForegroundExtended(string text) => Format(text, FormatCode.ForegroundExtended);
    public static string ForegroundDefault(string text) => Format(text, FormatCode.ForegroundDefault);
    public static string BackgroundBlack(string text) => Format(text, FormatCode.BackgroundBlack);
    public static string BackgroundRed(string text) => Format(text, FormatCode.BackgroundRed);
    public static string BackgroundGreen(string text) => Format(text, FormatCode.BackgroundGreen);
    public static string BackgroundYellow(string text) => Format(text, FormatCode.BackgroundYellow);
    public static string BackgroundBlue(string text) => Format(text, FormatCode.BackgroundBlue);
    public static string BackgroundMagenta(string text) => Format(text, FormatCode.BackgroundMagenta);
    public static string BackgroundCyan(string text) => Format(text, FormatCode.BackgroundCyan);
    public static string BackgroundWhite(string text) => Format(text, FormatCode.BackgroundWhite);
    public static string BackgroundExtended(string text) => Format(text, FormatCode.BackgroundExtended);
    public static string BackgroundDefault(string text) => Format(text, FormatCode.BackgroundDefault);
    public static string BrightForegroundBlack(string text) => Format(text, FormatCode.BrightForegroundBlack);
    public static string BrightForegroundRed(string text) => Format(text, FormatCode.BrightForegroundRed);
    public static string BrightForegroundGreen(string text) => Format(text, FormatCode.BrightForegroundGreen);
    public static string BrightForegroundYellow(string text) => Format(text, FormatCode.BrightForegroundYellow);
    public static string BrightForegroundBlue(string text) => Format(text, FormatCode.BrightForegroundBlue);
    public static string BrightForegroundMagenta(string text) => Format(text, FormatCode.BrightForegroundMagenta);
    public static string BrightForegroundCyan(string text) => Format(text, FormatCode.BrightForegroundCyan);
    public static string BrightForegroundWhite(string text) => Format(text, FormatCode.BrightForegroundWhite);
    public static string BrightBackgroundBlack(string text) => Format(text, FormatCode.BrightBackgroundBlack);
    public static string BrightBackgroundRed(string text) => Format(text, FormatCode.BrightBackgroundRed);
    public static string BrightBackgroundGreen(string text) => Format(text, FormatCode.BrightBackgroundGreen);
    public static string BrightBackgroundYellow(string text) => Format(text, FormatCode.BrightBackgroundYellow);
    public static string BrightBackgroundBlue(string text) => Format(text, FormatCode.BrightBackgroundBlue);
    public static string BrightBackgroundMagenta(string text) => Format(text, FormatCode.BrightBackgroundMagenta);
    public static string BrightBackgroundCyan(string text) => Format(text, FormatCode.BrightBackgroundCyan);
    public static string BrightBackgroundWhite(string text) => Format(text, FormatCode.BrightBackgroundWhite);
}
