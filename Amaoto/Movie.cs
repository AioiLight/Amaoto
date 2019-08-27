using DxLibDLL;

namespace Amaoto
{
    /// <summary>
    /// 動画再生クラス。
    /// </summary>
    public class Movie : Texture
    {
        /// <summary>
        /// 動画ファイルのオープンを行います。
        /// </summary>
        /// <param name="fileName">ファイル名。</param>
        public Movie(string fileName)
            : base(fileName)
        {

        }

        /// <summary>
        /// 再生を開始します。
        /// </summary>
        public void Play()
        {
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
        /// 時間。
        /// </summary>
        public int Time
        {
            get
            {
                return DX.TellMovieToGraph(ID);
            }
            set
            {
                DX.SeekMovieToGraph(ID, value);
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
    }
}
