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
            var complexObjects = sceneObjects.GetComplexObjectsWiThGlobalModelMatrices();
            var cameras = sceneObjects.GetCamerasWiThGlobalModelMatrices();

            var test = complexObjects.FirstOrDefault(x => x.Object.Name == "Test");
            if (test != null)
                test.Object.Rotation += new Vector3(0, deltaTime * 0.75f, 0);

            var car = complexObjects.FirstOrDefault(x => x.Object.Name == "Car");
            if (car != null)
            {
               // car.Object.Rotation += new Vector3(0, deltaTime * 1f, 0);

                var dynamicCamera = cameras.FirstOrDefault(x => x.Object.Name == "DynamicCam");
                if (dynamicCamera != null)
                {
                    var ball = sceneObjects.GetPrimitivesWiThGlobalModelMatrices().First(x => x.Object.Name == "ball");
                    var direction = dynamicCamera.Object.Position - ball.Object.Position.ApplyOnPoint(ball.GlobalModelMatrix);
                    //var direction = dynamicCamera.Object.Position - car.Object.Position;
                    var vectorY = new Vector3(direction.X, 0, direction.Z);
                    var defaultDirection = -Vector3.UnitZ;

                    var angleX = Vector3.CalculateAngle(direction, vectorY);

                    var angleY = Vector3.CalculateAngle(vectorY, defaultDirection);
                    if (vectorY.X >0)
                        angleY = 2 * (float)Math.PI - angleY;

                    dynamicCamera.Object.Rotation = new Vector3(angleX, angleY, 0);
                }
            }
        }
    }
}
