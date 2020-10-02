namespace Amaoto.Animation
{
    /// <summary>
    /// フェードアウトを行うクラス。
    /// </summary>
    public class FadeOut : Animator
    {
        /// <summary>
        /// フェードアウトを初期化します。
        /// </summary>
        /// <param name="timeUs">フェードアウトに掛ける秒数(マイクロ秒)</param>
        public FadeOut(int timeUs) : base(0, timeUs - 1, 1, false)
        {
            TimeUs = timeUs;
        }

        /// <summary>
        /// フェードアウトの不透明度を0～1の範囲で返します。
        /// </summary>
        /// <returns>不透明度。</returns>
        public override double GetAnimation()
        {
            var opacity = 1.0 * (TimeUs - base.Counter.Value) / TimeUs;
            return opacity;
        }

        private readonly int TimeUs;
    }
}