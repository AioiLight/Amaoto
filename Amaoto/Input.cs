using DxLibDLL;

namespace Amaoto
{
    /// <summary>
    /// 入力を管理するクラス。
    /// </summary>
    public class Input
    {
        /// <summary>
        /// 入力を管理するクラス。
        /// </summary>
        public Input()
        {
            Keys = new byte[256];
            Buffer = new byte[256];
        }

        /// <summary>
        /// キー入力の状態をチェックします。毎フレーム呼び出す必要があります。
        /// </summary>
        public void Update()
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
        /// そのキーが入力されたかどうかチェックします。
        /// </summary>
        /// <param name="key">キーコード。</param>
        /// <returns>入力されたかどうか。</returns>
        public bool IsPushedKey(int key)
        {
            return Keys[key] == 1;
        }

        /// <summary>
        /// そのキーが入力されているかどうかチェックします。
        /// </summary>
        /// <param name="key">キーコード。</param>
        /// <returns>入力されているかどうか。</returns>
        public bool IsPushingKey(int key)
        {
            return Keys[key] > 0;
        }

        private readonly byte[] Keys;
        private readonly byte[] Buffer;
    }
}