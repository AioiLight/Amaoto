using System;
using DxLibDLL;

namespace Amaoto
{
    /// <summary>
    /// ログクラス。
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// ログを追加する。
        /// </summary>
        /// <param name="str">内容。</param>
        /// <returns>このクラスのインスタンス。</returns>
        public Logger Add(string str)
        {
            DX.ErrorLogAdd(str + Environment.NewLine);
            return this;
        }

        /// <summary>
        /// ログを追加する。
        /// </summary>
        /// <param name="strArray">内容。</param>
        /// <returns>このクラスのインスタンス。</returns>
        public Logger Add(string[] strArray)
        {
            Array.ForEach(strArray, s => DX.ErrorLogAdd(s + Environment.NewLine));
            return this;
        }

        /// <summary>
        /// ログのインデントを増やす。
        /// </summary>
        /// <returns>このクラスのインスタンス。</returns>
        public Logger Indent()
        {
            DX.ErrorLogTabAdd();
            return this;
        }

        /// <summary>
        /// ログのインデントを減らす。
        /// </summary>
        /// <returns>このクラスのインスタンス。</returns>
        public Logger UnIndent()
        {
            DX.ErrorLogTabSub();
            return this;
        }
    }
}
