using System;

namespace Amaoto.GUI
{
    public class CheckBox : DrawPart
    {
        public CheckBox(ITextureReturnable boxTexture, ITextureReturnable checkTexture, ITextureReturnable description, bool check, int width, int height)
            : base(width, height)
        {
            VirtualScreen = new VirtualScreen(width, height);
            BoxTex = boxTexture;
            CheckTex = checkTexture;
            Description = description;

            Checked = check;

            Clicked += CheckBox_Clicked;
        }

        private void CheckBox_Clicked(object sender, EventArgs e)
        {
            Checked = !Checked;
            Switched?.Invoke(this, Checked);
        }

        /// <summary>
        /// GUI部品を描画する。
        /// </summary>
        public override void Draw()
        {
            VirtualScreen.ClearScreen();

            VirtualScreen.Draw(() =>
            {
                var size = VirtualScreen.ScreenSize;
                // 文字
                var d = Description.GetTexture();
                d.ReferencePoint = ReferencePoint.CenterLeft;
                d.Draw(0, size.Height / 2);

                // チェックボックス
                var b = BoxTex.GetTexture();
                b.ReferencePoint = ReferencePoint.CenterRight;
                b.Draw(size.Width, size.Height / 2);

                // チェック
                if (Checked)
                {
                    var c = CheckTex.GetTexture();
                    c.ReferencePoint = ReferencePoint.CenterRight;
                    c.Draw(size.Width, size.Height / 2);
                }
            });

            Texture = VirtualScreen.GetTexture();

            base.Draw();
        }

        public event EventHandler<bool> Switched;

        private bool Checked;
        private readonly ITextureReturnable CheckTex;
        private readonly ITextureReturnable Description;
        private readonly ITextureReturnable BoxTex;
        private readonly VirtualScreen VirtualScreen;
    }
}