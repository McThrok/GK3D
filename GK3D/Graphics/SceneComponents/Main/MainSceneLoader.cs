using GK3D.Graphics.Objects;
using GK3D.Graphics.Objects.Renderable;
using GK3D.Graphics.SceneComponents.Base;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace GK3D.Graphics.SceneComponents.Test
{
    public class MainSceneLoader : SceneLoader
    {
        public MainSceneLoader() : base() { }

        public override SceneCollection Load()
        {
            var collection = new SceneCollection();

            LoadMaterials(collection, "Graphics\\Resources\\Materials\\opentk.mtl");

            LoadShaders(collection);
            LoadMainCameras(collection);

            var map = LoadMap();
            map.Scale = new Vector3(10, 10, 10);
            collection.SceneObjects.ComplexObjects.Add(map);

            var car = LoadCar();
            collection.SceneObjects.ComplexObjects.Add(car);


            var light = LoadMainLight();
            collection.SceneObjects.Lights.Add(light);


            return collection;
        }
        private void LoadShaders(SceneCollection collection)
        {
            collection.Shaders.Add("phong_phong", new ShaderProgram("Graphics\\Resources\\Shaders\\ready\\vs_phong.c", "Graphics\\Resources\\Shaders\\ready\\fs_phong_phong.c", true));
            collection.Shaders.Add("phong_blinn", new ShaderProgram("Graphics\\Resources\\Shaders\\ready\\vs_phong.c", "Graphics\\Resources\\Shaders\\ready\\fs_phong_blinn.c", true));
            collection.Shaders.Add("gouraud_phong", new ShaderProgram("Graphics\\Resources\\Shaders\\ready\\vs_gouraud_phong.c", "Graphics\\Resources\\Shaders\\ready\\fs_gouraud.c", true));
            collection.Shaders.Add("gouraud_blinn", new ShaderProgram("Graphics\\Resources\\Shaders\\ready\\vs_gouraud_blinn.c", "Graphics\\Resources\\Shaders\\ready\\fs_gouraud.c", true));
            collection.ActiveShader = "phong_phong";
        }
        private void LoadMainCameras(SceneCollection collection)
        {
            Camera staticCam = new Camera();
            staticCam.Name = "StaticCamera";
            staticCam.Position = new Vector3(0, 8f, 8);
            staticCam.Rotation = new Vector3((float)Math.PI / 4, (float)Math.PI, 0);
            collection.SceneObjects.Cameras.Add(staticCam);

            Camera dynamicCam = new Camera();
            dynamicCam.Name = "DynamicCam";
            dynamicCam.Position = new Vector3(0, 5f, 0);
            dynamicCam.Rotation = new Vector3(-(float)Math.PI / 4, (float)Math.PI, 0);
            dynamicCam.Rotation += new Vector3((float)Math.PI / 4, 0, 0);
            collection.SceneObjects.Cameras.Add(dynamicCam);

            collection.ActiveCamera = collection.SceneObjects.GetCamerasWiThGlobalModelMatrices().First(x => x.Object.Name == "StaticCamera");
        }
        private ComplexObject LoadCar()
        {
            ComplexObject car = new ComplexObject();
            car.Name = "Car";
            car.Position = new Vector3(0, 0, 4.5f);
            car.Rotation = new Vector3(0, -(float)Math.PI / 2, 0);

            Camera carCamera = new Camera()
            {
                Name = "CarCamera",
                Position = new Vector3(0, 2, 1),
                Rotation = new Vector3((float)Math.PI / 4, (float)Math.PI, 0),
            };
            car.Cameras.Add(carCamera);


            var carModel = ObjVolume.LoadFromFile("Graphics\\Resources\\Models\\racing_car.obj");
            var c = carModel.faces.Min(x => x.Item3.Position.Y);
            carModel.Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5);
            carModel.Position = new Vector3(0, -0.12f, 0);
            carModel.Rotation = new Vector3(0, 0.415f, 0);
            var colorData = new List<Vector3>();
            colorData.AddRange(Enumerable.Repeat(new Vector3(0, 0, 0), 320 * 3));
            colorData.AddRange(Enumerable.Repeat(new Vector3(1, 0, 0), 88 * 6));
            colorData.AddRange(Enumerable.Repeat(new Vector3(1, 1, 1), 182 * 6));
            colorData.AddRange(Enumerable.Repeat(new Vector3(1, 0, 0), 161 * 6));
            colorData.AddRange(Enumerable.Repeat(new Vector3(0, 0, 1), 10 * 6));//glass
            colorData.AddRange(Enumerable.Repeat(new Vector3(1, 1, 1), 4 * 6));//light
            colorData.AddRange(Enumerable.Repeat(new Vector3(1, 1, 1), 168 * 6));//wheel out
            colorData.AddRange(Enumerable.Repeat(new Vector3(0, 0, 0), 672));//wheel in
            colorData.AddRange(Enumerable.Repeat(new Vector3(1, 1, 1), 168 * 6));//wheel out
            colorData.AddRange(Enumerable.Repeat(new Vector3(0, 0, 0), 672));//wheel in
            carModel.ColorData = colorData.ToArray();
            carModel.Scale = new Vector3(0.003f, 0.003f, 0.003f);
            carModel.CalculateNormals();
            car.Primitives.Add(carModel);


            Light light1 = new Light(new Vector3(0.1f, 0.1f, -0.5f), new Vector3(1, 1, 1), 1f, 0.5f)
            {
                Rotation = new Vector3((float)Math.PI / 16, (float)Math.PI * 15 / 16, 0),
                ConeAngle = 60,
                Type = LightType.Spot
            };
            car.Lights.Add(light1);


            Light light2 = new Light(new Vector3(-0.1f, 0.1f, -0.5f), new Vector3(1, 1, 1), 1f, 0.5f)
            {
                Rotation = new Vector3((float)Math.PI / 16, (float)Math.PI * 17 / 16, 0),
                ConeAngle = 60,
                Type = LightType.Spot
            };
            car.Lights.Add(light2);

            return car;
        }
        private Light LoadMainLight()
        {
            Light mainLight = new Light(new Vector3(1f,1f, 1f), new Vector3(1, 1, 1), 1f, 0.5f)
            {
                Rotation = new Vector3((float)Math.PI / 4, (float)Math.PI * 15 / 16, 0),
                Type = LightType.Directional,
            };

            return mainLight;
        }

        private ComplexObject LoadMap()
        {
            ComplexObject map = new ComplexObject();
            LoadTerrain(map);
            LoadLantenrs(map);
            LoadBalls(map);

            return map;
        }
        private void LoadLantenrs(ComplexObject map)
        {
            for (int i = 0; i < 4; i++)
            {
                int x = 1;
                int z = 1;

                if (i / 2 == 0)
                    x = -x;
                if (i % 2 == 0)
                    z = -z;

                float rotationZ = 0;
                if (z == -1)
                    rotationZ += (float)Math.PI;

                var lamp1 = LoadLamp();
                lamp1.Position += new Vector3(x * 0.5f, 0, z * 0.33f);
                lamp1.Rotation = new Vector3(0, x * z * (float)Math.PI / 5 + rotationZ, 0);
                lamp1.Scale = new Vector3(0.1f);
                map.ComplexObjects.Add(lamp1);

                var lamp2 = LoadLamp();
                lamp2.Position += new Vector3(x * 0.17f, 0, z * 0.34f);
                lamp2.Rotation = new Vector3(0, rotationZ, 0);
                lamp2.Scale = new Vector3(0.1f);
                map.ComplexObjects.Add(lamp2);

                var lamp3 = LoadLamp();
                lamp3.Position += new Vector3(x * 0.74f, 0, z * 0.17f);
                lamp3.Rotation = new Vector3(0, x*z*(float)Math.PI / 2.5f + rotationZ, 0);
                lamp3.Scale = new Vector3(0.1f);
                map.ComplexObjects.Add(lamp3);
                
            }
        }
        private ComplexObject LoadLamp()
        {
            ComplexObject lamp = new ComplexObject();

            var lampModel = ObjVolume.LoadFromFile("Graphics\\Resources\\Models\\street_lamp.obj");
            lampModel.Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5);
            lampModel.Position = new Vector3(0, 0, 0);
            lampModel.Rotation = new Vector3(0, -(float)Math.PI / 2, 0);
            lampModel.ColorData = Enumerable.Repeat(new Vector3(0.3f, 0.3f, 0.3f), lampModel.ColorDataCount).ToArray();
            lampModel.Scale = new Vector3(0.15f, 0.15f, 0.15f);
            lampModel.CalculateNormals();
            lamp.Primitives.Add(lampModel);

            Light lampLight = new Light(new Vector3(0, 2, 1.1f), new Vector3(1, 0.85f, 0.55f), 0.75f, 0)
            {
                Rotation = new Vector3((float)Math.PI / 2, 0, 0),
                ConeAngle = 60f,
                ConeExponent = 6,
                Type = LightType.Spot
            };
            lamp.Lights.Add(lampLight);

            return lamp;
        }

        private void LoadTerrain(ComplexObject map)
        {
            float layerOffset = 0.001f;
            Vector3 grassColor = new Vector3(0.12f, 0.35f, 0.12f);

            Capsule2D plain = new Capsule2D(1, grassColor, 1);
            plain.Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5);
            plain.Scale = new Vector3(5f, 5f, 5f);
            plain.Rotation = new Vector3(-(float)Math.PI / 2, -(float)Math.PI / 2, 0);
            plain.Position = new Vector3(0, -layerOffset, 0);
            plain.CalculateNormals();
            map.Primitives.Add(plain);

            Capsule2D roadOut = new Capsule2D(1, new Vector3(0.25f, 0.25f, 0.25f), 100);
            roadOut.Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5);
            roadOut.Rotation = new Vector3(-(float)Math.PI / 2, -(float)Math.PI / 2, 0);
            roadOut.Scale = new Vector3(1.2f, 1.2f, 1.2f);
            roadOut.CalculateNormals();
            map.Primitives.Add(roadOut);

            Capsule2D roadIn = new Capsule2D(1, grassColor, 100);
            roadIn.Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5);
            roadIn.Position = new Vector3(0, layerOffset, 0);
            roadIn.Scale = new Vector3(0.7f, 0.8f, 0.7f);
            roadIn.Rotation = new Vector3(-(float)Math.PI / 2, -(float)Math.PI / 2, 0);
            roadIn.CalculateNormals();
            map.Primitives.Add(roadIn);

        }
        private void LoadBalls(ComplexObject map)
        {
            var ball = ObjVolume.LoadFromFile("Graphics\\Resources\\Models\\ball.obj");
            ball.Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5);
            ball.Position = new Vector3(0, -0.015f, 0);
            ball.ColorData = Enumerable.Repeat(new Vector3(0.33f, 0.33f, 0.33f), ball.ColorDataCount).ToArray();
            ball.Scale = new Vector3(0.15f, 0.15f, 0.15f);
            map.Primitives.Add(ball);

            var ball2 = ball.Clone();
            ball2.Position += new Vector3(0.6f, 0, 0);
            map.Primitives.Add(ball2);

            var ball3 = ball.Clone();
            ball3.Position += new Vector3(-0.6f, 0, 0);
            map.Primitives.Add(ball3);


            for (int i = 0; i < 4; i++)
            {
                int x = 1;
                int y = 1;

                if (i / 2 == 0)
                    x = -x;
                if (i % 2 == 0)
                    y = -y;

                var ball5 = ball.Clone();
                ball5.Position += new Vector3(x * 1.25f, 0, y * 0.2f);
                ball5.Scale *= new Vector3(0.5f, 0.5f, 0.5f);
                map.Primitives.Add(ball5);

                var ball4 = ball.Clone();
                ball4.Position += new Vector3(x * 1.05f, 0, y * 0.55f);
                ball4.Scale *= new Vector3(0.5f, 0.5f, 0.5f);
                map.Primitives.Add(ball4);

                var ball6 = ball.Clone();
                ball6.Position += new Vector3(x * 0.2f, 0, y * 0.7f);
                ball6.Scale *= new Vector3(0.5f, 0.5f, 0.5f);
                map.Primitives.Add(ball6);

                var ball7 = ball.Clone();
                ball7.Position += new Vector3(x * 0.65f, 0, y * 0.7f);
                ball7.Scale *= new Vector3(0.5f, 0.5f, 0.5f);
                map.Primitives.Add(ball7);

                var ball8 = ball.Clone();
                ball8.Position += new Vector3(x * 0.8f, 0, y * 0.7f);
                ball8.Scale *= new Vector3(0.5f, 0.5f, 0.5f);
                map.Primitives.Add(ball6);
            }

        }
    }
}
