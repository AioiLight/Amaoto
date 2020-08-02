﻿using System;

namespace Amaoto.Animation
{
    /// <summary>
    /// イーズアウトバックを行うクラス。
    /// </summary>
    public class EaseOutBack : Animator
    {
        /// <summary>
        /// イーズアウトバックを初期化します。
        /// </summary>
        /// <param name="startPoint">始点。</param>
        /// <param name="endPoint">終点。</param>
        /// <param name="timeNs">イージングにかける時間。</param>
        public EaseOutBack(int startPoint, int endPoint, int timeNs) : base(0, timeNs - 1, 1, false)
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
            var c1 = 1.70158;
            var c3 = c1 + 1;
            return Sa * (1 + c3 * Math.Pow(persent - 1, 3) + c1 * Math.Pow(persent - 1, 2)) + StartPoint;
        }

        private readonly int StartPoint;
        private readonly int EndPoint;
        private readonly int Sa;
        private readonly int TimeNs;
    }
}