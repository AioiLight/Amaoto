﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amaoto.Animation
{
    /// <summary>
    /// アニメーター抽象クラス、
    /// </summary>
    abstract class Animator
    {
        /// <summary>
        /// アニメーターを初期化します。
        /// </summary>
        /// <param name="startValue">開始値。</param>
        /// <param name="endValue">終了値。</param>
        /// <param name="tickInterval">Tick間隔。</param>
        /// <param name="isLoop">ループするかどうか。</param>
        public Animator(long startValue, long endValue, long tickInterval, bool isLoop)
        {
            StartValue = startValue;
            EndValue = endValue;
            TickInterval = tickInterval;
            IsLoop = isLoop;
            Counter = new Counter(StartValue, EndValue, TickInterval, IsLoop);
        }
        /// <summary>
        /// アニメーションを開始します。
        /// </summary>
        public void Start()
        {
            Counter?.Start();
        }
        /// <summary>
        /// アニメーションを停止します。
        /// </summary>
        public void Stop()
        {
            Counter?.Stop();
        }
        /// <summary>
        /// アニメーターをリセットします。
        /// </summary>
        public void Reset()
        {
            Counter?.Reset();
        }
        /// <summary>
        /// アニメーターの更新をします。
        /// </summary>
        public void Tick()
        {
            Counter?.Tick();
        }

        public abstract object GetAnimation();



        // プロパティ
        public Counter Counter { get; private set; }
        public long StartValue { get; private set; }
        public long EndValue { get; private set; }
        public long TickInterval { get; private set; }
        public bool IsLoop { get; private set; }
    }
}
