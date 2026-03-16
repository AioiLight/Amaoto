using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

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
        /// <param name="fontSize">フォントサイズ(px)。</param>
        /// <param name="edge">縁取りの大きさ。</param>
        /// <param name="fontStyle">フォントスタイル。</param>
        public FontRender(FontFamily fontFamily, int fontSize, int edge = 0, FontStyle fontStyle = FontStyle.Regular)
        {
            FontFamily = fontFamily;
            FontSize = fontSize;
            FontStyle = fontStyle;
            ForeColor = Color.White;
            BackColor = Color.Black;
            Edge = edge;
        }

        /// <summary>
        /// 文字テクスチャを生成します。
        /// </summary>
        /// <param name="text">文字列。</param>
        /// <param name="size">文字の範囲。</param>
        /// <returns>テクスチャ。</returns>
        public Texture GetTexture(string text, Size? size = null)
        {
            if (FontFamily == null) return new Texture();
            var stringFormat = GetStringFormat(size.HasValue, size.HasValue);
            var trueSize = MeasureText(text, size, stringFormat);
            var bitmap = new Bitmap((int)Math.Ceiling(trueSize.Width), (int)Math.Ceiling(trueSize.Height));
            bitmap.MakeTransparent();
            var graphics = Graphics.FromImage(bitmap);
            SetGraphicsMode(graphics);

            if (Amaoto.FontRenderDebug)
            {
                graphics.Clear(Color.FromArgb(128, 255, 0, 0));
            }

            var gp = DrawString(text, graphics, stringFormat, trueSize.ToSize());

            var tex = new Texture(bitmap);

            // 破棄
            bitmap.Dispose();
            graphics.Dispose();
            gp.Dispose();
            return tex;
        }

        /// <summary>
        /// 文字テクスチャを生成します。
        /// </summary>
        /// <param name="text">文字列。</param>
        /// <param name="width">文字の幅 (px)。</param>
        /// <param name="isEllipsis">省略するかしないか。省略しない場合、折り返す。</param>
        /// <returns>テクスチャ。</returns>
        public Texture GetTexture(string text, int width, bool isEllipsis)
        {
            if (FontFamily == null) return new Texture();
            var stringFormat = GetStringFormat(true, isEllipsis);
            var trueSize = MeasureText(text, width, stringFormat);
            var bitmap = new Bitmap((int)Math.Ceiling(trueSize.Width), (int)Math.Ceiling(trueSize.Height));
            bitmap.MakeTransparent();
            var graphics = Graphics.FromImage(bitmap);
            SetGraphicsMode(graphics);

            if (Amaoto.FontRenderDebug)
            {
                graphics.Clear(Color.FromArgb(128, 255, 0, 0));
            }

            var gp = DrawString(text, graphics, stringFormat, new Size((int)Math.Ceiling(trueSize.Width), (int)Math.Ceiling(trueSize.Height)));

            var tex = new Texture(bitmap);

            // 破棄
            bitmap.Dispose();
            graphics.Dispose();
            gp.Dispose();
            return tex;
        }

        private static StringFormat GetStringFormat(bool isWrap, bool isEllipsis)
        {
            var stringFormat = new StringFormat(StringFormat.GenericTypographic)
            {
                // どんなに長くて単語の区切りが良くても改行しない
                FormatFlags = !isWrap ? StringFormatFlags.NoWrap : StringFormatFlags.NoClip,
                // どんなに長くてもトリミングしない
                Trimming = !isEllipsis ? StringTrimming.None : StringTrimming.EllipsisCharacter
            };
            return stringFormat;
        }

        private static void SetGraphicsMode(Graphics graphics)
        {
            // ハイクオリティレンダリング
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            // アンチエイリアスをかける
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            // 透過背景対応のアンチエイリアス (ClearTypeは不透明背景前提のため使用しない)
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
        }

        private GraphicsPath DrawString(string text, Graphics graphics, StringFormat stringFormat, Size? size)
        {
            var gp = new GraphicsPath();

            // GraphicsPath.AddString はバリアブルフォントのグリフアウトラインを
            // 正しく生成できないため、Graphics.DrawString で直接描画する。
            using (var font = new Font(FontFamily, FontSize, FontStyle, GraphicsUnit.Pixel))
            {
                float offsetX = Edge / 2f;
                float offsetY = Edge / 2f;

                if (Edge > 0)
                {
                    // 縁取り: 円形マルチパスで BackColor をオフセット描画してから前景を重ねる。
                    int steps = Math.Max(8, Edge * 2);
                    float radius = Edge / 2f;
                    using (var outlineBrush = new SolidBrush(BackColor))
                    {
                        for (int i = 0; i < steps; i++)
                        {
                            double angle = 2.0 * Math.PI * i / steps;
                            float dx = (float)(radius * Math.Cos(angle));
                            float dy = (float)(radius * Math.Sin(angle));

                            if (size.HasValue)
                            {
                                graphics.DrawString(text, font, outlineBrush,
                                    new RectangleF(offsetX + dx, offsetY + dy, size.Value.Width, size.Value.Height),
                                    stringFormat);
                            }
                            else
                            {
                                graphics.DrawString(text, font, outlineBrush,
                                    offsetX + dx, offsetY + dy, stringFormat);
                            }
                        }
                    }
                }

                using (var foreBrush = new SolidBrush(ForeColor))
                {
                    if (size.HasValue)
                    {
                        graphics.DrawString(text, font, foreBrush,
                            new RectangleF(offsetX, offsetY, size.Value.Width, size.Value.Height),
                            stringFormat);
                    }
                    else
                    {
                        graphics.DrawString(text, font, foreBrush, offsetX, offsetY, stringFormat);
                    }
                }
            }

            return gp;
        }

        private SizeF MeasureText(string text, Size? size, StringFormat stringFormat)
        {
            if (size.HasValue)
            {
                return new SizeF(size.Value.Width, size.Value.Height);
            }

            var bitmap = new Bitmap(16, 16);
            // .NETの敗北
            var graphicsSize = Graphics.FromImage(bitmap).
                MeasureString(text, new Font(FontFamily, FontSize, FontStyle, GraphicsUnit.Pixel));
            var trueGraphicsSize = Graphics.FromImage(bitmap).
                MeasureString(text, new Font(FontFamily, FontSize, FontStyle, GraphicsUnit.Pixel), (int)graphicsSize.Width, stringFormat);
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

        private SizeF MeasureText(string text, int width, StringFormat stringFormat)
        {
            var bitmap = new Bitmap(16, 16);
            var trueGraphicsSize = Graphics.FromImage(bitmap).
                MeasureString(text, new Font(FontFamily, FontSize, FontStyle, GraphicsUnit.Pixel), width, stringFormat);
            bitmap.Dispose();
            if (trueGraphicsSize.Width == 0 || trueGraphicsSize.Height == 0)
            {
                // サイズが0だったとき、とりあえずテクスチャとして成り立つそれっぽいサイズを返す。
                trueGraphicsSize = new SizeF(16f, 16f);
            }

            System.Diagnostics.Trace.WriteLine(trueGraphicsSize);

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

        private FontFamily FontFamily { get; set; }
        private FontStyle FontStyle { get; set; }
        private float FontSize { get; set; }
    }
}