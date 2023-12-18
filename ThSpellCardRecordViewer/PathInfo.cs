namespace ThSpellCardRecordViewer
{
    internal class PathInfo
    {
        public static string AppPath => typeof(App).Assembly.Location;

        public static string? AppLocation => Path.GetDirectoryName(AppPath);

        public static string ScoreFilePathSettingsFile => $"{AppLocation}\\ScoreFilePath.xml";

        public static string MainWindowSettingsFile => $"{AppLocation}\\MainWindowSettings.xml";

        public static string SpellCardDataDirectory => $"{AppLocation}\\SpellCardData";
    }
}
