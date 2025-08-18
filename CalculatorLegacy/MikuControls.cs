using System.Diagnostics;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CalculatorLegacy
{
    public static class ColorList
    {
        public static readonly Color green = Color.FromArgb(0x3D, 0xC1, 0xC0);
        public static readonly Color pink = Color.FromArgb(0xFF, 0x08, 0x8D);
        public static readonly Color gray = Color.FromArgb(0xBB, 0xBB, 0xBB);
        public static readonly Color lightGray = Color.FromArgb(0xC0, 0xC0, 0xC0);
        public static readonly Color borderGray = Color.FromArgb(0x99, 0x99, 0x99);
        public static readonly Color darkGray = Color.FromArgb(0x45, 0x4D, 0x4F);
    }

    public enum Sound
    {
        All,
        Answer,
        None
    }

    public enum Page
    {
        Clear,
        Second,
        Gitguo
    }

    public class Numbers
    {
        private int _deep = -1;
        private long _fullNumber;
        public delegate void NumberChangedEventHandler(object sender, EventArgs e);
        private const long MaxValue = 1000000000000000;

        public event NumberChangedEventHandler? NumberChanged;

        public int Deep
        {
            get => _deep;
            set => _deep = Math.Max(-1, value);
        }

        public long FullNumber
        {
            get => _fullNumber;
        }

        public Numbers(long num = 0, int deep = -1) => SetNumber(num, deep);
        public Numbers(Numbers number) => SetNumber(number);

        public void Clear()
        {
            _fullNumber = 0;
            _deep = -1;
        }

        public override string ToString()
        {
            bool xuejian = _fullNumber > MaxValue && _deep > 0;
            int tail = 0;

            Debug.WriteLine($"转换前：{_fullNumber}，{_deep}");

            while (OutValue(_fullNumber) && _deep > 0)
            {
                tail = (int)(_fullNumber % 10);
                _fullNumber /= 10;
                _deep--;
            }

            if (xuejian && tail >= 5) _fullNumber += 1;

            string origin = Math.Abs(_fullNumber).ToString();
            string ffloat;

            Debug.WriteLine($"转换后：{_fullNumber}，{_deep}");

            if (_deep > 0)
            {
                if (origin.Length < _deep)
                {
                    origin = $"{new string('0', _deep - origin.Length + 1)}{origin}";
                }

                ffloat = $".{origin[^_deep..]}";
                origin = origin[..^_deep];
            }
            else
            {
                ffloat = _deep == 0 && !xuejian ? "." : "";
            }

            if (OutValue(_fullNumber))
            {
                Clear();
                return "ERROR!";
            }

            origin = string.IsNullOrEmpty(origin) ? "0" : origin;
            string org = long.Parse(origin).ToString("N0");
            origin = _fullNumber >= 0 ? org : $"-{org}";

            return $"{origin}{ffloat}";
        }

        public static bool OutValue(long value) => value > MaxValue || value < -MaxValue;

        public static bool IsNullOrZero(Numbers? numbers) => numbers == null || numbers.FullNumber == 0;

        public static Numbers[] TongyiWeishu(Numbers n1, Numbers n2)
        {
            n1 = new(n1);
            n2 = new(n2);
            int ca = n1._deep - n2._deep;

            if (ca == 0)
            {
                goto ret;
            }
            else
            {
                if (ca > 0)
                {
                    n2._fullNumber *= (int)Math.Pow(10, ca);
                    n2._deep += ca;
                }
                else
                {
                    n1._fullNumber *= (int)Math.Pow(10, ca);
                    n1._deep += ca;
                }
            }

        ret: return [n1, n2];
        }

        public Numbers Plus(Numbers nump)
        {
            Numbers[] ns = TongyiWeishu(this, nump);
            return new(ns[0]._fullNumber + ns[1]._fullNumber, ns[0]._deep);
        }

        public Numbers Minus(Numbers numm)
        {
            Numbers[] ns = TongyiWeishu(this, numm);
            return new(ns[0]._fullNumber - ns[1]._fullNumber, ns[0]._deep);
        }

        public Numbers Times(Numbers numt)
        {
            Debug.WriteLine($"{_fullNumber}, {numt._fullNumber}, {numt._fullNumber}");
            return (numt._fullNumber == 0) ? numt : new(_fullNumber * numt._fullNumber, _deep + numt._deep);
        }

        public Numbers Divide(Numbers numd)
        {
            if (numd._fullNumber == 0)
                return new(MaxValue + 1);

            Numbers num = new(this);
            int deep = Math.Max(num._deep, 0);
            int deep2 = Math.Max(numd._deep, 0);
            int deepp = MaxValue.ToString().Length - _fullNumber.ToString().Length + deep2 + 1;
            num._fullNumber *= (long)Math.Pow(10, deepp);

            Debug.WriteLine($"{num._fullNumber}, {numd._fullNumber}, {deep + deepp}");
            return new(num._fullNumber / numd._fullNumber, deep + deepp);
        }

        public void AddPoint()
        {
            if (_deep < 0)
            {
                _deep = 0;
                OnNumberChanged();
            }
        }

        public void SetNumber(long num, int deep = -1)
        {
            Deep = deep;
            _fullNumber = num;
            OnNumberChanged();
        }

        public void SetNumber(Numbers num, bool calc = false)
        {
            Debug.WriteLine($"传入数据：deep：{num._deep}，FN：{num._fullNumber}");
            int deep = num._deep;
            long fullNumber = num._fullNumber;
            if (num._deep > 0 && calc)
            {
                string str = num._fullNumber.ToString();
                int zerosAtEnd = Math.Min(str.Length - str.TrimEnd('0').Length, num._deep);
                if (zerosAtEnd > 0)
                {
                    deep = num._deep - zerosAtEnd;
                    fullNumber = num._fullNumber / (long)Math.Pow(10, zerosAtEnd);
                }
                Debug.WriteLine($"有{zerosAtEnd}个0约掉了");
                deep = deep == 0 ? -1 : deep;
            }
            _deep = deep;
            _fullNumber = fullNumber;
            OnNumberChanged();
        }

        public void AddNumber(int num)
        {
            long a = _fullNumber * 10 + num;
            if (a > MaxValue) return;

            if (_deep >= 0) Deep++;
            _fullNumber = a;
            OnNumberChanged();
        }

        public void DeleteNumber()
        {
            if (_deep != 0)
            {
                _fullNumber /= 10;
            }
            Deep--;
            OnNumberChanged();
        }

        protected virtual void OnNumberChanged()
        {
            NumberChanged?.Invoke(this, new EventArgs());
        }
    }

    public enum CalcMode
    {
        Plus,
        Minus,
        Times,
        Divide,
        None
    }

    public class HutsuuButton : Button
    {
        public HutsuuButton() : base()
        {
            BackColor = ColorList.lightGray;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            TextAlign = ContentAlignment.MiddleCenter;
        }

        public override void ResetBackColor() => BackColor = ColorList.lightGray;
        public virtual bool ShouldSerializeBackColor() => BackColor != ColorList.lightGray;
        public virtual void ResetFlatStyle() => FlatStyle = FlatStyle.Flat;
        public virtual bool ShouldSerializeFlatStyle() => FlatStyle != FlatStyle.Flat;
        public virtual void ResetTextAlign() => TextAlign = ContentAlignment.MiddleCenter;
        public virtual bool ShouldSerializeTextAlign() => TextAlign != ContentAlignment.MiddleCenter;
    }

    public class CalculateButton : HutsuuButton
    {
        public CalculateButton() : base()
        {
            ForeColor = Color.White;
            BackColor = ColorList.green;
        }

        public override void ResetForeColor() => ForeColor = Color.White;
        public virtual bool ShouldSerializeForeColor() => ForeColor != Color.White;
        public override void ResetBackColor() => BackColor = ColorList.green;
        public override bool ShouldSerializeBackColor() => BackColor != ColorList.green;
    }

    public class ControlButton : HutsuuButton
    {
        public ControlButton() : base() => BackColor = ColorList.gray;
        public override void ResetBackColor() => BackColor = ColorList.gray;
        public override bool ShouldSerializeBackColor() => BackColor != ColorList.gray;
    }

    public class ButtonPanel : Panel
    {
        public ButtonPanel() : base() => BackColor = ColorList.borderGray;
        public override void ResetForeColor() => ForeColor = Color.Empty;
        protected virtual bool ShouldSerializeForeColor() => !ForeColor.IsEmpty;
        public override void ResetBackColor() => BackColor = ColorList.borderGray;
        protected virtual bool ShouldSerializeBackColor() => BackColor != ColorList.borderGray;
    }

    public class BackupPanel : Panel
    {
        public const string strCopy = "COPY";
        public const string strPaste = "PASTE";
        public readonly Numbers Content = new();
        private readonly HutsuuButton _bbCOPY = new() { Text = strCopy };
        private readonly HutsuuButton _bbPASTE = new() { Text = strPaste };

        public BackupPanel() : base()
        {
            SizeChanged += (_, _) =>
            {
                _bbCOPY.Width = (Width - 3) / 4;
                _bbPASTE.Width = Width - _bbCOPY.Width - 1;
                _bbCOPY.Height = _bbPASTE.Height = Height;
                _bbPASTE.Left = _bbCOPY.Right + 1;
            };

            _bbCOPY.MouseClick += (_, _) =>
            {
                Numbers? cn = Program.MainWindow.CurrentNumber;

                if (!Numbers.IsNullOrZero(cn))
                {
                    _bbPASTE.Text = Program.MainWindow.ShowNumber;
                    _bbPASTE.ForeColor = ColorList.pink;
                    Content.SetNumber(cn);
                }
                else
                {
                    ClearCopies();
                }
            };

            _bbPASTE.MouseClick += (_, _) =>
            {
                if (!Numbers.IsNullOrZero(Content))
                {
                    Program.MainWindow.CurrentNumber.SetNumber(Content);
                    if (Program.MainWindow.Page == Page.Gitguo)
                    {
                        Program.MainWindow.Mode = CalcMode.None;
                        Program.MainWindow.Page = Page.Clear;
                    }
                }
            };

            _bbPASTE.MouseDown += new MouseEventHandler(
                (sender, e) =>
                {
                    if (e.Button == MouseButtons.Right) ClearCopies();
                });

            Controls.Add(_bbCOPY);
            Controls.Add(_bbPASTE);
        }

        private void ClearCopies()
        {
            _bbPASTE.Text = strPaste;
            _bbPASTE.ForeColor = ColorList.green;
            Content.SetNumber(0);
        }
    }

    public class BackupButtonPanel : ButtonPanel
    {
        public BackupPanel[] bps = [new(), new(), new()];
        public BackupButtonPanel() : base()
        {
            ForeColor = ColorList.green;

            foreach (var b in bps)
            {
                Controls.Add(b);
            }

            SizeChanged += (_, _) =>
            {
                int hei = (Height - bps.Length) / bps.Length;
                for (int i = 0; i < bps.Length; i++)
                {
                    bps[i].Height = hei;
                    bps[i].Width = Width;
                    bps[i].Top = (hei + 1) * i;
                }
            };
        }

        public override void ResetForeColor() => ForeColor = ColorList.green;
        protected override bool ShouldSerializeForeColor() => ForeColor != ColorList.green;
    }

    public class KeyBoardPanel : ButtonPanel
    {
        private static readonly CalculateButton[] modes = [
            new() { Text = "+" }, new() { Text = "-" }, new() { Text = "×" }, new() { Text = "÷" }];
        private static readonly HutsuuButton[] numbers = [new() { Text = "0" },
            new() { Text = "1" }, new() { Text = "2" }, new() { Text = "3" },
            new() { Text = "4" }, new() { Text = "5" }, new() { Text = "6" },
            new() { Text = "7" },new() { Text = "8" },new() { Text = "9" }];
        private static readonly HutsuuButton point = new() { Text = "." };
        private static readonly ControlButton ac = new() { Text = "AC" };
        private static readonly ControlButton delete = new() { Text = "DEL" };
        private static readonly ControlButton sound = new() { Text = "♪" , Enabled = false}; // TODO: 音乐功能
        private static readonly CalculateButton equal = new() { BackColor = ColorList.pink, Text = "=" };
        private static readonly HutsuuButton[,] Keyboard = new HutsuuButton[4, 5] {
            {modes[(int)CalcMode.Plus],   numbers[7], numbers[8], numbers[9], ac},
            {modes[(int)CalcMode.Minus],  numbers[4], numbers[5], numbers[6], delete},
            {modes[(int)CalcMode.Times],  numbers[1], numbers[2], numbers[3], sound},
            {modes[(int)CalcMode.Divide], numbers[0], point,      equal,      equal}
        };

        public KeyBoardPanel() : base()
        {
            ForeColor = ColorList.pink;
            SoundButton();

            for (int i = 0; i < modes.Length; i++)
            {
                int j = i;
                modes[j].MouseClick += (sender, e) =>
                {
                    if (Program.MainWindow.Page != Page.Clear)
                    {
                        if (Program.MainWindow.Page == Page.Second)
                            Equal_MouseClick(sender, e);
                        Program.MainWindow.Page = Page.Clear;
                    }
                    else Program.MainWindow.LastNumber.SetNumber(Program.MainWindow.CurrentNumber);
                    Program.MainWindow.Mode = (CalcMode)j;
                    Program.MainWindow.DontkeepText = true;
                };
            }

            for (int i = 0; i < numbers.Length; i++)
            {
                int j = i;
                numbers[j].MouseClick += (_, _) =>
                {
                    if (Program.MainWindow.DontkeepText)
                    {
                        Program.MainWindow.CurrentNumber.SetNumber(j);
                        Program.MainWindow.DontkeepText = false;

                        if (Program.MainWindow.Page == Page.Gitguo)
                        {
                            Program.MainWindow.Page = Page.Clear;
                            Program.MainWindow.Mode = CalcMode.None;
                        }
                        else if (Program.MainWindow.Page == Page.Clear) Program.MainWindow.Page = Page.Second;
                    }
                    else Program.MainWindow.CurrentNumber.AddNumber(j);
                };
            }

            ac.MouseClick += (_, _) =>
            {
                Program.MainWindow.CurrentNumber.SetNumber(0);
                Program.MainWindow.UsingNumber.SetNumber(0);
                Program.MainWindow.LastNumber.SetNumber(0);
                Program.MainWindow.Mode = CalcMode.None;
            };

            delete.MouseClick += (_, _) =>
            {
                if (Program.MainWindow.Page == Page.Gitguo) Program.MainWindow.CurrentNumber.SetNumber(0);
                else Program.MainWindow.CurrentNumber.DeleteNumber();

                if (Program.MainWindow.Page == Page.Gitguo) Program.MainWindow.Mode = CalcMode.None;
                if (Program.MainWindow.DontkeepText) Program.MainWindow.CurrentNumber.SetNumber(0);
            };

            point.MouseClick += (_, _) => Program.MainWindow.CurrentNumber.AddPoint();

            equal.MouseClick += Equal_MouseClick;

            sound.MouseClick += (_, _) =>
            {
                switch (Program.MainWindow.Sound)
                {
                    case Sound.All:
                        Program.MainWindow.Sound = Sound.Answer;
                        break;
                    case Sound.Answer:
                        Program.MainWindow.Sound = Sound.None;
                        break;
                    case Sound.None:
                        Program.MainWindow.Sound = Sound.All;
                        break;
                    default:
                        break;
                }

                SoundButton();
            };

            sound.MouseDown += new MouseEventHandler(
                (sender, e) =>
                {
                    if (e.Button == MouseButtons.Right) Program.MainWindow.ShowHelp();
                });

            SizeChanged += (_, _) =>
            {
                int wid = (Width - Keyboard.GetLength(1) + 1) / Keyboard.GetLength(1);
                int hei = (Height - Keyboard.GetLength(0) + 1) / Keyboard.GetLength(0);
                Size btnSize = new(wid, hei);

                for (int i = 0; i < Keyboard.GetLength(0); i++)
                {
                    for (int j = 0; j < Keyboard.GetLength(1); j++)
                    {
                        if (i != 3 || j != 4)
                        {
                            Keyboard[i, j].Location = new Point(j * (wid + 1), i * (hei + 1));
                            Keyboard[i, j].Size = btnSize;
                        }
                    }
                }

                equal.Width = (wid + 1) * 2;
            };

            for (int i = 0; i < Keyboard.GetLength(0); i++)
            {
                for (int j = 0; j < Keyboard.GetLength(1); j++)
                {
                    if (i != 3 || j != 4)
                    {
                        Controls.Add(Keyboard[i, j]);
                    }
                }
            }
        }

        private void SoundButton()
        {
            switch (Program.MainWindow.Sound)
            {
                case Sound.All:
                    sound.Text = "♪";
                    sound.ForeColor = ColorList.pink;
                    break;
                case Sound.Answer:
                    sound.Text = "ANS";
                    sound.ForeColor = ColorList.pink;
                    break;
                case Sound.None:
                    sound.Text = "♪";
                    sound.ForeColor = ColorList.darkGray;
                    break;
                default:
                    break;
            }
        }

        private void Equal_MouseClick(object? sender, MouseEventArgs e)
        {
            if (!Program.MainWindow.DontkeepText)
            {
                Program.MainWindow.UsingNumber.SetNumber(Program.MainWindow.CurrentNumber);
            }


            switch (Program.MainWindow.Mode)
            {
                case CalcMode.Plus:
                    Program.MainWindow.Page = Page.Gitguo;
                    Program.MainWindow.CurrentNumber.SetNumber(Program.MainWindow.LastNumber.Plus(Program.MainWindow.UsingNumber), true);
                    break;
                case CalcMode.Minus:
                    Program.MainWindow.Page = Page.Gitguo;
                    Program.MainWindow.CurrentNumber.SetNumber(Program.MainWindow.LastNumber.Minus(Program.MainWindow.UsingNumber), true);
                    break;
                case CalcMode.Times:
                    Program.MainWindow.Page = Page.Gitguo;
                    Program.MainWindow.CurrentNumber.SetNumber(Program.MainWindow.LastNumber.Times(Program.MainWindow.UsingNumber), true);
                    break;
                case CalcMode.Divide:
                    Program.MainWindow.Page = Page.Gitguo;
                    Program.MainWindow.CurrentNumber.SetNumber(Program.MainWindow.LastNumber.Divide(Program.MainWindow.UsingNumber), true);
                    break;
                default:
                    break;
            }

            Program.MainWindow.LastNumber.SetNumber(Program.MainWindow.CurrentNumber);
            Program.MainWindow.DontkeepText = true;
        }

        public override void ResetForeColor() => ForeColor = ColorList.pink;
        protected override bool ShouldSerializeForeColor() => ForeColor != ColorList.pink;
    }
}
