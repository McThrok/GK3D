using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3D.Graphics.Common
{
    public struct FaceVertexInd
    {
        public int Vertex;
        public int Normal;
        public int Texcoord;

        public FaceVertexInd(int vert = 0, int norm = 0, int tex = 0)
        {
            Vertex = vert;
            Normal = norm;
            Texcoord = tex;
        }
    }
}
