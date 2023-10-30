using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;

namespace Amaoto
{
    /// <summary>
    /// テクスチャを使用したマスクを作成し、描画範囲を切り取ります。
    /// </summary>
    public class TextureMask : IDisposable
    {
        /// <summary>
        /// テクスチャを使用したマスクを作成します。
        /// </summary>
        /// <param name="width">マスクの横解像度。</param>
        /// <param name="height">マスクの縦解像度。</param>
        public TextureMask(int width, int height)
        {
            Screen = new VirtualScreen(width, height);
        }

        /// <summary>
        /// マスクを作る。
        /// </summary>
        /// <param name="mask">マスクにする内容。</param>
        /// <returns>TextureMask。</returns>
        public TextureMask CreateMask(Action mask)
        {
            Screen.ClearScreen();
            Screen.Draw(mask);
            return this;
        }

        /// <summary>
        /// マスクを使用して描画する。
        /// </summary>
        /// <param name="masking">マスクする内容。</param>
        /// <param name="reverse">マスクを反転するかどうか。</param>
        /// <returns>TextureMask。</returns>
        public TextureMask Masking(Action masking, bool reverse = false)
        {
            if (reverse)
            {
                DX.SetMaskReverseEffectFlag(DX.TRUE);
            }
            DX.SetMaskScreenGraph(Screen.Texture.ID);
            DX.SetUseMaskScreenFlag(DX.TRUE);


            masking?.Invoke();

            DX.SetUseMaskScreenFlag(DX.FALSE);
            DX.SetMaskScreenGraph(-1);
            if (reverse)
            {
                DX.SetMaskReverseEffectFlag(DX.FALSE);
            }
            return this;
        }

        /// <summary>
        /// マスク画像を破棄する。
        /// </summary>
        public void Dispose()
        {
            Screen?.Dispose();
        }

        private readonly VirtualScreen Screen;
    }
}
