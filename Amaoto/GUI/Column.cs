using System.Linq;

namespace Amaoto.GUI
{
    /// <summary>
    /// 縦並びにGUIを配置する。
    /// </summary>
    public class Column : DrawPart
    {
        /// <summary>
        /// 縦並びにGUIを配置する。
        /// </summary>
        /// <param name="children">子アイテム。</param>
        /// <param name="padding">パディング。</param>
        /// <param name="spacing">GUIとGUIの間。</param>
        public Column(DrawPart[] children, int padding = 0, int spacing = 0)
            : base(children.Max(gui => gui.Width) + (padding * 2), CalcHeight(children, padding, spacing))
        {
            Child = children.ToList();
            Padding = padding;
            Spacing = spacing;
        }

        public override void Build()
        {
            SetPosition();
            base.Build();
        }

        /// <summary>
        /// パディング、間隔を変更する。
        /// </summary>
        /// <param name="padding">パディング。nullだと前の設定を引き継ぐ。</param>
        /// <param name="spacing">GUIとGUIの間。nullだと前の設定を引き継ぐ。</param>
        public void ChangePadding(int? padding = null, int? spacing = null)
        {
            Padding = padding ?? Padding;
            Spacing = spacing ?? Spacing;
            // 再生成。
            ShouldBuild = true;
        }

        private void SetPosition()
        {
            var y = 0;
            y += Padding;
            foreach (var item in Child)
            {
                item.X = Padding;
                item.Y = y;
                y += item.Height;
                y += Spacing;
            }
        }

        private static int CalcHeight(DrawPart[] children, int padding, int spacing)
        {
            var height = 0;
            height += padding;
            for (var i = 0; i < children.Length; i++)
            {
                height += children[i].Height;
                if (children[i] != children.Last())
                {
                    height += spacing;
                }
            }
            height += padding;
            return height;
        }

        private int Padding;
        private int Spacing;
    }
}
