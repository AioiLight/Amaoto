using DxLibDLL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amaoto
{
    /// <summary>
    /// リサイズ可能なウィンドウ上のテクスチャの描画機能を提供するクラス。
    /// 一般的に 9 スライスなどと呼ばれています。
    /// </summary>
    public class ResizeableBoxTexture : IDisposable
    {
        public ResizeableBoxTexture(int cornerWidth, int cornerHeight, Texture texture)
        {
            var t = new AtlasTexture(texture);

            TopLeft = t.DerivateTexture(new Rectangle(0, 0, cornerWidth, cornerHeight));
            Top = t.DerivateTexture(new Rectangle(cornerWidth, 0, texture.TextureSize.Width - (cornerWidth * 2), cornerHeight));
            TopRight = t.DerivateTexture(new Rectangle(texture.TextureSize.Width - cornerWidth, 0, cornerWidth, cornerHeight));

            Left = t.DerivateTexture(new Rectangle(0, cornerHeight, cornerWidth, texture.TextureSize.Height - (cornerHeight * 2)));
            Center = t.DerivateTexture(new Rectangle(cornerWidth, cornerHeight, texture.TextureSize.Width - (cornerWidth * 2), texture.TextureSize.Height - (cornerHeight * 2)));
            Right = t.DerivateTexture(new Rectangle(texture.TextureSize.Width - cornerWidth, cornerHeight, cornerWidth, texture.TextureSize.Height - (cornerHeight * 2)));

            BottomLeft = t.DerivateTexture(new Rectangle(0, texture.TextureSize.Height - cornerHeight, cornerWidth, cornerHeight));
            Bottom = t.DerivateTexture(new Rectangle(cornerWidth, texture.TextureSize.Height - cornerHeight, texture.TextureSize.Width - (cornerWidth * 2), cornerHeight));
            BottomRight = t.DerivateTexture(new Rectangle(texture.TextureSize.Width - cornerWidth, texture.TextureSize.Height - cornerHeight, cornerWidth, cornerHeight));

            CornerWidth = cornerWidth;
            CornerHeight = cornerHeight;
        }

        ~ResizeableBoxTexture()
        {
            Dispose();
        }

        public void Draw(Rectangle r)
        {
            TopLeft.Draw(r.X, r.Y);
            Extend(Top, r.X + CornerWidth, r.Y, r.Width - (CornerWidth * 2), CornerHeight);
            TopRight.Draw(r.Right - CornerWidth, r.Y);

            Extend(Left, r.X, r.Y + CornerHeight, CornerWidth, r.Height - (CornerHeight * 2));
            Extend(Center, r.X + CornerWidth, r.Y + CornerHeight, r.Width - (CornerWidth * 2), r.Height - (CornerHeight * 2));
            Extend(Right, r.Right - CornerWidth, r.Y + CornerHeight, CornerWidth, r.Height - (CornerHeight * 2));

            BottomLeft.Draw(r.X, r.Bottom - CornerHeight);
            Extend(Bottom, r.X + CornerWidth, r.Bottom - CornerHeight, r.Width - (CornerWidth * 2), CornerHeight);
            BottomRight.Draw(r.Right - CornerWidth, r.Bottom - CornerHeight);
        }

        private void Extend(Texture t, int x, int y, int width, int height)
        {
            DX.DrawExtendGraph(x, y, x + width, y + height, t.ID, DX.TRUE);
        }

        public void Dispose()
        {
            TopLeft?.Dispose();
            Top?.Dispose();
            TopRight?.Dispose();
            Left?.Dispose();
            Center?.Dispose();
            Right?.Dispose();
            BottomLeft?.Dispose();
            Bottom?.Dispose();
            BottomRight?.Dispose();
        }

        private readonly Texture TopLeft;
        private readonly Texture Top;
        private readonly Texture TopRight;
        private readonly Texture Left;
        private readonly Texture Center;
        private readonly Texture Right;
        private readonly Texture BottomLeft;
        private readonly Texture Bottom;
        private readonly Texture BottomRight;
        private readonly int CornerWidth;
        private readonly int CornerHeight;
    }
}
