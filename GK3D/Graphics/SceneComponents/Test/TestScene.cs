using GK3D.Graphics.Objects;
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

            LoadMaterials(collection, "Graphics\\Resources\\Materials\\opentk.mtl");

            collection.SceneObjects.Lights.Add(new Light(new Vector3(), new Vector3(0.8f, 0.8f, 0.8f), 0.3f));
            collection.SceneObjects.Lights.Add(new Light(new Vector3(), new Vector3(0.8f, 0.8f, 0.8f), 0.3f));
            collection.SceneObjects.Lights.Add(new Light(new Vector3(), new Vector3(0.8f, 0.8f, 0.8f), 0.3f));

            // Load shaders from file
            //collection.Shaders.Add("colored", new ShaderProgram("Graphics\\Resources\\Shaders\\vs_color.glsl", "Graphics\\Resources\\Shaders\\fs_color.glsl", true));
            //collection.Shaders.Add("colored", new ShaderProgram("Graphics\\Resources\\Shaders\\vs_color.glsl", "Graphics\\Resources\\Shaders\\test\\fs_color.glsl", true));
            collection.Shaders.Add("colored", new ShaderProgram("Graphics\\Resources\\Shaders\\old\\vs.glsl", "Graphics\\Resources\\Shaders\\old\\fs.glsl", true));

            //Move camera away from origin
            Camera cam = new Camera();
            cam.Name = "MainCamera";
            cam.Position += new Vector3(0, 5f, 5f);
            cam.Position = new Vector3(0, 0.1f, 10);
            cam.Rotation = new Vector3(0, (float)Math.PI, 0f);
            //collection.SceneObjects.Cameras.Add(cam);


            LoadMap(collection);
            LoadCar(collection);

            collection.ActiveCamera = collection.SceneObjects.GetCamerasWiThGlobalModelMatrices().First(x => x.Object.Name == "CarCamera");
           // collection.ActiveCamera = collection.SceneObjects.GetCamerasWiThGlobalModelMatrices().First(x => x.Object.Name == "MainCamera");
            return collection;
        }

        public void LoadCar(SceneCollection collection)
        {

            ComplexObject car = new ComplexObject();
            car.Position = new Vector3(5, 0, 5);
            collection.SceneObjects.ComplexObjects.Add(car);

            Camera carCamera = new Camera()
            {
                Name = "CarCamera",
               // Position = new Vector3(0, 1, 0),
                //Rotation = new Vector3(0, (float)Math.PI, 0f),
            };
            car.Cameras.Add(carCamera);


            var carModel = ObjVolume.LoadFromFile("Graphics\\Resources\\Models\\racing_car.obj");
            var c = carModel.faces.Min(x => x.Item3.Position.Y);
            carModel.Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5);
            carModel.Position = new Vector3(0, -0.12f, 0);
            carModel.Rotation = new Vector3(0, 0.415f, 0);
            //carModel.Rotation += new Vector3(0, (float)Math.PI / 2, 0);
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
            carModel.Scale = new Vector3(0.005f, 0.005f, 0.005f);
            car.Primitives.Add(carModel);


            Cube qwe = new Cube()
            {
                Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5),
                Position = new Vector3(0.25f, 0.13f, -0.62f),
                Rotation = new Vector3(0, 0, 0),
                Scale = new Vector3(0.1f, 0.1f, 0.1f)
            };
            qwe.CalculateNormals();
            // car.Primitives.Add(nameof(qwe), qwe);


            Cube qwe2 = new Cube()
            {
                Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5),
                Position = new Vector3(-0.25f, 0.135f, -0.62f),
                Rotation = new Vector3(0, 0, 0),
                Scale = new Vector3(0.1f, 0.1f, 0.1f)
            };
            qwe2.CalculateNormals();
            // car.Primitives.Add(nameof(qwe2), qwe2);
        }

        public void LoadMap(SceneCollection collection)
        {
            ComplexObject map = new ComplexObject();
            map.Scale = new Vector3(10, 10, 10);
            map.Rotation = new Vector3(-(float)Math.PI / 2, 0, 0);
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
        }
    }
}
