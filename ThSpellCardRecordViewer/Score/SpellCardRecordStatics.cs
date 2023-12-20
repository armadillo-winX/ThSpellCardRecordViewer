using System.Collections.ObjectModel;

namespace ThSpellCardRecordViewer.Score
{
    internal class SpellCardRecordStatics
    {
        public string? AllSpellCardCount { get; set; }

        public string? GetSpellCardCount { get; set; }

        public string? ChallengeSpellCardCount { get; set; }

        public string? GetSpellCardCountRate { get; set; }

        public static SpellCardRecordStatics? CalculateSpellCardRecordStatics()
        {
            ObservableCollection<SpellCardRecordData>? spellCardRecordDatas
                = SpellCardRecord.SpellCardRecordDataLists;
            if (spellCardRecordDatas != null &&
                spellCardRecordDatas.Count > 0)
            {
                double allSpellCardCount = spellCardRecordDatas.Count;
                double getSpellCardCount = 0;
                double challengeSpellCardCount = 0;
                foreach (SpellCardRecordData spellCardRecordData in spellCardRecordDatas)
                {
                    if (int.Parse(spellCardRecordData.Get) > 0)
                        getSpellCardCount++;

                    if (int.Parse(spellCardRecordData.Challenge) > 0)
                        challengeSpellCardCount++;
                }

                string getSpellCardCountRate
                    = Calculator.CalcSpellCardGetRate(getSpellCardCount, allSpellCardCount);

                SpellCardRecordStatics spellCardRecordStatics = new()
                {
                    AllSpellCardCount = allSpellCardCount.ToString(),
                    GetSpellCardCount = getSpellCardCount.ToString(),
                    ChallengeSpellCardCount = challengeSpellCardCount.ToString(),
                    GetSpellCardCountRate = getSpellCardCountRate
                };

                return spellCardRecordStatics;
            }
            else
            {
                return null;
            }
        }
    }
}
