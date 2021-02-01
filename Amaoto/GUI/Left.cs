namespace Amaoto.GUI
{
    /// <summary>
    /// 上下中央、左揃えにGUI部品を描画する。
    /// </summary>
    public class Left : DrawPart
    {
        /// <summary>
        /// 上下中央、左揃えにGUI部品を描画する。
        /// </summary>
        /// <param name="child">子アイテム。</param>
        /// <param name="padding">(オプション)パディング。</param>
        /// <param name="width">(オプション)横幅。</param>
        /// <param name="height">(オプション)縦幅。</param>
        public Left(DrawPart child, int padding = 0, int? width = null, int? height = null)
            : base(width ?? child.Width + (padding * 2), height ?? child.Height + (padding * 2))
        {
            Child.Add(child);
            Padding = padding;
        }

        public override void Build()
        {
            SetToLeft();
            base.Build();
        }

        private void SetToLeft()
        {
            // 座標を求める。
            var gui = Child[0];
            Child[0].X = Padding;
            Child[0].Y = (Screen.ScreenSize.Height / 2) - (gui.Height / 2);
        }

        private int Padding;
    }
}
