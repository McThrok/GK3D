using GK3D.Graphics.SceneComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3D.Graphics
{
    public abstract class SceneController
    {
        public abstract void ChangeCamera();
    }
    public class TestSceneController : SceneController
    {
        private TestScene _scene;
        public TestSceneController(TestScene scene)
        {
            _scene = scene;
        }
        public override void ChangeCamera()
        {
            if (_scene.ActiveCamera != null)
            {
                var activeCam = _scene.Collection.Cameras.SingleOrDefault(x => x.Value == _scene.ActiveCamera);
                if (activeCam.Value != null)
                {
                    var cameraList = _scene.Collection.Cameras.ToList();
                    _scene.ActiveCamera = cameraList[(cameraList.IndexOf(activeCam) + 1) % cameraList.Count].Value;
                }
            }
        }
    }

}
