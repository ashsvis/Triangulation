﻿using System;
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
        private readonly Random random = new Random();
        private readonly Vertices vertices = new Vertices();
        private readonly Edges edges = new Edges();
        private Point lastPoint = new Point();
        private int lastVertexIndex = -1;
        private bool buttonPressed = false;

        private bool contentChanged = false;
        public event EventHandler<EventArgs> ContentChanged;

        public int Side { get; set; } = 6;

        public ViewUC()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        }

        /// <summary>
        /// Построение рёбер
        /// </summary>
        private void BuildEdges()
        {
            var sortedVertices = new Vertices();
            // сортируем вершины по абсциссе
            foreach (var vertex in vertices.OrderBy(item => item.X))
                sortedVertices.Add(vertex);
            // собираем рёбра
            edges.Clear();
            for (var i = 0; i < sortedVertices.Count - 1; i++)
            {
                for (var j = i + 1; j < sortedVertices.Count; j++)
                {
                    var edge = new Edge() { V1 = sortedVertices[i], V2 = sortedVertices[j] };
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
            PaintContent(gr);
        }

        private void PaintContent(Graphics gr)
        {
            // рисуем рёбра
            foreach (var edge in edges)
                gr.DrawLine(Pens.Blue, (float)edge.V1.X, (float)edge.V1.Y, (float)edge.V2.X, (float)edge.V2.Y);
            var n = 1;
            foreach (var pt in vertices)
            {
                // рисуем номера вершин
                var text = n++.ToString();
                var size = gr.MeasureString(text, Font);
                var rect = new Rectangle(Point.Ceiling(pt.Location), Size.Ceiling(size));
                gr.DrawString(text, Font, Brushes.White, rect);
            }
            // рисуем вершины
            foreach (var vertex in vertices)
                DrawVertex(gr, vertex);
        }

        /// <summary>
        /// Прорисовка одной вершины
        /// </summary>
        /// <param name="graphics">контекст для рисования</param>
        /// <param name="pt">точка вершины</param>
        private void DrawVertex(Graphics graphics, Vertex vertex)
        {
            Rectangle rect = vertex.GetVertexRect(Side);
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
                var vertex = vertices.Find(v => v.GetVertexRect(Side).Contains(e.Location));
                lastPoint = vertex != null ? vertex.Location : Point.Empty;
                lastVertexIndex = lastPoint.IsEmpty ? -1 : vertices.FindIndex(v => v.Location.Equals(lastPoint));
                buttonPressed = e.Button == MouseButtons.Left;
                contentChanged = false;
            }
        }

        /// <summary>
        /// Метод обработки перемещения курсора мыши на поверхности компонента
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (vertices.Count > 0)
            {
                // над наденой вершиной курсор превращается в руку
                Cursor = vertices.Any(vertex => vertex.GetVertexRect(Side).Contains(e.Location)) ? Cursors.Hand : Cursors.Default;
                if (buttonPressed)
                {
                    // контроль границ для перемещения вершин
                    var rect = ClientRectangle;
                    rect.Inflate(-Side, -Side);
                    if (!lastPoint.IsEmpty && lastVertexIndex >= 0 && lastVertexIndex < vertices.Count && rect.Contains(e.Location))
                    {
                        // перемещаем одну вершину
                        vertices[lastVertexIndex].Location = e.Location;
                        BuildEdges();
                        contentChanged = true;
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
            if (contentChanged)
            {
                ContentUnsaved = true;
                ContentChanged?.Invoke(this, new EventArgs());
                contentChanged = false;
            }
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
        protected override void OnResize(EventArgs e)
        {
            if (ResizeContent())
                Invalidate();
        }

        public bool ResizeContent()
        {
            var newSize = Size;
            if (size.IsEmpty) return false;
            var kX = newSize.Width / (double)size.Width;
            var kY = newSize.Height / (double)size.Height;
            foreach (var vertex in vertices)
            {
                vertex.X *= kX;
                vertex.Y *= kY;
            }
            size = newSize;
            return true;
        }

        /// <summary>
        /// При показе контекстного меню прячем или показываем пункты меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            Cursor = Cursors.Default;
            var pointAboveVertex = vertices.Any(vertex => vertex.GetVertexRect(Side).Contains(PointToClient(MousePosition)));
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
            rect.Inflate(-Side, -Side);
            if (rect.Contains(point))
            {
                vertices.Add(new Vertex(point));
                BuildEdges();
                Invalidate();
                ContentUnsaved = true;
                ContentChanged?.Invoke(this, new EventArgs());
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
            var vertex = vertices.FirstOrDefault(v => v.GetVertexRect(Side).Contains(point));
            if (vertex != null)
            {
                // удаляем одну вершину
                vertices.Remove(vertex);
                BuildEdges();
                Invalidate();
                ContentUnsaved = true;
                ContentChanged?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Генерация случайно расположенных вершин
        /// </summary>
        /// <param name="count"></param>
        public void GenerateVertexList(int count)
        {
            vertices.Clear();
            foreach (var vertex in Enumerable.Range(1, count).Select(n =>
                     new Vertex(random.Next(Side, ClientSize.Width - Side), random.Next(Side, ClientSize.Height - Side))))
            {
                vertices.Add(vertex);
            }
            BuildEdges();
            ContentUnsaved = true;
            ContentChanged?.Invoke(this, new EventArgs());
        }

        public bool ContentUnsaved { get; private set; }

        public void Clear()
        {
            vertices.Clear();
            edges.Clear();
            lastPoint = Point.Empty;
            lastVertexIndex = -1;
            ContentUnsaved = false;
        }

        public void LoadVertices(string fileName)
        {
            Clear();
            try
            {
                try
                {
                    vertices.AddRange(SaverLoader.LoadVerticesFromFile(fileName, ClientSize));
                    ResizeContent();
                    BuildEdges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            finally
            {
                ContentUnsaved = false;
            }
        }

        public void SaveVertices(string fileName)
        {
            if (!ContentUnsaved) return;
            try
            {
                try
                {
                    SaverLoader.SaveVerticesToFile(fileName, vertices, ClientSize);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            finally
            {
                ContentUnsaved = false;
            }
        }
    }
}
