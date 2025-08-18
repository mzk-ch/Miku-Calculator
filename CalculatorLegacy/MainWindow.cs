
namespace CalculatorLegacy
{
    public partial class MainWindow : Form
    {
        public byte SoundMode = 0;
        public CalcMode Mode = CalcMode.None;
        public Page Page = Page.Clear;
        public bool DontkeepText = false;
        public Numbers CurrentNumber { get; } = new();
        public Numbers LastNumber { get; } = new();
        public Numbers UsingNumber { get; } = new();
        public Sound Sound { get; set; }

        public string ShowNumber
        {
            get => lblText.Text;
        }

        public MainWindow()
        {
            Program.MainWindow = this;
            InitializeComponent();
            lblText.ForeColor = ColorList.pink;
            CurrentNumber.NumberChanged += (_, _) => lblText.Text = CurrentNumber.ToString();
            CurrentNumber.SetNumber(0);
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1) MessageBox.Show($"初音電卓{Environment.NewLine}{Environment.NewLine}本アプリは、Applicott（サイト：http://applicott.com 、現在は閉鎖；メール：m@applicott.com）によって制作されたAndroidアプリ「初音電卓」をPC向けに .NET 8.0 で復刻したものです。本バージョンは簡易版であり、オリジナルの雰囲気を最も忠実に再現したバージョンとなります。{Environment.NewLine}{Environment.NewLine}現在の作者情報{Environment.NewLine}Web: https://dtsm.mqmrx.cn{Environment.NewLine}Mail: shimadamizuki@qq.com{Environment.NewLine}{Environment.NewLine}この作品はピアプロ・キャラクター・ライセンスに基づいてクリプトン・フューチャー・メディア株式会社のキャラクター「初音ミク」を描いたものです。{Environment.NewLine}「初音ミク」はクリプトン・フューチャー・メディア株式会社の商標または登録商標です。", "About");
        }
    }
}
