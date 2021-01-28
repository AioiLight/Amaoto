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
            var plus = fontRender.GetTexture("+");
            var minus = fontRender.GetTexture("-");

            var op = width / 4;

            Increase = new Button(buttonTexture, plus, op, height);
            Decrease = new Button(buttonTexture, minus, op, height);
            NowValue = new Center(new Image(fontRender.GetTexture(Value.ToString())), 0, op * 2, height);
        }

        private void RoundValue()
        {
            if (Value > Maximum)
            {
                Value = Maximum;
            }
            else if (Value < Minimum)
            {
                Value = Minimum;
            }
        }

        /// <summary>
        /// 最小値。
        /// </summary>
        public decimal Minimum { get; set; }
        /// <summary>
        /// 最大値。
        /// </summary>
        public decimal Maximum { get; set; }
        /// <summary>
        /// ボタンを押したときに上昇する数。
        /// </summary>
        public decimal Step { get; set; }
        /// <summary>
        /// 現在の値。
        /// </summary>
        public decimal Value { get; set; }

        private Button Increase;
        private Button Decrease;
        private Center NowValue;
    }
}
