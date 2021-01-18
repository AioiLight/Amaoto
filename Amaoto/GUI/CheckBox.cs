using System;

namespace Amaoto.GUI
{
    public class CheckBox : DrawPart
    {
        public CheckBox(int width, int height, ITextureReturnable boxTexture, ITextureReturnable checkTexture, ITextureReturnable description, bool check)
            : base(width, height)
        {
            //var textTex = description.GetTexture();
            //// 土台の描画。
            //var boxTex = boxTexture.GetTexture();
            //var boxTextureSize = boxTex.TextureSize;

            //var screen = new VirtualScreen(Width, Height);

            //Checked = check;

            //// 文字の描画
            //textTex.ReferencePoint = ReferencePoint.CenterLeft;
            //screen.Draw(() => textTex.Draw(boxTextureSize.Width, height / 2));

            //Texture = screen.Texture;
            //Texture.ReferencePoint = ReferencePoint.CenterLeft;
            //CheckTex = checkTexture.GetTexture();
            //CheckTex.ReferencePoint = ReferencePoint.CenterLeft;

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
                var c = CheckTex.GetTexture();
                c.ReferencePoint = ReferencePoint.CenterRight;
                c.Draw(size.Width, size.Height / 2);
            });

            Texture = VirtualScreen.GetTexture();

            base.Draw();
        }

        private bool Checked;
        private readonly ITextureReturnable CheckTex;
        private readonly ITextureReturnable Description;
        private readonly ITextureReturnable BoxTex;
        private readonly VirtualScreen VirtualScreen;
    }
}