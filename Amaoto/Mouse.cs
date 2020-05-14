using DxLibDLL;

namespace Amaoto
{
    /// <summary>
    /// マウス操作。
    /// </summary>
    public class Mouse
    {
        /// <summary>
        /// マウス操作。
        /// </summary>
        public Mouse()
        {
            Buttons = new int[5];
        }

        /// <summary>
        /// マウスの入力を処理する。
        /// </summary>
        public void Update()
        {
            Wheel = DX.GetMouseWheelRotVolF();

            DX.GetMousePoint(out var x, out var y);
            Point = (x, y);

            var mouse = DX.GetMouseInput();

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
        }

        /// <summary>
        /// マウスが押されたかどうかチェックする。
        /// </summary>
        /// <param name="mouseButton">ボタン。</param>
        /// <returns>押されたかどうか。</returns>
        public bool IsPushedButton(MouseButton mouseButton)
        {
            return Buttons[GetIndexFromMouseButton(mouseButton)] == 1;
        }

        /// <summary>
        /// マウスが押されているかどうかチェックする。
        /// </summary>
        /// <param name="mouseButton">ボタン。</param>
        /// <returns>押されているかどうか。</returns>
        public bool IsPushingButton(MouseButton mouseButton)
        {
            return Buttons[GetIndexFromMouseButton(mouseButton)] > 0;
        }

        /// <summary>
        /// マウスのボタンが離されたかどうかチェックする。
        /// </summary>
        /// <param name="mouseButton">ボタン。</param>
        /// <returns>離されたかどうか。</returns>
        public bool IsLeftButton(MouseButton mouseButton)
        {
            return Buttons[GetIndexFromMouseButton(mouseButton)] == -1;
        }

        private int GetIndexFromMouseButton(MouseButton mouseButton)
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

        private MouseButton GetMouseButtonFromIndex(int index)
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

        private readonly int[] Buttons;

        /// <summary>
        /// マウスホイール回転量。
        /// 奥に回すと正の数になる。
        /// </summary>
        public float Wheel { get; private set; }

        /// <summary>
        /// マウス座標。
        /// </summary>
        public (int x, int y) Point { get; private set; }
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
