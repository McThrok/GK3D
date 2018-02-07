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
    public class TestSceneLoader : SceneLoader
    {
        public TestSceneLoader() : base() { }

        public override SceneCollection Load()
        {
            var collection = new SceneCollection();

            collection.Shaders.Add("phong", new ShaderProgram("Graphics\\Resources\\Shaders\\ready\\vs_phong.c", "Graphics\\Resources\\Shaders\\ready\\fs_phong.c", true));
            collection.Shaders.Add("gouraud", new ShaderProgram("Graphics\\Resources\\Shaders\\ready\\vs_gouraud.c", "Graphics\\Resources\\Shaders\\ready\\fs_gouraud.c", true));

            LoadMaterials(collection, "Graphics\\Resources\\Materials\\opentk.mtl");

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

            


            LoadMap(collection);
            LoadCar(collection);
            LoadLamp(collection);

            collection.ActiveCamera = collection.SceneObjects.GetCamerasWiThGlobalModelMatrices().First(x => x.Object.Name == "StaticCamera");
            collection.ActiveShader = "phong";

            return collection;
        }
        public void LoadLamp(SceneCollection collection)
        {
            ComplexObject lamp = new ComplexObject();
            lamp.Position = new Vector3(0, 0, 3.5f);
            lamp.Rotation = new Vector3(0, -(float)Math.PI / 2, 0);
            collection.SceneObjects.ComplexObjects.Add(lamp);

            var lampModel = ObjVolume.LoadFromFile("Graphics\\Resources\\Models\\street_lamp.obj");
            lampModel.Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5);
            lampModel.Position = new Vector3(0, 0, 0);
            lampModel.ColorData = Enumerable.Repeat(new Vector3(1, 1, 1), 320 * 3).ToArray();
            lampModel.Scale = new Vector3(0.15f, 0.15f, 0.15f);
            lampModel.CalculateNormals();
            lamp.Primitives.Add(lampModel);

            Light lampLight = new Light(new Vector3(0, 4, 0), new Vector3(1, 1, 1))
            {
                Position = new Vector3(1.1f, 2, 0),
                Rotation = new Vector3((float)Math.PI / 2, (float)Math.PI, 0),
            };
            lamp.Lights.Add(lampLight);

        }
        public void LoadCar(SceneCollection collection)
        {

            ComplexObject car = new ComplexObject();
            car.Name = "Car";
            car.Position = new Vector3(0, 0, 4.5f);
            car.Rotation = new Vector3(0, -(float)Math.PI / 2, 0);
            collection.SceneObjects.ComplexObjects.Add(car);

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


            Light light1 = new Light(new Vector3(0.1f, 0.1f, -0.5f), new Vector3(1, 1, 1))
            {
                Rotation = new Vector3((float)Math.PI / 16, (float)Math.PI * 7 / 8, 0),
            };
            car.Lights.Add(light1);


            Light light2 = new Light(new Vector3(-0.1f, 0.1f, -0.5f), new Vector3(1, 1, 1))
            {
                Rotation = new Vector3((float)Math.PI / 16, (float)Math.PI * 9 / 8, 0),
            };
            car.Lights.Add(light2);
        }

        public void LoadMap(SceneCollection collection)
        {
            ComplexObject map = new ComplexObject();
            map.Scale = new Vector3(10, 10, 10);
            map.Rotation = new Vector3(-(float)Math.PI / 2, -(float)Math.PI / 2, 0);
            collection.SceneObjects.ComplexObjects.Add(map);


            float layerOffset = 0.001f;
            Vector3 grassColor = new Vector3(0.12f, 0.35f, 0.12f);

            Capsule2D plain = new Capsule2D(1, grassColor, 1);
            plain.Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5);
            plain.Scale = new Vector3(5f, 5f, 5f);
            plain.Position = new Vector3(0, 0, -layerOffset);
            plain.CalculateNormals();
            map.Primitives.Add(plain);

            Capsule2D roadOut = new Capsule2D(1, new Vector3(0.15f, 0.15f, 0.15f), 100);
            roadOut.Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5);
            roadOut.Position = new Vector3(0, 0, 0);
            roadOut.Scale = new Vector3(1.2f, 1.2f, 1.2f);
            roadOut.CalculateNormals();
            map.Primitives.Add(roadOut);

            Capsule2D roadIn = new Capsule2D(1, grassColor, 100);
            roadIn.Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5);
            roadIn.Position = new Vector3(0, 0, layerOffset);
            roadIn.Scale = new Vector3(0.7f, 0.8f, 0.7f);
            roadIn.CalculateNormals();
            map.Primitives.Add(roadIn);

            var ball = ObjVolume.LoadFromFile("Graphics\\Resources\\Models\\ball.obj");
            ball.Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5);
            ball.Position = new Vector3(0, 0, -0.015f);
            ball.ColorData = Enumerable.Repeat(new Vector3(0.55f, 0.43f, 0.33f), ball.ColorDataCount).ToArray();
            ball.Scale = new Vector3(0.15f, 0.15f, 0.15f);
            map.Primitives.Add(ball);

            var ball2 = ball.Clone();
            ball2.Position += new Vector3(0, 0.6f, 0);
            map.Primitives.Add(ball2);

            var ball3 = ball.Clone();
            ball3.Position += new Vector3(0, -0.6f, 0);
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
                ball5.Position += new Vector3(x * 0.2f, y * 1.25f, 0);
                ball5.Scale *= new Vector3(0.5f, 0.5f, 0.5f);
                map.Primitives.Add(ball5);

                var ball4 = ball.Clone();
                ball4.Position += new Vector3(x * 0.55f, y * 1.05f, 0);
                ball4.Scale *= new Vector3(0.5f, 0.5f, 0.5f);
                map.Primitives.Add(ball4);

                var ball6 = ball.Clone();
                ball6.Position += new Vector3(x * 0.7f, y * 0.15f, 0);
                ball6.Scale *= new Vector3(0.5f, 0.5f, 0.5f);
                map.Primitives.Add(ball6);

                var ball7 = ball.Clone();
                ball7.Position += new Vector3(x * 0.7f,y* 0.5f, 0);
                ball7.Scale *= new Vector3(0.5f, 0.5f, 0.5f);
                map.Primitives.Add(ball7);

                var ball8 = ball.Clone();
                ball8.Position += new Vector3(0.7f, 0.8f, 0);
                ball8.Scale *= new Vector3(0.5f, 0.5f, 0.5f);
                map.Primitives.Add(ball6);
            }


        }
    }
}
