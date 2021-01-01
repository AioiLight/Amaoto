namespace Amaoto.GUI
{
    /// <summary>
    /// ただ画像表示を行うだけの GUI 部品。
    /// </summary>
    public class Image : DrawPart
    {
        public Image(Texture texture, int? width = null, int? height = null)
            : base(width.HasValue ? width.Value : texture.TextureSize.width, height.HasValue ? height.Value : texture.TextureSize.height)
        {
            if (width.HasValue)
            {
                texture.ScaleX = (float)(1.0 * width.Value / texture.TextureSize.width);
            }

            if (height.HasValue)
            {
                texture.ScaleY = (float)(1.0 * height.Value / texture.TextureSize.height);
            }

            Texture = texture;
        }
    }
}
