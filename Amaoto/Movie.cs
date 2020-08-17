using DxLibDLL;

namespace Amaoto
{
    /// <summary>
    /// 動画再生クラス。
    /// </summary>
    public class Movie : Texture, IPlayable
    {
        /// <summary>
        /// 動画ファイルのオープンを行います。
        /// </summary>
        /// <param name="fileName">ファイル名。</param>
        public Movie(string fileName)
            : base(fileName)
        {
            Volume = 1.0;
        }

        /// <summary>
        /// 再生を開始します。
        /// </summary>
        public void Play(bool playFromBegin = true)
        {
            if (playFromBegin)
            {
                Time = 0;
            }
            DX.PlayMovieToGraph(ID);
        }

        /// <summary>
        /// 再生を停止します。
        /// </summary>
        public void Stop()
        {
            DX.PauseMovieToGraph(ID);
        }

        /// <summary>
        /// 動画の音量。
        /// </summary>
        public double Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                _volume = (int)(value * 255);
                DX.ChangeMovieVolumeToGraph(_volume, ID);
            }
        }

        /// <summary>
        /// 時間。単位はミリ秒。
        /// </summary>
        public double Time
        {
            get
            {
                return DX.TellMovieToGraph(ID);
            }
            set
            {
                DX.SeekMovieToGraph(ID, (int)value);
            }
        }

        /// <summary>
        /// 再生中かどうか。
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                return DX.GetMovieStateToGraph(ID) == 1 ? true : false;
            }
        }

        private int _volume;
    }
}