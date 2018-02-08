using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using GK3D.Graphics.Common;

namespace GK3D.Graphics.Objects.Renderable
{
    public class Capsule2D : Primitive
    {
        public int Lod { get; set; }
        public Vector3 Color { get; set; }

        private Vector3[] _verticies;
        private int[] _indices;

        public override int IndiceCount { get => _indices.Length; }
        public override int VertCount { get => _verticies.Length; }
        public override int ColorDataCount { get => _verticies.Length; }
        public override int NormalCount { get => _verticies.Length; }

        public Capsule2D(float rectToRound, Vector3 color, int lod = 100)
        {
            Lod = Math.Max(lod, 1);
            Color = color;
            Init(rectToRound);
        }
        public void Init(float rectToRound)
        {
            float r = 1 / (rectToRound + 1);
            float l = 2 - 2 * r;

            //wheel up
            List<Vector3> verticiesWheelUp = new List<Vector3>();
            List<int> indicesWheelUp = new List<int>();

            var centerUp = new Vector3(0, l / 2, 0);
            var startVectorUp = new Vector4(r, 0, 0, 1);
            verticiesWheelUp.Add(centerUp);
            verticiesWheelUp.Add(centerUp + startVectorUp.Xyz);

            for (int i = 0; i < Lod; i++)
            {
                var rotation = MatrixHelper.CreateRotationZ(-(i + 1) * (float)Math.PI / Lod); // '-' !!
                var current = centerUp + (rotation * startVectorUp).Xyz;
                verticiesWheelUp.Add(current);
                indicesWheelUp.Add(0);
                indicesWheelUp.Add(verticiesWheelUp.Count - 2);
                indicesWheelUp.Add(verticiesWheelUp.Count - 1);
            }

            //wheel down
            List<Vector3> verticiesWheelDown = new List<Vector3>();
            List<int> indicesWheelDown = new List<int>();

            var centerDown = new Vector3(0, -l / 2, 0);
            var startVectorDown = new Vector4(-r, 0, 0, 1);
            verticiesWheelDown.Add(centerDown);
            verticiesWheelDown.Add(centerDown + startVectorDown.Xyz);

            for (int i = 0; i < Lod; i++)
            {
                var rotation = MatrixHelper.CreateRotationZ(-(i + 1) * (float)Math.PI / Lod); // '-' !!
                var current = centerDown + (rotation * startVectorDown).Xyz;
                verticiesWheelDown.Add(current);
                indicesWheelDown.Add(0);
                indicesWheelDown.Add(verticiesWheelDown.Count - 2);
                indicesWheelDown.Add(verticiesWheelDown.Count - 1);
            }

            //rect
            List<Vector3> verticiesRect = new List<Vector3>();
            List<int> indicesRect = new List<int>();

            var sqrtLod = (float)Math.Sqrt(Lod);
            var squareX = 2 * r / sqrtLod;
            var squareY = l / sqrtLod;

            for (int i = 0; i < sqrtLod; i++)
                for (int j = 0; j < sqrtLod; j++)
                {
                    verticiesRect.Add(new Vector3(squareX * (i + 1)-r, squareY*(j+1) - l / 2, 0));
                    verticiesRect.Add(new Vector3(squareX * i - r, squareY * (j + 1) - l / 2, 0));
                    verticiesRect.Add(new Vector3(squareX * i - r, squareY * j - l / 2, 0));
                    verticiesRect.Add(new Vector3(squareX * (i + 1) - r, squareY * j - l / 2, 0));

                    //verticiesRect.Add(new Vector3(r, l / 2, 0));
                    //verticiesRect.Add(new Vector3(-r, l / 2, 0));
                    //verticiesRect.Add(new Vector3(-r, -l / 2, 0));
                    //verticiesRect.Add(new Vector3(r, -l / 2, 0));

                    var offset = (i * (int)Math.Floor(sqrtLod) + j) * 4;

                    indicesRect.Add(offset + 0);
                    indicesRect.Add(offset + 1);
                    indicesRect.Add(offset + 2);
                    indicesRect.Add(offset + 2);
                    indicesRect.Add(offset + 3);
                    indicesRect.Add(offset + 0);
                }


            //add all
            List<Vector3> verticies = new List<Vector3>();
            verticies.AddRange(verticiesWheelUp);
            verticies.AddRange(verticiesWheelDown);
            verticies.AddRange(verticiesRect);
            _verticies = verticies.ToArray();

            List<int> indices = new List<int>();
            indices.AddRange(indicesWheelUp);
            indices.AddRange(indicesWheelDown.Select(x => x + verticiesWheelUp.Count));
            indices.AddRange(indicesRect.Select(x => x + verticiesWheelUp.Count + verticiesWheelDown.Count));
            _indices = indices.ToArray();
        }
        public override Vector3[] GetColorData()
        {
            return Enumerable.Repeat(Color, VertCount).ToArray();
        }

        public override int[] GetIndices(int offset = 0)
        {
            return _indices.Select(x => x + offset).ToArray();
        }

        public override Vector2[] GetTextureCoords()
        {
            return new Vector2[] { };
        }

        public override Vector3[] GetVerts()
        {
            return _verticies.ToArray();
        }
    }
}
