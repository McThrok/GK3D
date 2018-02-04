using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3D.Graphics
{
    public class MatrixHelper
    {
        public static Matrix4 CreateScale(Vector3 scale)
        {
            var matrix = new Matrix4
            {
                M11 = scale.X,
                M22 = scale.Y,
                M33 = scale.Z,
                M44 = 1
            };
            return matrix;
        }
        public static Matrix4 CreateRotationX(float angle)
        {
            var matrix = new Matrix4
            {
                M11 = 1,
                M22 = (float)Math.Cos(angle),
                M23 = (float)Math.Sin(angle),
                M32 = -(float)Math.Sin(angle),
                M33 = (float)Math.Cos(angle),
                M44 = 1
            };
            return matrix;
        }
        public static Matrix4 CreateRotationY(float angle)
        {
            var matrix = new Matrix4
            {
                M11 = (float)Math.Cos(angle),
                M13 = -(float)Math.Sin(angle),
                M22 = 1,
                M31 = (float)Math.Sin(angle),
                M33 = (float)Math.Cos(angle),
                M44 = 1
            };
            return matrix;
        }
        public static Matrix4 CreateRotationZ(float angle)
        {
            var matrix = new Matrix4
            {
                M11 = (float)Math.Cos(angle),
                M12 = (float)Math.Sin(angle),
                M21 = -(float)Math.Sin(angle),
                M22 = (float)Math.Cos(angle),
                M33 = 1,
                M44 = 1
            };
            return matrix;
        }
        public static Matrix4 CreateTranslation(Vector3 vector)
        {
            var matrix = new Matrix4
            {
                M11 = 1,
                M22 = 1,
                M33 = 1,
                Row3 = new Vector4(vector, 1),
            };
            return matrix;
        }
        public static Matrix4 LookAt(Vector3 eye, Vector3 target, Vector3 up)
        {
            Vector3 z = Vector3.Normalize(eye - target);
            Vector3 x = Vector3.Normalize(Vector3.Cross(up, z));
            Vector3 y = Vector3.Normalize(Vector3.Cross(z, x));

            Matrix4 rot = new Matrix4(new Vector4(x.X, y.X, z.X, 0.0f),
                                        new Vector4(x.Y, y.Y, z.Y, 0.0f),
                                        new Vector4(x.Z, y.Z, z.Z, 0.0f),
                                        Vector4.UnitW);

            Matrix4 trans = Matrix4.CreateTranslation(-eye);

            var matrix =  trans * rot;

            return matrix;
        }
        public static Matrix4 CreatePerspectiveFieldOfView(float fovy, float aspect, float zNear, float zFar)
        {
            float yMax = zNear * (float)Math.Tan(0.5f * fovy);
            float yMin = -yMax;
            float xMin = yMin * aspect;
            float xMax = yMax * aspect;

            float x = (2.0f * zNear) / (xMax - xMin);
            float y = (2.0f * zNear) / (yMax - yMin);
            float a = (xMax + xMin) / (xMax - xMin);
            float b = (yMax + yMin) / (yMax - yMin);
            float c = -(zFar + zNear) / (zFar - zNear);
            float d = -(2.0f * zFar * zNear) / (zFar - zNear);

            var matrix = new Matrix4(x, 0, 0, 0,
                                 0, y, 0, 0,
                                 a, b, c, -1,
                                 0, 0, d, 0);
            return matrix;
        }
    }
}
