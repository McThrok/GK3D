using GK3D.Graphics.Common;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3D.Graphics.Objects
{
    public class ComplexObject
    {
        public Vector3 _position { get; set; }
        private Vector3 _scale { get; set; } = new Vector3(1, 1, 1);
        private Vector3 _rotation { get; set; }

        public Dictionary<string, Light> Lights { get; private set; } = new Dictionary<string, Light>();
        public Dictionary<string, Volume> Objects { get; private set; } = new Dictionary<string, Volume>();
        public Dictionary<string, Camera> Cameras { get; private set; } = new Dictionary<string, Camera>();

        public Vector3 GetPosition()
        {
            return _position;
        }
        public void SetPosition(Vector3 position)
        {
            foreach (var item in GetObjects())
                item.Position = position + (item.Position - _position);
        }
        public void Move(Vector3 offset)
        {
            SetPosition(_position + offset);
        }

        public Vector3 GetRotation()
        {
            return _rotation;
        }
        public void SetRotation(Vector3 rotation)
        {
            Rotate(-_rotation);
            Rotate(rotation);
        }
        public void Rotate(Vector3 rotation)
        {
            var a1 = new Matrix4();
            a1[0, 0] = 1;
            a1[1, 0] = 1;
            var qwe = a1 * Vector4.One;

            var m1 =  Matrix4.CreateRotationX((float)Math.PI / 2) * Matrix4.CreateRotationY((float)Math.PI / 2);
            //var m1 = Matrix4.CreateRotationY((float)Math.PI / 2);
            var m2 = Matrix4.CreateRotationY((float)Math.PI / 2) * Matrix4.CreateRotationX((float)Math.PI / 2);

            var v = new Vector4(1, 0, 0, 1);
            var a = m1 * v;
            var a2 =  v*m1;
            var b = m2 * v;
            var b2 = v * m2;


            var mat =  Matrix4.CreateRotationY(_rotation.Y)*Matrix4.CreateRotationZ(_rotation.Z)* Matrix4.CreateRotationX(_rotation.X);
            var aa = mat * new Vector4(4, 0, 0, 1);
            var rot = mat * new Vector4(rotation, 1);
            var rotationMatrix = Matrix4.CreateRotationX(rot.X) * Matrix4.CreateRotationY(rot.Y) * Matrix4.CreateRotationZ(rot.Z); ;
            foreach (var item in GetObjects())
            {
                var a11 = new Vector4(item.Position - _position, 1);
                var rotated = rotationMatrix * new Vector4(item.Position - _position, 1);
                item.Position = _position + rotated.Xyz;
                item.Rotation += -rotation;
            }
            _rotation += rotation;
        }

        public Vector3 GetScale()
        {
            return _scale;
        }
        public void SetScale(Vector3 scale)
        {
            foreach (var item in GetObjects())
            {
                item.Scale = scale;
                item.Position = _position + (item.Position - _position) * scale;
            }
        }
        public void MultiplyScale(Vector3 scale)
        {
            SetScale(_scale * scale);
        }

        private IEnumerable<GameObject> GetObjects()
        {
            var objects = new List<GameObject>(Objects.Values);
            objects.AddRange(Cameras.Values);
            objects.AddRange(Lights.Values);
            return objects;
        }
    }
}
