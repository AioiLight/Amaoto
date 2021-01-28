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
            var x = 0;
            x += padding;
            foreach (var item in Child)
            {
                item.X = x;
                item.Y = 0;
                x += item.Width;
                x += spacing;
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
    }
}
