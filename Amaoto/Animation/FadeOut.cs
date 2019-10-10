﻿using System;
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
        /// <param name="timeMs">フェードアウトに掛ける秒数(ミリ秒)</param>
        public FadeOut(int timeMs) : base(0, timeMs - 1, 1, false)
        {
            TimeNs = timeMs;
        }

        /// <summary>
        /// フェードアウトの不透明度を0～1の範囲で返します。
        /// </summary>
        /// <returns>不透明度。</returns>
        public override object GetAnimation()
        {
            var opacity = (TimeNs - base.Counter.Value) / TimeNs;
            return opacity;
        }

        private readonly int TimeNs;
    }
}
