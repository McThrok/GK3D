using GK3D.Graphics.Common;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK3D.Graphics.SceneComponents;

namespace GK3D.Graphics
{
    public class Camera : GameObject
    {
        public float MoveSpeed { get; set; } = 0.2f;
        public float MouseSensitivity { get; set; } = 0.003f;

        public Matrix4 GetViewMatrix(Matrix4 globalModelMatrix)
        {
            Vector4 lookat = new Vector4(Vector3.UnitZ, 1);
            Vector4 up = new Vector4(Vector3.UnitY, 1);

            lookat = lookat.ApplyOnVector(globalModelMatrix);

            up = up.ApplyOnVector(globalModelMatrix);

            var position = Vector3.Zero.ApplyOnPoint(globalModelMatrix);
            return MatrixHelper.LookAt(position, position + lookat.Xyz, up.Xyz);
        }

        public void Move(float x, float y, float z)
        {
            Vector4 offset4 = new Vector4(x, y, z, 1);
            offset4 = MatrixHelper.CreateRotationX(Rotation.X) * MatrixHelper.CreateRotationY(Rotation.Y) * MatrixHelper.CreateRotationZ(Rotation.Z) * offset4;

            var offset = offset4.Xyz;
            offset.NormalizeFast();
            offset = Vector3.Multiply(offset, MoveSpeed);

            Position += offset;
        }
        public void MoveWithSeparatedY(float x, float y, float z)
        {
            var direction = MatrixHelper.CreateRotationY(-Rotation.Y) * new Vector4(x, 0, z, 1);

            var offset = new Vector3(direction.X, y, direction.Z);
            offset.NormalizeFast();
            offset = Vector3.Multiply(offset, MoveSpeed);

            Position += offset;
        }
        public void AddRotation(float x, float y)
        {
            x *= -MouseSensitivity;
            y *= -MouseSensitivity;
            Rotation.Y = (Rotation.Y + x) % ((float)Math.PI * 2.0f);
            Rotation.X = Math.Max(Math.Min(Rotation.X - y, (float)Math.PI / 2.0f - 0.1f), (float)-Math.PI / 2.0f + 0.1f);

        }
    }
}
