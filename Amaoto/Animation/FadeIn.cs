using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="timeNs">フェードインに掛ける秒数(ミリ秒)</param>
        public FadeIn(int timeNs) : base(0, timeNs - 1, 1, false)
        {
            TimeNs = timeNs;
        }

        /// <summary>
        /// フェードインの不透明度を0～1の範囲で返します。
        /// </summary>
        /// <returns>不透明度。</returns>
        public override double GetAnimation()
        {
            var opacity = 1.0 * base.Counter.Value / TimeNs;
            return opacity;
        }

        private readonly int TimeNs;
    }
}
