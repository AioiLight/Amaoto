using System;
using System.Drawing;

namespace Amaoto
{
    /// <summary>
    /// 文字テクスチャを生成するクラス。
    /// </summary>
    public class FontRender
    {
        /// <summary>
        /// 文字テクスチャを生成するクラスの初期化をします。
        /// </summary>
        /// <param name="fontFamily">書体名。</param>
        /// <param name="fontSize">フォントサイズ。</param>
        /// <param name="edge">縁取りの大きさ。</param>
        /// <param name="fontStyle">フォントスタイル。</param>
        public FontRender(FontFamily fontFamily, int fontSize, int edge = 0, FontStyle fontStyle = FontStyle.Regular)
        {
            FontFamily = fontFamily;
            FontSize = fontSize * 96.0f / 72.0f;
            FontStyle = fontStyle;
            ForeColor = Color.White;
            BackColor = Color.Black;
            Edge = edge;
        }

        /// <summary>
        /// 文字テクスチャを生成します。
        /// </summary>
        /// <param name="text">文字列。</param>
        /// <returns>テクスチャ。</returns>
        public Texture GetTexture(string text)
        {
            if (FontFamily == null) return new Texture();
            var size = MeasureText(text);
            var bitmap = new Bitmap((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height));
            bitmap.MakeTransparent();
            var graphics = Graphics.FromImage(bitmap);
            var stringFormat = new StringFormat();
            // どんなに長くて単語の区切りが良くても改行しない
            stringFormat.FormatFlags = StringFormatFlags.NoWrap;
            // どんなに長くてもトリミングしない
            stringFormat.Trimming = StringTrimming.None;
            // ハイクオリティレンダリング
            graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            // アンチエイリアスをかける
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            var gp = new System.Drawing.Drawing2D.GraphicsPath();

            if (Edge > 0)
            {
                gp.AddString(text, FontFamily, (int)FontStyle, FontSize, new Point(Edge, Edge), stringFormat);

                // 縁取りをする。
                graphics.DrawPath(new Pen(BackColor, Edge), gp);

                graphics.FillPath(new SolidBrush(ForeColor), gp);
            }
            else
            {
                gp.AddString(text, FontFamily, (int)FontStyle, FontSize, new Point(0, 0), stringFormat);
                graphics.FillPath(new SolidBrush(ForeColor), gp);
            }

            var tex = new Texture(bitmap);

            // Tex.SaveAsPng(File.Open(@"D:\aaa.png", FileMode.OpenOrCreate, FileAccess.Write), Tex.Width, Tex.Height);

            // 破棄
            bitmap.Dispose();
            graphics.Dispose();
            gp.Dispose();
            return tex;
        }

        private SizeF MeasureText(string text)
        {
            var bitmap = new Bitmap(16, 16);
            var graphicsSize = Graphics.FromImage(bitmap).
                MeasureString(text, new Font(FontFamily, FontSize, FontStyle, GraphicsUnit.Pixel));
            bitmap.Dispose();
            if (graphicsSize.Width == 0 || graphicsSize.Height == 0)
            {
                // サイズが0だったとき、とりあえずテクスチャとして成り立つそれっぽいサイズを返す。
                graphicsSize = new SizeF(16f, 16f);
            }

            if (Edge > 0)
            {
                // 縁取りをするので、補正分。
                graphicsSize.Width += Edge;
                graphicsSize.Height += Edge;
            }

            return graphicsSize;
        }

        /// <summary>
        /// 文字色。
        /// </summary>
        public Color ForeColor { get; set; }
        /// <summary>
        /// 縁色。
        /// </summary>
        public Color BackColor { get; set; }
        /// <summary>
        /// 縁取りのサイズ。
        /// </summary>
        public int Edge { get; set; }
        private FontFamily FontFamily;
        private FontStyle FontStyle;
        private float FontSize;
    }
}
