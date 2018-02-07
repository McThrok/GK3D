using GK3D.Graphics.SceneComponents;
using OpenTK.Input;
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
        public SceneCollection Collection { get; set; }
        public SceneScenario SceneScenario { get; set; }

        public SceneController(SceneLoader loader, SceneScenario scenario)
        {
            Loader = loader;
            SceneScenario = scenario;
            Collection = Loader.Load();
        }

        public abstract void ChangeCamera();
        public abstract void ChangeShading();
        public abstract void ChangeLighting();

        public abstract void HandleInput(KeyboardState keyboardState, MouseState mouseState);
        public abstract void HandleFocusChange(bool focused);
    }

}
