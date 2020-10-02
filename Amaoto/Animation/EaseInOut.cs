namespace Amaoto.Animation
{
    /// <summary>
    /// イーズイン・アウトを行うクラス。
    /// </summary>
    public class EaseInOut : Animator
    {
        /// <summary>
        /// イーズイン・アウトを初期化します。
        /// </summary>
        /// <param name="startPoint">始点。</param>
        /// <param name="endPoint">終点。</param>
        /// <param name="timeUs">イージングにかける時間。</param>
        public EaseInOut(int startPoint, int endPoint, int timeUs) : base(0, timeUs - 1, 1, false)
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
            var persent = Counter.Value / (double)TimeUs * 2.0;
            if (persent < 1)
            {
                return (Sa / 2.0 * persent * persent * persent) + StartPoint;
            }
            else
            {
                persent -= 2;
                return (Sa / 2.0 * ((persent * persent * persent) + 2)) + StartPoint;
            }
        }

        public int StartPoint { get; private set; }
        public int EndPoint { get; private set; }
        private readonly int Sa;
        private readonly int TimeUs;
    }
}