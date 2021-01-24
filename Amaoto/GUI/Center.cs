namespace Amaoto.GUI
{
    /// <summary>
    /// 上下中央揃えにGUI部品を描画する。
    /// </summary>
    public class Center : DrawPart
    {
        /// <summary>
        /// 上下中央揃えにGUI部品を描画する。
        /// </summary>
        /// <param name="child">子アイテム。</param>
        /// <param name="padding">(オプション)パディング。</param>
        /// <param name="width">(オプション)横幅。</param>
        /// <param name="height">(オプション)縦幅。</param>
        public Center(DrawPart child, int padding = 0, int? width = null, int? height = null)
            : base(width ?? child.Width + (padding * 2), height ?? child.Height + (padding * 2))
        {
            Child.Add(child);

            // 真ん中を求める。
            Child[0].X = (Screen.ScreenSize.Width / 2) - (child.Width / 2);
            Child[0].Y = (Screen.ScreenSize.Height / 2) - (child.Height / 2);
        }
    }
}
