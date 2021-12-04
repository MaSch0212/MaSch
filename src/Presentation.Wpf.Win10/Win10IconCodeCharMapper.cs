namespace MaSch.Presentation.Wpf.Win10;

internal static class Win10IconCodeCharMapper
{
    public static string GetChar(this Win10IconCode iconCode)
    {
        return Encoding.UTF32.GetString(BitConverter.GetBytes((uint)iconCode));
    }

    public static Win10IconCode GetWin10IconCode(this string s)
    {
        return (Win10IconCode)BitConverter.ToUInt32(Encoding.UTF32.GetBytes(s), 0);
    }
}
