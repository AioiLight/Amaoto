using System.Drawing;

namespace Amaoto.GUI
{
    /// <summary>
    /// 決まったサイズを持つコンテナを生成する。
    /// </summary>
    public class Container : DrawPart
    {
        /// <summary>
        /// 決まったサイズを持つコンテナを生成する。
        /// </summary>
        public Container(int width, int height, Color backgroundColor)
            : base(width, height)
        {
            BackgroundColor = backgroundColor;
            VirtualScreen = new VirtualScreen(Width, Height);
        }

        public override void Draw()
        {
            VirtualScreen.ClearScreen();
            VirtualScreen.Draw(() =>
            {
                var a = BackgroundColor.A;
                DXLibUtil.DrawWithOpacity(a, BlendMode.None, () =>
                {
                    DxLibDLL.DX.DrawFillBox(0, 0, Width, Height, DxLibDLL.DX.GetColor(BackgroundColor.R, BackgroundColor.G, BackgroundColor.B));
                });
            });

            Texture = VirtualScreen.GetTexture();

            base.Draw();
        }

        private Color BackgroundColor;
        private VirtualScreen VirtualScreen;
    }
}
