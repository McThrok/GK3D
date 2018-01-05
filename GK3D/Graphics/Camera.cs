using GK3D.Graphics.Common;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3D.Graphics
{
    public class Camera : GameObject
    {
        public float MoveSpeed = 0.2f;
        public float MouseSensitivity = 0.01f;
        public Camera()
        {
            Rotation = new Vector3((float)Math.PI, 0f, 0f);
        }

        public Matrix4 GetViewMatrix()
        {
            Vector3 lookat = new Vector3();

            lookat.X = (float)(Math.Sin((float)Rotation.X) * Math.Cos((float)Rotation.Y));
            lookat.Y = (float)Math.Sin((float)Rotation.Y);
            lookat.Z = (float)(Math.Cos((float)Rotation.X) * Math.Cos((float)Rotation.Y));

            return Matrix4.LookAt(Position, Position + lookat, Vector3.UnitY);
        }
        public void Move(float x, float y, float z)
        {
            Vector3 offset = new Vector3();

            Vector3 forward = new Vector3((float)Math.Sin((float)Rotation.X), 0, (float)Math.Cos((float)Rotation.X));
            Vector3 right = new Vector3(-forward.Z, 0, forward.X);

            offset += x * right;
            offset += y * forward;
            offset.Y += z;

            offset.NormalizeFast();
            offset = Vector3.Multiply(offset, MoveSpeed);

            Position += offset;
        }
        public void AddRotation(float x, float y)
        {
            x *= MouseSensitivity;
            y *= MouseSensitivity;

            Rotation.X = (Rotation.X + x) % ((float)Math.PI * 2.0f);
            Rotation.Y = Math.Max(Math.Min(Rotation.Y + y, (float)Math.PI / 2.0f - 0.1f), (float)-Math.PI / 2.0f + 0.1f);
        }
    }
}
