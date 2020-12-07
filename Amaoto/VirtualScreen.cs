using System;
using System.Drawing;
using DxLibDLL;

namespace Amaoto
{
    /// <summary>
    /// 仮想スクリーン。
    /// </summary>
    public class VirtualScreen : IDisposable, ITextureReturnable
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
        /// <param name="drawing">テクスチャを描画するラムダ式。</param>
        /// <returns>VirtualScreen。</returns>
        public VirtualScreen Draw(Action drawing)
        {
            var getDrawScreen = DX.GetDrawScreen();

            DX.SetDrawScreen(Texture.ID);

            drawing?.Invoke();

            DX.SetDrawScreen(getDrawScreen);
            return this;
        }

        /// <summary>
        /// 仮想スクリーンに描画する。
        /// ラムダ式によるDraw()を使用してください。
        /// </summary>
        /// <param name="texture">テクスチャ。</param>
        /// <param name="x">X座標。</param>
        /// <param name="y">Y座標。</param>
        /// <param name="rectangle">描画範囲。</param>
        /// <returns>VirtualScreen。</returns>
        [Obsolete("ラムダ式によるDraw()を使用してください")]
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
        /// 仮想スクリーンのテクスチャを取得する。
        /// </summary>
        /// <returns>仮想スクリーンのテクスチャ。</returns>
        public Texture GetTexture()
        {
            return Texture;
        }

        /// <summary>
        /// 仮想スクリーンのハンドル。
        /// </summary>
        public Texture Texture { get; private set; }
    }
}