using DxLibDLL;
using System;
using System.Drawing;

namespace Amaoto
{
    /// <summary>
    /// アトラスを生成する。
    /// 単一の画像から複数のテクスチャを切り出すことで、描画の高速化を実現する。
    /// </summary>
    public class AtlasTexture : IDisposable
    {
        /// <summary>
        /// アトラスを生成する。
        /// </summary>
        /// <param name="fileName">アトラスのファイル名。</param>
        public AtlasTexture(string fileName)
        {
            Source = new Texture(fileName);
        }

        ~AtlasTexture()
        {
            Dispose();
        }

        /// <summary>
        /// アトラスからテクスチャを取得する。
        /// </summary>
        /// <param name="rectangle">矩形。</param>
        /// <returns>テクスチャ。</returns>
        public Texture GetTexture(Rectangle rectangle)
        {
            var t = DX.DerivationGraph(rectangle.X, rectangle.Y,
                rectangle.Width, rectangle.Height,
                Source.ID);

            return new Texture(t);
        }

        /// <summary>
        /// アトラスを破棄する。
        /// </summary>
        public void Dispose()
        {
            Source?.Dispose();
        }

        private protected Texture Source;
    }
}
