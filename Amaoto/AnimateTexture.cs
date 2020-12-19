namespace Amaoto
{
    /// <summary>
    /// 連番画像を読み込み、動画のように見せる。
    /// </summary>
    public class AnimateTexture : IPlayable, ITextureReturnable
    {
        /// <summary>
        /// 連番画像を読み込み、動画のように見せる。
        /// </summary>
        /// <param name="textures">連番画像。</param>
        /// <param name="fps">FPS。</param>
        /// <param name="loop">ループするかどうか。</param>
        public AnimateTexture(Texture[] textures, double fps, bool loop)
        {
            Textures = textures;
            Animation = new Counter(0, textures.Length - 1, (long)(1000.0 * 1000 / fps), loop);
        }

        private readonly Counter Animation;
        private readonly Texture[] Textures;

        /// <summary>
        /// 現在再生中かどうか。
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                return Animation.State == TimerState.Started;
            }
        }

        /// <summary>
        /// 何コマ目かを設定または取得します。
        /// </summary>
        public double Time
        {
            get
            {
                Animation.Tick();
                return Animation.Value;
            }
            set
            {
                Animation.Value = (long)value;
            }
        }

        /// <summary>
        /// 音声はないため、必ず0がリターンされます。
        /// </summary>
        public double Volume
        {
            get
            {
                return 0;
            }
            set
            {

            }
        }

        /// <summary>
        /// 再生を開始します。
        /// </summary>
        /// <param name="playFromBegin">最初から再生するか？</param>
        public void Play(bool playFromBegin = true)
        {
            Animation.Stop();
            if (playFromBegin)
            {
                Animation.Reset();
            }
            Animation.Start();
        }

        /// <summary>
        /// 再生を停止します。
        /// </summary>
        public void Stop()
        {
            Animation.Stop();
        }

        /// <summary>
        /// 現在のコマのテクスチャをリターンします。
        /// </summary>
        /// <returns>現在のコマのテクスチャ。</returns>
        public Texture GetTexture()
        {
            Animation.Tick();
            return Textures[Animation.Value];
        }
    }
}
