using GK3D.Graphics.SceneComponents.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK3D.Graphics.Objects;
using OpenTK;

namespace GK3D.Graphics.SceneComponents.Test
{
    public class TestSceneScenario : SceneScenario
    {
        public override void Process(ComplexObject sceneObjects, float deltaTime)
        {
            var car = sceneObjects.ComplexObjects.FirstOrDefault(x => x.Name == "Car");
            if (car != null)
                car.Rotation += new Vector3(0, deltaTime * 1f, 0);
        }
    }
}
