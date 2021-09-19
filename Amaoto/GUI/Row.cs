using System.Linq;

namespace Amaoto.GUI
{
    /// <summary>
    /// 横並びにGUIを配置する。
    /// </summary>
    public class Row : DrawPart
    {
        /// <summary>
        /// 横並びにGUIを配置する。
        /// </summary>
        /// <param name="children">子アイテム。</param>
        /// <param name="padding">パディング。</param>
        /// <param name="spacing">GUIとGUIの間。</param>
        public Row(DrawPart[] children, int padding = 0, int spacing = 0)
            : base(CalcWidth(children, padding, spacing), children.Max(gui => gui.Height) + (padding * 2))
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
            var x = 0;
            x += Padding;
            foreach (var item in Child)
            {
                item.X = x;
                item.Y = Padding;
                x += item.Width;
                x += Spacing;
            }
        }

        private static int CalcWidth(DrawPart[] children, int padding, int spacing)
        {
            var width = 0;
            width += padding;
            for (int i = 0; i < children.Length; i++)
            {
                width += children[i].Width;
                if (children[i] != children.Last())
                {
                    width += spacing;
                }
            }
            width += padding;
            return width;
        }

        private int Padding;
        private int Spacing;
    }
}
