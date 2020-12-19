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

            LongClickCounter = new Counter(0, Amaoto.LongClickMs - 1, 1000, false);

            Child = new List<DrawPart>();
        }

        /// <summary>
        /// GUI部品を更新する。
        /// </summary>
        /// <param name="mouse">マウス。</param>
        /// <param name="pointX">マウスの相対X座標。</param>
        /// <param name="pointY">マウスの相対Y座標。</param>
        public virtual void Update(Mouse mouse = null, int? pointX = null, int? pointY = null)
        {
            if (mouse == null)
            {
                LeftJudge = (false, (0, 0));

                LongClickCounter.Stop();
                LongClickCounter.Reset();

                Dragging = false;
                return;
            }

            if (!pointX.HasValue || !pointY.HasValue)
            {
                MousePoint = (mouse.Point.x - X, mouse.Point.y - Y);
            }
            else
            {
                MousePoint = (pointX.Value - X, pointY.Value - Y);
            }

            var outSide = IsOutSide();

            if (!outSide)
            {
                OnHovering?.Invoke(this, new MouseClickEventArgs(MousePoint.x, MousePoint.y));
                if (!Hovering)
                {
                    OnMouseEnter?.Invoke(this, new MouseClickEventArgs(MousePoint.x, MousePoint.y));
                    Hovering = true;
                }
            }
            else
            {
                if (Hovering)
                {
                    OnMouseLeave?.Invoke(this, new MouseClickEventArgs(MousePoint.x, MousePoint.y));
                    Hovering = false;
                }
            }

            if (mouse.IsPushedButton(MouseButton.Left))
            {
                // マウス初回クリック処理
                if (!outSide)
                {
                    LeftJudge = (true, MousePoint);
                    LongClickCounter?.Start();
                    OnMouseDown?.Invoke(this, new MouseClickEventArgs(MousePoint.x, MousePoint.y));
                }
            }
            else if (mouse.IsPushingButton(MouseButton.Left))
            {
                // マウスが要素内をクリックしてるかどうかの判定
                if (LeftJudge.Item1)
                {
                    if (outSide)
                    {
                        LeftJudge = (false, MousePoint);
                        OnMouseUp?.Invoke(this, new MouseClickEventArgs(MousePoint.x, MousePoint.y));
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
                            OnMouseUp?.Invoke(this, new MouseClickEventArgs(MousePoint.x, MousePoint.y));
                            if (!Dragging)
                            {
                                LongClicked?.Invoke(this, new MouseClickEventArgs(MousePoint.x, MousePoint.y));
                            }
                            LongClickCounter.Stop();
                            LongClickCounter.Reset();

                            Dragging = false;
                        }
                    }
                }
            }
            else if (mouse.IsLeftButton(MouseButton.Left))
            {
                // クリック判定
                if (LeftJudge.Item1)
                {
                    if (!Dragging)
                    {
                        Clicked?.Invoke(this, new MouseClickEventArgs(MousePoint.x, MousePoint.y));
                    }
                    OnMouseUp?.Invoke(this, new MouseClickEventArgs(MousePoint.x, MousePoint.y));
                }
                LongClickCounter.Stop();
                LongClickCounter.Reset();

                Dragging = false;
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
        /// ドラッグされたことを通知して、クリックイベントを発生させないようにする。
        /// </summary>
        public void StartDragging()
        {
            Dragging = true;
        }

        private bool IsOutSide()
        {
            return MousePoint.x < 0 || MousePoint.y < 0 || MousePoint.x > Width || MousePoint.y > Height;
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
        public event EventHandler<MouseClickEventArgs> Clicked;

        /// <summary>
        /// 要素がロングクリックされた。
        /// </summary>
        public event EventHandler<MouseClickEventArgs> LongClicked;

        /// <summary>
        /// 要素が押下された。
        /// </summary>
        public event EventHandler<MouseClickEventArgs> OnMouseDown;

        /// <summary>
        /// 要素の押下が終わった。
        /// </summary>
        public event EventHandler<MouseClickEventArgs> OnMouseUp;

        /// <summary>
        /// マウスで要素をホバリングしている。
        /// </summary>
        public event EventHandler<MouseClickEventArgs> OnHovering;

        /// <summary>
        /// マウスが要素内に入ってきた。
        /// </summary>
        public event EventHandler<MouseClickEventArgs> OnMouseEnter;

        /// <summary>
        /// マウスが要素内から出て行った。
        /// </summary>
        public event EventHandler<MouseClickEventArgs> OnMouseLeave;

        /// <summary>
        /// 相対座標。
        /// </summary>
        public (int x, int y) MousePoint { get; private set; }

        private (bool, (int x, int y)) LeftJudge;

        private bool Hovering;

        private bool Dragging;

        /// <summary>
        /// ロングクリックを検知するためのカウンター。
        /// </summary>
        protected readonly Counter LongClickCounter;
    }
}