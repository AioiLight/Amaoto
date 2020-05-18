using System;
using System.Collections.Generic;

namespace Amaoto.GUI
{
    /// <summary>
    /// GUI部品。
    /// </summary>
    public abstract class DrawPart
    {
        /// <summary>
        /// GUI部品を初期化する。
        /// </summary>
        /// <param name="width">横幅。</param>
        /// <param name="height">縦幅。</param>
        public DrawPart(int width, int height)
        {
            Width = width;
            Height = height;

            Screen = new VirtualScreen(Width, Height);

            LongClickCounter = new Counter(0, 999, 1000, false);

            Child = new List<DrawPart>();
        }

        /// <summary>
        /// GUI部品を更新する。
        /// </summary>
        /// <param name="mouse">マウス。</param>
        /// <param name="pointX">マウスの相対X座標。</param>
        /// <param name="pointY">マウスの相対Y座標。</param>
        public virtual void Update(Mouse mouse, int? pointX = null, int? pointY = null)
        {
            if (!pointX.HasValue || !pointY.HasValue)
            {
                MousePoint.x = mouse.Point.x - X;
                MousePoint.y = mouse.Point.y - Y;
            }
            else
            {
                MousePoint.x = pointX.Value - X;
                MousePoint.y = pointY.Value - Y;
            }
            
            if (mouse.IsPushedButton(MouseButton.Left))
            {
                // マウス初回クリック処理
                if (!(MousePoint.x < 0 || MousePoint.y < 0 || MousePoint.x > Width || MousePoint.y > Height))
                {
                    LeftJudge = (true, MousePoint);
                    LongClickCounter?.Start();
                    OnMouseDown?.Invoke(this, null);
                }
            }
            else if (mouse.IsPushingButton(MouseButton.Left))
            {
                // マウスが要素内をクリックしてるかどうかの判定
                if (LeftJudge.Item1)
                {
                    if (MousePoint.x < 0 || MousePoint.y < 0 || MousePoint.x > Width || MousePoint.y > Height)
                    {
                        LeftJudge = (false, MousePoint);
                        OnMouseUp?.Invoke(this, null);
                        LongClickCounter.Stop();
                        LongClickCounter.Reset();
                    }
                    else
                    {
                        LongClickCounter?.Tick();
                        if (LongClickCounter.State == TimerState.Stopped)
                        {
                            // ロングタップ
                            LeftJudge = (false, MousePoint);
                            OnMouseUp?.Invoke(this, null);
                            LongClicked?.Invoke(this, null);
                            LongClickCounter.Stop();
                            LongClickCounter.Reset();
                        }
                    }
                }
            }
            else if (mouse.IsLeftButton(MouseButton.Left))
            {
                // クリック判定
                if (LeftJudge.Item1)
                {
                    Clicked?.Invoke(this, null);
                    OnMouseUp?.Invoke(this, null);
                }
                LongClickCounter.Stop();
                LongClickCounter.Reset();
            }

            foreach (var item in Child)
            {
                item.Update(mouse, MousePoint.x, MousePoint.y);
            }
        }

        /// <summary>
        /// GUI部品を描画する。
        /// </summary>
        public virtual void Draw()
        {
            Screen.ClearScreen();
            
            Screen.Draw(Texture, 0, 0);

            foreach (var item in Child)
            {
                item.Draw();
                Screen.Draw(item.Screen.Texture, item.X, item.Y);
            }
        }

        /// <summary>
        /// X座標。
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y座標。
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// 横幅。
        /// </summary>
        public int Width { get; protected set; }

        /// <summary>
        /// 縦幅。
        /// </summary>
        public int Height { get; protected set; }

        /// <summary>
        /// 描画するテクスチャ。
        /// </summary>
        public Texture Texture { get; protected set; }

        /// <summary>
        /// 仮想スクリーン
        /// </summary>
        public VirtualScreen Screen { get; protected set; }

        /// <summary>
        /// 子アイテム。
        /// </summary>
        public List<DrawPart> Child { get; protected set; }
        
        /// <summary>
        /// 要素がクリックされた。
        /// </summary>
        public event EventHandler Clicked;

        /// <summary>
        /// 要素がロングクリックされた。
        /// </summary>
        public event EventHandler LongClicked;

        /// <summary>
        /// 要素が押下された。
        /// </summary>
        public event EventHandler OnMouseDown;

        /// <summary>
        /// 要素の押下が終わった。
        /// </summary>
        public event EventHandler OnMouseUp;

        /// <summary>
        /// 相対座標。
        /// </summary>
        private (int x, int y) MousePoint;

        private (bool, (int x, int y)) LeftJudge;

        protected readonly Counter LongClickCounter;
    }
}
