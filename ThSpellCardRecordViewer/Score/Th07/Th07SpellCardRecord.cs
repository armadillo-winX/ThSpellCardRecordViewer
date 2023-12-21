using System.Collections.ObjectModel;
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

            int allChallenge = Convert.ToInt32(BitConverter.ToString(allChallengeCountData, 0).Replace("-", ""), 16);
            int allGetCount = Convert.ToInt32(BitConverter.ToString(allGetCountData, 0).Replace("-", ""), 16);

            int reimuAChallengeCount = Convert.ToInt32(BitConverter.ToString(reimuAChallengeCountData, 0).Replace("-", ""), 16);
            int reimuBChallengeCount = Convert.ToInt32(BitConverter.ToString(reimuBChallengeCountData, 0).Replace("-", ""), 16);
            int marisaAChallengeCount = Convert.ToInt32(BitConverter.ToString(marisaAChallengeCountData, 0).Replace("-", ""), 16);
            int marisaBChallengeCount = Convert.ToInt32(BitConverter.ToString(marisaBChallengeCountData, 0).Replace("-", ""), 16);
            int sakuyaAChallengeCount = Convert.ToInt32(BitConverter.ToString(sakuyaAChallengeCountData, 0).Replace("-", ""), 16);
            int sakuyaBChallengeCount = Convert.ToInt32(BitConverter.ToString(sakuyaBChallengeCountData, 0).Replace("-", ""), 16);

            int reimuAGetCount = Convert.ToInt32(BitConverter.ToString(reimuAGetCountData, 0).Replace("-", ""), 16);
            int reimuBGetCount = Convert.ToInt32(BitConverter.ToString(reimuBGetCountData, 0).Replace("-", ""), 16);
            int marisaAGetCount = Convert.ToInt32(BitConverter.ToString(marisaAGetCountData, 0).Replace("-", ""), 16);
            int marisaBGetCount = Convert.ToInt32(BitConverter.ToString(marisaBGetCountData, 0).Replace("-", ""), 16);
            int sakuyaAGetCount = Convert.ToInt32(BitConverter.ToString(sakuyaAGetCountData, 0).Replace("-", ""), 16);
            int sakuyaBGetCount = Convert.ToInt32(BitConverter.ToString(sakuyaBGetCountData, 0).Replace("-", ""), 16);

            IndividualSpellCardRecordData reimuASpellCardRecordData = new()
            {
                Player = GamePlayers.GetGamePlayers(GameIndex.Th07)[0],
                Get = reimuAGetCount.ToString(),
                Challenge = reimuAChallengeCount.ToString(),
                GetRate = Calculator.CalcSpellCardGetRate(reimuAGetCount, reimuAChallengeCount)
            };

            IndividualSpellCardRecordData reimuBSpellCardRecordData = new()
            {
                Player = GamePlayers.GetGamePlayers(GameIndex.Th07)[1],
                Get = reimuBGetCount.ToString(),
                Challenge = reimuBChallengeCount.ToString(),
                GetRate = Calculator.CalcSpellCardGetRate(reimuBGetCount, reimuBChallengeCount)
            };

            IndividualSpellCardRecordData marisaASpellCardRecordData = new()
            {
                Player = GamePlayers.GetGamePlayers(GameIndex.Th07)[2],
                Get = marisaAGetCount.ToString(),
                Challenge = marisaAChallengeCount.ToString(),
                GetRate = Calculator.CalcSpellCardGetRate(marisaAGetCount, marisaAChallengeCount)
            };

            IndividualSpellCardRecordData marisaBSpellCardRecordData = new()
            {
                Player = GamePlayers.GetGamePlayers(GameIndex.Th07)[3],
                Get = marisaBGetCount.ToString(),
                Challenge = marisaBChallengeCount.ToString(),
                GetRate= Calculator.CalcSpellCardGetRate(marisaBGetCount, marisaBChallengeCount)
            };

            IndividualSpellCardRecordData sakuyaASpellCardRecordData = new()
            {
                Player = GamePlayers.GetGamePlayers(GameIndex.Th07)[4],
                Get = sakuyaAGetCount.ToString(),
                Challenge = sakuyaAChallengeCount.ToString(),
                GetRate = Calculator.CalcSpellCardGetRate(sakuyaAGetCount, sakuyaAChallengeCount)
            };

            IndividualSpellCardRecordData sakuyaBSpellCardRecordData = new()
            {
                Player = GamePlayers.GetGamePlayers(GameIndex.Th07)[5],
                Get = sakuyaBGetCount.ToString(),
                Challenge = sakuyaBChallengeCount.ToString(),
                GetRate = Calculator.CalcSpellCardGetRate(sakuyaBGetCount, sakuyaBChallengeCount)
            };

            ObservableCollection<IndividualSpellCardRecordData> individualSpellCardRecordDatas = new()
            {
                reimuASpellCardRecordData,
                reimuBSpellCardRecordData,
                marisaASpellCardRecordData,
                marisaBSpellCardRecordData,
                sakuyaASpellCardRecordData,
                sakuyaBSpellCardRecordData
            };

            SpellCardInfo spellcardData = SpellCardInfo.GetSpellCardInfo(GameIndex.Th07, cardId);
            string? cardName
                = displayUnchallengedCard ? spellcardData.CardName : allChallenge != 0 ? spellcardData.CardName : "----------------";

            string rate = Calculator.CalcSpellCardGetRate(allGetCount, allChallenge);

            SpellCardRecordData spellCardRecordList = new()
            {
                CardId = cardId.ToString(),
                CardName = cardName,
                Challenge = allChallenge.ToString(),
                Get = allGetCount.ToString(),
                GetRate = rate,
                Enemy = spellcardData.Enemy,
                Place = spellcardData.Place,
                IndividualSpellCards = individualSpellCardRecordDatas
            };
            return spellCardRecordList;
        }
    }
}
