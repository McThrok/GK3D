using GK3D.Graphics.Common;
using GK3D.Graphics.Objects;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3D.Graphics.SceneComponents.Base
{
    public class SceneCollection
    {
        public Dictionary<string, int> Textures { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, Material> Materials { get; set; } = new Dictionary<string, Material>();
        public Dictionary<string, ShaderProgram> Shaders { get; set; } = new Dictionary<string, ShaderProgram>();
        public ComplexObject SceneObjects { get; set; } = new ComplexObject();

        public CollectionItem<Camera> ActiveCamera { get; set; }
        public string ActiveShader { get; set; }
    }
}
