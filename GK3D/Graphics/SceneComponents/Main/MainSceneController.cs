using GK3D.Graphics.SceneComponents.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3D.Graphics.SceneComponents.Main
{
    public class MainSceneController : SceneController
    {
        public MainSceneController(SceneLoader loader = null, SceneScenario scenario = null) : base(loader, scenario)
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

        public override void ChangeShading()
        {
            switch (Collection.ActiveShader)
            {
                case "phong_phong":
                    Collection.ActiveShader = "gouraud_phong";
                    break;

                case "gouraud_phong":
                    Collection.ActiveShader = "phong_phong";
                    break;

                case "phong_blinn":
                    Collection.ActiveShader = "gouraud_blinn";
                    break;

                case "gouraud_blinn":
                    Collection.ActiveShader = "phong_blinn";
                    break;
            }
        }

        public override void ChangeLighting()
        {


            switch (Collection.ActiveShader)
            {
                case "phong_phong":
                    Collection.ActiveShader = "phong_blinn";
                    break;

                case "gouraud_phong":
                    Collection.ActiveShader = "gouraud_blinn";
                    break;

                case "phong_blinn":
                    Collection.ActiveShader = "gouraud_phong";
                    break;

                case "gouraud_blinn":
                    Collection.ActiveShader = "gouraud_phong";
                    break;
            }

        }
    }
}
