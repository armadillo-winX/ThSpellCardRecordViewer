using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThSpellCardRecordViewer.Score
{
    internal class SpellCardRecord
    {
        public static ObservableCollection<SpellCardRecordData>? SpellCardRecordDataLists { get; set; }

        public static void GetSpellCardRecord(string gameId, bool diplayNotChallengedCardName)
        {
            if (gameId == GameIndex.Th06)
            {
                Th06.Th06SpellCardRecord.GetSpellCardRecord(diplayNotChallengedCardName);
            }
        }
    }
}
