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
                    // 前のフレームでは押していなかった
                    if (!IsPushing(i))
                    {
                        // 押していない状態から押している状態になった
                        Keys[i] = 1;
                    }
                    else
                    {
                        // 押している状態から押している状態になった。
                        Keys[i] = 2;
                    }
                }
                else
                {
                    // 前のフレームでは押していた
                    if (IsPushing(i))
                    {
                        // 押している状態から押していない状態になった
                        Keys[i] = -1;
                    }
                    else
                    {
                        // 押していない状態から押していない状態になった。
                        Keys[i] = 0;
                    }
                }
            }
        }

        /// <summary>
        /// そのキーを押したかどうかチェックします。
        /// </summary>
        /// <param name="key">キーコード。</param>
        /// <returns>押したかどうか。</returns>
        public static bool IsPushed(int key)
        {
            return Keys[key] == 1;
        }

        /// <summary>
        /// そのキーを押しているかどうかチェックします。
        /// </summary>
        /// <param name="key">キーコード。</param>
        /// <returns>押しているかどうか。</returns>
        public static bool IsPushing(int key)
        {
            return Keys[key] > 0;
        }

        /// <summary>
        /// そのキーを離したかどうかチェックします。
        /// </summary>
        /// <param name="key">キーコード。</param>
        /// <returns>離したかどうか。</returns>
        public static bool IsLeft(int key)
        {
            return Keys[key] == -1;
        }

        private static readonly int[] Keys = new int[256];
        private static readonly byte[] Buffer = new byte[256];
    }
}