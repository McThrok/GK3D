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
                item.Position = position;
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
            var matrix = Matrix4.CreateTranslation(_position) * Matrix4.CreateRotationX(rotation.X) * Matrix4.CreateRotationY(rotation.Y) * Matrix4.CreateRotationZ(rotation.Z) * Matrix4.CreateTranslation(_position).Inverted();
            foreach (var item in GetObjects())
            {
                item.Position += matrix.ExtractTranslation();
                item.Rotation += matrix.ExtractRotation().Xyz;
            }
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
                item.Position += (item.Position - _position) * scale;
            }
        }
        public void Scale(Vector3 scale)
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
