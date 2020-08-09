namespace Amaoto.GUI
{
    /// <summary>
    /// ただ画像表示を行うだけの GUI 部品。
    /// </summary>
    public class Image : DrawPart
    {
        public Image(Texture texture)
            : base(texture.TextureSize.width, texture.TextureSize.height)
        {
            Texture = texture;
        }
    }
}
