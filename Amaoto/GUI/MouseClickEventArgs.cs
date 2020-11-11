using System;

namespace Amaoto.GUI
{
    /// <summary>
    /// マウスでクリックしたときのイベント。
    /// </summary>
    public class MouseClickEventArgs : EventArgs
    {
        /// <summary>
        /// マウスでクリックしたときのイベント。
        /// </summary>
        /// <param name="posX">X座標。</param>
        /// <param name="posY">Y座標。</param>
        public MouseClickEventArgs(int posX, int posY)
        {
            PosX = posX;
            PosY = posY;
        }

        /// <summary>
        /// X座標。
        /// </summary>
        public int PosX { get; }
        /// <summary>
        /// Y座標。
        /// </summary>
        public int PosY { get; }
    }
}
