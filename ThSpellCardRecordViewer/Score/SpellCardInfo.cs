using System.Xml;

namespace ThSpellCardRecordViewer.Score
{
    internal class SpellCardInfo
    {
        public string? CardID { get; set; }

        public string? CardName { get; set; }

        public string? Enemy { get; set; }

        public string? Place { get; set; }

        public static SpellCardInfo GetSpellCardInfo(string gameId, int cardId)
        {
            string spellcardDataFilePath = GetSpellCardDataFilePath(gameId);

            if (File.Exists(spellcardDataFilePath))
            {
                XmlDocument spellcardDataDocument = new();
                spellcardDataDocument.Load(spellcardDataFilePath);
                XmlNode? cardNameNode = spellcardDataDocument.SelectSingleNode($"//SpellCard[@ID='{cardId}']/Name");
                XmlNode? cardEnemyNode = spellcardDataDocument.SelectSingleNode($"//SpellCard[@ID='{cardId}']/Enemy");
                XmlNode? cardPlaceNode = spellcardDataDocument.SelectSingleNode($"//SpellCard[@ID='{cardId}']/Place");

                SpellCardInfo spellCardInfo = new()
                {
                    CardID = cardId.ToString(),
                    CardName = cardNameNode.InnerText,
                    Enemy = cardEnemyNode.InnerText,
                    Place = cardPlaceNode.InnerText
                };
                return spellCardInfo;
            }
            else
            {
                throw new FileNotFoundException("スペルカードデータファイルが見つかりませんでした。");
            }
        }

        public static string GetSpellCardDataFilePath(string gameId)
        {
            string spellcardDataDirectory = PathInfo.SpellCardDataDirectory;
            return $"{spellcardDataDirectory}\\{gameId}SpellCardData.xml";
        }
    }
}
