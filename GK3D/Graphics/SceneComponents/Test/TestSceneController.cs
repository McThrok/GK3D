using GK3D.Graphics.SceneComponents.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3D.Graphics.SceneComponents.Test
{
    public class TestSceneController : SceneController
    {
        public TestSceneController(SceneLoader loader = null, SceneScenario scenario = null) : base(loader, scenario)
        { }

        public override void ChangeCamera()
        {
            if (Collection.ActiveCamera != null)
            {
                var activeCam = Collection.SceneObjects.Cameras.SingleOrDefault(x => x == Collection.ActiveCamera);
                if (activeCam != null)
                {
                    var cameraList = Collection.SceneObjects.Cameras.ToList();
                    Collection.ActiveCamera = cameraList[(cameraList.IndexOf(activeCam) + 1) % cameraList.Count];
                }
            }
        }
    }
}
