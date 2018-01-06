using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3D.Graphics.Objects
{
    public class ColoredCube : Cube
    {
        private Vector3 _color;
        public ColoredCube(Vector3 color): base()
        {
            _color = color;
        }
       
        public override Vector3[] GetColorData()
        {
            return Enumerable.Repeat(_color, ColorDataCount).ToArray();
        }
       
    }
}
