using GK3D.Graphics.SceneComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3D.Graphics.SceneComponents.Base
{
    public abstract class SceneController
    {
        public SceneLoader Loader { get; set; }
        public SceneScenario Scenario { get; set; }
        public SceneCollection Collection { get; set; }


        public SceneController(SceneLoader loader = null, SceneScenario scenario = null)
        {
            Loader = loader;
            Scenario = scenario;
            Collection = Loader.Load();
        }

        public abstract void ChangeCamera();
        public abstract void ChangeShading();
        public abstract void ChangeLighting();
    }

}
