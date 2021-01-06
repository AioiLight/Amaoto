using DxLibDLL;
using System;

namespace Amaoto
{
    /// <summary>
    /// DXライブラリに関するユーティリティクラス。
    /// </summary>
    public static class DXLibUtil
    {
        /// <summary>
        /// 透明度とブレンドモードを指定してDXライブラリの描画命令を実行する。
        /// </summary>
        /// <param name="opacity">0～255の不透明度。</param>
        /// <param name="blendMode">ブレンドモード。</param>
        /// <param name="content">Textureクラスを用いないDXライブラリの描画命令。</param>
        public static void DrawWithOpacity(int opacity, BlendMode blendMode, Action content)
        {
            DX.SetDrawBlendMode(GetBlendModeConstant(blendMode), opacity);
            content?.Invoke();
            DX.SetDrawBlendMode(GetBlendModeConstant(BlendMode.None), 255);
        }

        /// <summary>
        /// 透明度とブレンドモードを指定してDXライブラリの描画命令を実行する。
        /// </summary>
        /// <param name="opacity">0～1.0の不透明度。</param>
        /// <param name="blendMode">ブレンドモード。</param>
        /// <param name="content">Textureクラスを用いないDXライブラリの描画命令。</param>
        public static void DrawWithOpacity(double opacity, BlendMode blendMode, Action content)
        {
            DrawWithOpacity((int)(opacity * 255), blendMode, content);
        }

        /// <summary>
        /// BlendMode列挙型からDXライブラリのブレンドモードの定数を取得する。
        /// </summary>
        /// <param name="blendMode">BlendMode列挙型。</param>
        /// <returns>ブレンドモードの定数。</returns>
        public static int GetBlendModeConstant(BlendMode blendMode)
        {
            if (blendMode == BlendMode.None)
            {
                return DX.DX_BLENDMODE_PMA_ALPHA;
            }
            else if (blendMode == BlendMode.Add)
            {
                return DX.DX_BLENDMODE_PMA_ADD;
            }
            else if (blendMode == BlendMode.Subtract)
            {
                return DX.DX_BLENDMODE_PMA_SUB;
            }
            return DX.DX_BLENDMODE_PMA_ALPHA;
        }
    }
}
