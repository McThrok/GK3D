using GK3D.Graphics.Objects;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace GK3D.Graphics.SceneComponents
{
    public class TestScene : Scene
    {
        public TestScene() : base() { }

        public override void Load()
        {
            Collection = new SceneCollection();
            Collection.Lights.Add("mainLight", new Light(new Vector3(), new Vector3(0.8f, 0.8f, 0.8f), 0.3f));
            Light spotLight2 = new Light(new Vector3(0, 3f, 0), new Vector3(1f, 1f, 1f), 1, 1);
            spotLight2.Type = LightType.Spot;
            spotLight2.Rotation = new Vector3(0, 1.0f, 0).Normalized();
            spotLight2.ConeAngle = 10f;
            Collection.Lights.Add("spotLight2", spotLight2);

            ActiveLights = Collection.Lights.Values.FirstOrDefault();

            // Load shaders from file
            Collection.Shaders.Add("lit_mat", new ShaderProgram("Graphics\\Resources\\Shaders\\vs_lit_mat.glsl", "Graphics\\Resources\\Shaders\\fs_lit_mat.glsl", true));
            ActiveShader = Collection.Shaders.Values.FirstOrDefault();

            // Create our objects
            Cube cubex = new ColoredCube(new Vector3(1, 1, 0))
            {
                Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5),
                Position = new Vector3(2f, 0, 0),
                Rotation = new Vector3(0, 0, 0)
            };
            cubex.CalculateNormals();
           // Collection.Objects.Add("x", cubex);

            Cube cubey = new ColoredCube(new Vector3(1, 0, 0))
            {
                Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5),
                Position = new Vector3(0, 5f, 0),
                Rotation = new Vector3(0, 0, 0)
            };
            cubey.CalculateNormals();
           // Collection.Objects.Add("y", cubey);

            Cube cubez = new ColoredCube(new Vector3(0, 1, 0))
            {
                Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5),
                Position = new Vector3(0, 0, 3.5f),
                Rotation = new Vector3(0, 0, 0)
            };
            cubez.CalculateNormals();
            //Collection.Objects.Add("z", cubez);

            Cube center = new ColoredCube(new Vector3(0, 0, 1))
            {
                Material = new Material(new Vector3(0.15f), new Vector3(1), new Vector3(0.2f), 5),
                Position = new Vector3(0, 0, 0),
                Rotation = new Vector3(0, 0, 0)
            };
            center.CalculateNormals();
            center.Scale = new Vector3(3f, 3f, 3f);
            Collection.Objects.Add("center", center);



            Cube car = new ColoredCube(new Vector3(1, 1, 1))
            {
                Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5),
                Position = new Vector3(2f,2f,2f),
                Rotation = new Vector3(0, 0, 0)
            };
            car.CalculateNormals();
            Collection.Objects.Add("car", car);

            ComplexObject co = new ComplexObject();
            co.Objects.Add("car", Collection.Objects["car"]);
            co.Objects.Add("center", Collection.Objects["center"]);
             co.Rotate(new Vector3(0, (float) Math.PI/16,0 ));
            Collection.ComplexObjects.Add("co", co);


            //Move camera away from origin
            Camera cam = new Camera();
            cam.Position += new Vector3(0, 0f, 7f);
            cam.Rotation = new Vector3(0f, (float)Math.PI, 0f);
            Collection.Cameras.Add("mainCamera", cam);


            Camera secondCamera = new Camera();
            secondCamera.Position = new Vector3(0f, 8f, 0);
            secondCamera.Rotation = new Vector3(-(float)Math.PI / 2 + 0.1f, (float)Math.PI, 0f);
            Collection.Cameras.Add("secondCamera", secondCamera);


            ActiveCamera = Collection.Cameras.Values.FirstOrDefault();
        }

        public override void SetStart()
        {
        }
        public override void Process(float deltaTime)
        {
            Collection.ComplexObjects["co"].Rotate(new Vector3(0,0,0.1f));
        }

    }
}
