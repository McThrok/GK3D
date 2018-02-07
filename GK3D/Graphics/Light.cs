using GK3D.Graphics.Common;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3D.Graphics
{

    public enum LightType { Point, Spot, Directional }

    public class Light : GameObject
    {
        public Light(Vector3 position, Vector3 color, float diffuseintensity = 1.0f, float ambientintensity = 1.0f)
        {
            Position = position;
            Color = color;

            DiffuseIntensity = diffuseintensity;
            AmbientIntensity = ambientintensity;

            Type = LightType.Point;
            Rotation = new Vector3(0, 0, 1);
            ConeAngle = 15.0f;
            ConeExponent = 16;
        }

        public Vector3 Color;
        public float DiffuseIntensity;
        public float AmbientIntensity;

        public LightType Type;
        public float ConeAngle;
        public float ConeExponent;
    }
}
