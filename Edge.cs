using System;
using System.Collections.Generic;

namespace Triangulation
{
    [Serializable]
    public class Edge
    {
        public Vertex V1 { get; set; }
        public Vertex V2 { get; set; }

        public double Length
        {
            get
            {
                return Math.Round(Math.Sqrt((V2.X - V1.X) * (V2.X - V1.X) + (V2.Y - V1.Y) * (V2.Y - V1.Y)));
            }
        }

        public override string ToString()
        {
            return $"{(int)Math.Round(Length)}";
        }
    }

    [Serializable]
    public class Edges : List<Edge>
    { 
    }
}
