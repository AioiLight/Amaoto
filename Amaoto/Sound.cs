using System;
using DxLibDLL;

namespace Amaoto
{
    /// <summary>
    /// サウンド管理を行うクラス。
    /// </summary>
    public class Sound : IDisposable
    {
        /// <summary>
        /// サウンドを生成します。
        /// </summary>
        public Sound(string fileName)
        {
            ID = DX.LoadSoundMem(fileName);
            if (ID != -1)
            {
                IsEnable = true;
            }
            FileName = fileName;
        }

        ~Sound()
        {
            if (IsEnable)
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            if (DX.DeleteSoundMem(ID) != -1)
            {
                IsEnable = false;
            }
        }

        /// <summary>
        /// サウンドを再生します。
        /// </summary>
        /// <param name="playFromBegin">はじめから</param>
        public void Play(bool playFromBegin = true)
        {
            if (IsEnable)
            {
                DX.PlaySoundMem(ID, DX.DX_PLAYTYPE_BACK, playFromBegin ? 1 : 0);
            }
        }

        /// <summary>
        /// サウンドを停止します。
        /// </summary>
        public void Stop()
        {
            if (IsEnable)
            {
                DX.StopSoundMem(ID);
            }
        }

        /// <summary>
        /// 有効かどうか。
        /// </summary>
        public bool IsEnable { get; private set; }

        /// <summary>
        /// ファイル名。
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// ID。
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// 再生中かどうか。
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                return DX.CheckSoundMem(ID) == 1;
            }
        }

        /// <summary>
        /// パン。
        /// </summary>
        public int Pan
        {
            get
            {
                return _pan;
            }
            set
            {
                _pan = value;
                DX.ChangePanSoundMem(value, ID);
            }
        }

        /// <summary>
        /// 音量。
        /// </summary>
        public int Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                _volume = value;
                DX.ChangeVolumeSoundMem(value, ID);
            }
        }

        /// <summary>
        /// 再生位置。秒が単位。
        /// </summary>
        public double Time
        {
            get
            {
                var freq = DX.GetFrequencySoundMem(ID);
                var pos = DX.GetCurrentPositionSoundMem(ID);
                // サンプル数で割ると秒数が出るが出る
                return 1.0 * pos / freq;
            }
            set
            {
                var freq = DX.GetFrequencySoundMem(ID);
                var pos = value;
                DX.SetCurrentPositionSoundMem((int)(1.0 * pos * freq), ID);
            }
        }

        /// <summary>
        /// 再生速度を倍率で変更する。
        /// </summary>
        public double PlaySpeed
        {
            get
            {
                return _ratio;
            }
            set
            {
                _ratio = value;
                DX.ResetFrequencySoundMem(ID);
                var freq = DX.GetFrequencySoundMem(ID);
                // 倍率変更
                var speed = value * freq;
                // 1秒間に再生すべきサンプル数を上げ下げすると速度が変化する。
                DX.SetFrequencySoundMem((int)speed, ID);
            }
        }

        private int _pan;
        private int _volume;
        private double _ratio;
    }
}