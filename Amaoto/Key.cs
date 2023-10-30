using DxLibDLL;
using System.Collections.Generic;

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
            IsAnyKeyPushing = false;
            DX.GetHitKeyStateAll(Buffer);
            for (int i = 0; i < 256; i++)
            {
                if (Buffer[i] == 1)
                {
                    if (!IsAnyKeyPushing)
                    {
                        IsAnyKeyPushing = true;
                    }

                    // 現在押している
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
                    // 現在押していない
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
        
        /// <summary>
        /// キーコードに対応する文字列を取得します。
        /// </summary>
        /// <param name="key">キーコード。</param>
        /// <returns><c>key</c> に対応する文字列。正しくないキーコードの場合は <c>UNKNOWN</c> が返る。</returns>
        public static string GetKeyCodeString(int key)
        {
            if (KeyCodeString.TryGetValue(key, out var result))
            {
                return result;
            }
            else
            {
                return "UNKNOWN";
            }
        }

        private static readonly int[] Keys = new int[256];
        private static readonly byte[] Buffer = new byte[256];

        /// <summary>
        /// キーコードの文字列。
        /// </summary>
        public static readonly Dictionary<int, string> KeyCodeString
            = new Dictionary<int, string>()
            {
                { 4, "3" },
                { 5, "4" },
                { 6, "5" },
                { 7, "6" },
                { 8, "7" },
                { 9, "8" },
                { 10, "9" },
                { 3, "2" },
                { 2, "1" },
                { 11, "0" },
                { 44, "Z" },
                { 32, "D" },
                { 18, "E" },
                { 33, "F" },
                { 34, "G" },
                { 35, "H" },
                { 23, "I" },
                { 36, "J" },
                { 37, "K" },
                { 38, "L" },
                { 50, "M" },
                { 49, "N" },
                { 25, "P" },
                { 16, "Q" },
                { 19, "R" },
                { 31, "S" },
                { 20, "T" },
                { 22, "U" },
                { 47, "V" },
                { 17, "W" },
                { 45, "X" },
                { 21, "Y" },
                { 24, "O" },
                { 46, "C" },
                { 48, "B" },
                { 88, "F12" },
                { 14, "BACK" },
                { 15, "TAB" },
                { 28, "RETURN" },
                { 42, "LSHIFT" },
                { 54, "RSHIFT" },
                { 29, "LCONTROL" },
                { 157, "RCONTROL" },
                { 1, "ESCAPE" },
                { 57, "SPACE" },
                { 201, "PGUP" },
                { 209, "PGDN" },
                { 207, "END" },
                { 199, "HOME" },
                { 203, "LEFT" },
                { 200, "UP" },
                { 205, "RIGHT" },
                { 208, "DOWN" },
                { 210, "INSERT" },
                { 211, "DELETE" },
                { 12, "MINUS" },
                { 125, "YEN" },
                { 30, "A" },
                { 144, "PREVTRACK" },
                { 53, "SLASH" },
                { 77, "NUMPAD6" },
                { 71, "NUMPAD7" },
                { 72, "NUMPAD8" },
                { 73, "NUMPAD9" },
                { 55, "MULTIPLY" },
                { 78, "ADD" },
                { 74, "SUBTRACT" },
                { 83, "DECIMAL" },
                { 181, "DIVIDE" },
                { 156, "NUMPADENTER" },
                { 59, "F1" },
                { 60, "F2" },
                { 61, "F3" },
                { 62, "F4" },
                { 63, "F5" },
                { 64, "F6" },
                { 65, "F7" },
                { 66, "F8" },
                { 67, "F9" },
                { 68, "F10" },
                { 87, "F11" },
                { 76, "NUMPAD5" },
                { 75, "NUMPAD4" },
                { 81, "NUMPAD3" },
                { 80, "NUMPAD2" },
                { 56, "LALT" },
                { 184, "RALT" },
                { 70, "SCROLL" },
                { 39, "SEMICOLON" },
                { 146, "COLON" },
                { 26, "LBRACKET" },
                { 27, "RBRACKET" },
                { 145, "AT" },
                { 43, "BACKSLASH" },
                { 51, "COMMA" },
                { 52, "PERIOD" },
                { 148, "KANJI" },
                { 123, "NOCONVERT" },
                { 112, "KANA" },
                { 221, "APPS" },
                { 58, "CAPSLOCK" },
                { 183, "SYSRQ" },
                { 197, "PAUSE" },
                { 219, "LWIN" },
                { 220, "RWIN" },
                { 82, "NUMPAD0" },
                { 79, "NUMPAD1" },
                { 121, "CONVERT" },
                { 69, "NUMLOCK" }
            };

        /// <summary>
        /// キーのどれかが押されているかどうか。
        /// フレームの更新のたびに false になる。
        /// </summary>
        public static bool IsAnyKeyPushing { get; private set; }
    }
}