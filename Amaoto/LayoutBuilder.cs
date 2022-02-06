using System.Linq;

namespace Amaoto
{
    /// <summary>
    /// 簡易的に複数テクスチャのレイアウトを組む静的クラス。
    /// </summary>
    public static class LayoutBuilder
    {
        /// <summary>
        /// 縦並びにテクスチャを結合する。
        /// </summary>
        /// <param name="children">テクスチャ。</param>
        /// <param name="padding">パディング。</param>
        /// <param name="spacing">テクスチャとテクスチャの間。</param>
        /// <param name="width">横幅。</param>
        /// <param name="height">縦幅。</param>
        /// <returns>結合結果。</returns>
        public static ITextureReturnable Column(ITextureReturnable[] children,
            int padding = 0, int spacing = 0,
            int? width = null, int? height = null)
        {
            var texWidth = width ?? GetMaximumWidth(children, padding);
            var texHeight = CalcHeight(children, padding, spacing);

            var screen = new VirtualScreen(texWidth, texHeight);

            var drawY = padding;
            screen.Draw(() =>
            {
                for (var i = 0; i < children.Length; i++)
                {
                    var t = children[i].GetTexture();
                    var origRef = t.ReferencePoint;
                    t.ReferencePoint = AmaotoUtil.GetToppedReferencePoint(t.ReferencePoint);
                    var (drawX, _) = AmaotoUtil.GetProperPositionFromReferencePoint(texWidth, texHeight, t.ReferencePoint);
                    if (t.ActualSize.Width > texWidth)
                    {
                        // はみ出してしまうのであれば、拡大率を変更する。
                        var r = t.ScaleX;
                        t.ScaleX = AmaotoUtil.GetProperScaleX(t, texWidth);
                        t.Draw(padding + drawX, drawY);
                        t.ScaleX = r;
                    }
                    else
                    {
                        t.Draw(padding + drawX, drawY);
                    }
                    t.ReferencePoint = origRef;
                    drawY += t.ActualSize.Height;
                    drawY += spacing;
                }
            });

            var result = new VirtualScreen(screen.GetTexture().TextureSize.Width, height ?? screen.GetTexture().TextureSize.Height);
            var screenTex = screen.GetTexture();
            screenTex.ScaleY = AmaotoUtil.GetProperScaleY(screenTex, height ?? screen.GetTexture().TextureSize.Height);
            result.Draw(() => screenTex.Draw(0, 0));

            return result;
        }

        /// <summary>
        /// 横並びにテクスチャを結合する。
        /// </summary>
        /// <param name="children">テクスチャ。</param>
        /// <param name="padding">パディング。</param>
        /// <param name="spacing">テクスチャとテクスチャの間。</param>
        /// <param name="width">横幅。</param>
        /// <param name="height">縦幅。</param>
        /// <returns>結合結果。</returns>
        public static ITextureReturnable Row(ITextureReturnable[] children,
            int padding = 0, int spacing = 0,
            int? width = null, int? height = null)
        {
            var texWidth = CalcWidth(children, padding, spacing);
            var texHeight = height ?? GetMaximumHeight(children, padding);

            var screen = new VirtualScreen(texWidth, texHeight);

            var drawX = padding;
            screen.Draw(() =>
            {
                for (var i = 0; i < children.Length; i++)
                {
                    var t = children[i].GetTexture();
                    var origRef = t.ReferencePoint;
                    t.ReferencePoint = AmaotoUtil.GetLeftedReferencePoint(t.ReferencePoint);
                    var (_, drawY) = AmaotoUtil.GetProperPositionFromReferencePoint(texWidth, texHeight, t.ReferencePoint);
                    if (t.ActualSize.Height > texHeight)
                    {
                        // はみ出してしまうのであれば、拡大率を変更する。
                        var r = t.ScaleY;
                        t.ScaleY = AmaotoUtil.GetProperScaleY(t, texHeight);
                        t.Draw(drawX, padding + drawY);
                        t.ScaleY = r;
                    }
                    else
                    {
                        t.Draw(drawX, padding + drawY);
                    }
                    t.ReferencePoint = origRef;
                    drawX += t.ActualSize.Width;
                    drawX += spacing;
                }
            });

            var result = new VirtualScreen(width ?? screen.GetTexture().TextureSize.Width, screen.GetTexture().TextureSize.Height);
            var screenTex = screen.GetTexture();
            screenTex.ScaleX = AmaotoUtil.GetProperScaleX(screenTex, width ?? screen.GetTexture().TextureSize.Width);
            result.Draw(() => screenTex.Draw(0, 0));

            return result;
        }

        private static int GetMaximumWidth(ITextureReturnable[] children, int padding)
        {
            return children.Max(c => c.GetTexture().ActualSize.Width) + (padding * 2);
        }

        private static int GetMaximumHeight(ITextureReturnable[] children, int padding)
        {
            return children.Max(c => c.GetTexture().ActualSize.Height) + (padding * 2);
        }

        private static int CalcWidth(ITextureReturnable[] children, int padding, int spacing)
        {
            var result = 0;
            result += padding;
            for (var i = 0; i < children.Length; i++)
            {
                result += children[i].GetTexture().ActualSize.Width;
                if (children[i] != children.Last())
                {
                    result += spacing;
                }
            }
            result += padding;
            return result;
        }

        private static int CalcHeight(ITextureReturnable[] children, int padding, int spacing)
        {
            var result = 0;
            result += padding;
            for (var i = 0; i < children.Length; i++)
            {
                result += children[i].GetTexture().ActualSize.Height;
                if (children[i] != children.Last())
                {
                    result += spacing;
                }
            }
            result += padding;
            return result;
        }
    }
}
