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
        /// <param name="timeNs">イージングにかける時間。</param>
        public EaseInBack(int startPoint, int endPoint, int timeNs) : base(0, timeNs - 1, 1, false)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            Sa = EndPoint - StartPoint;
            TimeNs = timeNs;
        }

        /// <summary>
        /// 座標を返します。
        /// </summary>
        /// <returns>double型の座標。</returns>
        public override double GetAnimation()
        {
            var persent = Counter.Value / (double)TimeNs;
            var c1 = 1.70158;
            var c3 = c1 + 1;

            return Sa * ((c3 * persent * persent * persent) - (c1 * persent * persent)) + StartPoint;
        }

        private readonly int StartPoint;
        private readonly int EndPoint;
        private readonly int Sa;
        private readonly int TimeNs;
    }
}