using DxLibDLL;

namespace Amaoto
{
    /// <summary>
    /// マウス操作を管理するクラス。
    /// </summary>
    public static class Mouse
    {
        /// <summary>
        /// マウスの入力を処理する。必ず毎フレーム呼ぶ必要があります。
        /// </summary>
        public static void Update()
        {
            Wheel = DX.GetMouseWheelRotVolF();

            DX.GetMousePoint(out var x, out var y);

            IsMouseMoving = false;
            if (X != x || Y != y || Wheel != 0)
            {
                IsMouseMoving = true;
            }

            X = x;
            Y = y;

            var mouse = DX.GetMouseInput();

            var isAnyButtonPushed = false;

            for (int i = 0; i < Buttons.Length; i++)
            {
                if ((mouse & (int)GetMouseButtonFromIndex(i)) != 0)
                {
                    // 押してる
                    if (Buttons[i] <= 0)
                    {
                        // 押してない状態から、押してる状態になった
                        Buttons[i] = 1;
                    }
                    else
                    {
                        // 押してる状態が継続してる
                        Buttons[i] = 2;
                    }

                    if (!isAnyButtonPushed)
                    {
                        isAnyButtonPushed = true;
                    }
                }
                else
                {
                    // 押してない
                    if (Buttons[i] >= 1)
                    {
                        // 押してる状態から、離した状態になった
                        Buttons[i] = -1;
                    }
                    else
                    {
                        // 押してない状態が継続してる
                        Buttons[i] = 0;
                    }
                }
            }

            IsAnyButtonPushing = isAnyButtonPushed;
        }

        /// <summary>
        /// マウスが押されたかどうかチェックします。。
        /// </summary>
        /// <param name="mouseButton">ボタン。</param>
        /// <returns>押されたかどうか。</returns>
        public static bool IsPushed(MouseButton mouseButton)
        {
            return Buttons[GetIndexFromMouseButton(mouseButton)] == 1;
        }

        /// <summary>
        /// マウスが押されているかどうかチェックします。
        /// </summary>
        /// <param name="mouseButton">ボタン。</param>
        /// <returns>押されているかどうか。</returns>
        public static bool IsPushing(MouseButton mouseButton)
        {
            return Buttons[GetIndexFromMouseButton(mouseButton)] > 0;
        }

        /// <summary>
        /// マウスのボタンが離されたかどうかチェックします。
        /// </summary>
        /// <param name="mouseButton">ボタン。</param>
        /// <returns>離されたかどうか。</returns>
        public static bool IsLeft(MouseButton mouseButton)
        {
            return Buttons[GetIndexFromMouseButton(mouseButton)] == -1;
        }

        private static int GetIndexFromMouseButton(MouseButton mouseButton)
        {
            switch (mouseButton)
            {
                case MouseButton.Left:
                    return 0;

                case MouseButton.Right:
                    return 1;

                case MouseButton.Middle:
                    return 2;

                case MouseButton.Button4:
                    return 3;

                case MouseButton.Button5:
                    return 4;

                default:
                    return 0;
            }
        }

        private static MouseButton GetMouseButtonFromIndex(int index)
        {
            switch (index)
            {
                case 0:
                    return MouseButton.Left;

                case 1:
                    return MouseButton.Right;

                case 2:
                    return MouseButton.Middle;

                case 3:
                    return MouseButton.Button4;

                case 4:
                    return MouseButton.Button5;

                default:
                    return MouseButton.Left;
            }
        }

        private static readonly int[] Buttons = new int[5];

        /// <summary>
        /// マウスホイール回転量。
        /// 奥に回すと正の数になる。
        /// </summary>
        public static float Wheel { get; private set; }

        /// <summary>
        /// マウスX座標。
        /// </summary>
        public static int X { get; private set; }
        /// <summary>
        /// マウスY座標。
        /// </summary>
        public static int Y { get; private set; }

        /// <summary>
        /// マウスボタンのどれかが押下されているかどうか。
        /// フレームの更新のたびに false になります。
        /// </summary>
        public static bool IsAnyButtonPushing { get; private set; }

        /// <summary>
        /// マウスが動いているかどうか。
        /// フレームの更新のたびに false になります。
        /// </summary>
        public static bool IsMouseMoving { get; private set; }
    }

    /// <summary>
    /// マウスのボタン。
    /// </summary>
    public enum MouseButton
    {
        /// <summary>
        /// 左
        /// </summary>
        Left = DX.MOUSE_INPUT_LEFT,

        /// <summary>
        /// 右
        /// </summary>
        Right = DX.MOUSE_INPUT_RIGHT,

        /// <summary>
        /// 中央
        /// </summary>
        Middle = DX.MOUSE_INPUT_MIDDLE,

        /// <summary>
        /// ボタン4。
        /// </summary>
        Button4 = DX.MOUSE_INPUT_4,

        /// <summary>
        /// ボタン5。
        /// </summary>
        Button5 = DX.MOUSE_INPUT_5
    }
}