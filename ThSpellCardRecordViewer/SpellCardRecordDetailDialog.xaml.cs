namespace ThSpellCardRecordViewer
{
    /// <summary>
    /// SpellCardRecordDetailDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class SpellCardRecordDetailDialog : Window
    {
        internal SpellCardRecordData SpellCardRecordData
        {
            set
            {
                this.DataContext = value;

                if (value != null && value.IndividualSpellCards != null) 
                {
                    IndividualSpellCardRecordGrid.AutoGenerateColumns = false;
                    IndividualSpellCardRecordGrid.DataContext = value.IndividualSpellCards;
                }
            }
        }

        public SpellCardRecordDetailDialog()
        {
            InitializeComponent();
        }
    }
}
