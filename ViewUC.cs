using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Triangulation
{
    public partial class ViewUC : UserControl
    {
        private readonly int side = 6;
        private readonly Random random = new Random();
        private readonly Vertexes vertexes = new Vertexes();
        private readonly Edges edges = new Edges();
        private Point lastPoint = new Point();
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

        /// <summary>
        /// Построение рёбер
        /// </summary>
        private void BuildEdges()
        {
            var sortedVertexes = new Vertexes();
            // сортируем вершины по абсциссе
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

        /// <summary>
        /// Метод прорисовки поверхности компонента
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            var gr = e.Graphics;
            gr.SmoothingMode = SmoothingMode.HighQuality;
            // рисуем рёбра
            foreach (var edge in edges)
                gr.DrawLine(Pens.Blue, (float)edge.V1.X, (float)edge.V1.Y, (float)edge.V2.X, (float)edge.V2.Y);
            var n = 1;
            foreach (var pt in vertexes)
            {
                // рисуем номера вершин
                var text = n++.ToString();
                var size = gr.MeasureString(text, Font);
                var rect = new Rectangle(Point.Ceiling(pt.Location), Size.Ceiling(size));
                using (var brush = new SolidBrush(Color.FromArgb(240, BackColor)))
                {
                    gr.FillRectangle(brush, rect);
                }
                gr.DrawString(text, Font, SystemBrushes.WindowText, rect);
            }
            // рисуем вершины
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

        /// <summary>
        /// Метод обработки нажатия мыши на поверхности компонента
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                // ищем вершину с координатами рядом с точкой нажатия
                var vertex = vertexes.Find(v => v.GetVertexRect(side).Contains(e.Location));
                lastPoint = vertex != null ? vertex.Location : Point.Empty;
                lastVertexIndex = lastPoint.IsEmpty ? -1 : vertexes.FindIndex(v => v.Location.Equals(lastPoint));
                buttonPressed = e.Button == MouseButtons.Left;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (vertexes.Count > 0)
            {
                Cursor = vertexes.Any(vertex => vertex.GetVertexRect(side).Contains(e.Location)) ? Cursors.Hand : Cursors.Default;
                if (buttonPressed)
                {
                    // контроль границ для перемещения вершин
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

        /// <summary>
        /// Метод обработки отпускания кнопи мыши
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            buttonPressed = false;
        }

        private Size size;

        private void ViewUC_Load(object sender, EventArgs e)
        {
            size = Size;
        }

        /// <summary>
        /// Метод обработки изменения размеров компонента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewUC_Resize(object sender, EventArgs e)
        {
            var newSize = Size;
            if (size.IsEmpty) return;
            var kX = newSize.Width / (double)size.Width;
            var kY = newSize.Height / (double)size.Height;
            foreach (var vertex in vertexes)
            {
                vertex.X *= kX;
                vertex.Y *= kY;
            }
            size = newSize;
            Invalidate();
        }

        /// <summary>
        /// При показе контекстного меню прячем или показываем пункты меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            Cursor = Cursors.Default;
            var pointAboveVertex = vertexes.Any(vertex => vertex.GetVertexRect(side).Contains(PointToClient(MousePosition)));
            // пункт удаления вершины будет показан только над вершиной
            tsmiRemoveVertex.Visible = pointAboveVertex;
            // пункт добавления вершины будет показан вне вершины
            tsmiAddVertex.Visible = !pointAboveVertex;
        }

        /// <summary>
        /// Добавление новой вершины в список вершин
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiAddVertex_Click(object sender, EventArgs e)
        {
            var point = PointToClient(contextMenu.Bounds.Location);
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

        /// <summary>
        /// Удаление выбранной вершины
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiRemoveVertex_Click(object sender, EventArgs e)
        {
            var point = PointToClient(contextMenu.Bounds.Location);
            var vertex = vertexes.FirstOrDefault(v => v.GetVertexRect(side).Contains(point));
            if (vertex != null)
            {
                // удаляем одну вершину
                vertexes.Remove(vertex);
                vertexCount--;
                BuildEdges();
                Invalidate();
            }

        }

    }
}
