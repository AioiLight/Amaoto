namespace Amaoto.Animation
{
    /// <summary>
    /// 何もしない
    /// </summary>
    public class Blank : Animator
    {
        /// <summary>
        /// 正真正銘、何もしない。
        /// </summary>
        /// <param name="param">維持させたい値</param>
        /// <param name="timeNs">維持させる時間。</param>
        public Blank(int param, int timeNs) : base(0, timeNs - 1, 1, false)
        {
            Param = param;
        }

        public override double GetAnimation()
        {
            return Param;
        }

        private readonly int Param;
    }
}
