using GK3D.Graphics.Objects;
using GK3D.Graphics.Objects.Renderable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3D.Graphics.SceneComponents.Base
{
    public abstract class SceneScenario
    {
        public abstract void Process(ComplexObject sceneObjects, float deltaTime);
    }
   
}
