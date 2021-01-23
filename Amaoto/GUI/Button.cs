using System;
using System.Drawing;

namespace Amaoto.GUI
{
    /// <summary>
    /// ボタン。
    /// </summary>
    public class Button : DrawPart
    {
        /// <summary>
        /// GUIのボタンを描画する。
        /// </summary>
        /// <param name="background">ボタンの背景として使用する画像。正方形でなければならない。</param>
        /// <param name="content">ボタンの中身。</param>
        /// <param name="width">(オプション)横のサイズ。</param>
        /// <param name="height">(オプション)縦のサイズ。</param>
        public Button(ITextureReturnable background, ITextureReturnable content, int? width = null, int? height = null)
            : base(width ?? content.GetTexture().TextureSize.Width, height ?? content.GetTexture().TextureSize.Height)
        {
            OnMouseDown += Button_OnMouseDown;
            OnMouseUp += Button_OnMouseUp;

            var contentTex = content.GetTexture();
            var backTex = background.GetTexture();
            // ボタンの土台の描画。
            var buttonTextureSize = backTex.TextureSize;
            var oneSize = ((int)Math.Ceiling(1.0 * buttonTextureSize.Width / 3), (int)Math.Ceiling(1.0 * buttonTextureSize.Height / 3));

            var screen = new VirtualScreen(Width, Height);
            VirtualScreen = new VirtualScreen(Width, Height);

            screen.Draw(() =>
            {
                // 左上
                backTex.ReferencePoint = ReferencePoint.TopLeft;
                backTex.ScaleX = 1.0;
                backTex.ScaleX = 1.0;
                backTex.Draw(0, 0, new Rectangle(0, 0, oneSize.Item1, oneSize.Item2));

                // 中央上
                backTex.ReferencePoint = ReferencePoint.TopCenter;
                backTex.ScaleX = 1.0 * (Width - (oneSize.Item1 * 2)) / oneSize.Item1;
                backTex.Draw(Width / 2.0, 0, new Rectangle(oneSize.Item1, 0, oneSize.Item1, oneSize.Item2));

                // 右上
                backTex.ReferencePoint = ReferencePoint.TopRight;
                backTex.ScaleX = 1.0;
                backTex.Draw(Width, 0, new Rectangle(oneSize.Item1 * 2, 0, oneSize.Item1, oneSize.Item2));

                // 左中央
                backTex.ReferencePoint = ReferencePoint.CenterLeft;
                backTex.ScaleY = 1.0 * (Height - (oneSize.Item2 * 2)) / oneSize.Item2;
                backTex.Draw(0, Height / 2.0, new Rectangle(0, oneSize.Item2, oneSize.Item1, oneSize.Item2));

                // 中央
                backTex.ReferencePoint = ReferencePoint.Center;
                backTex.ScaleX = 1.0 * (Width - (oneSize.Item1 * 2)) / oneSize.Item1;
                backTex.Draw(Width / 2.0, Height / 2.0, new Rectangle(oneSize.Item1, oneSize.Item2, oneSize.Item1, oneSize.Item2));

                // 右中央
                backTex.ReferencePoint = ReferencePoint.CenterRight;
                backTex.ScaleX = 1.0;
                backTex.Draw(Width, Height / 2.0, new Rectangle(oneSize.Item1 * 2, oneSize.Item2, oneSize.Item1, oneSize.Item2));

                // 左下
                backTex.ReferencePoint = ReferencePoint.BottomLeft;
                backTex.ScaleY = 1.0;
                backTex.Draw(0, Height, new Rectangle(0, oneSize.Item2 * 2, oneSize.Item1, oneSize.Item2));

                // 中央下
                backTex.ReferencePoint = ReferencePoint.BottomCenter;
                backTex.ScaleX = 1.0 * (Width - (oneSize.Item1 * 2)) / oneSize.Item1;
                backTex.Draw(Width / 2.0, Height, new Rectangle(oneSize.Item1, oneSize.Item2 * 2, oneSize.Item1, oneSize.Item2));

                // 右下
                backTex.ReferencePoint = ReferencePoint.BottomRight;
                backTex.ScaleX = 1.0;
                backTex.Draw(Width, Height, new Rectangle(oneSize.Item1 * 2, oneSize.Item2 * 2, oneSize.Item1, oneSize.Item2));

                // 文字の描画
                contentTex.ReferencePoint = ReferencePoint.Center;
                contentTex.Draw(Width / 2.0, Height / 2.0);
            });

            Texture = screen.GetTexture();

            DownAnimation = new Animation.EaseOut(100, 95, 1000 * 250);
            UpAnimation = new Animation.EaseOut(95, 100, 1000 * 250);

            Texture.ReferencePoint = ReferencePoint.Center;
            VirtualScreen.GetTexture().ReferencePoint = ReferencePoint.Center;
        }

        public override void Update(bool canHandle, int? pointX = null, int? pointY = null)
        {
            DownAnimation?.Tick();
            UpAnimation?.Tick();

            base.Update(canHandle, pointX, pointY);

            var v = VirtualScreen.GetTexture();
            if (DownAnimation.Counter.State == TimerState.Started)
            {
                v.ScaleX = v.ScaleY = DownAnimation.GetAnimation() / 100;
            }
            else if (UpAnimation.Counter.State == TimerState.Started)
            {
                v.ScaleX = v.ScaleY = UpAnimation.GetAnimation() / 100;
            }
            else
            {
                if (LongClickCounter.State == TimerState.Started)
                {
                    v.ScaleX = v.ScaleY = 0.95f;
                }
                else
                {
                    v.ScaleX = v.ScaleY = 1.0;
                }
            }
        }

        /// <summary>
        /// GUI部品を描画する。
        /// </summary>
        public override void Draw()
        {
            VirtualScreen.ClearScreen();

            VirtualScreen.Draw(() =>
            {
                Texture.Draw(Width / 2.0, Height / 2.0);

                foreach (var item in Child)
                {
                    item.Draw();
                    item.Screen.GetTexture().Draw(item.X, item.Y);
                }
            });

            if (!Enabled)
            {
                VirtualScreen.GetTexture().Saturation(-255);
            }

            Screen.ClearScreen();

            Screen.Draw(() => VirtualScreen.GetTexture().Draw(Width / 2.0, Height / 2.0));
        }

        private void Button_OnMouseDown(object sender, EventArgs e)
        {
            UpAnimation.Stop();
            UpAnimation.Reset();
            DownAnimation.Reset();
            DownAnimation.Start();
        }

        private void Button_OnMouseUp(object sender, EventArgs e)
        {
            DownAnimation.Stop();
            DownAnimation.Reset();
            UpAnimation.Reset();
            UpAnimation.Start();
        }

        private readonly Animation.EaseOut DownAnimation;
        private readonly Animation.EaseOut UpAnimation;
        private readonly VirtualScreen VirtualScreen;
    }
}