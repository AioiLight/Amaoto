using Amaoto.Animation;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Amaoto.GUI
{
    /// <summary>
    /// スクロールを提供する。
    /// </summary>
    public class Scroller : DrawPart
    {
        /// <summary>
        /// スクロールを提供する。
        /// </summary>
        /// <param name="width">表示可能な横幅。。</param>
        /// <param name="height">表示可能な縦幅。</param>
        /// <param name="sensitivity">マウスホイールの感度。</param>
        /// <param name="friction">スクロール摩擦度。</param>
        /// <param name="bounceDuration">終端のバウンスのスクロール時間。</param>
        /// <param name="wheelscrollDuration">ホイールのスクロール時間。</param>
        public Scroller(int width, int height, double sensitivity = 60, double friction = 1.025, int bounceDuration = 600 * 1000, int wheelscrollDuration = 200 * 1000)
            : base(width, height)
        {
            var scrollLine = SystemInformation.MouseWheelScrollLines;
            Sensitivity = sensitivity * (scrollLine != -1 ? scrollLine : 3);
            Friction = friction;

            FrictionCounter = new Counter(0, 999, 1, true);
            FrictionCounter.Looped += FrictionCounter_Looped;
            FrictionCounter.Start();

            Scrolled += Scroller_Scrolled;

            BounceDuration = bounceDuration;
            WheelScrollDuration = wheelscrollDuration;
        }

        /// <summary>
        /// スクロールを更新する。
        /// </summary>
        /// <param name="canHandle"></param>
        /// <param name="pointX"></param>
        /// <param name="pointY"></param>
        public override void Update(bool canHandle, int? pointX = null, int? pointY = null)
        {
            // 子GUIの更新。
            //base.Update(canHandle, X, Y);

            if (!pointX.HasValue || !pointY.HasValue)
            {
                MousePoint = (Mouse.X - X, Mouse.Y - Y);
            }
            else
            {
                MousePoint = (pointX.Value - X, pointY.Value - Y);
            }

            foreach (var item in Child)
            {
                item.Update(canHandle
                    && !IsOutSide() && new Rectangle(item.X, item.Y, item.Width, item.Height).Contains(MousePoint.x, MousePoint.y),
                    MousePoint.x - (int)Position.x,
                    MousePoint.y - (int)Position.y);
            }

            // 最大の子アイテムの座標
            var maxGUIX = Child.Max(gui => gui.X + gui.Width);
            var maxGUIY = Child.Max(gui => gui.Y + gui.Height);

            // 水平・垂直スクロールが可能であるかチェック
            var canScrollH = maxGUIX > Width;
            var canScrollV = maxGUIY > Height;

            if (canHandle && Enabled)
            {
                NowMousePos = (Mouse.X, Mouse.Y);
                var vx = canScrollH ? NowMousePos.x - OldMousePos.x : 0;
                var vy = canScrollV ? NowMousePos.y - OldMousePos.y : 0;


                if (ClickedMousePos.HasValue
                    || !IsOutSide())
                {
                    if (Mouse.Wheel != 0 && canScrollV)
                    {
                        // 垂直方向に回転した
                        var wheel = Mouse.Wheel;

                        if (ScrollToAnimator.y != null)
                        {
                            ScrollTo((Position.x, ScrollToAnimator.y.EndPoint + wheel * Sensitivity), WheelScrollDuration);
                        }
                        else
                        {
                            ScrollTo((Position.x, Position.y + wheel * Sensitivity), WheelScrollDuration);
                        }
                    }
                    else if (Mouse.IsPushed(MouseButton.Left))
                    {
                        ClickedMousePos = (Mouse.X, Mouse.Y);
                    }
                    else if (Mouse.IsPushing(MouseButton.Left))
                    {
                        Position = (Position.x + vx, Position.y + vy);
                        DuringScrollTo = false;

                        if (ClickedMousePos.HasValue)
                        {
                            if (Math.Abs(ClickedMousePos.Value.x - Mouse.X) > 20 || Math.Abs(ClickedMousePos.Value.y - Mouse.Y) > 20)
                            {
                                foreach (var item in Child)
                                {
                                    item.StartDragging();
                                }
                            }
                        }
                    }
                    else if (Mouse.IsLeft(MouseButton.Left))
                    {
                        Velocity = (vx / 2, vy / 2);
                        ClickedMousePos = null;
                    }
                }

                if (Mouse.IsPushing(MouseButton.Left) || DuringScrollTo)
                {
                    Scrolling?.Invoke(this, null);
                }

                if (!Mouse.IsPushing(MouseButton.Left) && !DuringScrollTo)
                {
                    Scrolled?.Invoke(this, null);
                }
            }

            if (DuringScrollTo)
            {
                if (ScrollToAnimator.x != null)
                {
                    ScrollToAnimator.x.Tick();
                    Position = (ScrollToAnimator.x.GetAnimation(), Position.y);
                }

                if (ScrollToAnimator.y != null)
                {
                    ScrollToAnimator.y.Tick();
                    Position = (Position.x, ScrollToAnimator.y.GetAnimation());
                }
                Scrolling?.Invoke(this, null);
            }

            OldMousePos = NowMousePos;
            FrictionCounter.Tick();
        }

        /// <summary>
        /// 表示可能領域に描画する。
        /// </summary>
        public override void Draw()
        {
            Screen.ClearScreen();

            Screen.Draw(() =>
            {
                Texture?.Draw(0, 0);

                foreach (var item in Child)
                {
                    item.Draw();
                    item.Screen.GetTexture().Draw(Position.x + item.X, Position.y + item.Y);
                }
            });
        }

        /// <summary>
        /// 指定位置にスクロールするアニメーションを開始する。
        /// </summary>
        /// <param name="pos">位置。</param>
        public virtual void ScrollTo((double x, double y) pos, int timeUs)
        {
            DuringScrollTo = true;
            ScrollToAnimator.x = new EaseOut((int)Position.x, (int)pos.x, timeUs);
            ScrollToAnimator.x.Counter.Ended += Counter_Ended;
            ScrollToAnimator.x.Start();

            ScrollToAnimator.y = new EaseOut((int)Position.y, (int)pos.y, timeUs);
            ScrollToAnimator.y.Counter.Ended += Counter_Ended;
            ScrollToAnimator.y.Start();
        }

        /// <summary>
        /// スクロール位置の計算をする。
        /// </summary>
        protected virtual void CalcPosition()
        {
            var px = Position.x + Velocity.x;
            var vx = Velocity.x / Friction;
            var py = Position.y + Velocity.y;
            var vy = Velocity.y / Friction;

            Position = (px, py);
            Velocity = (vx, vy);
        }

        /// <summary>
        /// 摩擦カウンターが更新されループした。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void FrictionCounter_Looped(object sender, EventArgs e)
        {
            CalcPosition();
        }

        /// <summary>
        /// ScrollToカウンターが終了した。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Counter_Ended(object sender, EventArgs e)
        {
            DuringScrollTo = false;
        }

        /// <summary>
        /// スクロールが終わった。ここでは、バウンスの処理を行っています。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Scroller_Scrolled(object sender, EventArgs e)
        {
            if (DuringScrollTo)
            {
                return;
            }

            var x = Position.x;
            var y = Position.y;

            // 最大の子アイテムの座標
            var mX = Child.Max(gui => gui.X + gui.Width);
            var maxGUIX = Child.Last(c => c.X + c.Width == mX);
            var mY = Child.Max(gui => gui.Y + gui.Height);
            var maxGUIY = Child.Last(c => c.Y + c.Height == mY);

            // 水平・垂直スクロールが可能であるかチェック
            var canScrollH = mX > Width;
            var canScrollV = mY > Height;

            // 左、上のバウンス
            if (Position.x > 0 || Position.y > 0)
            {
                if (Position.x > 0 && Position.y > 0 && canScrollH && canScrollV)
                {
                    x = 0;
                    y = 0;
                }
                else if (Position.x > 0 && canScrollH)
                {
                    x = 0;
                }
                else if (Position.y > 0 && canScrollV)
                {
                    y = 0;
                }
            }

            // 右、下のバウンス
            if (Child.Count > 0)
            {
                if (Position.x < Width - mX && Position.y < Height - mY && canScrollH && canScrollV)
                {
                    x = Width - mX;
                    y = Height - mY;
                }
                else if (Position.x < Width - mX && canScrollH)
                {
                    x = Width - mX;
                }
                else if (Position.y < Height - mY && canScrollV)
                {
                    y = Height - mY;
                }
            }
            

            if ((x, y) != Position)
            {
                ScrollTo((x, y), BounceDuration);
            }
        }

        // プロパティ

        /// <summary>
        /// スクロール速度。
        /// </summary>
        public (double x, double y) Velocity { get; private set; }

        /// <summary>
        /// 現在のスクロール位置。
        /// </summary>
        public (double x, double y) Position { get; internal set; }

        /// <summary>
        /// ホイール回転時のスクロール感度。
        /// </summary>
        public double Sensitivity { get; private set; }

        /// <summary>
        /// スクロール摩擦。
        /// </summary>
        public double Friction { get; private set; }

        /// <summary>
        /// 前フレームのマウスの位置。
        /// </summary>
        public (int x, int y) OldMousePos { get; private set; }

        /// <summary>
        /// クリックしたときのマウス座標。
        /// </summary>
        public (int x, int y)? ClickedMousePos { get; private set; }

        /// <summary>
        /// 現在フレームのマウスの位置。
        /// </summary>
        public (int x, int y) NowMousePos { get; private set; }

        /// <summary>
        /// ScrollTo中かどうか。
        /// </summary>
        public bool DuringScrollTo { get; private set; }

        private readonly Counter FrictionCounter;

        private readonly int BounceDuration;

        private readonly int WheelScrollDuration;

        private (EaseOut x, EaseOut y) ScrollToAnimator;

        /// <summary>
        /// スクロールした。
        /// </summary>
        public event EventHandler Scrolled;

        /// <summary>
        /// スクロールしている。
        /// </summary>
        public event EventHandler Scrolling;
    }
}
