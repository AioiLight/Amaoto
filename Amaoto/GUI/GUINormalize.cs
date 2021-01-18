namespace Amaoto.GUI
{
    /// <summary>
    /// GUI部品の描画をテクスチャのように行うためのノーマライザ。
    /// </summary>
    public class GUINormalize : ITextureReturnable
    {
        /// <summary>
        /// GUI部品をテクスチャのように扱えるメソッドを提供する。
        /// </summary>
        /// <param name="gui">GUI 部品。</param>
        public GUINormalize(DrawPart gui)
        {
            GUI = gui;
            Screen = new VirtualScreen(GUI.Width, GUI.Height);
        }

        /// <summary>
        /// GUI部品を更新する。
        /// </summary>
        /// <param name="pointX"></param>
        /// <param name="pointY"></param>
        public void Process(bool canHandle, int? pointX = null, int? pointY = null)
        {
            GUI.Update(canHandle, pointX, pointY);
        }

        public Texture GetTexture()
        {
            GUI.Draw();
            Screen.ClearScreen();
            Screen.Draw(() =>
            {
                GUI.Texture.ReferencePoint = ReferencePoint.Center;
                GUI.Texture.Draw(GUI.Width / 2, GUI.Height / 2);
            });

            return Screen.GetTexture();
        }

        /// <summary>
        /// GUI 部品。
        /// </summary>
        public DrawPart GUI { get; private set; }

        private readonly VirtualScreen Screen;
    }
}
