using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Estructuras_de_Datos_Arboles
{
    public class TreeRenderer
    {
        private const int NodeRadius = 18;
        private const int LevelOffsetY = 60;
        private readonly Font _nodeFont;
        private static readonly Pen   PenEdge        = new Pen(Color.FromArgb(60, 60, 60), 1.5f);
        private static readonly Pen   PenNode        = new Pen(Color.FromArgb(40, 40, 40), 1.8f);
        private static readonly Pen   PenHighlight   = new Pen(Color.FromArgb(30, 100, 200), 2.5f);
        private static readonly Brush BrushNormal    = new SolidBrush(Color.White);
        private static readonly Brush BrushHighlight = new SolidBrush(Color.FromArgb(210, 235, 255));
        private static readonly Brush BrushText      = new SolidBrush(Color.FromArgb(20, 20, 20));
        private static readonly Brush BrushEmpty     = new SolidBrush(Color.FromArgb(120, 120, 120));
        public TreeRenderer(Font? font = null)
        {
            _nodeFont = font ?? new Font("Arial", 8f);
        }
        public void Draw(Graphics g, Node? root, int? highlightedId, Rectangle canvasBounds)
        {
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            if (root == null)
            {
                DrawEmptyMessage(g, canvasBounds);
                return;
            }
            int initialOffsetX = Math.Min(canvasBounds.Width / 4, 230);
            int rootX          = canvasBounds.Width / 2;
            int rootY          = 35;

            DrawSubtree(g, root, rootX, rootY, initialOffsetX, highlightedId);
        }
        private void DrawEmptyMessage(Graphics g, Rectangle bounds)
        {
            string msg  = "Arbol vacio.";
            SizeF  size = g.MeasureString(msg, _nodeFont);
            float  x    = (bounds.Width  - size.Width)  / 2f;
            float  y    = (bounds.Height - size.Height) / 2f;
            g.DrawString(msg, _nodeFont, BrushEmpty, x, y);
        }
        private void DrawSubtree(Graphics g, Node current, int x, int y, int offsetX, int? highlightedId)
        {
            int nextOffsetX = Math.Max(offsetX / 2, 25);
            int childY      = y + LevelOffsetY;
            if (current.Left != null)
            {
                int childX = x - offsetX;
                g.DrawLine(PenEdge, x, y, childX, childY);
                DrawSubtree(g, current.Left, childX, childY, nextOffsetX, highlightedId);
            }
            if (current.Right != null)
            {
                int childX = x + offsetX;
                g.DrawLine(PenEdge, x, y, childX, childY);
                DrawSubtree(g, current.Right, childX, childY, nextOffsetX, highlightedId);
            }
            DrawNode(g, current, x, y, highlightedId);
        }
        private void DrawNode(Graphics g, Node node, int x, int y, int? highlightedId)
        {
            bool isHighlighted = highlightedId.HasValue && highlightedId.Value == node.Id;

            Rectangle rect = new Rectangle(x - NodeRadius, y - NodeRadius, NodeRadius * 2, NodeRadius * 2);

            Brush fill = isHighlighted ? BrushHighlight : BrushNormal;
            Pen   pen  = isHighlighted ? PenHighlight   : PenNode;
            g.FillEllipse(fill, rect);
            g.DrawEllipse(pen,  rect);
            string texto = node.Id.ToString();
            SizeF  size  = g.MeasureString(texto, _nodeFont);
            g.DrawString(texto, _nodeFont, BrushText,
                         x - size.Width  / 2f,
                         y - size.Height / 2f);
        }
    }
}
