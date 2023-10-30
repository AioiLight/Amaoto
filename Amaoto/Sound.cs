using System;
using DxLibDLL;

namespace Amaoto
{
    /// <summary>
    /// サウンド管理を行うクラス。
    /// </summary>
    public class Sound : IDisposable, IPlayable
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
                if (DX.GetUseASyncLoadFlag() != DX.TRUE)
                {
                    Frequency = DX.GetFrequencySoundMem(ID);
                }
            }
            FileName = fileName;

            _volume = 255;
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
        /// 次再生されるときの音量を設定します。プロパティは変更されません。
        /// </summary>
        /// <param name="volume">音量。</param>
        public void SetNextVolue(double volume)
        {
            DX.ChangeNextPlayVolumeSoundMem((int)(volume * 255), ID);
        }

        /// <summary>
        /// 次再生されるときのパンを設定します。プロパティは変更されません。
        /// </summary>
        /// <param name="pan">パン。-1～1の範囲。</param>
        public void SetNextPan(double pan)
        {
            DX.ChangeNextPlayPanSoundMem((int)(pan * 255), ID);
        }

        /// <summary>
        /// 次再生されるときの再生速度を変更します。プロパティは変更されません。
        /// </summary>
        /// <param name="value">再生速度。</param>
        public void SetNextSpeed(double value)
        {
            SetFreq();
            if (!Frequency.HasValue)
            {
                throw new Exception("Sound file is not loaded yet.");
            }

            var freq = Frequency.Value;
            // 倍率変更
            var speed = value * freq;
            // 1秒間に再生すべきサンプル数を上げ下げすると速度が変化する。
            DX.SetNextPlayFrequencySoundMem((int)speed, ID);
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

        private void SetFreq()
        {
            if (!Frequency.HasValue)
            {
                if (DX.CheckHandleASyncLoad(ID) == DX.FALSE)
                {
                    Frequency = DX.GetFrequencySoundMem(ID);
                }
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
        public double Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                _volume = (int)(value * 255);
                DX.ChangeVolumeSoundMem(_volume, ID);
            }
        }

        /// <summary>
        /// 再生位置。秒が単位。
        /// </summary>
        public double Time
        {
            get
            {
                SetFreq();
                if (!Frequency.HasValue)
                {
                    throw new Exception("Sound file is not loaded yet.");
                }

                var freq = Frequency.Value;
                var pos = DX.GetCurrentPositionSoundMem(ID);
                // サンプル数で割ると秒数が出る
                return 1.0 * pos / freq;
            }
            set
            {
                SetFreq();
                if (!Frequency.HasValue)
                {
                    throw new Exception("Sound file is not loaded yet.");
                }

                var freq = Frequency.Value;
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
                SetFreq();
                if (!Frequency.HasValue)
                {
                    throw new Exception("Sound file is not loaded yet.");
                }

                _ratio = value;
                var freq = Frequency.Value;
                // 倍率変更
                var speed = value * freq;
                // 1秒間に再生すべきサンプル数を上げ下げすると速度が変化する。
                DX.SetFrequencySoundMem((int)speed, ID);
            }
        }

        /// <summary>
        /// 音声の周波数。
        /// </summary>
        public int? Frequency { get; private set; }

        private int _pan;
        private int _volume;
        private double _ratio;
    }
}