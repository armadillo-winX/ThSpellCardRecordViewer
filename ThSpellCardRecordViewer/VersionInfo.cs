using System.Diagnostics;

namespace ThSpellCardRecordViewer
{
    internal class VersionInfo
    {
        private static readonly string _appPath = PathInfo.AppPath;

        public static string? AppName => FileVersionInfo.GetVersionInfo(_appPath).ProductName;

        public static string AppVersion => FileVersionInfo.GetVersionInfo(_appPath).ProductVersion.RemoveRightOf("+");

        public static string? Developer => FileVersionInfo.GetVersionInfo(_appPath).CompanyName;

        public static string OSVersion => Environment.OSVersion.ToString();

        public static string DotNetViersion => $".NET {Environment.Version}";
    }
}
