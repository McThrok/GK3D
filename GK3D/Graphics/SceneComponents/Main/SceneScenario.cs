using GK3D.Graphics.SceneComponents.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK3D.Graphics.Objects;
using OpenTK;
using GK3D.Graphics.Objects.Renderable;

namespace GK3D.Graphics.SceneComponents.Main
{
    public class SceneScenario
    {
        public float SunAnimationSpeed { get; set; } = 1;
        public float SunBrightness { get; set; } = 0.4f;

        public void Process(ComplexObject sceneObjects, float deltaTime)
        {
            MoveDynamicCamera(sceneObjects);
            MoveSun(sceneObjects, deltaTime);
        }

        private void MoveSun(ComplexObject sceneObjects, float deltaTime)
        {
                var lights = sceneObjects.GetLightsWiThGlobalModelMatrices();
                var light = lights.FirstOrDefault(x => x.Object.Name == "Sun");
                if (light != null)
                {
                    light.Object.Color = Vector3.One * SunBrightness;
                    light.Object.Rotation.X += deltaTime * SunAnimationSpeed;
                }
        }

        private void MoveDynamicCamera(ComplexObject sceneObjects)
        {
            var complexObjects = sceneObjects.GetComplexObjectsWiThGlobalModelMatrices();
            var cameras = sceneObjects.GetCamerasWiThGlobalModelMatrices();

            var car = complexObjects.FirstOrDefault(x => x.Object.Name == "Car");
            var dynamicCamera = cameras.FirstOrDefault(x => x.Object.Name == "DynamicCam");
            if (car != null && dynamicCamera != null)
            {
                var direction = dynamicCamera.Object.Position - car.Object.Position;
                var vectorY = new Vector3(direction.X, 0, direction.Z);
                var defaultDirection = -Vector3.UnitZ;

                var angleX = Vector3.CalculateAngle(direction, vectorY);

                var angleY = Vector3.CalculateAngle(vectorY, defaultDirection);
                if (vectorY.X > 0)
                    angleY = 2 * (float)Math.PI - angleY;

                dynamicCamera.Object.Rotation = new Vector3(angleX, angleY, 0);
            }

        }
    }
}
