using System;
using System.Drawing;
using System.Drawing.Drawing2D;

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
            var stringFormat = GetStringFormat(graphics);
            var gp = DrawString(text, graphics, stringFormat);

            var tex = new Texture(bitmap);

            // 破棄
            bitmap.Dispose();
            graphics.Dispose();
            gp.Dispose();
            return tex;
        }

        private static StringFormat GetStringFormat(Graphics graphics)
        {
            var stringFormat = new StringFormat(StringFormat.GenericTypographic);
            // どんなに長くて単語の区切りが良くても改行しない
            stringFormat.FormatFlags = StringFormatFlags.NoWrap;
            // どんなに長くてもトリミングしない
            stringFormat.Trimming = StringTrimming.None;
            // ハイクオリティレンダリング
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            // アンチエイリアスをかける
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            return stringFormat;
        }

        private GraphicsPath DrawString(string text, Graphics graphics, StringFormat stringFormat)
        {
            var gp = new GraphicsPath();

            if (Edge > 0)
            {
                gp.AddString(text, FontFamily, (int)FontStyle, FontSize, new Point(Edge / 2, Edge / 2), stringFormat);

                // 縁取りをする。
                graphics.DrawPath(new Pen(BackColor, Edge) { LineJoin = System.Drawing.Drawing2D.LineJoin.Round }, gp);

                graphics.FillPath(new SolidBrush(ForeColor), gp);
            }
            else
            {
                gp.AddString(text, FontFamily, (int)FontStyle, FontSize, new Point(0, 0), stringFormat);
                graphics.FillPath(new SolidBrush(ForeColor), gp);
            }

            return gp;
        }

        private SizeF MeasureText(string text)
        {
            var bitmap = new Bitmap(16, 16);
            // .NETの敗北
            var graphicsSize = Graphics.FromImage(bitmap).
                MeasureString(text, new Font(FontFamily, FontSize, FontStyle, GraphicsUnit.Pixel));
            var trueGraphicsSize = Graphics.FromImage(bitmap).
                MeasureString(text, new Font(FontFamily, FontSize, FontStyle, GraphicsUnit.Pixel), (int)graphicsSize.Width, StringFormat.GenericTypographic);
            bitmap.Dispose();
            if (trueGraphicsSize.Width == 0 || trueGraphicsSize.Height == 0)
            {
                // サイズが0だったとき、とりあえずテクスチャとして成り立つそれっぽいサイズを返す。
                trueGraphicsSize = new SizeF(16f, 16f);
            }

            if (Edge > 0)
            {
                // 縁取りをするので、補正分。
                trueGraphicsSize.Width += Edge;
                trueGraphicsSize.Height += Edge;
            }

            return trueGraphicsSize;
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