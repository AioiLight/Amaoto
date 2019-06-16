using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amaoto.Animation
{
    /// <summary>
    /// フェードアウトを行うクラス。
    /// </summary>
    public class FadeOut : Animator
    {
        /// <summary>
        /// フェードアウトを初期化します。
        /// </summary>
        /// <param name="timems">フェードアウトに掛ける秒数(ミリ秒)</param>
        public FadeOut(int timems) : base(0, timems - 1, 1, false)
        {
            TimeMs = timems;
        }

        /// <summary>
        /// フェードアウトの不透明度を0～1の範囲で返します。
        /// </summary>
        /// <returns>不透明度。</returns>
        public override object GetAnimation()
        {
            var opacity = (TimeMs - base.Counter.Value) / TimeMs;
            return opacity;
        }

        private readonly int TimeMs;
    }
}
