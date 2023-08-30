using System.Text;

namespace ThSpellCardRecordViewer.Score.Th07
{
    internal class Th07SpellCardRecord
    {
        public static void GetSpellCardRecord(bool displayNotChallengedCardName)
        {
            string? scoreFilePath = ScoreFilePath.Th07ScoreFile;
            if (File.Exists(scoreFilePath))
            {
                MemoryStream decodedData = new();
                bool decodeResult = Th07ScoreDecoder.Convert(scoreFilePath, decodedData);
                if (decodeResult)
                {
                    decodedData.Seek(0, SeekOrigin.Begin);
                    using (decodedData)
                    {
                        byte[] bytes = new byte[decodedData.Length];
                        _ = decodedData.Read(bytes, 0, (int)decodedData.Length);

                        int i = 40;
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
            byte[] cardIdData = data[40..42];
            byte[] cardNameData = data[42..90];
            byte[] reimuAChallengeCountData = data[91..93];
            byte[] reimuBChallengeCountData = data[93..95];
            byte[] marisaAChallengeCountData = data[95..97];
            byte[] marisaBChallengeCountData = data[97..99];
            byte[] sakuyaAChallengeCountData = data[99..101];
            byte[] sakuyaBChallengeCountData = data[101..103];
            byte[] allChallengeCountData = data[103..105];
            byte[] reimuAGetCountData = data[105..107];
            byte[] reimuBGetCountData = data[107..109];
            byte[] marisaAGetCountData = data[109..111];
            byte[] marisaBGetCountData = data[111..113];
            byte[] sakuyaAGetCountData = data[113..115];
            byte[] sakuyaBGetCountData = data[115..117];
            byte[] allGetCountData = data[117..119];

            int cardId = BitConverter.ToInt16(cardIdData, 0) + 1;

            int allChangeCount = Convert.ToInt32(BitConverter.ToString(allChallengeCountData, 0).Replace("-", ""), 16);
            int allGetCount = Convert.ToInt32(BitConverter.ToString(allGetCountData, 0).Replace("-", ""), 16);

            SpellCardInfo spellcardData = SpellCardInfo.GetSpellCardInfo(GameIndex.Th07, cardId);
            string? cardName
                = displayUnchallengedCard ? spellcardData.CardName : allChangeCount != 0 ? spellcardData.CardName : "Unchallenge Card";

            string rate = Calculator.CalcSpellCardGetRate(allGetCount, allChangeCount);

            SpellCardRecordData spellCardRecordList = new()
            {
                CardId = cardId.ToString(),
                CardName = cardName,
                Challenge = allChangeCount.ToString(),
                Get = allGetCount.ToString(),
                GetRate = rate,
                Enemy = spellcardData.Enemy,
                Place = spellcardData.Place
            };
            return spellCardRecordList;
        }
    }
}
