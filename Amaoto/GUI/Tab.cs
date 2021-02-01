using System;
using System.Linq;

namespace Amaoto.GUI
{
    /// <summary>
    /// タブを提供する。
    /// </summary>
    public class Tab : DrawPart
    {
        /// <summary>
        /// タブを提供する。
        /// </summary>
        /// <param name="children">タブ要素。</param>
        /// <param name="tabBackground">タブの背景。</param>
        /// <param name="tabNames">タブ名。</param>
        /// <param name="tabWidth">タブの横幅。はみ出す場合は、スクロールできるようになる。</param>
        /// <param name="tabHeight">タブの縦幅。</param>
        /// <param name="width">横幅。</param>
        /// <param name="height">縦幅。タブの高さを含む。</param>
        public Tab(DrawPart[] children, ITextureReturnable tabBackground, ITextureReturnable[] tabNames, int tabWidth, int tabHeight, int width, int height)
            : base(width, height)
        {
            Child = children.ToList();

            TabNames = new DrawPart[children.Length];
            for (int i = 0; i < TabNames.Length; i++)
            {
                // ボタンを生成
                TabNames[i] = new Button(tabBackground, tabNames.Length > i ? tabNames[i] : null, tabWidth, tabHeight);
                TabNames[i].Clicked += (sender, e) =>
                {
                    var indx = TabNames.ToList().IndexOf((DrawPart)sender);
                    TabClicked?.Invoke(this, indx);
                    ShowingTabIndex = indx;
                };
            }

            ShowingTabChanged += Tab_ShowingTabChanged;

            TabHeader = new Row(TabNames, 0, 0);

            TabHeaderScroller = new Scroller(Width, tabHeight);
            TabHeaderScroller.Child.Add(TabHeader);

            // タブの高さ分ずらす。
            foreach (var item in Child)
            {
                item.Y += tabHeight;
            }

            ShowingTabIndex = 0;
        }

        public override void Update(bool canHandle, int? pointX = null, int? pointY = null)
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

            TabHeaderScroller.Update(canHandle, MousePoint.x, MousePoint.y);

            for (int i = 0; i < Child.Count; i++)
            {
                var handle = canHandle && ShowingTabIndex == i; 
                Child[i].Update(handle, MousePoint.x, MousePoint.y);
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

        public override void Draw()
        {
            Screen.ClearScreen();

            Screen.Draw(() =>
            {
                Texture?.Draw(0, 0);

                TabHeaderScroller.Draw();
                TabHeaderScroller.Screen.GetTexture().Draw(0, 0);

                var gui = Child[ShowingTabIndex];
                gui.Draw();
                gui.Screen.GetTexture().Draw(gui.X, gui.Y);
            });
        }

        private void Tab_ShowingTabChanged(object sender, int e)
        {
            for (int i = 0; i < TabNames.Length; i++)
            {
                // タブの押せるかどうかを変更
                TabNames[i].Enabled = e != i;
            }
        }

        /// <summary>
        /// タブのボタンがクリックされた。
        /// </summary>
        public event EventHandler<int> TabClicked;

        /// <summary>
        /// タブが切り替わった。
        /// </summary>
        public event EventHandler<int> ShowingTabChanged;

        /// <summary>
        /// 開いているタブ。
        /// </summary>
        public int ShowingTabIndex
        {
            get
            {
                return _ShowingTabIndex;
            }
            set
            {
                if (TabHeader.Child.Count > value)
                {
                    _ShowingTabIndex = value;
                    ShowingTabChanged?.Invoke(this, value);
                }
            }
        }

        private DrawPart[] TabNames;
        private Row TabHeader;
        private Scroller TabHeaderScroller;

        private int _ShowingTabIndex;
    }
}
