using System;

namespace Amaoto.GUI
{
    /// <summary>
    /// 数値を上げ下げするGUI部品。
    /// </summary>
    public class NumericUpDown : DrawPart
    {
        /// <summary>
        /// 数値を上げ下げするGUI部品
        /// </summary>
        /// <param name="fontRender">FontRender。</param>
        /// <param name="buttonTexture">ボタンに使われる背景。</param>
        /// <param name="width">横幅。</param>
        /// <param name="height">縦幅。</param>
        public NumericUpDown(FontRender fontRender, ITextureReturnable buttonTexture, int width, int height)
            : base(width, height)
        {
            FontRender = fontRender;

            var plus = FontRender.GetTexture("+");
            var minus = FontRender.GetTexture("-");

            var op = width / 4;

            Increase = new Button(buttonTexture, plus, op, height);
            Decrease = new Button(buttonTexture, minus, op, height);
            NowValue = new Center(new Image(FontRender.GetTexture(Value.ToString())), 0, op * 2, height);

            Row = new Row(new DrawPart[] { Decrease, NowValue, Increase });

            Child.Add(Row);

            // イベントの定義。
            Increase.Clicked += Increase_Clicked;
            Decrease.Clicked += Decrease_Clicked;
            ValueChanged += NumericUpDown_ValueChanged;
        }

        private void NumericUpDown_ValueChanged(object sender, decimal e)
        {
            if (e >= Maximum)
            {
                // 最大値なので、増加ボタンを押せないようにする。
                Increase.Enabled = false;
            }
            else if (e <= Minimum)
            {
                // 最小値なので、減少ボタンを押せないようにする。
                Decrease.Enabled = false;
            }
            else
            {
                // どちらのボタンも押せるようにする。
                Increase.Enabled = true;
                Decrease.Enabled = true;
            }

            // レイアウトの更新。

            NowValue = new Center(new Image(FontRender.GetTexture(e.ToString())), 0, Width / 4 * 2, Height);
            // TODO: ステート更新
        }

        private void Decrease_Clicked(object sender, MouseClickEventArgs e)
        {
            // 値をStepに従って減少させる。
            Value -= Step;

            // イベント発火。
            ButtonClicked?.Invoke(this, null);
            Decreased?.Invoke(this, Value);
        }

        private void Increase_Clicked(object sender, MouseClickEventArgs e)
        {
            // 値をStepに従って増加させる。
            Value += Step;

            // イベント発火。
            ButtonClicked?.Invoke(this, null);
            Increased?.Invoke(this, Value);
        }

        private void RoundValue()
        {
            if (Value > Maximum)
            {
                Value = Maximum;
                ValueChanged?.Invoke(this, Value);
            }
            else if (Value < Minimum)
            {
                Value = Minimum;
                ValueChanged?.Invoke(this, Value);
            }
        }

        /// <summary>
        /// 最小値。
        /// </summary>
        public decimal Minimum
        {
            get
            {
                return _Minimum;
            }
            set
            {
                _Minimum = value;
                RoundValue();
            }
        }

        /// <summary>
        /// 最大値。
        /// </summary>
        public decimal Maximum
        {
            get
            {
                return _Maximum;
            }
            set
            {
                _Maximum = value;
                RoundValue();
            }
        }

        /// <summary>
        /// ボタンを押したときに上昇する数。
        /// </summary>
        public decimal Step { get; set; }

        /// <summary>
        /// 現在の値。
        /// </summary>
        public decimal Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (_Value != value)
                {
                    // 値が変更されるので
                    ValueChanged?.Invoke(this, value);
                }
                _Value = value;
                RoundValue();
            }
        }

        // イベント。

        /// <summary>
        /// 値が変更された。
        /// これは、丸め処理によって値が変わったときにも呼び出される (2回呼び出される可能性がある)。
        /// </summary>
        public event EventHandler<decimal> ValueChanged;

        /// <summary>
        /// 増加ボタンをクリックして増加した。。
        /// 直接数値を変更して値が増加した場合は、このイベントは呼び出されない。
        /// </summary>
        public event EventHandler<decimal> Increased;

        /// <summary>
        /// 現象ボタンをクリックして減少した。
        /// 直接数値を変更して値が減少した場合は、このイベントは呼び出されない。
        /// </summary>
        public event EventHandler<decimal> Decreased;

        /// <summary>
        /// 減少または増加のボタンがクリックされた。
        /// 効果音やエフェクト用。
        /// </summary>
        public event EventHandler ButtonClicked;
        

        private decimal _Minimum;
        private decimal _Maximum;
        private decimal _Value;

        private readonly FontRender FontRender;
        private readonly Button Increase;
        private readonly Button Decrease;
        private Center NowValue;
        private Row Row;
    }
}
