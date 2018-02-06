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
                var cameraList = Collection.SceneObjects.GetCamerasWiThGlobalModelMatrices();
                var activeCam = cameraList.SingleOrDefault(x => x.Object == Collection.ActiveCamera.Object);
                if (activeCam != null)
                {
                    Collection.ActiveCamera = cameraList[(cameraList.IndexOf(activeCam) + 1) % cameraList.Count];
                }
            }
        }

        public override void ChangeLighting()
        {
            throw new NotImplementedException();
        }

        public override void ChangeShading()
        {
            if (Collection.ActiveShader == "phong")
                Collection.ActiveShader = "gouraud";
            else
                Collection.ActiveShader = "phong";
        }
    }
}
