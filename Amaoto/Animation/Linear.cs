using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="timeNs">移動にかける時間。</param>
        public Linear(int startPoint, int endPoint, int timeNs) : base(0, timeNs - 1, 1, false)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            Sa = EndPoint - StartPoint;
            TimeNs = timeNs;
        }

        public override double GetAnimation()
        {
            var persent = Counter.Value / (double)TimeNs;
            return (Sa * persent) + StartPoint;
        }

        private readonly int StartPoint;
        private readonly int EndPoint;
        private readonly int Sa;
        private readonly int TimeNs;
    }
}
