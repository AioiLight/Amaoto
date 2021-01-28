namespace Amaoto.GUI
{
    /// <summary>
    /// ただ画像表示を行うだけの GUI 部品。
    /// </summary>
    public class Image : DrawPart
    {
        /// <summary>
        /// ただ画像表示を行うだけの GUI 部品。
        /// </summary>
        /// <param name="texture">テクスチャ。</param>
        /// <param name="width">(オプション)横幅。指定しない場合は画像の横幅がそのまま使用される。</param>
        /// <param name="height">(オプション)縦幅。指定しない場合は画像の縦幅がそのまま使用される。</param>
        public Image(Texture texture, int? width = null, int? height = null)
            : base(width ?? texture.TextureSize.Width, height ?? texture.TextureSize.Height)
        {
            SetScale(texture, width, height);

            Texture = texture;
            ImageWidth = width ?? texture.TextureSize.Width;
            ImageHeight = height ?? texture.TextureSize.Height;
        }

        /// <summary>
        /// 新しい画像で置換する。
        /// GUIのサイズは初期化時のサイズがそのまま使用される。
        /// </summary>
        /// <param name="texture">テクスチャ。</param>
        public void ReplaceImage(Texture texture)
        {
            SetScale(texture, ImageWidth, ImageHeight);
            Texture = texture;
        }

        private void SetScale(Texture texture, int? width = null, int? height = null)
        {
            if (width.HasValue)
            {
                texture.ScaleX = 1.0 * width.Value / texture.TextureSize.Width;
            }

            if (height.HasValue)
            {
                texture.ScaleY = 1.0 * height.Value / texture.TextureSize.Height;
            }
        }

        private readonly int ImageWidth;
        private readonly int ImageHeight;
    }
}
