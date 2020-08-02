namespace Amaoto.Animation
{
    /// <summary>
    /// アニメーター抽象クラス、
    /// </summary>
    public abstract class Animator
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

        /// <summary>
        /// アニメーターの現在の値を返します。
        /// </summary>
        /// <returns>現在の値。</returns>
        public abstract double GetAnimation();

        // プロパティ
        /// <summary>
        /// カウンター。
        /// </summary>
        public Counter Counter { get; private set; }

        /// <summary>
        /// 開始値。
        /// </summary>
        public long StartValue { get; private set; }

        /// <summary>
        /// 終了値。
        /// </summary>
        public long EndValue { get; private set; }

        /// <summary>
        /// 更新間隔。
        /// </summary>
        public long TickInterval { get; private set; }

        /// <summary>
        /// ループするかどうか。
        /// </summary>
        public bool IsLoop { get; private set; }
    }
}