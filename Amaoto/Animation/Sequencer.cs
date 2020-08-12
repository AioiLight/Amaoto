using System;
using System.Collections.Generic;
using System.Linq;

namespace Amaoto.Animation
{
    /// <summary>
    /// アニメーション シーケンサー
    /// 複数のAmaoto.Animation.Animatorを追加して、アニメーションを連続的に再生します。
    /// </summary>
    public class Sequencer
    {
        /// <summary>
        /// シーケンサーを初期化します。
        /// </summary>
        public Sequencer()
        {
            Animators = new List<Animator>();
        }

        /// <summary>
        /// アニメーターを追加する。ループするアニメーターは使えません。
        /// </summary>
        /// <param name="animator">アニメーター。</param>
        /// <returns>シーケンサー。メソッドチェーンできます。</returns>
        public Sequencer AddAnimator(Animator animator)
        {
            if (animator != null)
            {
                if (!animator.IsLoop)
                {
                    Animators.Add(animator);
                }
                else
                {
                    throw new NotSupportedException("Animator can not be loop.");
                }
            }
            return this;
        }

        /// <summary>
        /// アニメーターのタイマーを更新する。
        /// </summary>
        public void Update()
        {
            Animators[Index].Tick();
            if (Animators[Index].Counter.Value >= Animators[Index].Counter.End && Animators[Index].Counter.State == TimerState.Stopped)
            {
                if (Animators.Count - 1 > Index)
                {
                    Index++;
                    Animators[Index].Start();
                }
                else
                {
                    if (!EndFlag)
                    { 
                        SequenceEnded?.Invoke(this, null);
                        EndFlag = true;
                    }
                }
            }
            Animators[Index].Tick();
        }

        /// <summary>
        /// シーケンサーを開始する。
        /// </summary>
        public void Start()
        {
            Animators[Index].Start();
        }

        /// <summary>
        /// シーケンサーを止める。
        /// </summary>
        public void Stop()
        {
            Animators[Index].Stop();
        }

        /// <summary>
        /// シーケンサーをリセットして、再利用可能な状態にする。
        /// </summary>
        public void Reset()
        {
            Index = 0;
            foreach (var item in Animators)
            {
                item.Stop();
                item.Reset();
            }

            EndFlag = false;
        }

        /// <summary>
        /// 現在のアニメーターのインスタンスを返す。
        /// </summary>
        /// <returns>現在のアニメーターのインスタンス。</returns>
        public Animator GetCurrentAnimator()
        {
            return Animators[GetCurrentAnimatorIndex()];
        }

        /// <summary>
        /// 現在のアニメーターの位置を返す。
        /// </summary>
        /// <returns>現在のアニメーターの位置。</returns>
        public int GetCurrentAnimatorIndex()
        {
            return Index;
        }

        /// <summary>
        /// アニメーションが全て完了しているか
        /// </summary>
        /// <returns>アニメーションが全て完了しているかどうか。</returns>
        public bool IsFinished()
        {
            return Animators.Last().Counter.Value >= Animators.Last().Counter.End;
        }

        /// <summary>
        /// 現在のアニメーターからアニメーション結果を得る。
        /// </summary>
        /// <returns>アニメーション結果。</returns>
        public double GetAnimation()
        {
            return Animators[Index].GetAnimation();
        }

        /// <summary>
        /// アニメーターのリスト。
        /// </summary>
        public List<Animator> Animators { get; private set; }

        /// <summary>
        /// 現在どのアニメーターを再生してるかのインデックス。
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// アニメーションシーケンサーがすべてのアニメーションっを再生し終了した。
        /// </summary>
        public event EventHandler SequenceEnded;

        private bool EndFlag;
    }
}