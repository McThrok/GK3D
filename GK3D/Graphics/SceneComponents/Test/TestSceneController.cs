using GK3D.Graphics.SceneComponents.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace GK3D.Graphics.SceneComponents.Test
{
    public class TestSceneController : SceneController
    {
        public TestSceneController(SceneLoader loader = null) : base(loader)
        { }

        public override void ChangeCamera()
        {
            throw new NotImplementedException();
        }

        public override void ChangeLighting()
        {
            throw new NotImplementedException();
        }

        public override void ChangeShading()
        {
            throw new NotImplementedException();
        }

        public override void HandleFocusChange(bool focused)
        {
            throw new NotImplementedException();
        }

        public override void HandleInput(KeyboardState keybordState, MouseState mouseState)
        {
            throw new NotImplementedException();
        }
    }
}
