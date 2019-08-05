using DxLibDLL;

namespace Amaoto
{
    /// <summary>
    /// FPSを計測するクラス。
    /// </summary>
    public class FPSCounter
    {
        /// <summary>
        /// FPSを計測するクラス。
        /// </summary>
        public FPSCounter()
        {
            NowFPS = 0;
            FPS = 0;
            Counter = DX.GetNowHiPerformanceCount();
        }

        /// <summary>
        /// FPSカウンターを更新します。
        /// </summary>
        public void Update()
        {
            NowFPS++;
            var nowTime = DX.GetNowHiPerformanceCount();
            if(nowTime - Counter >= 1000 * 1000)
            {
                // 一秒経過
                FPS = NowFPS;
                NowFPS = 0;
                Counter = nowTime;
            }
        }

        /// <summary>
        /// 現在のFPS。
        /// </summary>
        public int FPS { get; private set; }
        private int NowFPS;
        private long Counter;
    }
}
