using DxLibDLL;
using System;

namespace Amaoto
{
    /// <summary>
    /// テクスチャにエフェクトを掛けるメソッドを集めた静的クラス。
    /// </summary>
    public static class Filter
    {
        /// <summary>
        /// モノトーンフィルターを適用する。
        /// </summary>
        /// <param name="g">テクスチャ。</param>
        /// <param name="cB">青色差。-255～255の範囲。</param>
        /// <param name="cR">赤色差。-255～255の範囲。</param>
        public static void Monotone(this ITextureReturnable g, int cB, int cR)
        {
            DX.GraphFilter(g.GetTexture().ID, DX.DX_GRAPH_FILTER_MONO, Clamp(cB, -255, 255), Clamp(cR, -255, 255));
        }

        /// <summary>
        /// ガウスフィルターを適用する。
        /// </summary>
        /// <param name="g">テクスチャ。</param>
        /// <param name="width">ピクセル幅。8, 16, 32のいずれか。</param>
        /// <param name="strength">ぼかしの強さ。100で約1ピクセル分。</param>
        public static void Gauss(this ITextureReturnable g, int width, int strength)
        {
            if (width != 8 && width != 16 && width != 32)
            {
                throw new ArgumentOutOfRangeException();
            }

            DX.GraphFilter(g.GetTexture().ID, DX.DX_GRAPH_FILTER_GAUSS, width, strength);
        }

        /// <summary>
        /// 色相フィルターを適用する。
        /// </summary>
        /// <param name="g">テクスチャ。</param>
        /// <param name="hue">ピクセルの色相からどのくらい変えるか。-180～180の範囲。</param>
        public static void HueRel(this ITextureReturnable g, int hue)
        {
            DX.GraphFilter(g.GetTexture().ID, DX.DX_GRAPH_FILTER_PMA_HSB, 0, Clamp(hue, -180, 180), 0, 0);
        }

        /// <summary>
        /// 色相フィルターを適用する。
        /// </summary>
        /// <param name="g">テクスチャ。</param>
        /// <param name="hue">色相。0～360の範囲。</param>
        public static void HueAbs(this ITextureReturnable g, int hue)
        {
            DX.GraphFilter(g.GetTexture().ID, DX.DX_GRAPH_FILTER_PMA_HSB, 1, Clamp(hue, 0, 360), 0, 0);
        }

        /// <summary>
        /// 彩度フィルターを適用する。
        /// </summary>
        /// <param name="g">テクスチャ。</param>
        /// <param name="saturation">彩度。-255～の範囲。</param>
        public static void Saturation(this ITextureReturnable g, int saturation)
        {
            DX.GraphFilter(g.GetTexture().ID, DX.DX_GRAPH_FILTER_PMA_HSB, 0, 0, Clamp(saturation, -255, saturation), 0);
        }

        /// <summary>
        /// 輝度フィルターを適用する。
        /// </summary>
        /// <param name="g">テクスチャ。</param>
        /// <param name="brightness">輝度。-255～255の範囲。</param>
        public static void Brightness(this ITextureReturnable g, int brightness)
        {
            DX.GraphFilter(g.GetTexture().ID, DX.DX_GRAPH_FILTER_PMA_HSB, 0, 0, 0, Clamp(brightness, -255, 255));
        }

        /// <summary>
        /// HSBフィルターを適用する。
        /// </summary>
        /// <param name="g">テクスチャ。</param>
        /// <param name="absHue">色相を統一(絶対値を用いる)するかどうか。</param>
        /// <param name="hue">色相。</param>
        /// <param name="saturation">彩度。</param>
        /// <param name="brightness">輝度。</param>
        public static void HSB(this ITextureReturnable g, bool absHue, int hue, int saturation, int brightness)
        {
            var hueType = absHue ? 1 : 0;
            DX.GraphFilter(g.GetTexture().ID, DX.DX_GRAPH_FILTER_PMA_HSB, hueType,
                absHue ? Clamp(hue, 0, 360) : Clamp(hue, -180, 180), // 色相
                Clamp(saturation, -255, saturation), // 彩度
                Clamp(brightness, -255, 255)); // 輝度
        }

        /// <summary>
        /// レベル補正フィルターを適用する。
        /// </summary>
        /// <param name="g">テクスチャ。</param>
        /// <param name="inputMin">入力レベルの最小値。0～255の範囲。</param>
        /// <param name="inputMax">入力レベルの最大値。0～255の範囲。</param>
        /// <param name="gamma">ガンマ値。100が1.0を表し、1～の範囲。</param>
        /// <param name="outputMin">出力レベルの最小値。0～255の範囲。</param>
        /// <param name="outputMax">出力レベルの最大値。0～255の範囲。</param>
        public static void Level(this ITextureReturnable g, int inputMin, int inputMax, int gamma, int outputMin, int outputMax)
        {
            DX.GraphFilter(g.GetTexture().ID, DX.DX_GRAPH_FILTER_PMA_LEVEL,
                Clamp(inputMin, 0, 255), // 入力レベルの最小値
                Clamp(inputMax, 0, 255), // 入力レベルの最大値
                Clamp(gamma, 1, gamma), // ガンマ値
                Clamp(outputMin, 0, 255), // 出力レベルの最小値
                Clamp(outputMax, 0, 255)); // 出力レベルの最大値
        }

        private static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(max) > 0)
            {
                return max;
            }
            else if (value.CompareTo(min) < 0)
            {
                return min;
            }
            else
            {
                return value;
            }
        }
    }
}
