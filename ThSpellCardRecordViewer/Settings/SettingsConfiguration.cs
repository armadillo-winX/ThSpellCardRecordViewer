using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ThSpellCardRecordViewer.Settings
{
    internal class SettingsConfiguration
    {
        public static ScoreFilePathSettings? _scoreFilePathSettings = new();

        public static void SaveScoreFilePathSettings()
        {
            string? scoreFilePathSettingsFile = PathInfo.ScoreFilePathSettings;

            _scoreFilePathSettings.Th06 = ScoreFilePath.Th06ScoreFile;
            _scoreFilePathSettings.Th07 = ScoreFilePath.Th07ScoreFile;
            _scoreFilePathSettings.Th08 = ScoreFilePath.Th08ScoreFile;
            _scoreFilePathSettings.Th09 = ScoreFilePath.Th09ScoreFile;
            _scoreFilePathSettings.Th10 = ScoreFilePath.Th10ScoreFile;
            _scoreFilePathSettings.Th11 = ScoreFilePath.Th11ScoreFile;
            _scoreFilePathSettings.Th12 = ScoreFilePath.Th12ScoreFile;
            _scoreFilePathSettings.Th13 = ScoreFilePath.Th13ScoreFile;
            _scoreFilePathSettings.Th14 = ScoreFilePath.Th14ScoreFile;
            _scoreFilePathSettings.Th15 = ScoreFilePath.Th15ScoreFile;
            _scoreFilePathSettings.Th16 = ScoreFilePath.Th16ScoreFile;
            _scoreFilePathSettings.Th17 = ScoreFilePath.Th17ScoreFile;
            _scoreFilePathSettings.Th18 = ScoreFilePath.Th18ScoreFile;

            if (!string.IsNullOrEmpty(scoreFilePathSettingsFile))
            {
                // XmlSerializerを使ってファイルに保存（SettingSerializerオブジェクトの内容を書き込む）
                XmlSerializer scoreFilePathSettingsSerializer = new(typeof(ScoreFilePathSettings));
                FileStream fileStream = new(scoreFilePathSettingsFile, FileMode.Create);
                // オブジェクトをシリアル化してXMLファイルに書き込む
                scoreFilePathSettingsSerializer.Serialize(fileStream, _scoreFilePathSettings);
                fileStream.Close();
            }
        }

        public static void ConfigureScoreFilePathSettings()
        {
            string? scoreFilePathSettingsFile = PathInfo.ScoreFilePathSettings;
            if (!string.IsNullOrEmpty(scoreFilePathSettingsFile) && File.Exists(scoreFilePathSettingsFile))
            {
                XmlSerializer scoreFilePathSettingsSerializer = new(typeof(ScoreFilePathSettings));
                FileStream fileStream = new(scoreFilePathSettingsFile, FileMode.Open);

                _scoreFilePathSettings = (ScoreFilePathSettings)scoreFilePathSettingsSerializer.Deserialize(fileStream);
                fileStream.Close();

                ScoreFilePath.Th06ScoreFile = _scoreFilePathSettings.Th06;
                ScoreFilePath.Th07ScoreFile = _scoreFilePathSettings.Th07;
                ScoreFilePath.Th08ScoreFile = _scoreFilePathSettings.Th08;
                ScoreFilePath.Th09ScoreFile = _scoreFilePathSettings.Th09;
                ScoreFilePath.Th10ScoreFile = _scoreFilePathSettings.Th10;
                ScoreFilePath.Th11ScoreFile = _scoreFilePathSettings.Th11;
                ScoreFilePath.Th12ScoreFile = _scoreFilePathSettings.Th12;
                ScoreFilePath.Th13ScoreFile = _scoreFilePathSettings.Th13;
                ScoreFilePath.Th14ScoreFile = _scoreFilePathSettings.Th14;
                ScoreFilePath.Th15ScoreFile = _scoreFilePathSettings.Th15;
                ScoreFilePath.Th16ScoreFile = _scoreFilePathSettings.Th16;
                ScoreFilePath.Th17ScoreFile = _scoreFilePathSettings.Th17;
                ScoreFilePath.Th18ScoreFile = _scoreFilePathSettings.Th18;
            }
            else
            {
                ScoreFilePath.Th06ScoreFile = string.Empty;
                ScoreFilePath.Th07ScoreFile = string.Empty;
                ScoreFilePath.Th08ScoreFile = string.Empty;
                ScoreFilePath.Th09ScoreFile = string.Empty;
                ScoreFilePath.Th10ScoreFile = string.Empty;
                ScoreFilePath.Th11ScoreFile = string.Empty;
                ScoreFilePath.Th12ScoreFile = string.Empty;
                ScoreFilePath.Th13ScoreFile = string.Empty;
                ScoreFilePath.Th14ScoreFile = string.Empty;
                ScoreFilePath.Th15ScoreFile = string.Empty;
                ScoreFilePath.Th16ScoreFile = string.Empty;
                ScoreFilePath.Th17ScoreFile = string.Empty;
                ScoreFilePath.Th18ScoreFile = string.Empty;
            }
        }

    }
}
