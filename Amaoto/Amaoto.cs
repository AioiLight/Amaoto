using DxLibDLL;

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
        }
    }
}
