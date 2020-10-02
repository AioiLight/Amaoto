namespace Amaoto.Animation
{
    /// <summary>
    /// リニア移動を行うクラス。
    /// </summary>
    public class Linear : Animator
    {
        /// <summary>
        /// リニア移動を初期化します。
        /// </summary>
        /// <param name="startPoint">始点。</param>
        /// <param name="endPoint">終点。</param>
        /// <param name="timeUs">移動にかける時間。</param>
        public Linear(int startPoint, int endPoint, int timeUs) : base(0, timeUs - 1, 1, false)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            Sa = EndPoint - StartPoint;
            TimeUs = timeUs;
        }

        public override double GetAnimation()
        {
            var persent = Counter.Value / (double)TimeUs;
            return (Sa * persent) + StartPoint;
        }

        public int StartPoint { get; private set; }
        public int EndPoint { get; private set; }
        private readonly int Sa;
        private readonly int TimeUs;
    }
}