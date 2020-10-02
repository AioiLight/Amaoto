namespace Amaoto.Animation
{
    /// <summary>
    /// イーズインバックを行うクラス。
    /// </summary>
    public class EaseInBack : Animator
    {
        /// <summary>
        /// イーズインバックを初期化します。
        /// </summary>
        /// <param name="startPoint">始点。</param>
        /// <param name="endPoint">終点。</param>
        /// <param name="timeUs">イージングにかける時間。</param>
        public EaseInBack(int startPoint, int endPoint, int timeUs) : base(0, timeUs - 1, 1, false)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            Sa = EndPoint - StartPoint;
            TimeUs = timeUs;
        }

        /// <summary>
        /// 座標を返します。
        /// </summary>
        /// <returns>double型の座標。</returns>
        public override double GetAnimation()
        {
            var persent = Counter.Value / (double)TimeUs;
            var c1 = 1.70158;
            var c3 = c1 + 1;

            return Sa * ((c3 * persent * persent * persent) - (c1 * persent * persent)) + StartPoint;
        }

        public int StartPoint { get; private set; }
        public int EndPoint { get; private set; }
        private readonly int Sa;
        private readonly int TimeUs;
    }
}