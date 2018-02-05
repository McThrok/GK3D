using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3D.Graphics
{
    public static class VectorExtension
    {
        public static Vector4 ApplyOnPoint(this Vector4 vector, Matrix4 matrix)
        {
            matrix.Transpose();

            var result = matrix * vector;
            return result;
        }
        public static Vector4 ApplyOnVector(this Vector4 vector, Matrix4 matrix)
        {
            matrix.Transpose();
            matrix.Column3 = new Vector4(Vector3.Zero, 1);

            var result = matrix * vector;
            return result;
        }
        public static Vector3 ApplyOnVector(this Vector3 vector, Matrix4 matrix)
        {
            return (new Vector4(vector, 1).ApplyOnVector(matrix)).Xyz;
        }
        public static Vector3 ApplyOnPoint(this Vector3 vector, Matrix4 matrix)
        {
            return (new Vector4(vector, 1).ApplyOnPoint(matrix)).Xyz;
        }

    }
}
