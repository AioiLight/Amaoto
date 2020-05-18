using System;
using System.Drawing;

namespace Amaoto.GUI
{
    public class Button : DrawPart
    {
        public Button(int width, int height, Texture buttonTexture, FontRender fontRender, string str)
            : base(width, height)
        {
            OnMouseDown += Button_OnMouseDown;
            OnMouseUp += Button_OnMouseUp;

            var textTex = fontRender.GetTexture(str);
            // ボタンの土台の描画。
            var buttonTextureSize = buttonTexture.TextureSize;
            var oneSize = (buttonTextureSize.width / 3, buttonTextureSize.height / 3);

            var screen = new VirtualScreen(Width, Height);

            // 左上
            buttonTexture.ReferencePoint = ReferencePoint.TopLeft;
            screen.Draw(buttonTexture, 0, 0, new Rectangle(0, 0, oneSize.Item1, oneSize.Item2));

            // 中央上
            buttonTexture.ReferencePoint = ReferencePoint.TopCenter;
            buttonTexture.ScaleX = (Width - oneSize.Item1 * 2) / oneSize.Item1;
            screen.Draw(buttonTexture, Width / 2, 0, new Rectangle(oneSize.Item1, 0, oneSize.Item1, oneSize.Item2));

            // 右上
            buttonTexture.ReferencePoint = ReferencePoint.TopRight;
            buttonTexture.ScaleX = 1.0f;
            screen.Draw(buttonTexture, Width, 0, new Rectangle(oneSize.Item1 * 2, 0, oneSize.Item1, oneSize.Item2));

            // 左中央
            buttonTexture.ReferencePoint = ReferencePoint.CenterLeft;
            buttonTexture.ScaleY = (Height - oneSize.Item2 * 2) / oneSize.Item2;
            screen.Draw(buttonTexture, 0, Height / 2, new Rectangle(0, oneSize.Item2, oneSize.Item1, oneSize.Item2));

            // 中央
            buttonTexture.ReferencePoint = ReferencePoint.Center;
            buttonTexture.ScaleX = (Width - oneSize.Item1 * 2) / oneSize.Item1;
            screen.Draw(buttonTexture, Width / 2, Height / 2, new Rectangle(oneSize.Item1, oneSize.Item2, oneSize.Item1, oneSize.Item2));

            // 右中央
            buttonTexture.ReferencePoint = ReferencePoint.CenterRight;
            buttonTexture.ScaleX = 1.0f;
            screen.Draw(buttonTexture, Width, Height / 2, new Rectangle(oneSize.Item1 * 2, oneSize.Item2, oneSize.Item1, oneSize.Item2));

            // 左下
            buttonTexture.ReferencePoint = ReferencePoint.BottomLeft;
            buttonTexture.ScaleY = 1.0f;
            screen.Draw(buttonTexture, 0, Height, new Rectangle(0, oneSize.Item2 * 2, oneSize.Item1, oneSize.Item2));

            // 中央下
            buttonTexture.ReferencePoint = ReferencePoint.BottomCenter;
            buttonTexture.ScaleX = (Width - oneSize.Item1 * 2) / oneSize.Item1;
            screen.Draw(buttonTexture, Width / 2, Height, new Rectangle(oneSize.Item1, oneSize.Item2 * 2, oneSize.Item1, oneSize.Item2));

            // 右下
            buttonTexture.ReferencePoint = ReferencePoint.BottomRight;
            buttonTexture.ScaleX = 1.0f;
            screen.Draw(buttonTexture, Width, Height, new Rectangle(oneSize.Item1 * 2, oneSize.Item2 * 2, oneSize.Item1, oneSize.Item2));

            // 文字の描画
            textTex.ReferencePoint = ReferencePoint.Center;
            screen.Draw(textTex, width / 2, height / 2);

            Texture = screen.Texture;

            DownAnimation = new Animation.EaseOut(100, 95, 1000 * 250);
            UpAnimation = new Animation.EaseOut(95, 100, 1000 * 250);

            Texture.ReferencePoint = ReferencePoint.Center;

        }

        public override void Update(Mouse mouse, int? pointX = null, int? pointY = null)
        {
            DownAnimation?.Tick();
            UpAnimation?.Tick();

            if (DownAnimation.Counter.State == TimerState.Started)
            {
                Texture.ScaleX = Texture.ScaleY = (float)(DownAnimation.GetAnimation() / 100);
            }
            else if (UpAnimation.Counter.State == TimerState.Started)
            {
                Texture.ScaleX = Texture.ScaleY = (float)(UpAnimation.GetAnimation() / 100);
            }
            else
            {
                if (LongClickCounter.State == TimerState.Started)
                {
                    Texture.ScaleX = Texture.ScaleY = 0.95f;
                }
                else
                {
                    Texture.ScaleX = Texture.ScaleY = 1.0f;
                }
            }
            base.Update(mouse, pointX, pointY);
        }

        /// <summary>
        /// GUI部品を描画する。
        /// </summary>
        public override void Draw()
        {
            Screen.ClearScreen();

            Screen.Draw(Texture, Width / 2, Height / 2);

            foreach (var item in Child)
            {
                item.Draw();
                Screen.Draw(item.Screen.Texture, item.X, item.Y);
            }
        }

        private void Button_OnMouseDown(object sender, EventArgs e)
        {
            UpAnimation.Stop();
            UpAnimation.Reset();
            DownAnimation.Start();
        }

        private void Button_OnMouseUp(object sender, EventArgs e)
        {
            DownAnimation.Stop();
            DownAnimation.Reset();
            UpAnimation.Start();
        }

        private readonly Animation.EaseOut DownAnimation;
        private readonly Animation.EaseOut UpAnimation;
    }
}
