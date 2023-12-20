using System.Collections.ObjectModel;

namespace ThSpellCardRecordViewer
{
    /// <summary>
    /// SpellCardRecordDataStaticsDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class SpellCardRecordDataStaticsDialog : Window
    {
        public SpellCardRecordDataStaticsDialog()
        {
            InitializeComponent();

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

                string getCardCountRate = Calculator.CalcSpellCardGetRate(getSpellCardCount, allSpellCardCount);

                AllSpellCardCountBlock.Text = allSpellCardCount.ToString();
                GetSpellCardCountBlock.Text = getSpellCardCount.ToString();
                ChallengeSpellCardCountBlock.Text = challengeSpellCardCount.ToString();
                GetCardCountRateBlock.Text = getCardCountRate;
            }
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
