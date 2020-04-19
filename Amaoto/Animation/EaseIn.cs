using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amaoto.Animation
{
    /// <summary>
    /// イーズインを行うクラス。
    /// </summary>
    public class EaseIn : Animator
    {
        /// <summary>
        /// イーズインを初期化します。
        /// </summary>
        /// <param name="startPoint">始点。</param>
        /// <param name="endPoint">終点。</param>
        /// <param name="timeNs">イージングにかける時間。</param>
        public EaseIn(int startPoint, int endPoint, int timeNs) : base(0, timeNs - 1, 1, false)
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
            return (Sa * persent * persent * persent) + StartPoint;
        }

        private readonly int StartPoint;
        private readonly int EndPoint;
        private readonly int Sa;
        private readonly int TimeNs;
    }
}
