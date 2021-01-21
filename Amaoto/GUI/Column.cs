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
            var y = 0;
            y += padding;
            foreach (var item in Child)
            {
                item.X = 0;
                item.Y = y;
                y += spacing;
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
    }
}
