using DxLibDLL;
using System;

namespace Amaoto
{
    /// <summary>
    /// Amaoto クラス。
    /// </summary>
    public static class Amaoto
    {
        /// <summary>
        /// Amaoto と DXライブラリの初期化をする。必ず Amaoto の使用前に呼び出す必要がある。
        /// </summary>
        /// <param name="beforeInit">DxLib_Initの前に設定するメソッド。</param>
        /// <param name="afterInit">DxLib_Initの後に設定するメソッド。</param>
        public static void Init(Action beforeInit, Action afterInit)
        {
            beforeInit?.Invoke();

            if (DX.DxLib_Init() == -1)
            {
                throw new Exception("DXLib initialize failed.");
            }

            DX.SetUsePremulAlphaConvertLoad(DX.TRUE);
            DX.CreateMaskScreen();

            afterInit?.Invoke();
        }

        /// <summary>
        /// Amaoto と DXライブラリの終了処理をする。
        /// </summary>
        public static void End()
        {
            if (DX.DxLib_End() == -1)
            {
                throw new Exception("DXLib ending failed.");
            }
        }

        /// <summary>
        /// ループ直後に呼び出すメソッド。
        /// </summary>
        public static void Loop()
        {
            MouseHandled = false;
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
        /// 現在のフレームでマウス操作したと言うことにする。
        /// </summary>
        public static void HandleMouse()
        {
            MouseHandled = true;
        }

        /// <summary>
        /// 長押し時間。
        /// </summary>
        public static int LongClickMs { get; private set; } = 400;

        /// <summary>
        /// FontRenderのデバッグを行うかどうか。
        /// </summary>
        public static bool FontRenderDebug { get; private set; } = false;

        /// <summary>
        /// 現在のフレームでマウス操作が行われたかどうか。
        /// </summary>
        public static bool MouseHandled { get; private set; } = false;
    }
}
