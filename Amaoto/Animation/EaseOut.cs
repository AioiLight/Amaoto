namespace Amaoto.Animation
{
    /// <summary>
    /// イーズアウトを行うクラス。
    /// </summary>
    public class EaseOut : Animator
    {
        /// <summary>
        /// イーズアウトを初期化します。
        /// </summary>
        /// <param name="startPoint">始点。</param>
        /// <param name="endPoint">終点。</param>
        /// <param name="timeUs">イージングにかける時間。</param>
        public EaseOut(int startPoint, int endPoint, int timeUs) : base(0, timeUs - 1, 1, false)
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
            persent -= 1;
            return (double)Sa * (persent * persent * persent + 1) + StartPoint;
        }

        public int StartPoint { get; private set; }
        public int EndPoint { get; private set; }
        private readonly int Sa;
        private readonly int TimeUs;
    }
}