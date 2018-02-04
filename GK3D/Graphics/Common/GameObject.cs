using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3D.Graphics.Common
{
    public abstract class GameObject
    {
        public string Name { get; set; }
        public Vector3 Position = Vector3.Zero;
        public Vector3 Rotation = Vector3.Zero;
        public Vector3 Scale = Vector3.One;

        public virtual Matrix4 CalculateModelMatrix()
        {
            return MatrixHelper.CreateScale(Scale) * MatrixHelper.CreateRotationX(Rotation.X) * MatrixHelper.CreateRotationY(Rotation.Y) * MatrixHelper.CreateRotationZ(Rotation.Z) * MatrixHelper.CreateTranslation(Position);
        }
    }
}
