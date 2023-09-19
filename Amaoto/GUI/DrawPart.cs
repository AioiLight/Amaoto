using System;
using System.Collections.Generic;
using System.Drawing;

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

            LongClickCounter = new Counter(0, Amaoto.LongClickMs - 1, 1000, false);
            LongClickCounter.Ended += LongClickCounter_Ended;

            Child = new List<DrawPart>();

            Enabled = true;

            Screen = new VirtualScreen(Width, Height);
            ShouldBuild = true;
        }

        /// <summary>
        /// GUI部品を更新する。
        /// 更新の優先順位は子アイテムが先。
        /// イベントにひとつでもデリゲートが追加されていれば、自動的にHandleMouseが呼び出される。
        /// 子アイテムでHandleMouseが呼び出されたときは、他の子アイテム及び親アイテムのクリック判定は行われない。
        /// </summary>
        /// <param name="canHandle">クリック判定を行うかどうか。これにかかわらず、MouseHandledがtrueであれば常にクリック判定を行いません。</param>
        /// <param name="pointX">マウスの相対X座標。</param>
        /// <param name="pointY">マウスの相対Y座標。</param>
        public virtual void Update(bool canHandle, int? pointX = null, int? pointY = null)
        {
            if (ShouldBuild)
            {
                Build();
            }

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
                item.Update(canHandle, MousePoint.x, MousePoint.y);
            }

            if (!canHandle || Amaoto.MouseHandled || !Enabled)
            {
                LeftJudge = (false, (0, 0));

                LongClickCounter.Stop();
                LongClickCounter.Reset();

                return;
            }

            var outSide = IsOutSide();

            if (!outSide)
            {
                InvokeOnHovering(new MouseClickEventArgs(MousePoint.x, MousePoint.y));
                if (!Hovering)
                {
                    InvokeOnMouseEnter(new MouseClickEventArgs(MousePoint.x, MousePoint.y));
                    Hovering = true;
                }

                if (Mouse.IsPushing(MouseButton.Left) && HasDelegate)
                {
                    Amaoto.HandleMouse();
                }
            }
            else
            {
                if (Hovering)
                {                    
                    InvokeOnMouseLeave(new MouseClickEventArgs(MousePoint.x, MousePoint.y));
                    Hovering = false;
                }
            }

            if (Mouse.IsPushed(MouseButton.Left))
            {
                // マウス初回クリック処理
                if (!outSide)
                {
                    LeftJudge = (true, MousePoint);
                    LongClickCounter?.Start();
                    InvokeOnMouseDown(new MouseClickEventArgs(MousePoint.x, MousePoint.y));
                    Dragging = false;
                }
                else
                {
                    LeftJudge = (false, MousePoint);
                }
            }
            else if (Mouse.IsPushing(MouseButton.Left))
            {
                // マウスが要素内をクリックしてるかどうかの判定
                if (LeftJudge.Item1)
                {
                    if (outSide)
                    {
                        LeftJudge = (false, MousePoint);
                        InvokeOnMouseUp(new MouseClickEventArgs(MousePoint.x, MousePoint.y));
                        LongClickCounter.Stop();
                        LongClickCounter.Reset();
                    }
                    else
                    {
                        LongClickCounter?.Tick();
                    }
                }
            }
            else if (Mouse.IsLeft(MouseButton.Left))
            {
                // クリック判定
                if (LeftJudge.Item1)
                {
                    if (!Dragging)
                    {
                        InvokeClicked(new MouseClickEventArgs(MousePoint.x, MousePoint.y));
                    }
                    InvokeOnMouseUp(new MouseClickEventArgs(MousePoint.x, MousePoint.y));
                    Dragging = false;
                }
                LongClickCounter.Stop();
                LongClickCounter.Reset();

            }
        }

        /// <summary>
        /// GUI部品を描画する。
        /// </summary>
        public virtual void Draw()
        {
            Screen.ClearScreen();

            Screen.Draw(() =>
            {
                Texture?.Draw(0, 0);

                foreach (var item in Child)
                {
                    item.Draw();
                    item.Screen.GetTexture().Draw(item.X, item.Y);   
                }
            });
        }

        /// <summary>
        /// 現在のプロパティで、GUI部品を再生成する。
        /// 子アイテムも再生成される。
        /// </summary>
        public virtual void Build()
        {
            // 現在の横幅、縦幅から、仮想スクリーンを再生成。
            Screen = new VirtualScreen(Width, Height);

            if (Child != null)
            {
                foreach (var item in Child)
                {
                    item.Build();
                }
            }

            OnBuilt?.Invoke(this, null);
            ShouldBuild = false;
        }

        /// <summary>
        /// ドラッグされたことを通知して、クリックイベントを発生させないようにする。
        /// </summary>
        public void StartDragging()
        {
            foreach (var item in Child)
            {
                item.StartDragging();
            }
            Dragging = true;
        }

        /// <summary>
        /// GUIの範囲を取得する。
        /// </summary>
        /// <returns>GUIの範囲。</returns>
        public Rectangle GetRectangle()
        {
            return new Rectangle(0, 0, Width, Height);
        }

        /// <summary>
        /// GUI部品の中にマウスがあるかどうか。
        /// </summary>
        /// <returns>マウスがあるかどうか。</returns>
        protected bool IsOutSide()
        {
            return !GetRectangle().Contains(MousePoint.x, MousePoint.y);
        }

        /// <summary>
        /// クリックイベントを発火させる。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        public void InvokeClicked(MouseClickEventArgs e)
        {
            Clicked?.Invoke(this, e);
        }

        /// <summary>
        /// ロングクリックイベントを発火させる。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected void InvokeLongClicked(MouseClickEventArgs e)
        {
            LongClicked?.Invoke(this, e);
        }

        /// <summary>
        /// マウスダウンイベントを発火させる。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected void InvokeOnMouseDown(MouseClickEventArgs e)
        {
            OnMouseDown?.Invoke(this, e);
        }

        /// <summary>
        /// マウスアップイベントを発火させる。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected void InvokeOnMouseUp(MouseClickEventArgs e)
        {
            OnMouseUp?.Invoke(this, e);
        }

        /// <summary>
        /// マウスホバリングイベントを発火させる。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected void InvokeOnHovering(MouseClickEventArgs e)
        {
            OnHovering?.Invoke(this, e);
        }

        /// <summary>
        /// マウスエンターイベントを発火させる。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected void InvokeOnMouseEnter(MouseClickEventArgs e)
        {
            OnMouseEnter?.Invoke(this, e);
        }

        /// <summary>
        /// マウスリーブイベントを発火させる。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected void InvokeOnMouseLeave(MouseClickEventArgs e)
        {
            OnMouseLeave?.Invoke(this, e);
        }

        private void LongClickCounter_Ended(object sender, EventArgs e)
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
        /// 相対座標。
        /// </summary>
        public (int x, int y) MousePoint { get; protected set; }

        /// <summary>
        /// ビルドするべきか。
        /// </summary>
        public bool ShouldBuild { get; protected set; }

        /// <summary>
        /// イベントのどれかにデリゲートが紐付けされている。
        /// </summary>
        public bool HasDelegate
        {
            get
            {
                int GetDelegateLength(MulticastDelegate d)
                {
                    return d == null || d.GetInvocationList() == null ? 0 : d.GetInvocationList().Length;
                }

                return GetDelegateLength(Clicked) > 0
                    || GetDelegateLength(LongClicked) > 0
                    || GetDelegateLength(OnMouseDown) > 0
                    || GetDelegateLength(OnMouseUp) > 0
                    || GetDelegateLength(OnHovering) > 0
                    || GetDelegateLength(OnMouseEnter) > 0
                    || GetDelegateLength(OnMouseLeave) > 0;
            }
        }

        /// <summary>
        /// GUI 部品が有効かどうか。
        /// </summary>
        public bool Enabled
        {
            get
            {
                return _Enabled;
            }
            set
            {
                if (value != _Enabled)
                {
                    if (value)
                    {
                        OnEnabled?.Invoke(this, null);
                    }
                    else
                    {
                        OnDisabled?.Invoke(this, null);
                    }
                    _Enabled = value;
                }
            }
        }

        /// <summary>
        /// ロングクリックを検知するためのカウンター。
        /// </summary>
        protected readonly Counter LongClickCounter;

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
        /// 有効になった。
        /// </summary>
        public event EventHandler OnEnabled;

        /// <summary>
        /// 無効になった。
        /// </summary>
        public event EventHandler OnDisabled;

        /// <summary>
        /// Build();が呼び出された。
        /// </summary>
        public event EventHandler OnBuilt;

        protected (bool, (int x, int y)) LeftJudge;

        protected bool Hovering;

        protected bool Dragging;

        protected bool _Enabled;

    }
}