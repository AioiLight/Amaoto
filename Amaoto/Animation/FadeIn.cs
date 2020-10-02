namespace Amaoto.Animation
{
    /// <summary>
    /// フェードインを行うクラス。
    /// </summary>
    public class FadeIn : Animator
    {
        /// <summary>
        /// フェードインを初期化します。
        /// </summary>
        /// <param name="timeUs">フェードインに掛ける秒数(マイクロ秒)</param>
        public FadeIn(int timeUs) : base(0, timeUs - 1, 1, false)
        {
            TimeUs = timeUs;
        }

        /// <summary>
        /// フェードインの不透明度を0～1の範囲で返します。
        /// </summary>
        /// <returns>不透明度。</returns>
        public override double GetAnimation()
        {
            var opacity = 1.0 * base.Counter.Value / TimeUs;
            return opacity;
        }

        private readonly int TimeUs;
    }
}