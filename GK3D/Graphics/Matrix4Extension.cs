using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3D.Graphics
{
    public static class MyMatrix4
    {
        public static Matrix4 CreateRotation(Vector3 rotation)
        {
            return Matrix4.CreateRotationX(rotation.X) * Matrix4.CreateRotationY(rotation.Y) * Matrix4.CreateRotationZ(rotation.Z);
        }
        public static Vector4 GetVector4(Vector3 vector, float w = 1)
        {
            return new Vector4(vector, w);
        }
    }
}
