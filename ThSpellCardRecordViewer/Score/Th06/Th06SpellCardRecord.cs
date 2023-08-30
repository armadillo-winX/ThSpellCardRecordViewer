using System.Text;

namespace ThSpellCardRecordViewer.Score.Th06
{
    internal class Th06SpellCardRecord
    {
        public static void GetSpellCardRecord(bool displayNotChallengedCardName)
        {
            string? scoreFilePath = ScoreFilePath.Th06ScoreFile;
            if (File.Exists(scoreFilePath))
            {
                MemoryStream decodedData = new();
                bool decodeResult = Th06ScoreDecoder.Convert(scoreFilePath, decodedData);
                if (decodeResult)
                {
                    decodedData.Seek(0, SeekOrigin.Begin);
                    using (decodedData)
                    {
                        byte[] bytes = new byte[decodedData.Length];
                        _ = decodedData.Read(bytes, 0, (int)decodedData.Length);

                        int i = 32;
                        while (i < decodedData.Length)
                        {
                            int n = i + 4;
                            int p = n + 2;
                            //レコードのデータサイズを取得
                            byte[] sizeData = bytes[n..p];
                            int size = BitConverter.ToInt16(sizeData, 0);

                            int r = i + size;
                            byte[] typeData = bytes[i..n];
                            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                            string type = Encoding.GetEncoding("Shift_JIS").GetString(typeData);
                            if (type == "HSCR")
                            {
                                i += size;
                            }
                            else if (type == "CLRD")
                            {
                                i += size;
                            }
                            else if (type == "CATK")
                            {
                                byte[] cardAttackData = bytes[i..r];
                                SpellCardRecordData spellCardRecordData
                                    = GetSpellCardRecord(cardAttackData, displayNotChallengedCardName);

                                SpellCardRecord.SpellCardRecordDataLists.Add(spellCardRecordData);

                                i += size;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        public static SpellCardRecordData GetSpellCardRecord(byte[] data, bool displayUnchallengedCard)
        {
            byte[] catkHeaderData = data[0..4];
            byte[] sizeData = data[4..6];
            byte[] cardIdData = data[16..18];
            byte[] cardNameData = data[24..60];
            byte[] challengeCountData = data[60..62];
            byte[] getCountData = data[62..64];

            int cardId = BitConverter.ToInt16(cardIdData, 0) + 1;
            int challenge = BitConverter.ToInt16(challengeCountData, 0);
            int get = BitConverter.ToInt16(getCountData, 0);

            SpellCardInfo spellcardData = SpellCardInfo.GetSpellCardInfo(GameIndex.Th06, cardId);
            string? cardName
                = displayUnchallengedCard ? spellcardData.CardName : challenge != 0 ? spellcardData.CardName : "Unchallenge Card";

            string rate = Calculator.CalcSpellCardGetRate(get, challenge);

            SpellCardRecordData spellCardRecordData = new()
            {
                CardId = cardId.ToString(),
                CardName = cardName,
                Get = get.ToString(),
                Challenge = challenge.ToString(),
                GetRate = rate,
                Enemy = spellcardData.Enemy,
                Place = spellcardData.Place
            };

            return spellCardRecordData;
        }
    }
}
