using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="timeMs">イージングにかける時間。</param>
        public EaseOut(int startPoint, int endPoint, int timeMs) : base(0, timeMs - 1, 1, false)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            Sa = EndPoint - StartPoint;
            TimeMs = timeMs;
        }
        /// <summary>
        /// 座標を返します。
        /// </summary>
        /// <returns>double型の座標。</returns>
        public override object GetAnimation()
        {
            var persent = Counter.Value / (double)TimeMs;
            persent -= 1;
            return (double)Sa * (persent * persent * persent + 1) + StartPoint;
        }

        private readonly int StartPoint;
        private readonly int EndPoint;
        private readonly int Sa;
        private readonly int TimeMs;
    }
}
