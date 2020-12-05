using System;
using System.Drawing;
using DxLibDLL;

namespace Amaoto
{
    /// <summary>
    /// 仮想スクリーン。
    /// </summary>
    public class VirtualScreen : IDisposable
    {
        /// <summary>
        /// 仮想スクリーンを作成します。
        /// </summary>
        /// <param name="width">横幅。</param>
        /// <param name="height">縦幅。</param>
        public VirtualScreen(int width, int height)
        {
            Texture = new Texture(DX.MakeScreen(width, height, DX.TRUE));
        }

        /// <summary>
        /// 仮想スクリーンに描画する。
        /// </summary>
        /// <param name="texture">テクスチャ。</param>
        /// <param name="x">X座標。</param>
        /// <param name="y">Y座標。</param>
        /// <param name="rectangle">描画範囲。</param>
        /// <returns>VirtualScreen。</returns>
        public VirtualScreen Draw(Texture texture, float x, float y, Rectangle? rectangle = null)
        {
            if (texture == null)
            {
                return this;
            }

            var getDrawScreen = DX.GetDrawScreen();

            DX.SetDrawScreen(Texture.ID);

            texture.Draw(x, y, rectangle);

            DX.SetDrawScreen(getDrawScreen);

            return this;
        }

        /// <summary>
        /// 画面をクリアする。
        /// </summary>
        public void ClearScreen()
        {
            var getDrawScreen = DX.GetDrawScreen();

            DX.SetDrawScreen(Texture.ID);

            DX.ClearDrawScreen();

            DX.SetDrawScreen(getDrawScreen);
        }

        /// <summary>
        /// 仮想スクリーンを破棄する。
        /// </summary>
        public void Dispose()
        {
            Texture?.Dispose();
            Texture = null;
        }

        /// <summary>
        /// 仮想スクリーンのハンドル。
        /// </summary>
        public Texture Texture { get; private set; }
    }
}