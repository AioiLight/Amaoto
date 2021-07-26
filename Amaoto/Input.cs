using DxLibDLL;
using System.Text;

namespace Amaoto
{
    /// <summary>
    /// 文字列入力を行う。
    /// </summary>
    public static class Input
    {
        /// <summary>
        /// 文字列入力の初期化を行う。
        /// </summary>
        /// <param name="maximumLength">最大文字数 (バイト)</param>
        /// <param name="escapeCancelable">エスケープキーで入力をキャンセルできるかどうか。</param>
        /// <param name="inputSingleOnly">半角文字のみ入力可能にするかどうか。</param>
        /// <param name="inputNumberOnly">数字のみ入力可能にするかどうか。</param>
        /// <param name="inputDoubleOnly">全角文字のみ入力可能にするかどうか。</param>
        /// <param name="multiLine">複数行入力にするかどうか。</param>
        public static void Init(ulong maximumLength = 512, bool escapeCancelable = true, bool inputSingleOnly = false, bool inputNumberOnly = false, bool inputDoubleOnly = false, bool multiLine = false)
        {
            MaximumLength = maximumLength;
            Handle = DX.MakeKeyInput(MaximumLength,
                escapeCancelable ? 1 : 0,
                inputSingleOnly ? 1 : 0,
                inputNumberOnly ? 1 : 0,
                inputDoubleOnly ? 1 : 0,
                multiLine ? 1 : 0); ;
            Builder = new StringBuilder((int)MaximumLength);
            DX.SetActiveKeyInput(Handle);
            IsEnable = true;
        }

        /// <summary>
        /// 文字入力を終了する。
        /// </summary>
        public static void End()
        {
            if (IsEnable)
            {
                var result = DX.DeleteKeyInput(Handle);
                if (result == 0)
                {
                    IsEnable = false;
                    Builder = null;
                }
            }
        }

        /// <summary>
        /// 現在のキー入力の状態。
        /// </summary>
        public static KeyInputState KeyInputState
        {
            get
            {
                var result = DX.CheckKeyInput(Handle);
                switch (result)
                {
                    case 0:
                        return KeyInputState.Typing;
                    case 1:
                        return KeyInputState.Finished;
                    case 2:
                        return KeyInputState.Canceled;
                    case -1:
                    default:
                        return KeyInputState.Error;
                }
            }
        }

        /// <summary>
        /// テキスト。
        /// </summary>
        public static string Text
        {
            get
            {
                DX.GetKeyInputString(Builder, Handle);
                return Builder.ToString();
            }
            set
            {
                DX.SetKeyInputString(value, Handle);
            }
        }

        /// <summary>
        /// 現在位置。
        /// </summary>
        public static int Position
        {
            get
            {
                return DX.GetKeyInputCursorPosition(Handle);
            }
            set
            {
                DX.SetKeyInputCursorPosition(value, Handle);
            }
        }

        /// <summary>
        /// 選択範囲。
        /// </summary>
        public static (int Start, int End) Selection
        {
            get
            {
                DX.GetKeyInputSelectArea(out var s, out var e, Handle);
                return (s, e);
            }
            set
            {
                DX.SetKeyInputSelectArea(value.Start, value.End, Handle);
            }
        }

        /// <summary>
        /// 有効かどうか。
        /// </summary>
        public static bool IsEnable { get; private set; }
        private static StringBuilder Builder;
        private static int Handle;
        private static ulong MaximumLength;
    }

    /// <summary>
    /// キー入力の状態。
    /// </summary>
    public enum KeyInputState
    {
        /// <summary>
        /// タイピング中。
        /// </summary>
        Typing,
        /// <summary>
        /// 完了。
        /// </summary>
        Finished,
        /// <summary>
        /// キャンセルされた。
        /// </summary>
        Canceled,
        /// <summary>
        /// エラー。
        /// </summary>
        Error
    }
}
