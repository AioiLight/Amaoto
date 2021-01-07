﻿using DxLibDLL;

namespace Amaoto
{
    /// <summary>
    /// Amaoto クラス。
    /// </summary>
    public static class Amaoto
    {
        /// <summary>
        /// Amaoto の初期化をする。必ず Amaoto の使用前に呼び出す必要がある。
        /// </summary>
        public static void Init()
        {
            DX.SetUsePremulAlphaConvertLoad(DX.TRUE);
            DX.CreateMaskScreen();
        }

        /// <summary>
        /// GUI で使われる長押し時間を変更する。
        /// </summary>
        /// <param name="ms">ミリ秒。</param>
        public static void SetLongClickNs(int ms)
        {
            LongClickMs = ms;
        }


        /// <summary>
        /// FontRenderのデバッグを行うかどうか設定する。
        /// </summary>
        /// <param name="debug">デバッグを行う。</param>
        public static void SetFontRenderDebug(bool debug)
        {
            FontRenderDebug = debug;
        }

        /// <summary>
        /// 長押し時間。
        /// </summary>
        public static int LongClickMs { get; private set; } = 400;

        /// <summary>
        /// FontRenderのデバッグを行うかどうか。
        /// </summary>
        public static bool FontRenderDebug { get; private set; } = false;
    }
}
