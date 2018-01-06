using GK3D.Graphics.Objects;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3D.Graphics.SceneComponents
{
    public class SceneCollection
    {
        public Dictionary<string, int> Textures { get; private set; } = new Dictionary<string, int>();
        public Dictionary<string, Material> Materials { get; private set; } = new Dictionary<string, Material>();
        public Dictionary<string, ShaderProgram> Shaders { get; private set; } = new Dictionary<string, ShaderProgram>();
        public Dictionary<string, Light> Lights { get; private set; } = new Dictionary<string, Light>();
        public Dictionary<string, Volume> Objects { get; private set; } = new Dictionary<string, Volume>();
        public Dictionary<string, Camera> Cameras { get; private set; } = new Dictionary<string, Camera>();
        public Dictionary<string, ComplexObject> ComplexObjects { get; private set; } = new Dictionary<string, ComplexObject>();
    }
}
