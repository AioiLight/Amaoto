namespace Amaoto
{
    /// <summary>
    /// Amaoto 関係のユーティリティクラス。
    /// </summary>
    public static class AmaotoUtil
    {
                /// <summary>
        /// テクスチャのX軸の適切なスケール率を横幅から求める。
        /// </summary>
        /// <param name="texture">テクスチャ。</param>
        /// <param name="width">横幅。</param>
        /// <param name="expandOrig">元のサイズより拡大するかどうか。</param>
        /// <returns>スケール率。</returns>
        public static float GetProperScaleX(Texture texture, int width, bool expandOrig = false)
        {
            if (texture == null)
            {
                return 1.0f;
            }

            var ratio = (float)(1.0 * width / texture.TextureSize.Width);
            
            if (expandOrig)
            {
                return ratio;
            }
            else
            {
                return ratio >= 1.0f ? 1.0f : ratio;
            }
        }

        /// <summary>
        /// テクスチャのY軸の適切なスケール率を縦幅から求める。
        /// </summary>
        /// <param name="texture">テクスチャ。</param>
        /// <param name="height">縦幅。</param>
        /// <param name="expandOrig">元のサイズより拡大するかどうか。</param>
        /// <returns>スケール率。</returns>
        public static float GetProperScaleY(Texture texture, int height, bool expandOrig = false)
        {
            if (texture == null)
            {
                return 1.0f;
            }

            var ratio = (float)(1.0 * height / texture.TextureSize.Height);

            if (expandOrig)
            {
                return ratio;
            }
            else
            {
                return ratio >= 1.0f ? 1.0f : ratio;
            }
        }

        /// <summary>
        /// ReferencePointと横幅縦幅から適切な位置を計算する。
        /// </summary>
        /// <param name="width">横幅</param>
        /// <param name="height">縦幅</param>
        /// <param name="referencePoint">ReferencePoint</param>
        /// <returns>座標。</returns>
        public static (int x, int y) GetProperPositionFromReferencePoint(int width, int height, ReferencePoint referencePoint)
        {
            if (referencePoint == ReferencePoint.TopLeft)
            {
                return (0, 0);
            }
            else if (referencePoint == ReferencePoint.TopCenter)
            {
                return (width / 2, 0);
            }
            else if (referencePoint == ReferencePoint.TopRight)
            {
                return (width, 0);
            }
            else if (referencePoint == ReferencePoint.CenterLeft)
            {
                return (0, height / 2);
            }
            else if (referencePoint == ReferencePoint.Center)
            {
                return (width / 2, height / 2);
            }
            else if (referencePoint == ReferencePoint.CenterRight)
            {
                return (width, height / 2);
            }
            else if (referencePoint == ReferencePoint.BottomLeft)
            {
                return (0, height);
            }
            else if (referencePoint == ReferencePoint.BottomCenter)
            {
                return (width / 2, height);
            }
            else
            {
                return (width, height);
            }
        }

        /// <summary>
        /// 左揃えにしたReferencePointを取得する。
        /// 例えば、Centerを引数とした場合、戻り値としてCenterLeftが取得できる。
        /// </summary>
        /// <param name="referencePoint">ReferencePoint</param>
        /// <returns>左揃えにしたReferencePoint</returns>
        public static ReferencePoint GetLeftedReferencePoint(ReferencePoint referencePoint)
        {
            return (ReferencePoint)((int)referencePoint - ((int)referencePoint % 3));
        }

        /// <summary>
        /// 上揃えにしたReferencePointを取得する。
        /// 例えば、Centerを引数とした場合、戻り値としてTopCenterが取得できる。
        /// </summary>
        /// <param name="referencePoint">ReferencePoint</param>
        /// <returns>上揃えにしたReferencePoint</returns>
        public static ReferencePoint GetToppedReferencePoint(ReferencePoint referencePoint)
        {
            return (ReferencePoint)((int)referencePoint % 3);
        }
    }
}
