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
        public SceneLoader Loader { get; set; }
        public SceneScenario Scenario { get; set; }
        public SceneCollection Collection { get; set; }


        public abstract void ChangeCamera();

        public SceneController(SceneLoader loader = null, SceneScenario scenario = null)
        {
            Loader = loader;
            Scenario = scenario;
            Collection = Loader.Load();
        }
    }
    public class TestSceneController : SceneController
    {
        public TestSceneController(SceneLoader loader = null, SceneScenario scenario = null) : base(loader, scenario)
        { }

        public override void ChangeCamera()
        {
            if (Collection.ActiveCamera != null)
            {
                var activeCam = Collection.Cameras.SingleOrDefault(x => x.Value == Collection.ActiveCamera);
                if (activeCam.Value != null)
                {
                    var cameraList = Collection.Cameras.ToList();
                    Collection.ActiveCamera = cameraList[(cameraList.IndexOf(activeCam) + 1) % cameraList.Count].Value;
                }
            }
        }
    }

}
