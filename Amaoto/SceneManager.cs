using System.Collections.Generic;

namespace Amaoto
{
    /// <summary>
    /// シーン管理クラス。
    /// </summary>
    public class SceneManager
    {
        /// <summary>
        /// シーン管理クラスを初期化します。
        /// </summary>
        public SceneManager()
        {
            Scenes = new List<Scene>();
        }

        /// <summary>
        /// シーンを追加します。
        /// </summary>
        /// <param name="scene">シーンのインスタンス。</param>
        public void AddScene(Scene scene)
        {
            Scenes.Add(scene);
            scene.Enable();
        }

        /// <summary>
        /// シーンを削除します。
        /// </summary>
        /// <param name="scene">シーンのインスタンス。</param>
        public void RemoveScene(Scene scene)
        {
            if (Scenes.Contains(scene))
            {
                Scenes.Remove(scene);
                scene.Disable();
            }
        }

        /// <summary>
        /// シーンを指定の位置に追加します。
        /// </summary>
        /// <param name="scene">シーン。</param>
        /// <param name="index">位置。</param>
        public void InsertScene(Scene scene, int index = 0)
        {
            if (Scenes.Count <= index)
            {
                AddScene(scene);
            }
            else
            {
                Scenes.Insert(index, scene);
            }
            scene.Enable();
        }

        /// <summary>
        /// 描画します。
        /// </summary>
        public void Draw()
        {
            foreach (var item in Scenes)
            {
                item.Draw();
            }
        }

        /// <summary>
        /// 更新します。
        /// </summary>
        public void Update()
        {
            foreach (var item in Scenes)
            {
                item.Update();
            }
        }

        /// <summary>
        /// シーンたち。
        /// </summary>
        public List<Scene> Scenes { get; private set; }
    }
}
