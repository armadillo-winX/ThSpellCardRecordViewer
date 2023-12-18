using System.Reflection;

namespace ThSpellCardRecordViewer.Score
{
    internal class ScoreFilePath
    {
        public static string? Th06ScoreFile { get; set; }

        public static string? Th07ScoreFile { get; set; }

        public static string? Th08ScoreFile { get; set; }

        public static string? Th09ScoreFile { get; set; }

        public static string? Th10ScoreFile { get; set; }

        public static string? Th11ScoreFile { get; set; }

        public static string? Th12ScoreFile { get; set; }

        public static string? Th13ScoreFile { get; set; }

        public static string? Th14ScoreFile { get; set; }

        public static string? Th15ScoreFile { get; set; }

        public static string? Th16ScoreFile { get; set; }

        public static string? Th17ScoreFile { get; set; }

        public static string? Th18ScoreFile { get; set; }

        public static void SetScoreFilePath(string gameId, string scoreFilePath)
        {
            //プロパティ名からプロパティを取得
            PropertyInfo? scoreFilePathProperty = typeof(ScoreFilePath).GetProperty($"{gameId}ScoreFile");
            //取得したプロパティに値を代入
            scoreFilePathProperty.SetValue(null, scoreFilePath);
        }

        public static string? GetScoreFilePath(string gameId)
        {
            PropertyInfo? scoreFilePathProperty = typeof(ScoreFilePath).GetProperty($"{gameId}ScoreFile");
            string? scoreFile 
                = scoreFilePathProperty.GetValue(null, null) != null ? 
                scoreFilePathProperty.GetValue(null, null).ToString() : 
                string.Empty;

            return scoreFile;
        }
    }
}
