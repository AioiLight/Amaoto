namespace Amaoto.GUI
{
    /// <summary>
    /// ただ画像表示を行うだけの GUI 部品。
    /// </summary>
    public class Image : DrawPart
    {
        public Image(Texture texture, int? width = null, int? height = null)
            : base(width ?? texture.TextureSize.Width, height ?? texture.TextureSize.Height)
        {
            if (width.HasValue)
            {
                texture.ScaleX = (float)(1.0 * width.Value / texture.TextureSize.Width);
            }

            if (height.HasValue)
            {
                texture.ScaleY = (float)(1.0 * height.Value / texture.TextureSize.Height);
            }

            Texture = texture;
        }
    }
}
