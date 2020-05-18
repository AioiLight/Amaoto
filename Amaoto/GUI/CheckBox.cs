using System;

namespace Amaoto.GUI
{
    public class CheckBox : DrawPart
    {
        public CheckBox(int width, int height, Texture boxTexture, Texture checkTexture, bool check, FontRender fontRender, string str)
            : base(width, height)
        {
            var textTex = fontRender.GetTexture(str);
            // 土台の描画。
            var boxTextureSize = boxTexture.TextureSize;
            
            var screen = new VirtualScreen(Width, Height);

            Checked = check;

            // 文字の描画
            textTex.ReferencePoint = ReferencePoint.CenterLeft;
            screen.Draw(textTex, boxTextureSize.width, height / 2);

            Texture = screen.Texture;
            Texture.ReferencePoint = ReferencePoint.CenterLeft;
            CheckTex = checkTexture;
            CheckTex.ReferencePoint = ReferencePoint.CenterLeft;

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
            Screen.ClearScreen();

            Screen.Draw(Texture, 0, Height / 2);

            if (Checked)
            {
                Screen.Draw(CheckTex, 0, Height / 2);
            }

            foreach (var item in Child)
            {
                item.Draw();
                Screen.Draw(item.Screen.Texture, item.X, item.Y);
            }
        }

        private bool Checked;
        private Texture CheckTex;
    }
}
