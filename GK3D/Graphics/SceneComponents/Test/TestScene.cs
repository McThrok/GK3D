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

            collection.SceneObjects.Lights.Add("mainLight", new Light(new Vector3(), new Vector3(0.8f, 0.8f, 0.8f), 0.3f));
            Light spotLight2 = new Light(new Vector3(0, 3f, 0), new Vector3(1f, 1f, 1f), 1, 1);
            spotLight2.Type = LightType.Spot;
            spotLight2.Rotation = new Vector3(0, 1.0f, 0).Normalized();
            spotLight2.ConeAngle = 10f;
            collection.SceneObjects.Lights.Add("spotLight2", spotLight2);


            // Load shaders from file
            collection.Shaders.Add("colored", new ShaderProgram("Graphics\\Resources\\Shaders\\vs_color.glsl", "Graphics\\Resources\\Shaders\\fs_color.glsl", true));
            collection.Shaders.Add("textured", new ShaderProgram("Graphics\\Resources\\Shaders\\vs_texture.glsl", "Graphics\\Resources\\Shaders\\fs_texture.glsl", true));
            collection.ActiveShader = collection.Shaders.Values.FirstOrDefault();

            //var car = ObjVolume.LoadFromFile("Graphics\\Resources\\Models\\racing_car.obj");
            //car.Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5);
            //car.Scale = new Vector3(0.05f, 0.05f, 0.05f);
            //collection.Objects.Add(nameof(car), car);

            //collection.Textures.Add("mario_main", LoadImage("Graphics\\Resources\\Textures\\mario_main.png"));
            //var mario = ObjVolume.LoadFromFile("Graphics\\Resources\\Models\\mario.obj");
            ////mario.Material = collection.Materials["mario"];
            ////mario.TextureID =collection.Textures[ collection.Materials["mario"].DiffuseMap];
            //mario.Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5);
            //mario.TextureID = collection.Textures["mario_main"];
            ////mario.IsTextured = true;
            ////mario.Scale = new Vector3(2f, 0.5f, 0.5f);
            //mario.Rotation = new Vector3(-(float)Math.PI / 2, (float)Math.PI,0);
            //collection.SceneObjects.Primitives.Add(nameof(mario), mario);


            //Capsule2D capsule = new Capsule2D(1, new Vector3(1, 1, 0), 1);
            //capsule.Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5);
            //capsule.Position = new Vector3(0, 0, -3f);
            //capsule.CalculateNormals();
            //collection.Objects.Add("cap", capsule);



            Cube center = new ColoredCube(new Vector3(1,0 , 0))
            {
                Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5),
                Position = new Vector3(0, 0, 0),
                Rotation = new Vector3(0, 0, 0)
            };
            center.CalculateNormals();
            collection.SceneObjects.Primitives.Add("center", center);

            var com = new ComplexObject();
            com.Position = new Vector3(0, 1, 0);
            com.Rotation = new Vector3(0, (float)Math.PI / 16, 0);

            Cube cubex = new ColoredCube(new Vector3(1, 1, 0))
            {
                Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5),
                Position = new Vector3(1f, 0, 0),
                Rotation = new Vector3(0, (float)Math.PI / 16, 0)
            };
            cubex.CalculateNormals();
            com.Primitives.Add("x", cubex);

            Cube cubexx = new ColoredCube(new Vector3(1, 1, 0))
            {
                Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5),
                Position = new Vector3(-1f, 0, 0),
                Rotation = new Vector3(0, 0, 0)
            };
            cubexx.CalculateNormals();
            com.Primitives.Add("xx", cubexx);


            var com2 = new ComplexObject();
            collection.SceneObjects.ComplexObjects.Add("com2", com2);
            com2.Position = new Vector3(1, 0, 0);
            com2.Rotation = new Vector3(0, (float)Math.PI / 16, 0);

            Cube qwe = new ColoredCube(new Vector3(1, 1, 0))
            {
                Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5),
                Position = new Vector3(-2f, 0, 0),
                Rotation = new Vector3(0, 0, 0)
            };
            qwe.CalculateNormals();
            com2.Primitives.Add("xx", qwe);
            com2.ComplexObjects.Add("xxx", com);

            //Cube center = new ColoredCube(new Vector3(0, 0, 1))
            //{
            //    Material = new Material(new Vector3(0.15f), new Vector3(1), new Vector3(0.2f), 5),
            //    Position = new Vector3(0, 0, 0),
            //    Rotation = new Vector3(0, 0, 0),
            //    Scale = new Vector3(3f, 3f, 3f)
            //};
            //center.CalculateNormals();
            //Collection.Objects.Add("center", center);



            //Cube car = new ColoredCube(new Vector3(1, 1, 1))
            //{
            //    Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5),
            //    Position = new Vector3(2f, 2f, 2f),
            //    Rotation = new Vector3(0, 0, 0)
            //};
            //car.CalculateNormals();
            //Collection.Objects.Add("car", car);

            //ComplexObject co = new ComplexObject();
            //co.Objects.Add("car", Collection.Objects["car"]);
            //co.Objects.Add("center", Collection.Objects["center"]);
            //co.Rotate(new Vector3(0, (float)Math.PI / 16, 0));
            //Collection.ComplexObjects.Add("co", co);


            //Move camera away from origin
            Camera cam = new Camera();
            cam.Position += new Vector3(0, 0f, 7f);
            cam.Rotation = new Vector3(0f, (float)Math.PI, 0f);
            collection.SceneObjects.Cameras.Add("mainCamera", cam);


            Camera secondCamera = new Camera();
            secondCamera.Position = new Vector3(0f, 8f, 0);
            secondCamera.Rotation = new Vector3(-(float)Math.PI / 2 + 0.1f, (float)Math.PI, 0f);
            collection.SceneObjects.Cameras.Add("secondCamera", secondCamera);


            collection.ActiveCamera = collection.SceneObjects.Cameras.Values.FirstOrDefault();

            return collection;
        }
    }
}
