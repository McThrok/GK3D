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
            return Matrix4.CreateScale(scale);
        }
        public static Matrix4 CreateRotationX(float angle)
        {
            return Matrix4.CreateRotationX(angle);
        }
        public static Matrix4 CreateRotationY(float angle)
        {
            return Matrix4.CreateRotationY(angle);
        }
        public static Matrix4 CreateRotationZ(float angle)
        {
            return Matrix4.CreateRotationZ(angle);
        }
        public static Matrix4 CreateTranslation(Vector3 vector)
        {
            return Matrix4.CreateTranslation(vector);
        }
        public static Matrix4 LookAt(Vector3 eye, Vector3 target, Vector3 up)
        {
            return Matrix4.LookAt(eye, target, up);
        }
        public static Matrix4 CreatePerspectiveFieldOfView(float fovy, float aspect, float zNear, float zFar)
        {
            return Matrix4.CreatePerspectiveFieldOfView(fovy, aspect, zNear, zFar);
        }
    }
}
