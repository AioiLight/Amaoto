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
        /// <param name="timems">フェードインに掛ける秒数(ミリ秒)</param>
        public FadeIn(int timems) : base(0, timems - 1, 1, false)
        {
            TimeMs = timems;
        }

        /// <summary>
        /// フェードインの不透明度を0～1の範囲で返します。
        /// </summary>
        /// <returns>不透明度。</returns>
        public override object GetAnimation()
        {
            var opacity = base.Counter.Value / TimeMs;
            return opacity;
        }

        private readonly int TimeMs;
    }
}
