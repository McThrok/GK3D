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

        private bool IsMovieMode;
        private bool IsMovieAnimated;
        private float _redDistance;
        private float _greenDistance;
        private float _redVelocity;
        private float _greenVelocity;

        public void Process(ComplexObject sceneObjects, float deltaTime)
        {
            MoveDynamicCamera(sceneObjects);
            MoveSun(sceneObjects, deltaTime);
            ProgressMovie(sceneObjects, deltaTime);
        }
        public void StartStopMovie()
        {
            IsMovieMode = !IsMovieMode;
            StartStopAnimation();
        }
        public void StartStopAnimation()
        {
            IsMovieAnimated = !IsMovieAnimated;
            if (IsMovieAnimated)
            {
                _redDistance = 0;
                _greenDistance = 0;
                _redVelocity = 3.0f;
                _greenVelocity = 3.39f;
            }
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

            var car = complexObjects.FirstOrDefault(x => x.Object.Name == "RedCar");
            var dynamicCamera = cameras.FirstOrDefault(x => x.Object.Name == "DynamicCamera");
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
        private void ProgressMovie(ComplexObject sceneObjects, float deltaTime)
        {
            if (IsMovieAnimated)
            {
                var complexObjects = sceneObjects.GetComplexObjectsWiThGlobalModelMatrices();
                var redCar = complexObjects.FirstOrDefault(x => x.Object.Name == "RedCar").Object;
                var greenCar = complexObjects.FirstOrDefault(x => x.Object.Name == "GreenCar").Object;

                _redDistance += deltaTime * _redVelocity;
                _greenDistance += deltaTime * _greenVelocity;


                SetCarPosition(redCar, 2.15f, _redDistance);
                SetCarPosition(greenCar, 2.65f, _greenDistance);
            }
        }
        private void SetCarPosition(ComplexObject car, float radius, float carDistance)
        {
            var distance = carDistance;

            if (distance > 0)
            {
                car.Position = new Vector3(Math.Min(distance, 5), 0, 2 * radius);
                car.Rotation = new Vector3(0, -(float)Math.PI / 2, 0);
                distance -= 5;
            }

            if (distance > 0)
            {
                var angle = Math.Min(distance / (2 * radius), (float)Math.PI);
                var x = (float)Math.Sin(angle) * 2 * radius;
                var z = (float)Math.Cos(angle) * 2 * radius - 2 * radius;
                car.Position += new Vector3(x, 0, z);

                car.Rotation = new Vector3(0, angle - (float)Math.PI / 2, 0);
                distance -= 2 * (float)Math.PI * radius;
            }

            if (distance > 0)
            {
                car.Position -= new Vector3(Math.Min(distance, 10), 0, 0);
                distance -= 10;
            }


            if (distance > 0)
            {
                var angle = Math.Min(distance / (2 * radius), (float)Math.PI);
                var x = -(float)Math.Sin(angle) * 2 * radius;
                var z = -(float)Math.Cos(angle) * 2 * radius + 2 * radius;
                car.Position += new Vector3(x, 0, z);

                car.Rotation = new Vector3(0, angle + (float)Math.PI / 2, 0);
                distance -= 2 * (float)Math.PI * radius;
            }

            if (distance > 0)
            {
                car.Position += new Vector3(Math.Min(distance, 10), 0, 0);
                distance -= 10;
            }
        }
    }
}
