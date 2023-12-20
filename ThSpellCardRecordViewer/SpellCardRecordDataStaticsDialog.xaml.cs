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

            SpellCardRecordStatics spellCardRecordStatics
                = SpellCardRecordStatics.CalculateSpellCardRecordStatics();
            if (spellCardRecordStatics != null)
            {
                AllSpellCardCountBlock.Text = spellCardRecordStatics.AllSpellCardCount;
                GetSpellCardCountBlock.Text = spellCardRecordStatics.GetSpellCardCount;
                ChallengeSpellCardCountBlock.Text = spellCardRecordStatics.ChallengeSpellCardCount;
                GetCardCountRateBlock.Text = spellCardRecordStatics.GetSpellCardCountRate;
            }
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
