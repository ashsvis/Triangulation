using System;
using System.Drawing;
using System.Security.Policy;

namespace Triangulation
{
    public class Vertex
    {
        public Vertex()
        {
        }

        public Vertex(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
     
        public int Y { get; set; }

        public Point Location
        {
            get 
            { 
                return new Point(X, Y); 
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
}
