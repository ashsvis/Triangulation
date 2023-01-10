using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Triangulation
{
    [Serializable]
    public class Vertex
    {
        public Vertex()
        {
        }

        public Vertex(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vertex(Point point)
        {
            X = point.X;
            Y = point.Y;
        }

        public double X { get; set; }
     
        public double Y { get; set; }

        public Point Location
        {
            get 
            { 
                return new Point((int)X, (int)Y); 
            }
            set 
            { 
                X = value.X;
                Y = value.Y;
            }
        }

        public Rectangle GetVertexRect(int side)
        {
            var rect = new Rectangle(Location, new Size(side, side));
            rect.Offset(-rect.Width / 2, -rect.Height / 2);
            return rect;
        }
    }

    [Serializable]
    public class Vertices : List<Vertex>
    {
        /// <summary>
        /// Возвращаем вершины с нормализованными координатами
        /// </summary>
        /// <param name="clientSize"></param>
        /// <returns></returns>
        public Vertices NormalizeXY(Size clientSize) 
        {
            var vertices = new Vertices();
            foreach (var vertex in this)
                vertices.Add(new Vertex(vertex.X / clientSize.Width, vertex.Y / clientSize.Height));
            return vertices;
        }

        /// <summary>
        /// Растягиваем положение вершин из нормализованного списка в размер клиента
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="size"></param>
        public static Vertices UnNormalize(Vertices vertices, Size size)
        {
            var verts = new Vertices();
            foreach (var vertex in vertices)
                verts.Add(new Vertex(vertex.X * size.Width, vertex.Y * size.Height));
            return verts;
        }
    }
}
