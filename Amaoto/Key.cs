using DxLibDLL;

namespace Amaoto
{
    /// <summary>
    /// キーボードを管理するクラス。
    /// </summary>
    public static class Key
    {
        /// <summary>
        /// キーボード入力の状態をチェックします。毎フレーム呼び出す必要があります。
        /// </summary>
        public static void Update()
        {
            DX.GetHitKeyStateAll(Buffer);
            for (int i = 0; i < 256; i++)
            {
                if (Buffer[i] == 1)
                {
                    if (Keys[i] == 0) Keys[i] = 1;
                    else if (Keys[i] == 1) Keys[i] = 2;
                }
                else
                {
                    Keys[i] = 0;
                }
            }
        }

        /// <summary>
        /// そのキーを押したかどうかチェックします。
        /// </summary>
        /// <param name="key">キーコード。</param>
        /// <returns>入力されたかどうか。</returns>
        public static bool IsPushed(int key)
        {
            return Keys[key] == 1;
        }

        /// <summary>
        /// そのキーを押しているかどうかチェックします。
        /// </summary>
        /// <param name="key">キーコード。</param>
        /// <returns>入力されているかどうか。</returns>
        public static bool IsPushing(int key)
        {
            return Keys[key] > 0;
        }

        private static readonly int[] Keys = new int[256];
        private static readonly byte[] Buffer = new byte[256];
    }
}