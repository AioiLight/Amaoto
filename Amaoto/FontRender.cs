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
        /// <param name="rectangle">文字の範囲。</param>
        /// <returns>テクスチャ。</returns>
        public Texture GetTexture(string text, Rectangle? rectangle = null)
        {
            if (FontFamily == null) return new Texture();
            var size = MeasureText(text, rectangle);
            var bitmap = new Bitmap((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height));
            bitmap.MakeTransparent();
            var graphics = Graphics.FromImage(bitmap);
            var stringFormat = GetStringFormat(rectangle);
            SetGraphicsMode(graphics);
            var gp = DrawString(text, graphics, stringFormat, rectangle);

            var tex = new Texture(bitmap);

            // 破棄
            bitmap.Dispose();
            graphics.Dispose();
            gp.Dispose();
            return tex;
        }

        private static StringFormat GetStringFormat(Rectangle? rectangle)
        {
            var stringFormat = new StringFormat(StringFormat.GenericTypographic)
            {
                // どんなに長くて単語の区切りが良くても改行しない
                FormatFlags = !rectangle.HasValue ? StringFormatFlags.NoWrap : StringFormatFlags.NoClip,
                // どんなに長くてもトリミングしない
                Trimming = !rectangle.HasValue ? StringTrimming.None : StringTrimming.EllipsisCharacter
            };
            return stringFormat;
        }

        private static void SetGraphicsMode(Graphics graphics)
        {
            // ハイクオリティレンダリング
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            // アンチエイリアスをかける
            graphics.SmoothingMode = SmoothingMode.HighQuality;
        }

        private GraphicsPath DrawString(string text, Graphics graphics, StringFormat stringFormat, Rectangle? rectangle)
        {
            var gp = new GraphicsPath();

            if (Edge > 0)
            {
                if (rectangle.HasValue)
                {
                    gp.AddString(text, FontFamily, (int)FontStyle, FontSize, new Rectangle(Edge / 2, Edge / 2, rectangle.Value.Width, rectangle.Value.Height), stringFormat);
                }
                else
                {
                    gp.AddString(text, FontFamily, (int)FontStyle, FontSize, new Point(Edge / 2, Edge / 2), stringFormat);
                }

                // 縁取りをする。
                graphics.DrawPath(new Pen(BackColor, Edge) { LineJoin = LineJoin.Round }, gp);

                graphics.FillPath(new SolidBrush(ForeColor), gp);
            }
            else
            {
                if (rectangle.HasValue)
                {
                    gp.AddString(text, FontFamily, (int)FontStyle, FontSize, new Rectangle(0, 0, rectangle.Value.Width, rectangle.Value.Height), stringFormat);
                }
                else
                {
                    gp.AddString(text, FontFamily, (int)FontStyle, FontSize, new Point(0, 0), stringFormat);
                }
                graphics.FillPath(new SolidBrush(ForeColor), gp);
            }

            return gp;
        }

        private SizeF MeasureText(string text, Rectangle? rectangle)
        {
            if (rectangle.HasValue)
            {
                return new SizeF(rectangle.Value.Width, rectangle.Value.Height);
            }

            var bitmap = new Bitmap(16, 16);
            // .NETの敗北
            var graphicsSize = Graphics.FromImage(bitmap).
                MeasureString(text, new Font(FontFamily, FontSize, FontStyle, GraphicsUnit.Pixel));
            var trueGraphicsSize = Graphics.FromImage(bitmap).
                MeasureString(text, new Font(FontFamily, FontSize, FontStyle, GraphicsUnit.Pixel), (int)graphicsSize.Width, GetStringFormat(rectangle));
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