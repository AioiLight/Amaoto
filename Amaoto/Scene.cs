using System.Collections.Generic;

namespace Amaoto
{
    /// <summary>
    /// シーンクラス。
    /// </summary>
    public class Scene
    {
        /// <summary>
        /// シーンの初期化を行います。
        /// </summary>
        public Scene()
        {
            Enabled = true;
            ChildScene = new List<Scene>();
        }

        ~Scene()
        {
            Enabled = false;
            ChildScene.Clear();
        }

        /// <summary>
        /// 子シーンを追加します。
        /// </summary>
        /// <param name="scene">子シーン。</param>
        public void AddChildScene(Scene scene)
        {
            ChildScene.Add(scene);
        }

        /// <summary>
        /// アクティブ化する。
        /// </summary>
        public virtual void Enable()
        {
            if (!Enabled) return;
            foreach (var item in ChildScene)
            {
                item.Enable();
            }
        }

        /// <summary>
        /// 非アクティブ化する。
        /// </summary>
        public virtual void Disable()
        {
            if (!Enabled) return;
            foreach (var item in ChildScene)
            {
                item.Disable();
            }
            ChildScene.Clear();
        }

        /// <summary>
        /// 更新を行う。
        /// </summary>
        public virtual void Update()
        {
            if (!Enabled) return;
        }

        /// <summary>
        /// 描画を行う。
        /// </summary>
        public virtual void Draw()
        {
            if (!Enabled) return;
        }

        /// <summary>
        /// そのシーンの名前(名前空間付き)を返します。
        /// </summary>
        /// <returns>そのシーンの名前(名前空間付き)。</returns>
        public override string ToString()
        {
            return GetType().ToString();
        }

        /// <summary>
        /// 利用可能かどうか。
        /// </summary>
        public bool Enabled { get; private set; }
        /// <summary>
        /// 子シーン。
        /// </summary>
        public List<Scene> ChildScene { get; private set; }
    }
}
