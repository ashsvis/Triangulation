using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Triangulation
{
    public partial class ViewUC : UserControl
    {
        private readonly int side = 6;
        private readonly Random random = new Random();
        private readonly List<Vertex> vertexes = new List<Vertex>();
        private readonly List<Vertex> sortedVertexes = new List<Vertex>();
        private readonly List<Edge> edges = new List<Edge>();
        private Point lastPoint = new Point();
        private Point lastPressed = new Point();
        private int lastVertexIndex = -1;
        private bool buttonPressed = false;
        private int vertexCount;

        public int VertexCount 
        { 
            get => vertexCount;
            set
            {
                vertexCount = value;
                GenerateVertexList(vertexCount);
            }
        }

        public ViewUC()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);

            GenerateVertexList(vertexCount);
        }

        private void GenerateVertexList(int count = 0)
        {
            vertexes.Clear();
            foreach (var vertex in Enumerable.Range(1, count).Select(n =>
                     new Vertex(random.Next(side, ClientSize.Width - side), random.Next(side, ClientSize.Height - side))))
            {
                vertexes.Add(vertex);
            }
            BuildEdges();
        }

        private void BuildEdges()
        {
            sortedVertexes.Clear();
            foreach (var vertex in vertexes.OrderBy(item => item.X))
                sortedVertexes.Add(vertex);
            // собираем рёбра
            edges.Clear();
            for (var i = 0; i < sortedVertexes.Count - 1; i++)
            {
                for (var j = i + 1; j < sortedVertexes.Count; j++)
                {
                    var edge = new Edge() { V1 = sortedVertexes[i], V2 = sortedVertexes[j] };
                    edges.Add(edge);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var gr = e.Graphics;
            gr.SmoothingMode = SmoothingMode.HighQuality;

            foreach (var edge in edges)
                gr.DrawLine(Pens.Blue, edge.V1.X, edge.V1.Y, edge.V2.X, edge.V2.Y);
            var n = 1;
            foreach (var pt in vertexes)
            {
                var text = n++.ToString();
                var size = gr.MeasureString(text, Font);
                var rect = new Rectangle(pt.Location, Size.Ceiling(size));
                using (var brush = new SolidBrush(Color.FromArgb(240, BackColor)))
                {
                    gr.FillRectangle(brush, rect);
                }
                gr.DrawString(text, Font, SystemBrushes.WindowText, rect);
            }
            foreach (var vertex in vertexes)
                DrawVertex(e.Graphics, vertex);

        }

        /// <summary>
        /// Прорисовка одной вершины
        /// </summary>
        /// <param name="graphics">контекст для рисования</param>
        /// <param name="pt">точка вершины</param>
        private void DrawVertex(Graphics graphics, Vertex vertex)
        {
            Rectangle rect = vertex.GetVertexRect(side);
            graphics.FillEllipse(SystemBrushes.Window, rect);
            graphics.DrawEllipse(SystemPens.WindowText, rect);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                var vertex = vertexes.Find(v => v.GetVertexRect(side).Contains(e.Location));
                lastPoint = vertex != null ? vertex.Location : Point.Empty;
                lastVertexIndex = lastPoint.IsEmpty ? -1 : vertexes.FindIndex(v => v.Location.Equals(lastPoint));
                lastPressed = e.Location;
                buttonPressed = e.Button == MouseButtons.Left;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (vertexes.Count > 2)
            {
                Cursor = vertexes.Any(vertex => vertex.GetVertexRect(side).Contains(e.Location)) ? Cursors.Hand : Cursors.Default;
                if (buttonPressed)
                {
                    var rect = ClientRectangle;
                    rect.Inflate(-side, -side);
                    if (!lastPoint.IsEmpty && lastVertexIndex >= 0 && lastVertexIndex < vertexes.Count && rect.Contains(e.Location))
                    {
                        // перемещаем одну вершину
                        vertexes[lastVertexIndex].Location = e.Location;
                        BuildEdges();
                        Invalidate();
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            buttonPressed = false;
        }

        private void ViewUC_Resize(object sender, EventArgs e)
        {
            GenerateVertexList(vertexCount);
            Invalidate();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            Cursor = Cursors.Default;
            tsmiRemoveVertex.Visible = vertexes.Any(vertex => vertex.GetVertexRect(side).Contains(PointToClient(MousePosition)));
            tsmiAddVertex.Visible = !vertexes.Any(vertex => vertex.GetVertexRect(side).Contains(PointToClient(MousePosition)));
        }

        private void tsmiAddVertex_Click(object sender, EventArgs e)
        {
            var point = PointToClient(contextMenuStrip1.Bounds.Location);
            var rect = ClientRectangle;
            rect.Inflate(-side, -side);
            if (rect.Contains(point))
            {
                vertexes.Add(new Vertex(point));
                vertexCount++;
                BuildEdges();
                Invalidate();
            }
        }

        private void tsmiRemoveVertex_Click(object sender, EventArgs e)
        {

        }
    }
}
