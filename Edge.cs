using System;

namespace Triangulation
{
    public class Edge
    {
        public Vertex V1 { get; set; }
        public Vertex V2 { get; set; }

        public int Length
        {
            get
            {
                return (int)Math.Round(Math.Sqrt((V2.X - V1.X) * (V2.X - V1.X) + (V2.Y - V1.Y) * (V2.Y - V1.Y)));
            }
        }

        public override string ToString()
        {
            return $"{Length}";
        }
    }
}
