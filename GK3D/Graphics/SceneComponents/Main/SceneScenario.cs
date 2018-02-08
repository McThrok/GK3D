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

        private bool _isMovieMode;
        private bool _isRaceAnimated;
        private float _delay;

        private float _speed = 1.8f;
        private float _greenMultipleFactor = 1.12f;

        private float _redDistance;
        private float _greenDistance;
        private float _redVelocity;
        private float _greenVelocity;
        private float _redVelocityChanges;
        private float _greenVelocityChanges;

        private Random rd = new Random();

        public void Process(SceneCollection collection, float deltaTime)
        {
            MoveDynamicCamera(collection.SceneObjects);
            MoveSun(collection.SceneObjects, deltaTime);
            ProgressMovie(collection, deltaTime);
        }
        public void StartStopMovie()
        {
            _isMovieMode = !_isMovieMode;

            if (_isMovieMode)
                _delay = 5f;

            StartStopAnimation();
        }
        public void StartStopAnimation()
        {
            _isRaceAnimated = !_isRaceAnimated;
            if (_isRaceAnimated)
            {
                _redDistance = 0;
                _greenDistance = 0;
                _redVelocityChanges = 1;
                _greenVelocityChanges = 1;
            }
        }

        private void MoveSun(ComplexObject sceneObjects, float deltaTime)
        {
            var lights = sceneObjects.GetLightsWiThGlobalModelMatrices();
            var light = lights.FirstOrDefault(x => x.Object.Name == "Sun");
            if (light != null)
            {
                light.Object.DiffuseIntensity = SunBrightness;
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
        private void ProgressMovie(SceneCollection collection, float deltaTime)
        {
            if (_isRaceAnimated)
            {
                var complexObjects = collection.SceneObjects.GetComplexObjectsWiThGlobalModelMatrices();
                var redCar = complexObjects.FirstOrDefault(x => x.Object.Name == "RedCar").Object;
                var greenCar = complexObjects.FirstOrDefault(x => x.Object.Name == "GreenCar").Object;

                var redRadius = 4.3f;
                var greenRadius = 5.3f;

                if (_isMovieMode && _delay >= 0)
                {
                    SetCarPosition(redCar, redRadius, _redDistance);
                    SetCarPosition(greenCar, greenRadius, _greenDistance);
                    _delay -= deltaTime;
                }
                else
                {


                    _redVelocity = SetCarVelocity(redRadius, _redDistance);
                    _greenVelocity = SetCarVelocity(greenRadius, _greenDistance) * _greenMultipleFactor;


                    RandomizeVelocity(redRadius, _redDistance, ref _redVelocityChanges);
                    _redVelocity *= _redVelocityChanges;
                    RandomizeVelocity(greenRadius, _greenDistance, ref _greenVelocityChanges);
                    _greenVelocity *= _greenVelocityChanges;

                    _redDistance += deltaTime * _redVelocity;
                    _greenDistance += deltaTime * _greenVelocity;

                    SetCarPosition(redCar, redRadius, _redDistance);
                    SetCarPosition(greenCar, greenRadius, _greenDistance);
                }


                if (_isMovieMode)
                {
                    collection.ActiveCamera = collection.SceneObjects.GetCamerasWiThGlobalModelMatrices().FirstOrDefault(x => x.Object.Name == "MovieCamera");
                    if (collection.ActiveCamera != null)
                        AnimateCameras(collection.ActiveCamera.Object, redCar, greenCar);
                }
            }

        }
        private void RandomizeVelocity(float radius, float distance, ref float velocityChange)
        {
            if (distance > 10 + Math.PI * radius)
            {
                if (velocityChange == 1)
                    velocityChange = 0.90f + (float)rd.NextDouble() / 5;
            }
        }
        private float SetCarVelocity(float radius, float distance)
        {
            var velocity = 1f;

            //fisrt stright
            if (distance > 0)
            {
                velocity += Math.Min((float)distance, 1) * 2;
                distance -= 1;
            }

            if (distance > 0)
            {
                distance -= 4;
            }

            //fisrt angle
            if (distance > 0)
            {
                distance -= (float)Math.PI * radius;
            }

            //second stright
            if (distance > 0)
            {
                velocity += Math.Min((float)distance, 2) / 2 * 1;
                distance -= 2;
            }

            if (distance > 0)
            {
                distance -= 7;
            }

            if (distance > 0)
            {
                velocity -= Math.Min((float)distance, 1) / 1 * 1;
                distance -= 1;
            }


            //second angle
            if (distance > 0)
            {
                distance -= (float)Math.PI * radius;
            }

            //third stright
            if (distance > 0)
            {
                velocity += Math.Min((float)distance, 2) / 2 * 2;
                distance -= 2;
            }

            if (distance > 0)
            {
                distance -= 4;
            }
            if (distance > 0)
            {
                velocity -= Math.Min((float)distance, 4) / 4 * 5;
                distance -= 4;
            }

            return velocity * _speed;
        }
        private void SetCarPosition(ComplexObject car, float radius, float distance)
        {
            if (distance >= 0)
            {
                car.Position = new Vector3(Math.Min((float)distance, 5), 0, radius);
                car.Rotation = new Vector3(0, -(float)Math.PI / 2, 0);
                distance -= 5;
            }

            if (distance > 0)
            {
                var angle = Math.Min((float)(distance / radius), (float)Math.PI);
                var x = (float)Math.Sin(angle) * radius;
                var z = (float)Math.Cos(angle) * radius - radius;
                car.Position += new Vector3(x, 0, z);

                car.Rotation = new Vector3(0, angle - (float)Math.PI / 2, 0);
                distance -= (float)Math.PI * radius;
            }

            if (distance > 0)
            {
                car.Position -= new Vector3(Math.Min((float)distance, 10), 0, 0);
                distance -= 10;
            }


            if (distance > 0)
            {
                var angle = Math.Min((float)(distance / radius), (float)Math.PI);
                var x = -(float)Math.Sin(angle) * radius;
                var z = -(float)Math.Cos(angle) * radius + radius;
                car.Position += new Vector3(x, 0, z);

                car.Rotation = new Vector3(0, angle + (float)Math.PI / 2, 0);
                distance -= (float)Math.PI * radius;
            }

            if (distance > 0)
            {
                car.Position += new Vector3(Math.Min((float)distance, 10), 0, 0);
                distance -= 10;
            }
        }

        private void AnimateCameras(Camera camera, ComplexObject redCar, ComplexObject greenCar)
        {
            AnimateBeginningScenes(camera, redCar, greenCar);
            //beginning
            if (_delay > 2)
            {

            }
            else if (_delay > 0.5f)
            {

            }
            else if (_delay > 0)
            {

            }

            var distance = Math.Max(_redDistance, _greenDistance);


        }
        private void AnimateBeginningScenes(Camera camera, ComplexObject redCar, ComplexObject greenCar)
        {
            var time = 5f - _delay;

            //test
            time += 4.5f;
            //beginning
            if (time < 2)
            {
                camera.Rotation = greenCar.Rotation;
                camera.Rotation += new Vector3(0, (float)Math.PI, 0);

                camera.Position = greenCar.Position;
                camera.Position += new Vector3(-0.6f, 0.14f, -0.1f - time * 0.4f);
            }
            else if (time < 3f)
            {
                camera.Rotation = greenCar.Rotation;
                var rotationY = Math.Max(Math.Min((float)Math.PI / 2, (time - 2.5f) * 7.2f), 0);
                camera.Rotation += new Vector3(0.2f, (float)Math.PI + rotationY, 0);

                camera.Position = greenCar.Position;
                camera.Position += new Vector3(0, 0.3f, -0.15f);
            }
            else if (time < 3.75f)
            {
                camera.Rotation = redCar.Rotation;
                camera.Rotation += new Vector3(0, (float)Math.PI / 2, 0);

                camera.Position = redCar.Position;
                camera.Position += new Vector3(0, 0.3f, 0.1f);
            }
            else if (time < 4.5f)
            {
                camera.Rotation = greenCar.Rotation;
                var rotationY = Math.Min((float)Math.PI / 2, (time - 3.75f) * 7.2f);
                camera.Rotation += new Vector3(0.2f, (float)Math.PI * 3 / 2 - rotationY, 0);

                camera.Position = greenCar.Position;
                camera.Position += new Vector3(0, 0.3f, -0.15f);
            }
            else if (time < 5f)
            {
                camera.Rotation = new Vector3(0, -(float)Math.PI/2, 0);
                camera.Position = new Vector3(1.1f, 0.1f, 4.8f);
            }
        }
    }
}
